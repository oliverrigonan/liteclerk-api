using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MstJobTypeInformationAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeInformationAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{jobTypeId}")]
        public async Task<ActionResult> GetJobTypeInformationList(Int32 jobTypeId)
        {
            try
            {
                IEnumerable<DTO.MstJobTypeInformationDTO> jobTypeInformations = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.JobTypeId == jobTypeId
                    select new DTO.MstJobTypeInformationDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
                        },
                        InformationCode = d.InformationCode,
                        InformationGroup = d.InformationGroup,
                        IsPrinted = d.IsPrinted
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypeInformations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
