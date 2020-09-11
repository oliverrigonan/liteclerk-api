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
    public class TrnJobOrderDepartmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnJobOrderDepartmentAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{JOId}")]
        public async Task<ActionResult> GetJobOrderDepartmentListByJobOrder(Int32 JOId)
        {
            try
            {
                IEnumerable<DTO.TrnJobOrderDepartmentDTO> jobOrderDepartments = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.JOId == JOId
                    select new DTO.TrnJobOrderDepartmentDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment,
                        },
                        Particulars = d.Particulars,
                        Status = d.Status,
                        StatusByUserId = d.StatusByUserId,
                        StatusByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_StatusByUserId.Username,
                            Fullname = d.MstUser_StatusByUserId.Fullname
                        },
                        AssignedToUserId = d.AssignedToUserId,
                        AssignedToUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_AssignedToUserId.Username,
                            Fullname = d.MstUser_AssignedToUserId.Fullname
                        },
                        StatusUpdatedDateTime = d.StatusUpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrderDepartments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobOrderDepartmentDetail(Int32 id)
        {
            try
            {
                DTO.TrnJobOrderDepartmentDTO jobOrderDepartment = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.Id == id
                    select new DTO.TrnJobOrderDepartmentDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        JobDepartmentId = d.JobDepartmentId,
                        JobDepartment = new DTO.MstJobDepartmentDTO
                        {
                            JobDepartmentCode = d.MstJobDepartment_JobDepartmentId.JobDepartmentCode,
                            ManualCode = d.MstJobDepartment_JobDepartmentId.ManualCode,
                            JobDepartment = d.MstJobDepartment_JobDepartmentId.JobDepartment,
                        },
                        Particulars = d.Particulars,
                        Status = d.Status,
                        StatusByUserId = d.StatusByUserId,
                        StatusByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_StatusByUserId.Username,
                            Fullname = d.MstUser_StatusByUserId.Fullname
                        },
                        AssignedToUserId = d.AssignedToUserId,
                        AssignedToUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_AssignedToUserId.Username,
                            Fullname = d.MstUser_AssignedToUserId.Fullname
                        },
                        StatusUpdatedDateTime = d.StatusUpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobOrderDepartment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrderDepartment([FromBody] DTO.TrnJobOrderDepartmentDTO trnJobOrderDepartmentDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to add a job order department.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job order department.");
                }

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderDepartmentDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add job order departments if the current job order is locked.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == trnJobOrderDepartmentDTO.JobDepartmentId
                    select d
                ).FirstOrDefaultAsync();

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstUserDBSet statusByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDepartmentDTO.StatusByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (statusByUser == null)
                {
                    return StatusCode(404, "Status by user not found.");
                }

                DBSets.MstUserDBSet assignedToUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDepartmentDTO.AssignedToUserId
                    select d
                ).FirstOrDefaultAsync();

                if (assignedToUser == null)
                {
                    return StatusCode(404, "Assigned to user not found.");
                }

                DBSets.TrnJobOrderDepartmentDBSet newJobOrderDepartment = new DBSets.TrnJobOrderDepartmentDBSet()
                {
                    JOId = trnJobOrderDepartmentDTO.JOId,
                    JobDepartmentId = trnJobOrderDepartmentDTO.JobDepartmentId,
                    Particulars = trnJobOrderDepartmentDTO.Particulars,
                    Status = trnJobOrderDepartmentDTO.Status,
                    StatusByUserId = trnJobOrderDepartmentDTO.StatusByUserId,
                    StatusUpdatedDateTime = DateTime.Now,
                    AssignedToUserId = trnJobOrderDepartmentDTO.AssignedToUserId
                };

                _dbContext.TrnJobOrderDepartments.Add(newJobOrderDepartment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobOrderDepartment(Int32 id, [FromBody] DTO.TrnJobOrderDepartmentDTO trnJobOrderDepartmentDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a job order department.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job order department.");
                }

                DBSets.TrnJobOrderDepartmentDBSet jobOrderDepartment = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderDepartment == null)
                {
                    return StatusCode(404, "Job order attachment not found.");
                }

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderDepartmentDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update job order departments if the current job order is locked.");
                }

                DBSets.MstJobDepartmentDBset jobDepartment = await (
                    from d in _dbContext.MstJobDepartments
                    where d.Id == trnJobOrderDepartmentDTO.JobDepartmentId
                    select d
                ).FirstOrDefaultAsync();

                if (jobDepartment == null)
                {
                    return StatusCode(404, "Job department not found.");
                }

                DBSets.MstUserDBSet statusByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDepartmentDTO.StatusByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (statusByUser == null)
                {
                    return StatusCode(404, "Status by user not found.");
                }

                DBSets.MstUserDBSet assignedToUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDepartmentDTO.AssignedToUserId
                    select d
                ).FirstOrDefaultAsync();

                if (assignedToUser == null)
                {
                    return StatusCode(404, "Assigned to user not found.");
                }

                DBSets.TrnJobOrderDepartmentDBSet updateJobOrderDepartments = jobOrderDepartment;
                updateJobOrderDepartments.JOId = trnJobOrderDepartmentDTO.JOId;
                updateJobOrderDepartments.JobDepartmentId = trnJobOrderDepartmentDTO.JobDepartmentId;
                updateJobOrderDepartments.Particulars = trnJobOrderDepartmentDTO.Particulars;
                updateJobOrderDepartments.Status = trnJobOrderDepartmentDTO.Status;
                updateJobOrderDepartments.StatusByUserId = trnJobOrderDepartmentDTO.StatusByUserId;
                updateJobOrderDepartments.AssignedToUserId = trnJobOrderDepartmentDTO.AssignedToUserId;
                updateJobOrderDepartments.StatusUpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobOrderDepartment(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to delete a job order department.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job order department.");
                }

                DBSets.TrnJobOrderDepartmentDBSet jobOrderDepartment = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderDepartment == null)
                {
                    return StatusCode(404, "Job order department not found.");
                }

                if (jobOrderDepartment.TrnJobOrder_JOId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete job order departments if the current job order is locked.");
                }

                _dbContext.TrnJobOrderDepartments.Remove(jobOrderDepartment);
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
