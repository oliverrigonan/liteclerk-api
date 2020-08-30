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
    public class TrnJobOrderInformationAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnJobOrderInformationAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{JOId}")]
        public async Task<ActionResult> GetJobOrderInformationListByJobOrder(Int32 JOId)
        {
            try
            {
                IEnumerable<DTO.TrnJobOrderInformationDTO> jobOrderInformations = await (
                    from d in _dbContext.TrnJobOrderInformations
                    where d.JOId == JOId
                    select new DTO.TrnJobOrderInformationDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        InformationCode = d.InformationCode,
                        InformationGroup = d.InformationGroup,
                        Value = d.Value,
                        Particulars = d.Particulars,
                        IsPrinted = d.IsPrinted,
                        InformationByUserId = d.InformationByUserId,
                        InformationByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_InformationByUser.Username,
                            Fullname = d.MstUser_InformationByUser.Fullname
                        },
                        InformationUpdatedDateTime = d.InformationUpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrderInformations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobOrderInformationDetail(Int32 id)
        {
            try
            {
                DTO.TrnJobOrderInformationDTO jobOrderInformation = await (
                    from d in _dbContext.TrnJobOrderInformations
                    where d.Id == id
                    select new DTO.TrnJobOrderInformationDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        InformationCode = d.InformationCode,
                        InformationGroup = d.InformationGroup,
                        Value = d.Value,
                        Particulars = d.Particulars,
                        IsPrinted = d.IsPrinted,
                        InformationByUserId = d.InformationByUserId,
                        InformationByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_InformationByUser.Username,
                            Fullname = d.MstUser_InformationByUser.Fullname
                        },
                        InformationUpdatedDateTime = d.InformationUpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobOrderInformation);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrderInformation([FromBody] DTO.TrnJobOrderInformationDTO trnJobOrderInformationDTO)
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

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderInformationDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add job order information(s) if the current job order is locked.");
                }

                DBSets.TrnJobOrderInformationDBSet newJobOrderInformation = new DBSets.TrnJobOrderInformationDBSet()
                {
                    JOId = trnJobOrderInformationDTO.JOId,
                    InformationCode = trnJobOrderInformationDTO.InformationCode,
                    InformationGroup = trnJobOrderInformationDTO.InformationGroup,
                    Value = trnJobOrderInformationDTO.Value,
                    Particulars = trnJobOrderInformationDTO.Particulars,
                    IsPrinted = trnJobOrderInformationDTO.IsPrinted,
                    InformationByUserId = userId,
                    InformationUpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnJobOrderInformations.Add(newJobOrderInformation);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobOrderInformation(Int32 id, [FromBody] DTO.TrnJobOrderInformationDTO trnJobOrderInformationDTO)
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

                DBSets.TrnJobOrderInformationDBSet jobOrderInformation = await (
                    from d in _dbContext.TrnJobOrderInformations
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderInformation == null)
                {
                    return StatusCode(404, "Job order information not found.");
                }

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderInformationDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update job order information(s) if the current job order is locked.");
                }

                DBSets.TrnJobOrderInformationDBSet updateJobOrderInformations = jobOrderInformation;
                updateJobOrderInformations.JOId = trnJobOrderInformationDTO.JOId;
                updateJobOrderInformations.InformationCode = trnJobOrderInformationDTO.InformationCode;
                updateJobOrderInformations.InformationGroup = trnJobOrderInformationDTO.InformationGroup;
                updateJobOrderInformations.Value = trnJobOrderInformationDTO.Value;
                updateJobOrderInformations.Particulars = trnJobOrderInformationDTO.Particulars;
                updateJobOrderInformations.IsPrinted = trnJobOrderInformationDTO.IsPrinted;
                updateJobOrderInformations.InformationByUserId = userId;
                updateJobOrderInformations.InformationUpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> GetJobOrderInformation(int id)
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

                DBSets.TrnJobOrderInformationDBSet jobOrderInformation = await (
                    from d in _dbContext.TrnJobOrderInformations
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderInformation == null)
                {
                    return StatusCode(404, "Job order information not found.");
                }

                if (jobOrderInformation.TrnJobOrder_JobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete job order information(s) if the current job order is locked.");
                }

                _dbContext.TrnJobOrderInformations.Remove(jobOrderInformation);
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
