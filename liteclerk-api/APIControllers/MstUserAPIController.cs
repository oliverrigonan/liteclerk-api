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
    public class MstUserAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstUserAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstUserDTO>>> GetUserList()
        {
            try
            {
                IEnumerable<DTO.MstUserDTO> users = await _dbContext.MstUsers
                    .Select(d =>
                        new DTO.MstUserDTO
                        {
                            Id = d.Id,
                            Username = d.Username,
                            Fullname = d.Fullname,
                            CompanyId = d.CompanyId,
                            Company = d.MstCompany_Company.Company,
                            BranchId = d.BranchId,
                            Branch = d.MstCompanyBranch_Branch.Branch,
                            IsActive = d.IsActive
                        })
                    .ToListAsync();

                return StatusCode(200, users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("active/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstUserDTO>>> GetActiveUserList()
        {
            try
            {
                IEnumerable<DTO.MstUserDTO> activeUsers = await _dbContext.MstUsers
                    .Where(d => d.IsActive == true)
                    .Select(d =>
                        new DTO.MstUserDTO
                        {
                            Id = d.Id,
                            Username = d.Username,
                            Fullname = d.Fullname,
                            CompanyId = d.CompanyId,
                            Company = d.MstCompany_Company.Company,
                            BranchId = d.BranchId,
                            Branch = d.MstCompanyBranch_Branch.Branch,
                            IsActive = d.IsActive
                        })
                    .ToListAsync();

                return StatusCode(200, activeUsers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
