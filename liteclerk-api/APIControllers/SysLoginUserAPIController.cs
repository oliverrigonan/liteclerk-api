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
    public class SysLoginUserAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysLoginUserAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("detail")]
        public async Task<ActionResult> GetLoginUserDetail()
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                DTO.MstUserDTO user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select new DTO.MstUserDTO
                    {
                        Id = d.Id,
                        Username = d.Username,
                        Fullname = d.Fullname,
                        CompanyId = d.CompanyId,
                        Company = new DTO.MstCompanyDTO
                        {
                            ManualCode = d.MstCompany_CompanyId.ManualCode,
                            Company = d.MstCompany_CompanyId.Company
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        IsActive = d.IsActive
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("changeBranch/{branchId}")]
        public async Task<ActionResult> ChangeBranch(Int32 branchId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == branchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserDBSet changeBranch = loginUser;
                changeBranch.BranchId = branchId;

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
