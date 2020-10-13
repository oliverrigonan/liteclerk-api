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
                            Username = d.MstUser_InformationByUserId.Username,
                            Fullname = d.MstUser_InformationByUserId.Fullname
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
                            Username = d.MstUser_InformationByUserId.Username,
                            Fullname = d.MstUser_InformationByUserId.Fullname
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
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job order information.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job order information.");
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

                DBSets.MstUserDBSet informationByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderInformationDTO.InformationByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (informationByUser == null)
                {
                    return StatusCode(404, "Information by loginUser not found.");
                }

                DBSets.TrnJobOrderInformationDBSet newJobOrderInformation = new DBSets.TrnJobOrderInformationDBSet()
                {
                    JOId = trnJobOrderInformationDTO.JOId,
                    InformationCode = trnJobOrderInformationDTO.InformationCode,
                    InformationGroup = trnJobOrderInformationDTO.InformationGroup,
                    Value = trnJobOrderInformationDTO.Value,
                    Particulars = trnJobOrderInformationDTO.Particulars,
                    IsPrinted = trnJobOrderInformationDTO.IsPrinted,
                    InformationByUserId = trnJobOrderInformationDTO.InformationByUserId,
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
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a job order information.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job order information.");
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

                DBSets.MstUserDBSet informationByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderInformationDTO.InformationByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (informationByUser == null)
                {
                    return StatusCode(404, "Information by loginUser not found.");
                }

                DBSets.TrnJobOrderInformationDBSet updateJobOrderInformations = jobOrderInformation;
                updateJobOrderInformations.JOId = trnJobOrderInformationDTO.JOId;
                updateJobOrderInformations.InformationCode = trnJobOrderInformationDTO.InformationCode;
                updateJobOrderInformations.InformationGroup = trnJobOrderInformationDTO.InformationGroup;
                updateJobOrderInformations.Value = trnJobOrderInformationDTO.Value;
                updateJobOrderInformations.Particulars = trnJobOrderInformationDTO.Particulars;
                updateJobOrderInformations.IsPrinted = trnJobOrderInformationDTO.IsPrinted;
                updateJobOrderInformations.InformationByUserId = trnJobOrderInformationDTO.InformationByUserId;
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
        public async Task<ActionResult> DeleteJobOrderInformation(int id)
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
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job order information.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job order information.");
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

                if (jobOrderInformation.TrnJobOrder_JOId.IsLocked == true)
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

        [HttpPost("import")]
        public async Task<ActionResult> ImportJobOrderInformation([FromBody] List<DTO.TrnJobOrderInformationDTO> trnJobOrderInformationDTOs)
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
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to import job order informations.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to import job order informations.");
                }

                if (trnJobOrderInformationDTOs.Any())
                {
                    DBSets.TrnJobOrderDBSet jobOrder = await (
                        from d in _dbContext.TrnJobOrders
                        where d.Id == trnJobOrderInformationDTOs.FirstOrDefault().JOId
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

                    List<DBSets.TrnJobOrderInformationDBSet> jobOrderInformations = await (
                        from d in _dbContext.TrnJobOrderInformations
                        where d.JOId == trnJobOrderInformationDTOs.FirstOrDefault().JOId
                        select d
                    ).ToListAsync();

                    if (jobOrderInformations.Any())
                    {
                        _dbContext.TrnJobOrderInformations.RemoveRange(jobOrderInformations);
                        await _dbContext.SaveChangesAsync();
                    }

                    foreach (var trnJobOrderInformationDTO in trnJobOrderInformationDTOs)
                    {
                        DBSets.MstUserDBSet informationByUser = await (
                            from d in _dbContext.MstUsers
                            where d.Username == trnJobOrderInformationDTO.InformationByUser.Username
                            select d
                        ).FirstOrDefaultAsync();

                        if (informationByUser == null)
                        {
                            return StatusCode(404, "Information by loginUser not found.");
                        }

                        DBSets.TrnJobOrderInformationDBSet newJobOrderInformation = new DBSets.TrnJobOrderInformationDBSet()
                        {
                            JOId = trnJobOrderInformationDTOs.FirstOrDefault().JOId,
                            InformationCode = trnJobOrderInformationDTO.InformationCode,
                            InformationGroup = trnJobOrderInformationDTO.InformationGroup,
                            Value = trnJobOrderInformationDTO.Value,
                            Particulars = trnJobOrderInformationDTO.Particulars,
                            IsPrinted = trnJobOrderInformationDTO.IsPrinted,
                            InformationByUserId = informationByUser.Id,
                            InformationUpdatedDateTime = DateTime.Now
                        };

                        _dbContext.TrnJobOrderInformations.Add(newJobOrderInformation);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
