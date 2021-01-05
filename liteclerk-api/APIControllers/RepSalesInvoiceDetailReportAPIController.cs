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
    public class RepSalesInvoiceDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepSalesInvoiceDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetSalesInvoiceDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.TrnSalesInvoice_SIId.SIDate >= Convert.ToDateTime(startDate)
                    && d.TrnSalesInvoice_SIId.SIDate <= Convert.ToDateTime(endDate)
                    && d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnSalesInvoice_SIId.BranchId == branchId
                    && d.TrnSalesInvoice_SIId.IsLocked == true
                    select new DTO.TrnSalesInvoiceItemDTO
                    {
                        Id = d.Id,
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            Id = d.TrnSalesInvoice_SIId.Id,
                            BranchId = d.TrnSalesInvoice_SIId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnSalesInvoice_SIId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnSalesInvoice_SIId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnSalesInvoice_SIId.MstCurrency_CurrencyId.Currency
                            },
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
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            },
                            TermId = d.TrnSalesInvoice_SIId.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.TrnSalesInvoice_SIId.MstTerm_TermId.ManualCode,
                                Term = d.TrnSalesInvoice_SIId.MstTerm_TermId.Term
                            },
                            DateNeeded = d.TrnSalesInvoice_SIId.DateNeeded.ToShortDateString(),
                            Remarks = d.TrnSalesInvoice_SIId.Remarks,
                            SoldByUserId = d.TrnSalesInvoice_SIId.SoldByUserId,
                            SoldByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesInvoice_SIId.MstUser_SoldByUserId.Username,
                                Fullname = d.TrnSalesInvoice_SIId.MstUser_SoldByUserId.Fullname
                            },
                            PreparedByUserId = d.TrnSalesInvoice_SIId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesInvoice_SIId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnSalesInvoice_SIId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnSalesInvoice_SIId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesInvoice_SIId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnSalesInvoice_SIId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnSalesInvoice_SIId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesInvoice_SIId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnSalesInvoice_SIId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnSalesInvoice_SIId.Amount,
                            PaidAmount = d.TrnSalesInvoice_SIId.PaidAmount,
                            AdjustmentAmount = d.TrnSalesInvoice_SIId.AdjustmentAmount,
                            BalanceAmount = d.TrnSalesInvoice_SIId.BalanceAmount,
                            Status = d.TrnSalesInvoice_SIId.Status
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventoryId.InventoryCode
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_ItemJobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_ItemJobTypeId.ManualCode,
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            DiscountCode = d.MstDiscount_DiscountId.DiscountCode,
                            ManualCode = d.MstDiscount_DiscountId.ManualCode,
                            Discount = d.MstDiscount_DiscountId.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_VATId.TaxCode,
                            ManualCode = d.MstTax_VATId.ManualCode,
                            TaxDescription = d.MstTax_VATId.TaxDescription
                        },
                        VATRate = d.VATRate,
                        VATAmount = d.VATAmount,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnitId.UnitCode,
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoiceItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
