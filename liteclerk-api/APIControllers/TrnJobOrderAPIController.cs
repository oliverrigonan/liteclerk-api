using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

        [HttpGet("list/byStatus/{status}")]
        public async Task<ActionResult> GetJobOrderListByStatus(String status)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == loginUser.BranchId
                    && d.Status == status
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
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
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                            CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true ?
                                           d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            }
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                IsInventory = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory : false
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory,
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        CurrentDepartment = d.CurrentDepartment,
                        CurrentDepartmentStatus = d.CurrentDepartmentStatus,
                        CurrentDepartmentUserFullName = d.CurrentDepartmentUserFullName,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
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

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetJobOrderListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == loginUser.BranchId
                    && d.JODate >= Convert.ToDateTime(startDate)
                    && d.JODate <= Convert.ToDateTime(endDate)
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
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
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                            CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true ?
                                           d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            }
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                IsInventory = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory : false
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory,
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        CurrentDepartment = d.CurrentDepartment,
                        CurrentDepartmentStatus = d.CurrentDepartmentStatus,
                        CurrentDepartmentUserFullName = d.CurrentDepartmentUserFullName,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
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

        [HttpGet("list/bySalesInvoice/{SIId}")]
        public async Task<ActionResult> GetJobOrderListBySalesInvoice(Int32 SIId)
        {
            try
            {
                var jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.SIId == SIId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
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
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                            CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true ?
                                           d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            }
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                IsInventory = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory : false
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory,
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        CurrentDepartment = d.CurrentDepartment,
                        CurrentDepartmentStatus = d.CurrentDepartmentStatus,
                        CurrentDepartmentUserFullName = d.CurrentDepartmentUserFullName,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
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
                var jobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.Id == id
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.TrnJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
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
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                            CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true ?
                                           d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            }
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                IsInventory = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory : false
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory,
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        CurrentDepartment = d.CurrentDepartment,
                        CurrentDepartmentStatus = d.CurrentDepartmentStatus,
                        CurrentDepartmentUserFullName = d.CurrentDepartmentUserFullName,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job order.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job order.");
                }

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String JONumber = "0000000001";
                DBSets.TrnJobOrderDBSet lastJobOrder = await (
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJobOrder != null)
                {
                    Int32 lastSINumber = Convert.ToInt32(lastJobOrder.JONumber) + 0000000001;
                    JONumber = PadZeroes(lastSINumber, 10);
                }

                var newJobOrder = new DBSets.TrnJobOrderDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    JONumber = JONumber,
                    JODate = DateTime.Today,
                    ManualNumber = JONumber,
                    DocumentReference = "",
                    DateScheduled = DateTime.Today,
                    DateNeeded = DateTime.Today,
                    SIId = null,
                    SIItemId = null,
                    ItemId = item.ArticleId,
                    ItemJobTypeId = jobType.Id,
                    Quantity = 0,
                    UnitId = item.UnitId,
                    Remarks = "",
                    BaseQuantity = 0,
                    BaseUnitId = item.UnitId,
                    CurrentDepartment = "",
                    CurrentDepartmentStatus = "",
                    CurrentDepartmentUserFullName = "",
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a job order.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a job order.");
                }

                var jobOrder = await (
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

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJobOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == trnJobOrderDTO.ItemJobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                var unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                var itemUnit = await (
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

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnJobOrderDTO.Status
                    && d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveJobOrder = jobOrder;
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
                saveJobOrder.UpdatedByUserId = loginUserId;
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a job order.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a job order.");
                }

                var jobOrder = await (
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

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJobOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnJobOrderDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var jobType = await (
                    from d in _dbContext.MstJobTypes
                    where d.Id == trnJobOrderDTO.ItemJobTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (jobType == null)
                {
                    return StatusCode(404, "Job type not found.");
                }

                var unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == trnJobOrderDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                var itemUnit = await (
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

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJobOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnJobOrderDTO.Status
                    && d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockJobOrder = jobOrder;
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
                lockJobOrder.UpdatedByUserId = loginUserId;
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a job order.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a job order.");
                }

                var jobOrder = await (
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

                var unlockJobOrder = jobOrder;
                unlockJobOrder.IsLocked = false;
                unlockJobOrder.UpdatedByUserId = loginUserId;
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a job order.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a job order.");
                }

                var jobOrder = await (
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

                var unlockJobOrder = jobOrder;
                unlockJobOrder.IsCancelled = true;
                unlockJobOrder.UpdatedByUserId = loginUserId;
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job order.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job order.");
                }

                var jobOrder = await (
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

        [HttpPost("create/fromSalesInvoice/{SIId}")]
        public async Task<ActionResult> CreateJobOrderFromSalesInvoice(Int32 SIId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var salesInvoice = await (
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

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var salesInvoiceItems = await (
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
                    var existingJobOrder = await (
                        from d in _dbContext.TrnJobOrders
                        where d.SIItemId == salesInvoiceItem.Id
                        select d
                    ).FirstOrDefaultAsync();

                    if (existingJobOrder == null)
                    {
                        String JONumber = "0000000001";
                        var lastJobOrder = await (
                            from d in _dbContext.TrnJobOrders
                            where d.BranchId == loginUser.BranchId
                            orderby d.Id descending
                            select d
                        ).FirstOrDefaultAsync();

                        if (lastJobOrder != null)
                        {
                            Int32 lastJONumber = Convert.ToInt32(lastJobOrder.JONumber) + 0000000001;
                            JONumber = PadZeroes(lastJONumber, 10);
                        }

                        var itemUnit = await (
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

                        Int32 baseUnitId = salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().UnitId : 0;

                        var newJobOrder = new DBSets.TrnJobOrderDBSet()
                        {
                            BranchId = Convert.ToInt32(loginUser.BranchId),
                            CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
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
                            CurrentDepartment = "",
                            CurrentDepartmentStatus = "",
                            CurrentDepartmentUserFullName = "",
                            PreparedByUserId = salesInvoice.PreparedByUserId,
                            CheckedByUserId = salesInvoice.CheckedByUserId,
                            ApprovedByUserId = salesInvoice.ApprovedByUserId,
                            Status = codeTableStatus.CodeValue,
                            IsCancelled = false,
                            IsPrinted = false,
                            IsLocked = true,
                            CreatedByUserId = loginUser.Id,
                            CreatedDateTime = DateTime.Now,
                            UpdatedByUserId = loginUser.Id,
                            UpdatedDateTime = DateTime.Now
                        };

                        _dbContext.TrnJobOrders.Add(newJobOrder);
                        await _dbContext.SaveChangesAsync();

                        var jobType = await (
                            from d in _dbContext.MstJobTypes
                            where d.Id == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                            select d
                        ).FirstOrDefaultAsync();

                        if (jobType != null)
                        {
                            var jobTypeAttachments = await (
                                from d in _dbContext.MstJobTypeAttachments
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeAttachments.Any() == true)
                            {
                                foreach (var jobTypeAttachment in jobTypeAttachments)
                                {
                                    var newJobOrderAttachment = new DBSets.TrnJobOrderAttachmentDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        AttachmentCode = jobTypeAttachment.AttachmentCode,
                                        AttachmentType = jobTypeAttachment.AttachmentType,
                                        AttachmentURL = "",
                                        Particulars = "",
                                        IsPrinted = jobTypeAttachment.IsPrinted
                                    };

                                    _dbContext.TrnJobOrderAttachments.Add(newJobOrderAttachment);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            var jobTypeDepartments = await (
                                from d in _dbContext.MstJobTypeDepartments
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeDepartments.Any() == true)
                            {
                                String status = "";

                                var codeTableJobOrderDepartmentStatus = await (
                                    from d in _dbContext.MstCodeTables
                                    where d.Category == "PRODUCTION DEPARTMENT STATUS"
                                    select d
                                ).FirstOrDefaultAsync();

                                if (codeTableJobOrderDepartmentStatus != null)
                                {
                                    status = codeTableJobOrderDepartmentStatus.CodeValue;
                                }

                                foreach (var jobTypeDepartment in jobTypeDepartments)
                                {
                                    var newJobOrderDepartment = new DBSets.TrnJobOrderDepartmentDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        JobDepartmentId = jobTypeDepartment.JobDepartmentId,
                                        Particulars = "",
                                        Status = status,
                                        StatusByUserId = loginUserId,
                                        StatusUpdatedDateTime = DateTime.Now,
                                        AssignedToUserId = loginUserId,
                                        SequenceNumber = jobTypeDepartment.SequenceNumber,
                                        IsRequired = jobTypeDepartment.IsRequired
                                    };

                                    _dbContext.TrnJobOrderDepartments.Add(newJobOrderDepartment);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            var jobTypeInformations = await (
                                from d in _dbContext.MstJobTypeInformations
                                where d.JobTypeId == Convert.ToInt32(salesInvoiceItem.ItemJobTypeId)
                                select d
                            ).ToListAsync();

                            if (jobTypeInformations.Any() == true)
                            {
                                foreach (var jobTypeInformation in jobTypeInformations)
                                {
                                    var newJobOrderInformation = new DBSets.TrnJobOrderInformationDBSet()
                                    {
                                        JOId = newJobOrder.Id,
                                        InformationCode = jobTypeInformation.InformationCode,
                                        InformationGroup = jobTypeInformation.InformationGroup,
                                        Value = "",
                                        Particulars = "",
                                        IsPrinted = jobTypeInformation.IsPrinted,
                                        InformationByUserId = loginUserId,
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

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintJobOrder(Int32 id)
        {
            FontFactory.RegisterDirectories();

            Font fontSegoeUI08 = FontFactory.GetFont("Segoe UI light", 8);
            Font fontSegoeUI08Bold = FontFactory.GetFont("Segoe UI light", 8, Font.BOLD);
            Font fontSegoeUI09 = FontFactory.GetFont("Segoe UI light", 9);
            Font fontSegoeUI09Bold = FontFactory.GetFont("Segoe UI light", 9, Font.BOLD);
            Font fontSegoeUI10 = FontFactory.GetFont("Segoe UI light", 10);
            Font fontSegoeUI10Bold = FontFactory.GetFont("Segoe UI light", 10, Font.BOLD);
            Font fontSegoeUI11 = FontFactory.GetFont("Segoe UI light", 11);
            Font fontSegoeUI11Bold = FontFactory.GetFont("Segoe UI light", 11, Font.BOLD);
            Font fontSegoeUI12 = FontFactory.GetFont("Segoe UI light", 12);
            Font fontSegoeUI12Bold = FontFactory.GetFont("Segoe UI light", 12, Font.BOLD);
            Font fontSegoeUI13 = FontFactory.GetFont("Segoe UI light", 13);
            Font fontSegoeUI13Bold = FontFactory.GetFont("Segoe UI light", 13, Font.BOLD);
            Font fontSegoeUI14 = FontFactory.GetFont("Segoe UI light", 14);
            Font fontSegoeUI14Bold = FontFactory.GetFont("Segoe UI light", 14, Font.BOLD);
            Font fontSegoeUI15 = FontFactory.GetFont("Segoe UI light", 15);
            Font fontSegoeUI15Bold = FontFactory.GetFont("Segoe UI light", 15, Font.BOLD);
            Font fontSegoeUI16 = FontFactory.GetFont("Segoe UI light", 16);
            Font fontSegoeUI16Bold = FontFactory.GetFont("Segoe UI light", 16, Font.BOLD);

            Document document = new Document(PageSize.Letter, 30f, 30f, 30f, 30f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.Black, Element.ALIGN_MIDDLE, 5F)));

            Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            var loginUser = await (
                from d in _dbContext.MstUsers
                where d.Id == loginUserId
                select d
            ).FirstOrDefaultAsync();

            if (loginUser != null)
            {
                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm != null)
                {
                    if (loginUserForm.CanPrint == true)
                    {
                        String companyName = "";
                        String companyAddress = "";
                        String companyTaxNumber = "";
                        String companyImageURL = "";

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyImageURL = loginUser.MstCompany_CompanyId.ImageURL;
                        }

                        DBSets.TrnJobOrderDBSet jobOrder = await (
                            from d in _dbContext.TrnJobOrders
                            where d.Id == id
                            && d.IsLocked == true
                            select d
                        ).FirstOrDefaultAsync();

                        if (jobOrder != null)
                        {
                            String reprinted = "";
                            if (jobOrder.IsPrinted == true)
                            {
                                reprinted = "(REPRINTED)";
                            }

                            //String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\colorideas_logo.png";
                            String logoPath = companyImageURL;

                            Image logoPhoto = Image.GetInstance(logoPath);
                            logoPhoto.Alignment = Image.ALIGN_JUSTIFIED;

                            PdfPCell logoPhotoPdfCell = new PdfPCell(logoPhoto, true) { FixedHeight = 40f };
                            logoPhotoPdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                            PdfPTable tableHeader = new PdfPTable(2);
                            tableHeader.SetWidths(new float[] { 80f, 20f });
                            tableHeader.WidthPercentage = 100f;
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontSegoeUI13Bold)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(logoPhotoPdfCell) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 3f, Rowspan = 4 });
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyAddress, fontSegoeUI09)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyTaxNumber, fontSegoeUI09)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt") + " " + reprinted, fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                            tableHeader.AddCell(new PdfPCell(new Phrase("JOB ORDER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String SKUCode = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                              jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                            String Barcode = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                              jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                            String Description = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                              jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";
                            String jobType = jobOrder.MstJobType_ItemJobTypeId.JobType;
                            String Quantity = jobOrder.Quantity.ToString("#,#00.00");
                            String Unit = jobOrder.MstUnit_UnitId.Unit;
                            String remarks = jobOrder.Remarks;

                            String branch = jobOrder.MstCompanyBranch_BranchId.Branch;
                            String JONumber = "JO-" + jobOrder.MstCompanyBranch_BranchId.ManualCode + "-" + jobOrder.JONumber;
                            String JODate = jobOrder.JODate.ToString("MMMM dd, yyyy");
                            String dateScheduled = jobOrder.DateScheduled.ToString("MMMM dd, yyyy");
                            String dateNeeded = jobOrder.DateNeeded.ToString("MMMM dd, yyyy");
                            String manualNumber = jobOrder.ManualNumber;
                            String documentReference = jobOrder.DocumentReference;

                            PdfPTable tableJobOrder = new PdfPTable(4);
                            tableJobOrder.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableJobOrder.WidthPercentage = 100;

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("SKU Code:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(SKUCode, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(JONumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Bar Code:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(Barcode, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Description:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(Description, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(JODate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Job Type:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(jobType, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Date Scheduled:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(dateScheduled, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Quantity:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(Quantity, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Date Needed:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(dateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Unit:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(Unit, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Remarks:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrder.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableJobOrder);

                            PdfPTable tableJobOrderInformationAndAttachment = new PdfPTable(3);
                            tableJobOrderInformationAndAttachment.SetWidths(new float[] { 60f, 3f, 40f });
                            tableJobOrderInformationAndAttachment.WidthPercentage = 100;
                            tableJobOrderInformationAndAttachment.SplitLate = false;
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("Information", fontSegoeUI09Bold)) { Border = PdfCell.TOP_BORDER | PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("Attachment", fontSegoeUI09Bold)) { Border = PdfCell.TOP_BORDER | PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            IEnumerable<DBSets.TrnJobOrderInformationDBSet> jobOrderInformations = await (
                                from d in _dbContext.TrnJobOrderInformations
                                where d.JOId == jobOrder.Id
                                && d.Value != String.Empty
                                select d
                            ).ToListAsync();

                            PdfPTable tableJobOrderInformation = new PdfPTable(1);
                            tableJobOrderInformation.WidthPercentage = 100;

                            if (jobOrderInformations.Any())
                            {
                                var groupedJobOrderInformationGroups = from d in jobOrderInformations
                                                                       group d by new
                                                                       {
                                                                           d.InformationGroup
                                                                       }
                                                                       into g
                                                                       select new
                                                                       {
                                                                           g.Key.InformationGroup
                                                                       };

                                if (groupedJobOrderInformationGroups.ToList().Any())
                                {
                                    Boolean hasInformationCodeColumn = false;

                                    foreach (var groupedJobOrderInformationGroup in groupedJobOrderInformationGroups)
                                    {
                                        var jobOrderInformationByGroups = from d in jobOrderInformations
                                                                          where d.InformationGroup == groupedJobOrderInformationGroup.InformationGroup
                                                                          select d;

                                        if (jobOrderInformationByGroups.Any() == true)
                                        {
                                            Int32 numberOfColumns = jobOrderInformationByGroups.Count();

                                            PdfPTable tableJobOrderInformationData = new PdfPTable(numberOfColumns);
                                            tableJobOrderInformationData.WidthPercentage = 100;

                                            Int32 countInformationCodeColumns = 0;

                                            foreach (var jobOrderInformationByGroup in jobOrderInformationByGroups)
                                            {
                                                countInformationCodeColumns += 1;

                                                if (hasInformationCodeColumn == false)
                                                {
                                                    tableJobOrderInformationData.AddCell(new PdfPCell(new Phrase(jobOrderInformationByGroup.InformationCode, fontSegoeUI08Bold)) { PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                                    if (countInformationCodeColumns == numberOfColumns)
                                                    {
                                                        hasInformationCodeColumn = true;
                                                    }
                                                }
                                            }

                                            foreach (var jobOrderInformationByGroup in jobOrderInformationByGroups)
                                            {
                                                tableJobOrderInformationData.AddCell(new PdfPCell(new Phrase(jobOrderInformationByGroup.Value, fontSegoeUI08)) { PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            }

                                            tableJobOrderInformation.AddCell(new PdfPCell(tableJobOrderInformationData) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                                        }
                                    }
                                }
                            }

                            IEnumerable<DBSets.TrnJobOrderAttachmentDBSet> jobOrderAttachments = await (
                                from d in _dbContext.TrnJobOrderAttachments
                                where d.JOId == jobOrder.Id
                                && d.AttachmentURL != String.Empty
                                select d
                            ).ToListAsync();

                            PdfPTable tableJobOrderAttachment = new PdfPTable(1);
                            tableJobOrderAttachment.SetWidths(new float[] { 100f });
                            tableJobOrderAttachment.WidthPercentage = 100;

                            if (jobOrderAttachments.Any())
                            {
                                foreach (var jobOrderAttachment in jobOrderAttachments)
                                {
                                    tableJobOrderAttachment.AddCell(new PdfPCell(new Phrase(jobOrderAttachment.AttachmentCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                    if (String.IsNullOrEmpty(jobOrderAttachment.AttachmentURL) == true)
                                    {
                                        tableJobOrderAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    }
                                    else
                                    {
                                        Image attachmentPhoto = Image.GetInstance(new Uri(jobOrderAttachment.AttachmentURL));
                                        PdfPCell attachmentPhotoPdfCell = new PdfPCell(attachmentPhoto, true) { };
                                        attachmentPhotoPdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                        tableJobOrderAttachment.AddCell(new PdfPCell(attachmentPhotoPdfCell) { Border = 0, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    }
                                }
                            }

                            IEnumerable<DBSets.TrnJobOrderDepartmentDBSet> jobOrderDepartments = await (
                                from d in _dbContext.TrnJobOrderDepartments
                                where d.JOId == jobOrder.Id
                                select d
                            ).ToListAsync();

                            PdfPTable tableJobOrderDepartment = new PdfPTable(5);
                            tableJobOrderDepartment.SetWidths(new float[] { 100f, 100f, 70f, 110f, 100f });
                            tableJobOrderDepartment.WidthPercentage = 100;
                            tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase("Department", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase("Assigned To", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase("Status", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase("Status Date / Time", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            if (jobOrderDepartments.Any())
                            {
                                foreach (var jobOrderDepartment in jobOrderDepartments)
                                {
                                    tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase(jobOrderDepartment.MstJobDepartment_JobDepartmentId.JobDepartment, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase(jobOrderDepartment.MstUser_AssignedToUserId.Fullname, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase(jobOrderDepartment.Status, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase(jobOrderDepartment.StatusUpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"), fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderDepartment.AddCell(new PdfPCell(new Phrase(jobOrderDepartment.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                }
                            }

                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(tableJobOrderInformation) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(tableJobOrderAttachment) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 3 });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("Department Status", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 3 });
                            tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(tableJobOrderDepartment) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 3 });
                            document.Add(tableJobOrderInformationAndAttachment);

                            String preparedBy = jobOrder.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = jobOrder.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = jobOrder.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(4);
                            tableUsers.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Received by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Date Received:", fontSegoeUI09Bold)) { HorizontalAlignment = 0, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableUsers);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Add("No rights to print sales invoice - job orders");

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Add("No rights to print sales invoice - job orders");

                    document.Add(paragraph);
                }
            }
            else
            {
                document.Add(line);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}
