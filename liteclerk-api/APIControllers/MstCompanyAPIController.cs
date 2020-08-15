using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<DTO.MstCompanyDTO>>> CompanyList()
        {
            try
            {
                IEnumerable<DTO.MstCompanyDTO> companies = await _dbContext.MstCompanies.Select(d => new DTO.MstCompanyDTO
                {
                    Id = d.Id,
                    CompanyCode = d.CompanyCode,
                    ManualCode = d.ManualCode,
                    Company = d.Company,
                    Address = d.Address,
                    TIN = d.TIN,
                    CurrencyId = d.CurrencyId,
                    CostMethod = d.CostMethod,
                    IsLocked = d.IsLocked,
                    CreatedByUserFullname = d.CreatedByUser.Fullname,
                    CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                    UpdatedByUserFullname = d.UpdatedByUser.Fullname,
                    UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString(),
                }).ToListAsync();

                return StatusCode(200, companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTO.MstCompanyDTO>> CompanyDetail(int id)
        {
            try
            {
                DBSets.MstCompanyDBSet company = await _dbContext.MstCompanies.FindAsync(id);
                if (company == null)
                {
                    return StatusCode(404, new DTO.MstCompanyDTO());
                }

                DBSets.MstCompanyDBSet d = company;
                DTO.MstCompanyDTO companyDetail = new DTO.MstCompanyDTO
                {
                    Id = d.Id,
                    CompanyCode = d.CompanyCode,
                    ManualCode = d.ManualCode,
                    Company = d.Company,
                    Address = d.Address,
                    TIN = d.TIN,
                    CurrencyId = d.CurrencyId,
                    CostMethod = d.CostMethod,
                    IsLocked = d.IsLocked,
                    CreatedByUserFullname = d.CreatedByUser.Fullname,
                    CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                    UpdatedByUserFullname = d.UpdatedByUser.Fullname,
                    UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString(),
                };

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

                DBSets.MstCurrencyDBSet currency = await _dbContext.MstCurrencies.FirstOrDefaultAsync();
                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                String companyCode = "0000000001";
                var lastCompany = await _dbContext.MstCompanies.OrderByDescending(d => d.Id).FirstOrDefaultAsync();
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
                    CreatedByDateTime = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedByDateTime = DateTime.Now
                };

                _dbContext.MstCompanies.Add(newCompany);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, CompanyDetail(newCompany.Id));
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

                DBSets.MstCompanyDBSet company = await _dbContext.MstCompanies.FindAsync(id);
                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a company that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await _dbContext.MstCurrencies
                                              .Where(d => d.Id == mstCompanyDTO.CurrencyId && d.IsLocked == true)
                                              .FirstOrDefaultAsync();

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
                saveCompany.UpdatedByDateTime = DateTime.Now;

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

                DBSets.MstCompanyDBSet company = await _dbContext.MstCompanies.FindAsync(id);
                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a company that is already locked.");
                }

                DBSets.MstCurrencyDBSet currency = await _dbContext.MstCurrencies
                                              .Where(d => d.Id == mstCompanyDTO.CurrencyId && d.IsLocked == true)
                                              .FirstOrDefaultAsync();

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
                lockCompany.UpdatedByDateTime = DateTime.Now;

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

                DBSets.MstCompanyDBSet company = await _dbContext.MstCompanies.FindAsync(id);
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
                unlockCompany.UpdatedByDateTime = DateTime.Now;

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
                DBSets.MstCompanyDBSet company = await _dbContext.MstCompanies.FindAsync(id);
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
