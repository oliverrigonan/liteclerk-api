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
    public class RepSalesOrderDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepSalesOrderDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetSalesOrderDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var salesOrderItems = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.TrnSalesOrder_SOId.SODate >= Convert.ToDateTime(startDate)
                    && d.TrnSalesOrder_SOId.SODate <= Convert.ToDateTime(endDate)
                    && d.TrnSalesOrder_SOId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnSalesOrder_SOId.BranchId == branchId
                    && d.TrnSalesOrder_SOId.IsLocked == true
                    select new DTO.TrnSalesOrderItemDTO
                    {
                        Id = d.Id,
                        SOId = d.SOId,
                        SalesOrder = new DTO.TrnSalesOrderDTO
                        {
                            Id = d.TrnSalesOrder_SOId.Id,
                            BranchId = d.TrnSalesOrder_SOId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                BranchCode = d.TrnSalesOrder_SOId.MstCompanyBranch_BranchId.BranchCode,
                                ManualCode = d.TrnSalesOrder_SOId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnSalesOrder_SOId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnSalesOrder_SOId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                CurrencyCode = d.TrnSalesOrder_SOId.MstCurrency_CurrencyId.CurrencyCode,
                                ManualCode = d.TrnSalesOrder_SOId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnSalesOrder_SOId.MstCurrency_CurrencyId.Currency
                            },
                            SONumber = d.TrnSalesOrder_SOId.SONumber,
                            SODate = d.TrnSalesOrder_SOId.SODate.ToShortDateString(),
                            ManualNumber = d.TrnSalesOrder_SOId.ManualNumber,
                            DocumentReference = d.TrnSalesOrder_SOId.DocumentReference,
                            CustomerId = d.TrnSalesOrder_SOId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesOrder_SOId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesOrder_SOId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnSalesOrder_SOId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            },
                            TermId = d.TrnSalesOrder_SOId.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                TermCode = d.TrnSalesOrder_SOId.MstTerm_TermId.TermCode,
                                ManualCode = d.TrnSalesOrder_SOId.MstTerm_TermId.ManualCode,
                                Term = d.TrnSalesOrder_SOId.MstTerm_TermId.Term
                            },
                            DateNeeded = d.TrnSalesOrder_SOId.DateNeeded.ToShortDateString(),
                            Remarks = d.TrnSalesOrder_SOId.Remarks,
                            SoldByUserId = d.TrnSalesOrder_SOId.SoldByUserId,
                            SoldByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesOrder_SOId.MstUser_SoldByUserId.Username,
                                Fullname = d.TrnSalesOrder_SOId.MstUser_SoldByUserId.Fullname
                            },
                            PreparedByUserId = d.TrnSalesOrder_SOId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesOrder_SOId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnSalesOrder_SOId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnSalesOrder_SOId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesOrder_SOId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnSalesOrder_SOId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnSalesOrder_SOId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnSalesOrder_SOId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnSalesOrder_SOId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnSalesOrder_SOId.Amount,
                            Status = d.TrnSalesOrder_SOId.Status,
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

                return StatusCode(200, salesOrderItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
