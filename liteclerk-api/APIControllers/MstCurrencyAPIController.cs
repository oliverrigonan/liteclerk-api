using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MstCurrencyAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstCurrencyAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetCurrencyList()
        {
            try
            {
                var currencies = await (
                    from d in _dbContext.MstCurrencies
                    select new DTO.MstCurrencyDTO
                    {
                        Id = d.Id,
                        CurrencyCode = d.CurrencyCode,
                        ManualCode = d.ManualCode,
                        Currency = d.Currency,
                        Remarks = d.Remarks,
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, currencies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedCurrencyList()
        {
            try
            {
                var currencies = await (
                    from d in _dbContext.MstCurrencies
                    where d.IsLocked == true
                    select new DTO.MstCurrencyDTO
                    {
                        Id = d.Id,
                        CurrencyCode = d.CurrencyCode,
                        ManualCode = d.ManualCode,
                        Currency = d.Currency,
                        Remarks = d.Remarks,
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, currencies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetCurrencyDetail(Int32 id)
        {
            try
            {
                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == id
                    select new DTO.MstCurrencyDTO
                    {
                        Id = d.Id,
                        CurrencyCode = d.CurrencyCode,
                        ManualCode = d.ManualCode,
                        Currency = d.Currency,
                        Remarks = d.Remarks,
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, currency);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCurrency()
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
                    && d.SysForm_FormId.Form == "SetupCurrencyList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a currency.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a currency.");
                }

                String currencyCode = "0000000001";
                var lastCurrency = await (
                    from d in _dbContext.MstCurrencies
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastCurrency != null)
                {
                    Int32 lastCurrencyCode = Convert.ToInt32(lastCurrency.CurrencyCode) + 0000000001;
                    currencyCode = PadZeroes(lastCurrencyCode, 10);
                }

                var newCurrency = new DBSets.MstCurrencyDBSet()
                {
                    CurrencyCode = currencyCode,
                    ManualCode = currencyCode,
                    Currency = "",
                    Remarks = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstCurrencies.Add(newCurrency);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newCurrency.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveCurrency(Int32 id, [FromBody] DTO.MstCurrencyDTO mstCurrencyDTO)
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
                    && d.SysForm_FormId.Form == "SetupCurrencyDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a currency.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a currency.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a currency that is locked.");
                }

                var saveCurrency = currency;
                saveCurrency.ManualCode = mstCurrencyDTO.ManualCode;
                saveCurrency.Currency = mstCurrencyDTO.Currency;
                saveCurrency.Remarks = mstCurrencyDTO.Remarks;
                saveCurrency.UpdatedByUserId = loginUserId;
                saveCurrency.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockCurrency(Int32 id, [FromBody] DTO.MstCurrencyDTO mstCurrencyDTO)
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
                    && d.SysForm_FormId.Form == "SetupCurrencyDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a currency.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a currency.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a currency that is locked.");
                }

                var lockCurrency = currency;
                lockCurrency.ManualCode = mstCurrencyDTO.ManualCode;
                lockCurrency.Currency = mstCurrencyDTO.Currency;
                lockCurrency.Remarks = mstCurrencyDTO.Remarks;
                lockCurrency.IsLocked = true;
                lockCurrency.UpdatedByUserId = loginUserId;
                lockCurrency.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockCurrency(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupCurrencyDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a currency.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a currency.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a currency that is unlocked.");
                }

                var unlockCurrency = currency;
                unlockCurrency.IsLocked = false;
                unlockCurrency.UpdatedByUserId = loginUserId;
                unlockCurrency.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCurrency(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupCurrencyList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a currency.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a currency.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a currency that is locked.");
                }

                _dbContext.MstCurrencies.Remove(currency);
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
