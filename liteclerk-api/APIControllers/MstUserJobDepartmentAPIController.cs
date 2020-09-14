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
    public class MstUserJobDepartmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstUserJobDepartmentAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{userId}")]
        public async Task<ActionResult> GetUserJobDepartmentList(Int32 userId)
        {
            try
            {
                IEnumerable<DTO.MstUserJobDepartmentDTO> userJobDepartments = await (
                    from d in _dbContext.MstUserJobDepartments
                    where d.UserId == userId
                    select new DTO.MstUserJobDepartmentDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, userJobDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byLoginUser")]
        public async Task<ActionResult> GetUserJobDepartmentListByLoginUser()
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.MstUserJobDepartmentDTO> userJobDepartments = await (
                    from d in _dbContext.MstUserJobDepartments
                    where d.UserId == loginUserId
                    select new DTO.MstUserJobDepartmentDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, userJobDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetUserJobDepartmentDetail(Int32 id)
        {
            try
            {
                DTO.MstUserJobDepartmentDTO userJobDepartment = await (
                    from d in _dbContext.MstUserJobDepartments
                    where d.Id == id
                    select new DTO.MstUserJobDepartmentDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment
                        }
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, userJobDepartment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddUserJobDepartment([FromBody] DTO.MstUserJobDepartmentDTO mstUserJobDepartmentDTO)
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
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a user job department.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a user job department.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == mstUserJobDepartmentDTO.UserId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a user job department if the current user is locked.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == mstUserJobDepartmentDTO.JobDepartmentId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstUserJobDepartmentDBSet newUserJobDepartment = new DBSets.MstUserJobDepartmentDBSet()
                {
                    UserId = mstUserJobDepartmentDTO.UserId,
                    JobDepartmentId = mstUserJobDepartmentDTO.JobDepartmentId
                };

                _dbContext.MstUserJobDepartments.Add(newUserJobDepartment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUserJobDepartment(int id, [FromBody] DTO.MstUserJobDepartmentDTO mstUserJobDepartmentDTO)
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
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a user job department.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a user job department.");
                }

                DBSets.MstUserJobDepartmentDBSet userJobDepartment = await (
                    from d in _dbContext.MstUserJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userJobDepartment == null)
                {
                    return StatusCode(404, "User job department not found.");
                }

                if (userJobDepartment.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a user job department if the current user is locked.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == mstUserJobDepartmentDTO.JobDepartmentId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstUserJobDepartmentDBSet updateUserJobDepartment = userJobDepartment;
                updateUserJobDepartment.JobDepartmentId = mstUserJobDepartmentDTO.JobDepartmentId;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUserJobDepartment(int id)
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
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a user job department.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a user job department.");
                }

                DBSets.MstUserJobDepartmentDBSet userJobDepartment = await (
                    from d in _dbContext.MstUserJobDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userJobDepartment == null)
                {
                    return StatusCode(404, "User job department not found.");
                }

                if (userJobDepartment.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a user job department if the current user is locked.");
                }

                _dbContext.MstUserJobDepartments.Remove(userJobDepartment);
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
