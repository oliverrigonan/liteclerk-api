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
    public class SysProductionAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysProductionAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
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

        [HttpGet("list/byDateRange/byJobDeparment/{startDate}/{endDate}/{jobDepartmentId}")]
        public async Task<ActionResult> GetProductionListByDateRangedAndJobDeparment(String startDate, String endDate, Int32 jobDepartmentId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var productions = new List<DTO.TrnJobOrderDepartmentDTO>();

                var jobOrderDepartments = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.TrnJobOrder_JOId.BranchId == loginUser.BranchId
                    && d.TrnJobOrder_JOId.JODate >= Convert.ToDateTime(startDate)
                    && d.TrnJobOrder_JOId.JODate <= Convert.ToDateTime(endDate)
                    && d.TrnJobOrder_JOId.IsLocked == true
                    && d.JobDepartmentId == jobDepartmentId
                    && d.Status != "DONE"
                    orderby d.Id descending
                    select d
                ).ToListAsync();

                if (jobOrderDepartments.Any())
                {
                    foreach (var jobOrderDepartment in jobOrderDepartments)
                    {
                        Boolean isValid = true;

                        if (jobOrderDepartment.IsRequired == true)
                        {
                            var previousJobOrderDepartments = await (
                                from d in _dbContext.TrnJobOrderDepartments
                                where d.Id != jobOrderDepartment.Id
                                && d.JOId == jobOrderDepartment.JOId
                                && d.SequenceNumber < jobOrderDepartment.SequenceNumber
                                && d.Status != "DONE"
                                select d
                            ).OrderByDescending(d => d.SequenceNumber).ToListAsync();

                            if (previousJobOrderDepartments.Any() == true)
                            {
                                isValid = false;
                            }
                        }

                        if (isValid == true)
                        {
                            productions.Add(new DTO.TrnJobOrderDepartmentDTO
                            {
                                Id = jobOrderDepartment.Id,
                                JOId = jobOrderDepartment.TrnJobOrder_JOId.Id,
                                JobOrder = new DTO.TrnJobOrderDTO
                                {
                                    JONumber = jobOrderDepartment.TrnJobOrder_JOId.JONumber,
                                    JODate = jobOrderDepartment.TrnJobOrder_JOId.JODate.ToShortDateString(),
                                    ManualNumber = jobOrderDepartment.TrnJobOrder_JOId.ManualNumber,
                                    DocumentReference = jobOrderDepartment.TrnJobOrder_JOId.DocumentReference,
                                    DateScheduled = jobOrderDepartment.TrnJobOrder_JOId.DateScheduled.ToShortDateString(),
                                    DateNeeded = jobOrderDepartment.TrnJobOrder_JOId.DateNeeded.ToShortDateString(),
                                    SIId = jobOrderDepartment.TrnJobOrder_JOId.SIId,
                                    SalesInvoice = new DTO.TrnSalesInvoiceDTO
                                    {
                                        SINumber = jobOrderDepartment.TrnJobOrder_JOId.SIId != null ? jobOrderDepartment.TrnJobOrder_JOId.TrnSalesInvoice_SIId.SINumber : "",
                                        SIDate = jobOrderDepartment.TrnJobOrder_JOId.SIId != null ? jobOrderDepartment.TrnJobOrder_JOId.TrnSalesInvoice_SIId.SIDate.ToShortDateString() : "",
                                        ManualNumber = jobOrderDepartment.TrnJobOrder_JOId.SIId != null ? jobOrderDepartment.TrnJobOrder_JOId.TrnSalesInvoice_SIId.ManualNumber : "",
                                        DocumentReference = jobOrderDepartment.TrnJobOrder_JOId.SIId != null ? jobOrderDepartment.TrnJobOrder_JOId.DocumentReference : "",
                                        Customer = new DTO.MstArticleCustomerDTO
                                        {
                                            Customer = jobOrderDepartment.TrnJobOrder_JOId.SIId != null ?
                                               jobOrderDepartment.TrnJobOrder_JOId.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                               jobOrderDepartment.TrnJobOrder_JOId.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "" : ""
                                        }
                                    },
                                    ItemId = jobOrderDepartment.TrnJobOrder_JOId.ItemId,
                                    Item = new DTO.MstArticleItemDTO
                                    {
                                        Article = new DTO.MstArticleDTO
                                        {
                                            ManualCode = jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.ManualCode
                                        },
                                        SKUCode = jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                        BarCode = jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                        Description = jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? jobOrderDepartment.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                                    },
                                    ItemJobTypeId = jobOrderDepartment.TrnJobOrder_JOId.ItemJobTypeId,
                                    ItemJobType = new DTO.MstJobTypeDTO
                                    {
                                        JobType = jobOrderDepartment.TrnJobOrder_JOId.MstJobType_ItemJobTypeId.JobType
                                    },
                                    Quantity = jobOrderDepartment.TrnJobOrder_JOId.Quantity,
                                    UnitId = jobOrderDepartment.TrnJobOrder_JOId.UnitId,
                                    Unit = new DTO.MstUnitDTO
                                    {
                                        UnitCode = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_UnitId.UnitCode,
                                        ManualCode = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_UnitId.ManualCode,
                                        Unit = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_UnitId.Unit
                                    },
                                    Remarks = jobOrderDepartment.TrnJobOrder_JOId.Remarks,
                                    BaseQuantity = jobOrderDepartment.TrnJobOrder_JOId.BaseQuantity,
                                    BaseUnitId = jobOrderDepartment.TrnJobOrder_JOId.BaseUnitId,
                                    BaseUnit = new DTO.MstUnitDTO
                                    {
                                        UnitCode = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_BaseUnitId.UnitCode,
                                        ManualCode = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_BaseUnitId.ManualCode,
                                        Unit = jobOrderDepartment.TrnJobOrder_JOId.MstUnit_BaseUnitId.Unit
                                    },
                                },
                                Status = jobOrderDepartment.Status
                            });
                        }
                    }
                }

                return StatusCode(200, productions);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/status/{id}")]
        public async Task<ActionResult> UpdateJobOrderDepartmentStatus(Int32 id, DTO.TrnJobOrderDepartmentDTO trnJobOrderDepartmentDTO)
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

                DBSets.TrnJobOrderDepartmentDBSet jobOrderDepartment = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderDepartment == null)
                {
                    return StatusCode(404, "Job order department not found.");
                }

                if (jobOrderDepartment.IsRequired == true)
                {
                    List<DBSets.TrnJobOrderDepartmentDBSet> previousJobOrderDepartments = await (
                        from d in _dbContext.TrnJobOrderDepartments
                        where d.Id != id
                        && d.JOId == jobOrderDepartment.JOId
                        && d.SequenceNumber < jobOrderDepartment.SequenceNumber
                        select d
                    ).OrderByDescending(d => d.SequenceNumber).ToListAsync();

                    if (previousJobOrderDepartments.Any())
                    {
                        foreach (var previousJobOrderDepartment in previousJobOrderDepartments)
                        {
                            if (previousJobOrderDepartment.Status != "DONE")
                            {
                                return StatusCode(404, "The previous department must be done first.");
                            }
                        }
                    }
                }

                DBSets.TrnJobOrderDepartmentDBSet updateJobOrderDepartments = jobOrderDepartment;
                updateJobOrderDepartments.Status = trnJobOrderDepartmentDTO.Status;
                updateJobOrderDepartments.StatusByUserId = loginUserId;
                updateJobOrderDepartments.StatusUpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                String PNNumber = "0000000001";
                DBSets.SysProductionDBSet lastProduction = await (
                    from d in _dbContext.SysProductions
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastProduction != null)
                {
                    Int32 lastPNNumber = Convert.ToInt32(lastProduction.PNNumber) + 0000000001;
                    PNNumber = PadZeroes(lastPNNumber, 10);
                }

                DBSets.SysProductionDBSet newProduction = new DBSets.SysProductionDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    PNNumber = PNNumber,
                    PNDate = DateTime.Today,
                    Status = trnJobOrderDepartmentDTO.Status,
                    Particulars = trnJobOrderDepartmentDTO.Particulars,
                    ProductionTimeStamp = DateTime.Now,
                    UserId = loginUserId,
                    JODepartmentId = trnJobOrderDepartmentDTO.Id
                };

                _dbContext.SysProductions.Add(newProduction);
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
