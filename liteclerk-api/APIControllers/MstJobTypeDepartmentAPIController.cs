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

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobTypeDepartmentDetail(Int32 id)
        {
            try
            {
                DTO.MstJobTypeDepartmentDTO jobTypeDepartment = await (
                    from d in _dbContext.MstJobTypeDepartments
                    where d.Id == id
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
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobTypeDepartment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobTypeDepartment([FromBody] DTO.MstJobTypeDepartmentDTO mstJobTypeDepartmentDTO)
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
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job type department.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job type department.");
                }

                DBSets.MstJobTypeDBSet article = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == mstJobTypeDepartmentDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a job type department if the current job type is locked.");
                }

                DBSets.MstJobTypeDepartmentDBSet jobTypeDepartment = await (
                    from d in _dbContext.MstJobTypeDepartments
                    where d.JobTypeId == mstJobTypeDepartmentDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobTypeDepartment == null)
                {
                    return StatusCode(404, "Job type department not found.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == mstJobTypeDepartmentDTO.JobDepartmentId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstJobTypeDepartmentDBSet newJobTypeDepartment = new DBSets.MstJobTypeDepartmentDBSet()
                {
                    JobTypeId = mstJobTypeDepartmentDTO.JobTypeId,
                    JobDepartmentId = mstJobTypeDepartmentDTO.JobDepartmentId,
                    NumberOfDays = mstJobTypeDepartmentDTO.NumberOfDays
                };

                _dbContext.MstJobTypeDepartments.Add(newJobTypeDepartment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobTypeDepartment(int id, [FromBody] DTO.MstJobTypeDepartmentDTO mstJobTypeDepartmentDTO)
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
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a job type department.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job type department.");
                }

                DBSets.MstJobTypeDepartmentDBSet jobTypeDepartment = await (
                    from d in _dbContext.MstJobTypeDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeDepartment == null)
                {
                    return StatusCode(404, "Job type department not found.");
                }

                if (jobTypeDepartment.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a job type department if the current item is locked.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == mstJobTypeDepartmentDTO.JobDepartmentId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstJobTypeDepartmentDBSet updateJobTypeDepartment = jobTypeDepartment;
                updateJobTypeDepartment.JobDepartmentId = mstJobTypeDepartmentDTO.JobDepartmentId;
                updateJobTypeDepartment.NumberOfDays = mstJobTypeDepartmentDTO.NumberOfDays;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobTypeDepartment(int id)
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
                    && d.SysForm_FormId.Form == "SetupJobTypeDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job type department.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job type department.");
                }

                DBSets.MstJobTypeDepartmentDBSet jobTypeDepartment = await (
                    from d in _dbContext.MstJobTypeDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeDepartment == null)
                {
                    return StatusCode(404, "Job type department not found.");
                }

                if (jobTypeDepartment.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job type department if the current item is locked.");
                }

                _dbContext.MstJobTypeDepartments.Remove(jobTypeDepartment);
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
