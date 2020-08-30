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
    public class MstJobTypeAttachmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeAttachmentAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{jobTypeId}")]
        public async Task<ActionResult> GetJobTypeAttachmentList(Int32 jobTypeId)
        {
            try
            {
                IEnumerable<DTO.MstJobTypeAttachmentDTO> jobTypeAttachments = await (
                    from d in _dbContext.MstJobTypeAttachments
                    where d.JobTypeId == jobTypeId
                    select new DTO.MstJobTypeAttachmentDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobType.JobTypeCode,
                            ManualCode = d.MstJobType_JobType.ManualCode,
                            JobType = d.MstJobType_JobType.JobType
                        },
                        AttachmentCode = d.AttachmentCode,
                        AttachmentType = d.AttachmentType,
                        IsPrinted = d.IsPrinted
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypeAttachments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
