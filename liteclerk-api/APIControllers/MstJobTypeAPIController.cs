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
    public class MstJobTypeAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstJobTypeDTO>>> GetJobTypeList()
        {
            try
            {
                IEnumerable<DTO.MstJobTypeDTO> jobTypes = await (
                    from d in _dbContext.MstJobTypes
                    select new DTO.MstJobTypeDTO
                    {
                        Id = d.Id,
                        JobTypeCode = d.JobTypeCode,
                        ManualCode = d.ManualCode,
                        JobType = d.JobType,
                        TotalNumberOfDays = d.TotalNumberOfDays,
                        Remarks = d.Remarks,
                        IsLocked = d.IsLocked,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstJobTypeDTO>>> GetLockedJobTypeList()
        {
            try
            {
                IEnumerable<DTO.MstJobTypeDTO> lockedJobTypes = await (
                    from d in _dbContext.MstJobTypes
                    where d.IsLocked == true
                    select new DTO.MstJobTypeDTO
                    {
                        Id = d.Id,
                        JobTypeCode = d.JobTypeCode,
                        ManualCode = d.ManualCode,
                        JobType = d.JobType,
                        TotalNumberOfDays = d.TotalNumberOfDays,
                        Remarks = d.Remarks,
                        IsLocked = d.IsLocked,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, lockedJobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
