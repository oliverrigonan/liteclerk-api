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
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
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

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobTypeAttachmentDetail(Int32 id)
        {
            try
            {
                DTO.MstJobTypeAttachmentDTO jobTypeAttachment = await (
                    from d in _dbContext.MstJobTypeAttachments
                    where d.Id == id
                    select new DTO.MstJobTypeAttachmentDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
                        },
                        AttachmentCode = d.AttachmentCode,
                        AttachmentType = d.AttachmentType,
                        IsPrinted = d.IsPrinted
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobTypeAttachment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobTypeAttachment([FromBody] DTO.MstJobTypeAttachmentDTO mstJobTypeAttachmentDTO)
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
                    return StatusCode(404, "No rights to add a job type attachment.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job type attachment.");
                }

                DBSets.MstJobTypeDBSet article = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == mstJobTypeAttachmentDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a job type attachment if the current job type is locked.");
                }

                DBSets.MstJobTypeAttachmentDBSet jobTypeAttachment = await (
                    from d in _dbContext.MstJobTypeAttachments
                    where d.JobTypeId == mstJobTypeAttachmentDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobTypeAttachment == null)
                {
                    return StatusCode(404, "Job type attachment not found.");
                }

                DBSets.MstJobTypeAttachmentDBSet newJobTypeAttachment = new DBSets.MstJobTypeAttachmentDBSet()
                {
                    JobTypeId = mstJobTypeAttachmentDTO.JobTypeId,
                    AttachmentCode = mstJobTypeAttachmentDTO.AttachmentCode,
                    AttachmentType = mstJobTypeAttachmentDTO.AttachmentType,
                    IsPrinted = mstJobTypeAttachmentDTO.IsPrinted
                };

                _dbContext.MstJobTypeAttachments.Add(newJobTypeAttachment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobTypeAttachment(int id, [FromBody] DTO.MstJobTypeAttachmentDTO mstJobTypeAttachmentDTO)
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
                    return StatusCode(404, "No rights to edit or update a job type attachment.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job type attachment.");
                }

                DBSets.MstJobTypeAttachmentDBSet jobTypeAttachment = await (
                    from d in _dbContext.MstJobTypeAttachments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeAttachment == null)
                {
                    return StatusCode(404, "Job type attachment not found.");
                }

                if (jobTypeAttachment.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a job type attachment if the current item is locked.");
                }

                DBSets.MstJobTypeAttachmentDBSet updateJobTypeAttachment = jobTypeAttachment;
                updateJobTypeAttachment.AttachmentCode = mstJobTypeAttachmentDTO.AttachmentCode;
                updateJobTypeAttachment.AttachmentType = mstJobTypeAttachmentDTO.AttachmentType;
                updateJobTypeAttachment.IsPrinted = mstJobTypeAttachmentDTO.IsPrinted;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobTypeAttachment(int id)
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
                    return StatusCode(404, "No rights to delete a job type attachment.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job type attachment.");
                }

                DBSets.MstJobTypeAttachmentDBSet jobTypeAttachment = await (
                    from d in _dbContext.MstJobTypeAttachments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeAttachment == null)
                {
                    return StatusCode(404, "Job type attachment not found.");
                }

                if (jobTypeAttachment.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job type attachment if the current item is locked.");
                }

                _dbContext.MstJobTypeAttachments.Remove(jobTypeAttachment);
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
