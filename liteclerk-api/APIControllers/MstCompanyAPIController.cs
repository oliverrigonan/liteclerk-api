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
    public class MstCompanyAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstCompanyAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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
        public async Task<ActionResult<IEnumerable<DTO.MstCompanyDTO>>> GetCompanyList()
        {
            try
            {
                IEnumerable<DTO.MstCompanyDTO> companies = await (
                    from d in _dbContext.MstCompanies
                    select new DTO.MstCompanyDTO
                    {
                        Id = d.Id,
                        CompanyCode = d.CompanyCode,
                        ManualCode = d.ManualCode,
                        Company = d.Company,
                        Address = d.Address,
                        TIN = d.TIN,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        CostMethod = d.CostMethod,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTO.MstCompanyDTO>> GetCompanyDetail(int id)
        {
            try
            {
                DTO.MstCompanyDTO companyDetail = await (
                     from d in _dbContext.MstCompanies
                     where d.Id == id
                     select new DTO.MstCompanyDTO
                     {
                         Id = d.Id,
                         CompanyCode = d.CompanyCode,
                         ManualCode = d.ManualCode,
                         Company = d.Company,
                         Address = d.Address,
                         TIN = d.TIN,
                         CurrencyId = d.CurrencyId,
                         Currency = new DTO.MstCurrencyDTO
                         {
                             CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                             ManualCode = d.MstCurrency_Currency.ManualCode,
                             Currency = d.MstCurrency_Currency.Currency
                         },
                         CostMethod = d.CostMethod,
                         IsLocked = d.IsLocked,
                         CreatedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_CreatedByUser.Username,
                             Fullname = d.MstUser_CreatedByUser.Fullname
                         },
                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                         UpdatedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_UpdatedByUser.Username,
                             Fullname = d.MstUser_UpdatedByUser.Fullname
                         },
                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                     }
                ).FirstOrDefaultAsync();

                return StatusCode(200, companyDetail);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<DTO.MstCompanyDTO>> AddCompany()
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                String companyCode = "0000000001";
                DBSets.MstCompanyDBSet lastCompany = await (
                    from d in _dbContext.MstCompanies
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastCompany != null)
                {
                    Int32 lastCompanyCode = Convert.ToInt32(lastCompany.CompanyCode) + 0000000001;
                    companyCode = PadZeroes(lastCompanyCode, 10);
                }

                DBSets.MstCompanyDBSet newCompany = new DBSets.MstCompanyDBSet()
                {
                    CompanyCode = companyCode,
                    ManualCode = companyCode,
                    Company = "",
                    Address = "",
                    TIN = "",
                    CurrencyId = currency.Id,
                    CostMethod = "Last Purchase Cost",
                    IsLocked = false,
                    CreatedByUserId = userId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstCompanies.Add(newCompany);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, GetCompanyDetail(newCompany.Id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<IActionResult> SaveCompany(int id, [FromBody] DTO.MstCompanyDTO mstCompanyDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a company that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCompanyDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstCompanyDBSet saveCompany = company;
                saveCompany.ManualCode = mstCompanyDTO.ManualCode;
                saveCompany.Company = mstCompanyDTO.Company;
                saveCompany.Address = mstCompanyDTO.Address;
                saveCompany.TIN = mstCompanyDTO.TIN;
                saveCompany.CurrencyId = mstCompanyDTO.CurrencyId;
                saveCompany.CostMethod = mstCompanyDTO.CostMethod;
                saveCompany.UpdatedByUserId = userId;
                saveCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockCompany(int id, [FromBody] DTO.MstCompanyDTO mstCompanyDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a company that is already locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCompanyDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstCompanyDBSet lockCompany = company;
                lockCompany.ManualCode = mstCompanyDTO.ManualCode;
                lockCompany.Company = mstCompanyDTO.Company;
                lockCompany.Address = mstCompanyDTO.Address;
                lockCompany.TIN = mstCompanyDTO.TIN;
                lockCompany.CurrencyId = mstCompanyDTO.CurrencyId;
                lockCompany.CostMethod = mstCompanyDTO.CostMethod;
                lockCompany.IsLocked = true;
                lockCompany.UpdatedByUserId = userId;
                lockCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnlockCompany(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a company that is already unlocked.");
                }

                DBSets.MstCompanyDBSet unlockCompany = company;
                unlockCompany.IsLocked = false;
                unlockCompany.UpdatedByUserId = userId;
                unlockCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a company that is locked.");
                }

                _dbContext.MstCompanies.Remove(company);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
