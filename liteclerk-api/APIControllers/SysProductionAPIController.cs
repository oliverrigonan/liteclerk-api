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

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnJobOrderDepartmentDTO> productions = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.TrnJobOrder_JOId.BranchId == loginUser.BranchId
                    && d.TrnJobOrder_JOId.JODate >= Convert.ToDateTime(startDate)
                    && d.TrnJobOrder_JOId.JODate <= Convert.ToDateTime(endDate)
                    && d.TrnJobOrder_JOId.IsLocked == true
                    && d.JobDepartmentId == jobDepartmentId
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDepartmentDTO
                    {
                        Id = d.Id,
                        JOId = d.TrnJobOrder_JOId.Id,
                        JobOrder = new DTO.TrnJobOrderDTO
                        {
                            JONumber = d.TrnJobOrder_JOId.JONumber,
                            JODate = d.TrnJobOrder_JOId.JODate.ToShortDateString(),
                            ManualNumber = d.TrnJobOrder_JOId.ManualNumber,
                            DocumentReference = d.TrnJobOrder_JOId.DocumentReference,
                            DateScheduled = d.TrnJobOrder_JOId.DateScheduled.ToShortDateString(),
                            DateNeeded = d.TrnJobOrder_JOId.DateNeeded.ToShortDateString(),
                            SIId = d.TrnJobOrder_JOId.SIId,
                            SalesInvoice = new DTO.TrnSalesInvoiceDTO
                            {
                                SINumber = d.TrnJobOrder_JOId.SIId != null ? d.TrnJobOrder_JOId.TrnSalesInvoice_SIId.SINumber : "",
                                SIDate = d.TrnJobOrder_JOId.SIId != null ? d.TrnJobOrder_JOId.TrnSalesInvoice_SIId.SIDate.ToShortDateString() : "",
                                ManualNumber = d.TrnJobOrder_JOId.SIId != null ? d.TrnJobOrder_JOId.TrnSalesInvoice_SIId.ManualNumber : "",
                                DocumentReference = d.TrnJobOrder_JOId.SIId != null ? d.TrnJobOrder_JOId.DocumentReference : "",
                                Customer = new DTO.MstArticleCustomerDTO
                                {
                                    Customer = d.TrnJobOrder_JOId.SIId != null ?
                                           d.TrnJobOrder_JOId.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                           d.TrnJobOrder_JOId.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "" : ""
                                }
                            },
                            ItemId = d.TrnJobOrder_JOId.ItemId,
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnJobOrder_JOId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                Description = d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                            },
                            ItemJobTypeId = d.TrnJobOrder_JOId.ItemJobTypeId,
                            ItemJobType = new DTO.MstJobTypeDTO
                            {
                                JobType = d.TrnJobOrder_JOId.MstJobType_ItemJobTypeId.JobType
                            },
                            Quantity = d.TrnJobOrder_JOId.Quantity,
                            UnitId = d.TrnJobOrder_JOId.UnitId,
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.TrnJobOrder_JOId.MstUnit_UnitId.UnitCode,
                                ManualCode = d.TrnJobOrder_JOId.MstUnit_UnitId.ManualCode,
                                Unit = d.TrnJobOrder_JOId.MstUnit_UnitId.Unit
                            },
                            Remarks = d.TrnJobOrder_JOId.Remarks,
                            BaseQuantity = d.TrnJobOrder_JOId.BaseQuantity,
                            BaseUnitId = d.TrnJobOrder_JOId.BaseUnitId,
                            BaseUnit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.TrnJobOrder_JOId.MstUnit_BaseUnitId.UnitCode,
                                ManualCode = d.TrnJobOrder_JOId.MstUnit_BaseUnitId.ManualCode,
                                Unit = d.TrnJobOrder_JOId.MstUnit_BaseUnitId.Unit
                            },
                        },
                        Status = d.Status
                    }
                ).ToListAsync();

                return StatusCode(200, productions);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/status/{jobOrderDepartmentId}")]
        public async Task<ActionResult> UpdateJobOrderDepartmentStatus(Int32 jobOrderDepartmentId, DTO.TrnJobOrderDepartmentDTO trnJobOrderDepartmentDTO)
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
                    where d.Id == jobOrderDepartmentId
                    select d
                ).FirstOrDefaultAsync();

                if (jobOrderDepartment == null)
                {
                    return StatusCode(404, "Job order department not found.");
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
