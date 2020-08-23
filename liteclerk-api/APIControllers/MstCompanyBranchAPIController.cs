using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MstCompanyBranchAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstCompanyBranchAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetCompanyBranchList()
        {
            try
            {
                IEnumerable<DTO.MstCompanyBranchDTO> companyBranches = await (
                    from d in _dbContext.MstCompanyBranches
                    select new DTO.MstCompanyBranchDTO
                    {
                        Id = d.Id,
                        BranchCode = d.BranchCode,
                        ManualCode = d.ManualCode,
                        CompanyId = d.CompanyId,
                        Branch = d.Branch,
                        Address = d.Address,
                        TIN = d.TIN
                    }
                ).ToListAsync();

                return StatusCode(200, companyBranches);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCompanyBranch([FromBody] DTO.MstCompanyBranchDTO mstCompanyBranchDTO)
        {
            try
            {
                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == mstCompanyBranchDTO.CompanyId
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a branch if the current company is locked.");
                }

                String branchCode = "0000000001";
                DBSets.MstCompanyBranchDBSet lastCompanyBranch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.CompanyId == mstCompanyBranchDTO.CompanyId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastCompanyBranch != null)
                {
                    Int32 lastBranchCode = Convert.ToInt32(lastCompanyBranch.BranchCode) + 0000000001;
                    branchCode = PadZeroes(lastBranchCode, 10);
                }

                DBSets.MstCompanyBranchDBSet newCompanyBranch = new DBSets.MstCompanyBranchDBSet()
                {
                    BranchCode = branchCode,
                    ManualCode = mstCompanyBranchDTO.ManualCode,
                    CompanyId = mstCompanyBranchDTO.CompanyId,
                    Branch = mstCompanyBranchDTO.Branch,
                    Address = mstCompanyBranchDTO.Address,
                    TIN = mstCompanyBranchDTO.TIN,
                };

                _dbContext.MstCompanyBranches.Add(newCompanyBranch);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCompanyBranch(int id, [FromBody] DTO.MstCompanyBranchDTO mstCompanyBranchDTO)
        {
            try
            {
                DBSets.MstCompanyBranchDBSet companyBranch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (companyBranch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                if (companyBranch.MstCompany_Company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a branch if the current company is locked.");
                }

                DBSets.MstCompanyBranchDBSet updateCompanyBranch = companyBranch;
                updateCompanyBranch.ManualCode = mstCompanyBranchDTO.ManualCode;
                updateCompanyBranch.Branch = mstCompanyBranchDTO.Branch;
                updateCompanyBranch.Address = mstCompanyBranchDTO.Address;
                updateCompanyBranch.TIN = mstCompanyBranchDTO.TIN;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCompanyBranch(int id)
        {
            try
            {
                DBSets.MstCompanyBranchDBSet companyBranch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (companyBranch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                if (companyBranch.MstCompany_Company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a branch if the current company is locked.");
                }

                _dbContext.MstCompanyBranches.Remove(companyBranch);
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
