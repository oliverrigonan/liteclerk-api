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
    public class MstJobDepartmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobDepartmentAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetJobDepartmentList()
        {
            try
            {
                IEnumerable<DTO.MstJobDepartmentDTO> jobDepartments = await (
                    from d in _dbContext.MstJobDepartments
                    select new DTO.MstJobDepartmentDTO
                    {
                        Id = d.Id,
                        JobDepartmentCode = d.JobDepartmentCode,
                        ManualCode = d.ManualCode,
                        JobDepartment = d.JobDepartment,
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

                return StatusCode(200, jobDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedJobDepartmentList()
        {
            try
            {
                IEnumerable<DTO.MstJobDepartmentDTO> lockedJobDepartments = await (
                    from d in _dbContext.MstJobDepartments
                    where d.IsLocked == true
                    select new DTO.MstJobDepartmentDTO
                    {
                        Id = d.Id,
                        JobDepartmentCode = d.JobDepartmentCode,
                        ManualCode = d.ManualCode,
                        JobDepartment = d.JobDepartment,
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

                return StatusCode(200, lockedJobDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobDepartmentDetail(Int32 id)
        {
            try
            {
                DTO.MstJobDepartmentDTO producedJobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == id
                    select new DTO.MstJobDepartmentDTO
                    {
                        Id = d.Id,
                        JobDepartmentCode = d.JobDepartmentCode,
                        ManualCode = d.ManualCode,
                        JobDepartment = d.JobDepartment,
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
                ).FirstOrDefaultAsync();

                return StatusCode(200, producedJobDepartment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobDepartment()
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

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobDepartmentList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job department.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job department.");
                }

                String jobDepartmentCode = "0000000001";
                DBSets.MstJobDepartmentDBset lastJobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJobDepartment != null)
                {
                    Int32 lastJobDepartmentCode = Convert.ToInt32(lastJobDepartment.JobDepartmentCode) + 0000000001;
                    jobDepartmentCode = PadZeroes(lastJobDepartmentCode, 10);
                }

                DBSets.MstJobDepartmentDBset newJobDepartment = new DBSets.MstJobDepartmentDBset()
                {
                    JobDepartmentCode = jobDepartmentCode,
                    ManualCode = jobDepartmentCode,
                    JobDepartment = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstJobDepartments.Add(newJobDepartment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJobDepartment.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJobDepartment(Int32 id, [FromBody] DTO.MstJobDepartmentDTO mstJobDepartmentDTO)
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

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobDepartmentDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a job department.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a job department.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                if (jobDepartment.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a job department that is locked.");
                }

                DBSets.MstJobDepartmentDBset saveJobDepartment = jobDepartment;
                saveJobDepartment.ManualCode = mstJobDepartmentDTO.ManualCode;
                saveJobDepartment.JobDepartment = mstJobDepartmentDTO.JobDepartment;
                saveJobDepartment.UpdatedByUserId = loginUserId;
                saveJobDepartment.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockJobDepartment(Int32 id, [FromBody] DTO.MstJobDepartmentDTO mstJobDepartmentDTO)
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

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobDepartmentDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a job department.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a job department.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                if (jobDepartment.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a job department that is locked.");
                }

                DBSets.MstJobDepartmentDBset lockJobDepartment = jobDepartment;
                lockJobDepartment.ManualCode = mstJobDepartmentDTO.ManualCode;
                lockJobDepartment.JobDepartment = mstJobDepartmentDTO.JobDepartment;
                lockJobDepartment.IsLocked = true;
                lockJobDepartment.UpdatedByUserId = loginUserId;
                lockJobDepartment.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockJobDepartment(Int32 id)
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

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobDepartmentDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a job department.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a job department.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                if (jobDepartment.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a job department that is unlocked.");
                }

                DBSets.MstJobDepartmentDBset unlockJobDepartment = jobDepartment;
                unlockJobDepartment.IsLocked = false;
                unlockJobDepartment.UpdatedByUserId = loginUserId;
                unlockJobDepartment.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobDepartment(Int32 id)
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

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupJobDepartmentList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job department.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job department.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "JobDepartment not found.");
                }

                if (jobDepartment.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job department that is locked.");
                }

                _dbContext.MstJobDepartments.Remove(jobDepartment);
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
