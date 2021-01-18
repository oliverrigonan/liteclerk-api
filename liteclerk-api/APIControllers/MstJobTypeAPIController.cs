using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [NonAction]
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
        public async Task<ActionResult> GetJobTypeList()
        {
            try
            {
                var jobTypes = await (
                    from d in _dbContext.MstJobTypes
                    select new DTO.MstJobTypeDTO
                    {
                        Id = d.Id,
                        JobTypeCode = d.JobTypeCode,
                        ManualCode = d.ManualCode,
                        JobType = d.JobType,
                        TotalNumberOfDays = d.TotalNumberOfDays,
                        Remarks = d.Remarks,
                        IsInventory = d.IsInventory,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedJobTypeList()
        {
            try
            {
                var lockedJobTypes = await (
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
                        IsInventory = d.IsInventory,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
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

        [HttpGet("list/byIsInventory/{isInventory}")]
        public async Task<ActionResult> GetJobTypeListByIsInventory(Boolean isInventory)
        {
            try
            {
                var jobTypes = await (
                    from d in _dbContext.MstJobTypes
                    where d.IsInventory == isInventory
                    select new DTO.MstJobTypeDTO
                    {
                        Id = d.Id,
                        JobTypeCode = d.JobTypeCode,
                        ManualCode = d.ManualCode,
                        JobType = d.JobType,
                        TotalNumberOfDays = d.TotalNumberOfDays,
                        Remarks = d.Remarks,
                        IsInventory = d.IsInventory,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }


        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobTypeDetail(Int32 id)
        {
            try
            {
                var producedJobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == id
                    select new DTO.MstJobTypeDTO
                    {
                        Id = d.Id,
                        JobTypeCode = d.JobTypeCode,
                        ManualCode = d.ManualCode,
                        JobType = d.JobType,
                        TotalNumberOfDays = d.TotalNumberOfDays,
                        Remarks = d.Remarks,
                        IsInventory = d.IsInventory,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, producedJobType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobType()
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobTypeList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job type.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job type.");
                }

                String jobTypeCode = "0000000001";
                var lastJobType = await (
                    from d in _dbContext.MstJobTypes
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJobType != null)
                {
                    Int32 lastJobTypeCode = Convert.ToInt32(lastJobType.JobTypeCode) + 0000000001;
                    jobTypeCode = PadZeroes(lastJobTypeCode, 10);
                }

                var newJobType = new DBSets.MstJobTypeDBSet()
                {
                    JobTypeCode = jobTypeCode,
                    ManualCode = jobTypeCode,
                    JobType = "",
                    TotalNumberOfDays = 0,
                    Remarks = "",
                    IsInventory = true,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstJobTypes.Add(newJobType);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJobType.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJobType(Int32 id, [FromBody] DTO.MstJobTypeDTO mstJobTypeDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a job type.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a job type.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (jobType.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a job type that is locked.");
                }

                var saveJobType = jobType;
                saveJobType.ManualCode = mstJobTypeDTO.ManualCode;
                saveJobType.JobType = mstJobTypeDTO.JobType;
                saveJobType.TotalNumberOfDays = mstJobTypeDTO.TotalNumberOfDays;
                saveJobType.Remarks = mstJobTypeDTO.Remarks;
                saveJobType.IsInventory = mstJobTypeDTO.IsInventory;
                saveJobType.UpdatedByUserId = loginUserId;
                saveJobType.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockJobType(Int32 id, [FromBody] DTO.MstJobTypeDTO mstJobTypeDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a job type.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a job type.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (jobType.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a job type that is locked.");
                }

                var lockJobType = jobType;
                lockJobType.ManualCode = mstJobTypeDTO.ManualCode;
                lockJobType.JobType = mstJobTypeDTO.JobType;
                lockJobType.TotalNumberOfDays = mstJobTypeDTO.TotalNumberOfDays;
                lockJobType.Remarks = mstJobTypeDTO.Remarks;
                lockJobType.IsInventory = mstJobTypeDTO.IsInventory;
                lockJobType.IsLocked = true;
                lockJobType.UpdatedByUserId = loginUserId;
                lockJobType.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockJobType(Int32 id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a job type.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a job type.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (jobType.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a job type that is unlocked.");
                }

                var unlockJobType = jobType;
                unlockJobType.IsLocked = false;
                unlockJobType.UpdatedByUserId = loginUserId;
                unlockJobType.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobType(Int32 id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobTypeList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job type.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job type.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (jobType.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job type that is locked.");
                }

                _dbContext.MstJobTypes.Remove(jobType);
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
