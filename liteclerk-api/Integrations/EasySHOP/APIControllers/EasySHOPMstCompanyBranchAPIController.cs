using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using liteclerk_api.Integrations.EasySHOP.DTO;

namespace liteclerk_api.Integrations.EasySHOP.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasySHOPMstCompanyBranchAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPMstCompanyBranchAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/{companyId}")]
        public async Task<ActionResult> GetCompanyBranchList(Int32 companyId)
        {
            try
            {
                List<EasySHOPMstCompanyBranchDTO> companyBranches = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.CompanyId == companyId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select new EasySHOPMstCompanyBranchDTO
                    {
                        Id = d.Id,
                        ManualCode = d.ManualCode,
                        CompanyId = d.CompanyId,
                        Company = new EasySHOPMstCompanyDTO
                        {
                            ManualCode = d.MstCompany_CompanyId.ManualCode,
                            Company = d.MstCompany_CompanyId.Company
                        },
                        Branch = d.Branch
                    }
                ).ToListAsync();

                return StatusCode(200, companyBranches);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
