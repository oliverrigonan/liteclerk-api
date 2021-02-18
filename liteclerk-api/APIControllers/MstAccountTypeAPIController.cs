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
    public class MstAccountTypeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstAccountTypeAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetAccountTypeList()
        {
            try
            {
                var accountTypes = await (
                    from d in _dbContext.MstAccountTypes
                    select new DTO.MstAccountTypeDTO
                    {
                        Id = d.Id,
                        AccountTypeCode = d.AccountTypeCode,
                        ManualCode = d.ManualCode,
                        AccountType = d.AccountType,
                        AccountCategoryId = d.AccountCategoryId,
                        AccountCategory = new DTO.MstAccountCategoryDTO
                        {
                            ManualCode = d.MstAccountCategory_AccountCategoryId.ManualCode,
                            AccountCategory = d.MstAccountCategory_AccountCategoryId.AccountCategory
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

                return StatusCode(200, accountTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetAccountTypeDetail(Int32 id)
        {
            try
            {
                var accountType = await (
                    from d in _dbContext.MstAccountTypes
                    where d.Id == id
                    select new DTO.MstAccountTypeDTO
                    {
                        Id = d.Id,
                        AccountTypeCode = d.AccountTypeCode,
                        ManualCode = d.ManualCode,
                        AccountType = d.AccountType,
                        AccountCategoryId = d.AccountCategoryId,
                        AccountCategory = new DTO.MstAccountCategoryDTO
                        {
                            ManualCode = d.MstAccountCategory_AccountCategoryId.ManualCode,
                            AccountCategory = d.MstAccountCategory_AccountCategoryId.AccountCategory
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

                return StatusCode(200, accountType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddAccountType([FromBody] DTO.MstAccountTypeDTO mstAccountTypeDTO)
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
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a account type.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a account type.");
                }

                var accountCategory = await (
                    from d in _dbContext.MstAccountCategories
                    where d.Id == mstAccountTypeDTO.AccountCategoryId
                    select d
                ).FirstOrDefaultAsync();

                if (accountCategory == null)
                {
                    return StatusCode(404, "Account category not found.");
                }

                String accountTypeCode = "0000000001";
                var lastAccountType = await (
                    from d in _dbContext.MstAccountTypes
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastAccountType != null)
                {
                    Int32 lastAccountTypeCode = Convert.ToInt32(lastAccountType.AccountTypeCode) + 0000000001;
                    accountTypeCode = PadZeroes(lastAccountTypeCode, 10);
                }

                var newAccountType = new DBSets.MstAccountTypeDBSet()
                {
                    AccountTypeCode = accountTypeCode,
                    ManualCode = mstAccountTypeDTO.ManualCode,
                    AccountType = mstAccountTypeDTO.AccountType,
                    AccountCategoryId = mstAccountTypeDTO.AccountCategoryId,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstAccountTypes.Add(newAccountType);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateAccountType(int id, [FromBody] DTO.MstAccountTypeDTO mstAccountTypeDTO)
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
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a account type.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a account type.");
                }

                var accountType = await (
                    from d in _dbContext.MstAccountTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (accountType == null)
                {
                    return StatusCode(404, "Account type not found.");
                }

                var accountCategory = await (
                    from d in _dbContext.MstAccountCategories
                    where d.Id == mstAccountTypeDTO.AccountCategoryId
                    select d
                ).FirstOrDefaultAsync();

                if (accountCategory == null)
                {
                    return StatusCode(404, "Account category not found.");
                }

                var updateAccountType = accountType;
                updateAccountType.ManualCode = mstAccountTypeDTO.ManualCode;
                updateAccountType.AccountType = mstAccountTypeDTO.AccountType;
                updateAccountType.AccountCategoryId = mstAccountTypeDTO.AccountCategoryId;
                updateAccountType.UpdatedByUserId = loginUserId;
                updateAccountType.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteAccountType(int id)
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
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a account type.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a account type.");
                }

                var accountType = await (
                    from d in _dbContext.MstAccountTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (accountType == null)
                {
                    return StatusCode(404, "Account type not found.");
                }

                _dbContext.MstAccountTypes.Remove(accountType);
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
