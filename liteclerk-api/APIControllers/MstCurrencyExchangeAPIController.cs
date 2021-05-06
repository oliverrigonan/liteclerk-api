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
    public class MstCurrencyExchangeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstCurrencyExchangeAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpGet("list/byBaseCurrency")]
        public async Task<ActionResult> GetCurrencyExchangeByBaseCurrencyList()
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

                var currencyExchanges = await (
                    from d in _dbContext.MstCurrencyExchanges
                    where d.CurrencyId == loginUser.MstCompany_CompanyId.CurrencyId
                    && d.MstCurrency_CurrencyId.IsLocked == true
                    select new DTO.MstCurrencyExchangeDTO
                    {
                        Id = d.Id,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_ExchangeCurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeDate = d.ExchangeDate.ToShortDateString(),
                        ExchangeRate = d.ExchangeRate
                    }
                ).ToListAsync();

                return StatusCode(200, currencyExchanges);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/{currencyId}")]
        public async Task<ActionResult> GetCurrencyExchangeList(Int32 currencyId)
        {
            try
            {
                var currencyExchanges = await (
                    from d in _dbContext.MstCurrencyExchanges
                    where d.CurrencyId == currencyId
                    select new DTO.MstCurrencyExchangeDTO
                    {
                        Id = d.Id,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_ExchangeCurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeDate = d.ExchangeDate.ToShortDateString(),
                        ExchangeRate = d.ExchangeRate
                    }
                ).ToListAsync();

                return StatusCode(200, currencyExchanges);
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
                var currencyExchange = await (
                    from d in _dbContext.MstCurrencyExchanges
                    where d.Id == id
                    select new DTO.MstCurrencyExchangeDTO
                    {
                        Id = d.Id,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_ExchangeCurrencyId.CurrencyCode,
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeDate = d.ExchangeDate.ToShortDateString(),
                        ExchangeRate = d.ExchangeRate
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, currencyExchange);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCurrencyExchange([FromBody] DTO.MstCurrencyExchangeDTO mstCurrencyExchangeDTO)
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
                    return StatusCode(404, "No rights to add a currency exchange.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a currency exchange.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCurrencyExchangeDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a currency exchange if the current currency is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCurrencyExchangeDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var newCurrencyExchange = new DBSets.MstCurrencyExchangeDBSet()
                {
                    CurrencyId = mstCurrencyExchangeDTO.CurrencyId,
                    ExchangeCurrencyId = mstCurrencyExchangeDTO.ExchangeCurrencyId,
                    ExchangeDate = DateTime.Today,
                    ExchangeRate = mstCurrencyExchangeDTO.ExchangeRate
                };

                _dbContext.MstCurrencyExchanges.Add(newCurrencyExchange);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newCurrencyExchange.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCurrencyExchange(Int32 id, [FromBody] DTO.MstCurrencyExchangeDTO mstCurrencyExchangeDTO)
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
                    return StatusCode(404, "No rights to edit or save a currency exchange.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a currency exchange.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCurrencyExchangeDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (currency.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a currency exchange if the current currency is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCurrencyExchangeDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var currencyExchange = await (
                    from d in _dbContext.MstCurrencyExchanges
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (currencyExchange == null)
                {
                    return StatusCode(404, "Currency exchange not found.");
                }

                var saveCurrencyExchange = currencyExchange;
                saveCurrencyExchange.CurrencyId = mstCurrencyExchangeDTO.CurrencyId;
                saveCurrencyExchange.ExchangeCurrencyId = mstCurrencyExchangeDTO.ExchangeCurrencyId;
                saveCurrencyExchange.ExchangeDate = Convert.ToDateTime(mstCurrencyExchangeDTO.ExchangeDate);
                saveCurrencyExchange.ExchangeRate = mstCurrencyExchangeDTO.ExchangeRate;

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
                    && d.SysForm_FormId.Form == "SetupCurrencyDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a currency exchange.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a currency exchange.");
                }

                var currencyExchange = await (
                    from d in _dbContext.MstCurrencyExchanges
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (currencyExchange == null)
                {
                    return StatusCode(404, "Currency exchange not found.");
                }

                if (currencyExchange.MstCurrency_CurrencyId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a currency exchange if the current currency is locked.");
                }

                _dbContext.MstCurrencyExchanges.Remove(currencyExchange);
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