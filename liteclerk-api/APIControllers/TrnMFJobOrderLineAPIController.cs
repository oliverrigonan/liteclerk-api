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
    public class TrnMFJobOrderLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnMFJobOrderLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{JOId}")]
        public async Task<ActionResult> GetJobOrderInformationListByJobOrder(Int32 JOId)
        {
            try
            {
                IEnumerable<DTO.TrnMFJobOrderLineDTO> jobOrderInformations = await (
                    from d in _dbContext.TrnMFJobOrderLines
                    where d.MFJOId == JOId
                    select new DTO.TrnMFJobOrderLineDTO
                    {
                        Id = d.Id,
                        MFJOId = d.MFJOId,
                        Description = d.Description,
                        Brand = d.Brand,
                        Serial = d.Serial,
                        Particulars = d.Particulars,
                        Quantity = d.Quantity
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
                DTO.TrnMFJobOrderLineDTO jobOrderInformation = await (
                    from d in _dbContext.TrnMFJobOrderLines
                    where d.Id == id
                    select new DTO.TrnMFJobOrderLineDTO
                    {
                        Id = d.Id,
                        MFJOId = d.MFJOId,
                        Description = d.Description,
                        Brand = d.Brand,
                        Serial = d.Serial,
                        Particulars = d.Particulars,
                        Quantity = d.Quantity
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
        public async Task<ActionResult> AddJobOrderInformation([FromBody] DTO.TrnMFJobOrderLineDTO trnMFJobOrderLineDTO)
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

                DBSets.TrnMFJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == trnMFJobOrderLineDTO.MFJOId
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


                DBSets.TrnMFJobOrderLineDBSet newJobOrderInformation = new DBSets.TrnMFJobOrderLineDBSet()
                {
                    MFJOId = trnMFJobOrderLineDTO.MFJOId,
                    Description = trnMFJobOrderLineDTO.Description,
                    Brand = trnMFJobOrderLineDTO.Brand,
                    Serial = trnMFJobOrderLineDTO.Serial,
                    Particulars = trnMFJobOrderLineDTO.Particulars,
                    Quantity = trnMFJobOrderLineDTO.Quantity,
                };

                _dbContext.TrnMFJobOrderLines.Add(newJobOrderInformation);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobOrderLine(Int32 id, [FromBody] DTO.TrnMFJobOrderLineDTO trnMFJobOrderlineDTO)
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

                DBSets.TrnMFJobOrderLineDBSet jobOrderInformation = await (
                    from d in _dbContext.TrnMFJobOrderLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderInformation == null)
                {
                    return StatusCode(404, "Job order information not found.");
                }

                DBSets.TrnMFJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == trnMFJobOrderlineDTO.MFJOId
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


                DBSets.TrnMFJobOrderLineDBSet updateJobOrderInformations = jobOrderInformation;
                updateJobOrderInformations.MFJOId = trnMFJobOrderlineDTO.MFJOId;
                updateJobOrderInformations.Description = trnMFJobOrderlineDTO.Description;
                updateJobOrderInformations.Brand = trnMFJobOrderlineDTO.Brand;
                updateJobOrderInformations.Serial = trnMFJobOrderlineDTO.Serial;
                updateJobOrderInformations.Particulars = trnMFJobOrderlineDTO.Particulars;
                updateJobOrderInformations.Quantity = trnMFJobOrderlineDTO.Quantity;

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

                DBSets.TrnMFJobOrderLineDBSet mfJobOrderLine = await (
                    from d in _dbContext.TrnMFJobOrderLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (mfJobOrderLine == null)
                {
                    return StatusCode(404, "Job order information not found.");
                }

                if (mfJobOrderLine.TrnMFJobOrder_MFJOId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete job order information(s) if the current job order is locked.");
                }

                _dbContext.TrnMFJobOrderLines.Remove(mfJobOrderLine);
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
