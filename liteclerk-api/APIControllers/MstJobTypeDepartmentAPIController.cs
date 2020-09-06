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
    public class MstJobTypeDepartmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeDepartmentAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{jobTypeId}")]
        public async Task<ActionResult> GetJobTypeDepartmentList(Int32 jobTypeId)
        {
            try
            {
                IEnumerable<DTO.MstJobTypeDepartmentDTO> jobTypeDepartments = await (
                    from d in _dbContext.MstJobTypeDepartments
                    where d.JobTypeId == jobTypeId
                    select new DTO.MstJobTypeDepartmentDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
                        },
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment
                        },
                        NumberOfDays = d.NumberOfDays
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypeDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
