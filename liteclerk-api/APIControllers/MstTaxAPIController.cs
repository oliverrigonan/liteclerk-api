using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MstTaxAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstTaxAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetTaxList()
        {
            try
            {
                var taxes = await (
                    from d in _dbContext.MstTaxes
                    select new DTO.MstTaxDTO
                    {
                        Id = d.Id,
                        TaxCode = d.TaxCode,
                        ManualCode = d.ManualCode,
                        TaxDescription = d.TaxDescription,
                        TaxRate = d.TaxRate,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, taxes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetTaxDetail(Int32 id)
        {
            try
            {
                var tax = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == id
                    select new DTO.MstTaxDTO
                    {
                        Id = d.Id,
                        TaxCode = d.TaxCode,
                        ManualCode = d.ManualCode,
                        TaxDescription = d.TaxDescription,
                        TaxRate = d.TaxRate,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, tax);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTax([FromBody] DTO.MstTaxDTO mstTaxDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a tax.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a tax.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstTaxDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                String taxCode = "0000000001";
                var lastTax = await (
                    from d in _dbContext.MstTaxes
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastTax != null)
                {
                    Int32 lastTaxCode = Convert.ToInt32(lastTax.TaxCode) + 0000000001;
                    taxCode = PadZeroes(lastTaxCode, 10);
                }

                var newTax = new DBSets.MstTaxDBSet()
                {
                    TaxCode = taxCode,
                    ManualCode = mstTaxDTO.ManualCode,
                    TaxDescription = mstTaxDTO.TaxDescription,
                    TaxRate = mstTaxDTO.TaxRate,
                    AccountId = mstTaxDTO.AccountId,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstTaxes.Add(newTax);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateTax(int id, [FromBody] DTO.MstTaxDTO mstTaxDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a tax.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a tax.");
                }

                var tax = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (tax == null)
                {
                    return StatusCode(404, "Tax not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstTaxDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var updateTax = tax;
                updateTax.ManualCode = mstTaxDTO.ManualCode;
                updateTax.TaxDescription = mstTaxDTO.TaxDescription;
                updateTax.TaxRate = mstTaxDTO.TaxRate;
                updateTax.AccountId = mstTaxDTO.AccountId;
                updateTax.UpdatedByUserId = loginUserId;
                updateTax.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteTax(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a tax.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a tax.");
                }

                var tax = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (tax == null)
                {
                    return StatusCode(404, "Tax not found.");
                }

                _dbContext.MstTaxes.Remove(tax);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
