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
    public class MstJobTypeInformationAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstJobTypeInformationAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{jobTypeId}")]
        public async Task<ActionResult> GetJobTypeInformationList(Int32 jobTypeId)
        {
            try
            {
                IEnumerable<DTO.MstJobTypeInformationDTO> jobTypeInformations = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.JobTypeId == jobTypeId
                    select new DTO.MstJobTypeInformationDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
                        },
                        InformationCode = d.InformationCode,
                        InformationGroup = d.InformationGroup,
                        IsPrinted = d.IsPrinted
                    }
                ).ToListAsync();

                return StatusCode(200, jobTypeInformations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobTypeInformationDetail(Int32 id)
        {
            try
            {
                DTO.MstJobTypeInformationDTO jobTypeInformation = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.Id == id
                    select new DTO.MstJobTypeInformationDTO
                    {
                        Id = d.Id,
                        JobTypeId = d.JobTypeId,
                        JobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_JobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_JobTypeId.ManualCode,
                            JobType = d.MstJobType_JobTypeId.JobType
                        },
                        InformationCode = d.InformationCode,
                        InformationGroup = d.InformationGroup,
                        IsPrinted = d.IsPrinted
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobTypeInformation);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobTypeInformation([FromBody] DTO.MstJobTypeInformationDTO mstJobTypeInformationDTO)
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
                    return StatusCode(404, "No rights to add a job type information.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job type information.");
                }

                DBSets.MstJobTypeDBSet article = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == mstJobTypeInformationDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a job type information if the current job type is locked.");
                }

                DBSets.MstJobTypeInformationDBSet jobTypeInformation = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.JobTypeId == mstJobTypeInformationDTO.JobTypeId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobTypeInformation == null)
                {
                    return StatusCode(404, "Job type information not found.");
                }

                DBSets.MstJobTypeInformationDBSet newJobTypeInformation = new DBSets.MstJobTypeInformationDBSet()
                {
                    JobTypeId = mstJobTypeInformationDTO.JobTypeId,
                    InformationCode = mstJobTypeInformationDTO.InformationCode,
                    InformationGroup = mstJobTypeInformationDTO.InformationGroup,
                    IsPrinted = mstJobTypeInformationDTO.IsPrinted
                };

                _dbContext.MstJobTypeInformations.Add(newJobTypeInformation);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobTypeInformation(int id, [FromBody] DTO.MstJobTypeInformationDTO mstJobTypeInformationDTO)
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
                    return StatusCode(404, "No rights to edit or update a job type information.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job type information.");
                }

                DBSets.MstJobTypeInformationDBSet jobTypeInformation = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeInformation == null)
                {
                    return StatusCode(404, "Job type information not found.");
                }

                if (jobTypeInformation.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a job type information if the current item is locked.");
                }

                DBSets.MstJobTypeInformationDBSet updateJobTypeInformation = jobTypeInformation;
                updateJobTypeInformation.InformationCode = mstJobTypeInformationDTO.InformationCode;
                updateJobTypeInformation.InformationGroup = mstJobTypeInformationDTO.InformationGroup;
                updateJobTypeInformation.IsPrinted = mstJobTypeInformationDTO.IsPrinted;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobTypeInformation(int id)
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
                    return StatusCode(404, "No rights to delete a job type information.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job type information.");
                }

                DBSets.MstJobTypeInformationDBSet jobTypeInformation = await (
                    from d in _dbContext.MstJobTypeInformations
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobTypeInformation == null)
                {
                    return StatusCode(404, "Job type information not found.");
                }

                if (jobTypeInformation.MstJobType_JobTypeId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job type information if the current item is locked.");
                }

                _dbContext.MstJobTypeInformations.Remove(jobTypeInformation);
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
