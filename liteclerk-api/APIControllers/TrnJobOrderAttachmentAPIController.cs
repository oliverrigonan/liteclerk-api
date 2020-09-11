using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrnJobOrderAttachmentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private IConfiguration _configuration { get; }

        public TrnJobOrderAttachmentAPIController(DBContext.LiteclerkDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet("list/{JOId}")]
        public async Task<ActionResult> GetJobOrderAttachmentListByJobOrder(Int32 JOId)
        {
            try
            {
                IEnumerable<DTO.TrnJobOrderAttachmentDTO> jobOrderAttachments = await (
                    from d in _dbContext.TrnJobOrderAttachments
                    where d.JOId == JOId
                    select new DTO.TrnJobOrderAttachmentDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        AttachmentCode = d.AttachmentCode,
                        AttachmentType = d.AttachmentType,
                        AttachmentURL = d.AttachmentURL,
                        Particulars = d.Particulars,
                        IsPrinted = d.IsPrinted
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrderAttachments);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobOrderAttachmentDetail(Int32 id)
        {
            try
            {
                DTO.TrnJobOrderAttachmentDTO jobOrderAttachment = await (
                    from d in _dbContext.TrnJobOrderAttachments
                    where d.Id == id
                    select new DTO.TrnJobOrderAttachmentDTO
                    {
                        Id = d.Id,
                        JOId = d.JOId,
                        AttachmentCode = d.AttachmentCode,
                        AttachmentType = d.AttachmentType,
                        AttachmentURL = d.AttachmentURL,
                        Particulars = d.Particulars,
                        IsPrinted = d.IsPrinted
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobOrderAttachment);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrderAttachment([FromBody] DTO.TrnJobOrderAttachmentDTO trnJobOrderAttachmentDTO)
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
                    return StatusCode(404, "No rights to add a job order attachment.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job order attachment.");
                }

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderAttachmentDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add job order attachments if the current job order is locked.");
                }

                DBSets.TrnJobOrderAttachmentDBSet newJobOrderAttachment = new DBSets.TrnJobOrderAttachmentDBSet()
                {
                    JOId = trnJobOrderAttachmentDTO.JOId,
                    AttachmentCode = trnJobOrderAttachmentDTO.AttachmentCode,
                    AttachmentType = trnJobOrderAttachmentDTO.AttachmentType,
                    AttachmentURL = trnJobOrderAttachmentDTO.AttachmentURL,
                    Particulars = trnJobOrderAttachmentDTO.Particulars,
                    IsPrinted = trnJobOrderAttachmentDTO.IsPrinted
                };

                _dbContext.TrnJobOrderAttachments.Add(newJobOrderAttachment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJobOrderAttachment(Int32 id, [FromBody] DTO.TrnJobOrderAttachmentDTO trnJobOrderAttachmentDTO)
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
                    return StatusCode(404, "No rights to edit or update a job order attachment.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a job order attachment.");
                }

                DBSets.TrnJobOrderAttachmentDBSet jobOrderAttachment = await (
                    from d in _dbContext.TrnJobOrderAttachments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderAttachment == null)
                {
                    return StatusCode(404, "Job order attachment not found.");
                }

                DBSets.TrnJobOrderDBSet jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == trnJobOrderAttachmentDTO.JOId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update job order attachments if the current job order is locked.");
                }

                DBSets.TrnJobOrderAttachmentDBSet updateJobOrderAttachments = jobOrderAttachment;
                updateJobOrderAttachments.JOId = trnJobOrderAttachmentDTO.JOId;
                updateJobOrderAttachments.AttachmentCode = trnJobOrderAttachmentDTO.AttachmentCode;
                updateJobOrderAttachments.AttachmentType = trnJobOrderAttachmentDTO.AttachmentType;
                updateJobOrderAttachments.AttachmentURL = trnJobOrderAttachmentDTO.AttachmentURL;
                updateJobOrderAttachments.Particulars = trnJobOrderAttachmentDTO.Particulars;
                updateJobOrderAttachments.IsPrinted = trnJobOrderAttachmentDTO.IsPrinted;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobOrderAttachment(int id)
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
                    return StatusCode(404, "No rights to delete a job order attachment.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job order attachment.");
                }

                DBSets.TrnJobOrderAttachmentDBSet jobOrderAttachment = await (
                    from d in _dbContext.TrnJobOrderAttachments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderAttachment == null)
                {
                    return StatusCode(404, "Job order attachment not found.");
                }

                if (jobOrderAttachment.TrnJobOrder_JOId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete job order attachments if the current job order is locked.");
                }

                _dbContext.TrnJobOrderAttachments.Remove(jobOrderAttachment);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadJobOrderAttachment(IFormFile file)
        {
            try
            {
                String cloudStorageConnectionString = _configuration["CloudStorage:ConnectionString"];
                String cloudStorageContainerName = _configuration["CloudStorage:ContainerName"];

                if (CloudStorageAccount.TryParse(cloudStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    var picBlob = cloudBlobContainer.GetBlockBlobReference(file.FileName);

                    await picBlob.UploadFromStreamAsync(file.OpenReadStream());

                    return Ok(picBlob.Uri);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
