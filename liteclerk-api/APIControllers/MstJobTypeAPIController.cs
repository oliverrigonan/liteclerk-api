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
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetJobTypeList()
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

                return StatusCode(200, jobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("locked/list")]
        public async Task<ActionResult> GetLockedJobTypeList()
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

                return StatusCode(200, lockedJobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
