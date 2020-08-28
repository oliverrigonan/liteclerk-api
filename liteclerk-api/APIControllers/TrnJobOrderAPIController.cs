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
        private readonly DBContext.LiteclerkDBContext _dbContext;

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

        [HttpGet("listByDateRanged/{startDate}/{endDate}")]
        public async Task<ActionResult> GetJobOrderListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnJobOrderDTO> jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == user.BranchId
                    && d.JODate >= Convert.ToDateTime(startDate)
                    && d.JODate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SalesInvoice.SINumber,
                            SIDate = d.TrnSalesInvoice_SalesInvoice.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SalesInvoice.ManualNumber,
                            DocumentReference = d.DocumentReference
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                Description = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Item.ManualCode
                            },
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnit.UnitCode,
                            ManualCode = d.MstUnit_BaseUnit.ManualCode,
                            Unit = d.MstUnit_BaseUnit.Unit
                        },
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUser.Username,
                            Fullname = d.MstUser_PreparedByUser.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUser.Username,
                            Fullname = d.MstUser_CheckedByUser.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUser.Username,
                            Fullname = d.MstUser_ApprovedByUser.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("listBySalesInvoice/{SIId}")]
        public async Task<ActionResult> GetJobOrderListBySalesInvoice(Int32 SIId)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnJobOrderDTO> jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.SIId == SIId
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SalesInvoice.SINumber,
                            SIDate = d.TrnSalesInvoice_SalesInvoice.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SalesInvoice.ManualNumber,
                            DocumentReference = d.DocumentReference
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                Description = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Item.ManualCode
                            },
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnit.UnitCode,
                            ManualCode = d.MstUnit_BaseUnit.ManualCode,
                            Unit = d.MstUnit_BaseUnit.Unit
                        },
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUser.Username,
                            Fullname = d.MstUser_PreparedByUser.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUser.Username,
                            Fullname = d.MstUser_CheckedByUser.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUser.Username,
                            Fullname = d.MstUser_ApprovedByUser.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobOrderDetail(Int32 id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                DTO.TrnJobOrderDTO jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == id
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SalesInvoice.SINumber,
                            SIDate = d.TrnSalesInvoice_SalesInvoice.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SalesInvoice.ManualNumber,
                            DocumentReference = d.DocumentReference
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                                Description = d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? d.TrnSalesInvoiceItem_SalesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Item.ManualCode
                            },
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnit.UnitCode,
                            ManualCode = d.MstUnit_BaseUnit.ManualCode,
                            Unit = d.MstUnit_BaseUnit.Unit
                        },
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUser.Username,
                            Fullname = d.MstUser_PreparedByUser.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUser.Username,
                            Fullname = d.MstUser_CheckedByUser.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUser.Username,
                            Fullname = d.MstUser_ApprovedByUser.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, jobOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrder()
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

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_Article.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstJobTypeDBSet jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                String JONumber = "0000000001";
                DBSets.TrnJobOrderDBSet lastJobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == user.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJobOrder != null)
                {
                    Int32 lastSINumber = Convert.ToInt32(lastJobOrder.JONumber) + 0000000001;
                    JONumber = PadZeroes(lastSINumber, 10);
                }

                DBSets.TrnJobOrderDBSet newJobOrder = new DBSets.TrnJobOrderDBSet()
                {
                    BranchId = Convert.ToInt32(user.BranchId),
                    CurrencyId = user.MstCompany_Company.CurrencyId,
                    JONumber = JONumber,
                    JODate = DateTime.Today,
                    ManualNumber = JONumber,
                    DocumentReference = "",
                    DateScheduled = DateTime.Today,
                    DateNeeded = DateTime.Today,
                    SIId = null,
                    SIItemId = null,
                    ItemId = item.Id,
                    ItemJobTypeId = jobType.Id,
                    Quantity = 0,
                    UnitId = item.UnitId,
                    Remarks = "",
                    BaseQuantity = 0,
                    BaseUnitId = item.UnitId,
                    PreparedByUserId = userId,
                    CheckedByUserId = userId,
                    ApprovedByUserId = userId,
                    Status = "",
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = userId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnJobOrders.Add(newJobOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJobOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJobOrder(Int32 id, [FromBody] DTO.TrnJobOrderDTO trnJobOrderDTO)
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
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a job order that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJobOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.MstArticle_Article.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstJobTypeDBSet jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == trnJobOrderDTO.ItemJobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.UnitId == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnJobOrderDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnJobOrderDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                DBSets.TrnJobOrderDBSet saveJobOrder = jobOrder;
                saveJobOrder.CurrencyId = trnJobOrderDTO.CurrencyId;
                saveJobOrder.JODate = Convert.ToDateTime(trnJobOrderDTO.JODate);
                saveJobOrder.ManualNumber = trnJobOrderDTO.ManualNumber;
                saveJobOrder.DocumentReference = trnJobOrderDTO.DocumentReference;
                saveJobOrder.DateScheduled = Convert.ToDateTime(trnJobOrderDTO.DateScheduled);
                saveJobOrder.DateNeeded = Convert.ToDateTime(trnJobOrderDTO.DateNeeded);
                saveJobOrder.ItemId = trnJobOrderDTO.ItemId;
                saveJobOrder.ItemJobTypeId = trnJobOrderDTO.ItemJobTypeId;
                saveJobOrder.Quantity = trnJobOrderDTO.Quantity;
                saveJobOrder.UnitId = trnJobOrderDTO.UnitId;
                saveJobOrder.Remarks = trnJobOrderDTO.Remarks;
                saveJobOrder.BaseQuantity = baseQuantity;
                saveJobOrder.BaseUnitId = item.UnitId;
                saveJobOrder.CheckedByUserId = trnJobOrderDTO.CheckedByUserId;
                saveJobOrder.ApprovedByUserId = trnJobOrderDTO.ApprovedByUserId;
                saveJobOrder.Status = trnJobOrderDTO.Status;
                saveJobOrder.UpdatedByUserId = userId;
                saveJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockJobOrder(Int32 id, [FromBody] DTO.TrnJobOrderDTO trnJobOrderDTO)
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
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a job order that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJobOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.MstArticle_Article.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstJobTypeDBSet jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == trnJobOrderDTO.ItemJobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.UnitId == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnJobOrderDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnJobOrderDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                DBSets.TrnJobOrderDBSet lockJobOrder = jobOrder;
                lockJobOrder.CurrencyId = trnJobOrderDTO.CurrencyId;
                lockJobOrder.JODate = Convert.ToDateTime(trnJobOrderDTO.JODate);
                lockJobOrder.ManualNumber = trnJobOrderDTO.ManualNumber;
                lockJobOrder.DocumentReference = trnJobOrderDTO.DocumentReference;
                lockJobOrder.DateScheduled = Convert.ToDateTime(trnJobOrderDTO.DateScheduled);
                lockJobOrder.DateNeeded = Convert.ToDateTime(trnJobOrderDTO.DateNeeded);
                lockJobOrder.ItemId = trnJobOrderDTO.ItemId;
                lockJobOrder.ItemJobTypeId = trnJobOrderDTO.ItemJobTypeId;
                lockJobOrder.Quantity = trnJobOrderDTO.Quantity;
                lockJobOrder.UnitId = trnJobOrderDTO.UnitId;
                lockJobOrder.Remarks = trnJobOrderDTO.Remarks;
                lockJobOrder.BaseQuantity = baseQuantity;
                lockJobOrder.BaseUnitId = item.UnitId;
                lockJobOrder.CheckedByUserId = trnJobOrderDTO.CheckedByUserId;
                lockJobOrder.ApprovedByUserId = trnJobOrderDTO.ApprovedByUserId;
                lockJobOrder.Status = trnJobOrderDTO.Status;
                lockJobOrder.IsLocked = true;
                lockJobOrder.UpdatedByUserId = userId;
                lockJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockJobOrder(Int32 id)
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
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a job order that is unlocked.");
                }

                DBSets.TrnJobOrderDBSet unlockJobOrder = jobOrder;
                unlockJobOrder.IsLocked = false;
                unlockJobOrder.UpdatedByUserId = userId;
                unlockJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelJobOrder(Int32 id)
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
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a job order that is unlocked.");
                }

                DBSets.TrnJobOrderDBSet unlockJobOrder = jobOrder;
                unlockJobOrder.IsCancelled = true;
                unlockJobOrder.UpdatedByUserId = userId;
                unlockJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobOrder(Int32 id)
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
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job order that is locked.");
                }

                _dbContext.TrnJobOrders.Remove(jobOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("createJobOrderFromSalesInvoice/{SIId}")]
        public async Task<ActionResult> CreateJobOrderFromSalesInvoice(Int32 SIId)
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

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(404, "Cannot create job orders if the referenced sales invoice is unlocked.");
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

                        DBSets.MstArticleItemUnitDBSet itemUnit = await (
                            from d in _dbContext.MstArticleItemUnits
                            where d.ArticleId == salesInvoiceItem.ItemId
                            && d.UnitId == salesInvoiceItem.UnitId
                            select d
                        ).FirstOrDefaultAsync();

                        if (itemUnit == null)
                        {
                            return StatusCode(404, "Item unit not found.");
                        }

                        Decimal baseQuantity = salesInvoiceItem.Quantity;
                        if (itemUnit.Multiplier > 0)
                        {
                            baseQuantity = salesInvoiceItem.Quantity * (1 / itemUnit.Multiplier);
                        }

                        Int32 baseUnitId = salesInvoiceItem.MstArticle_Item.MstArticleItems_Article.Any() ? salesInvoiceItem.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().UnitId : 0;

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
                            UnitId = salesInvoiceItem.UnitId,
                            Remarks = salesInvoiceItem.Particulars,
                            BaseQuantity = baseQuantity,
                            BaseUnitId = baseUnitId,
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
