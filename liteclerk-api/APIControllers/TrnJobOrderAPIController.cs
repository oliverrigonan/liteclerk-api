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
    public class TrnJobOrderAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public TrnJobOrderAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        [HttpPost("createJobOrderFromSalesInvoice/{SIId}")]
        public async Task<IActionResult> CreateJobOrderFromSalesInvoice(Int32 SIId)
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

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == SIId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == SIId
                    && d.ItemJobTypeId != null
                    select d
                ).ToListAsync();

                if (salesInvoiceItems.Any() == false)
                {
                    return StatusCode(404, "Sales invoice items may not found or some sales invoice items do not have job type.");
                }

                foreach (var salesInvoiceItem in salesInvoiceItems)
                {
                    DBSets.TrnJobOrderDBSet existingJobOrder = await (
                        from d in _dbContext.TrnJobOrders
                        where d.SIItemId == salesInvoiceItem.Id
                        select d
                    ).FirstOrDefaultAsync();

                    if (existingJobOrder == null)
                    {
                        String JONumber = "0000000001";
                        DBSets.TrnJobOrderDBSet lastJobOrder = await (
                            from d in _dbContext.TrnJobOrders
                            where d.BranchId == user.BranchId
                            orderby d.Id descending
                            select d
                        ).FirstOrDefaultAsync();

                        if (lastJobOrder != null)
                        {
                            Int32 lastJONumber = Convert.ToInt32(lastJobOrder.JONumber) + 0000000001;
                            JONumber = PadZeroes(lastJONumber, 10);
                        }

                        DBSets.TrnJobOrderDBSet newJobOrder = new DBSets.TrnJobOrderDBSet()
                        {
                            BranchId = Convert.ToInt32(user.BranchId),
                            CurrencyId = user.MstCompany_Company.CurrencyId,
                            JONumber = JONumber,
                            JODate = DateTime.Today,
                            ManualNumber = JONumber,
                            DocumentReference = "",
                            DateScheduled = salesInvoice.SIDate,
                            DateNeeded = salesInvoice.DateNeeded,
                            SIId = salesInvoice.Id,
                            SIItemId = salesInvoiceItem.Id,
                            ItemId = salesInvoiceItem.ItemId,
                            ItemJobTypeId = Convert.ToInt32(salesInvoiceItem.ItemJobTypeId),
                            Quantity = salesInvoiceItem.Quantity,
                            Remarks = salesInvoiceItem.Particulars,
                            PreparedByUserId = salesInvoice.PreparedByUserId,
                            CheckedByUserId = salesInvoice.CheckedByUserId,
                            ApprovedByUserId = salesInvoice.ApprovedByUserId,
                            Status = "",
                            IsCancelled = false,
                            IsPrinted = false,
                            IsLocked = true,
                            CreatedByUserId = user.Id,
                            CreatedDateTime = DateTime.Now,
                            UpdatedByUserId = user.Id,
                            UpdatedDateTime = DateTime.Now
                        };

                        _dbContext.TrnJobOrders.Add(newJobOrder);
                        await _dbContext.SaveChangesAsync();

                        DBSets.MstJobTypeDBSet jobType = await (
                            from d in _dbContext.MstJobTypes
                            where d.Id == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                            select d
                        ).FirstOrDefaultAsync();

                        if (jobType != null)
                        {
                            IEnumerable<DBSets.MstJobTypeAttachmentDBSet> jobTypeAttachments = await (
                                from d in _dbContext.MstJobTypeAttachments
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeAttachments.Any() == true)
                            {
                                foreach (var jobTypeAttachment in jobTypeAttachments)
                                {
                                    DBSets.TrnJobOrderAttachmentDBSet newJobOrderAttachment = new DBSets.TrnJobOrderAttachmentDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        AttachmentCode = jobTypeAttachment.AttachmentCode,
                                        AttachmentType = jobTypeAttachment.AttachmentType,
                                        AttachmentURL = "",
                                        Particulars = "",
                                        IsPrinted = false
                                    };

                                    _dbContext.TrnJobOrderAttachments.Add(newJobOrderAttachment);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            IEnumerable<DBSets.MstJobTypeDepartmentDBSet> jobTypeDepartments = await (
                                from d in _dbContext.MstJobTypeDepartments
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeDepartments.Any() == true)
                            {
                                foreach (var jobTypeDepartment in jobTypeDepartments)
                                {
                                    DBSets.TrnJobOrderDepartmentDBSet newJobOrderDepartment = new DBSets.TrnJobOrderDepartmentDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        JobDepartmentId = jobTypeDepartment.JobDepartmentId,
                                        Particulars = "",
                                        Status = "",
                                        StatusByUserId = userId,
                                        StatusUpdatedDateTime = DateTime.Now
                                    };

                                    _dbContext.TrnJobOrderDepartments.Add(newJobOrderDepartment);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            IEnumerable<DBSets.MstJobTypeInformationDBSet> jobTypeInformations = await (
                                from d in _dbContext.MstJobTypeInformations
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeInformations.Any() == true)
                            {
                                foreach (var jobTypeInformation in jobTypeInformations)
                                {
                                    DBSets.TrnJobOrderInformationDBSet newJobOrderInformation = new DBSets.TrnJobOrderInformationDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        InformationCode = jobTypeInformation.InformationCode,
                                        InformationGroup = jobTypeInformation.InformationGroup,
                                        Value = "",
                                        Particulars = "",
                                        IsPrinted = false,
                                        InformationByUserId = userId,
                                        InformationUpdatedDateTime = DateTime.Now,
                                    };

                                    _dbContext.TrnJobOrderInformations.Add(newJobOrderInformation);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                        }
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
