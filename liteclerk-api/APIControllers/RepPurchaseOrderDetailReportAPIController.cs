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
    public class RepPurchaseOrderDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepPurchaseOrderDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetPurchaseOrderDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var purchaseOrderItems = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.TrnPurchaseOrder_POId.PODate >= Convert.ToDateTime(startDate)
                    && d.TrnPurchaseOrder_POId.PODate <= Convert.ToDateTime(endDate)
                    && d.TrnPurchaseOrder_POId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnPurchaseOrder_POId.BranchId == branchId
                    && d.TrnPurchaseOrder_POId.IsLocked == true
                    select new DTO.TrnPurchaseOrderItemDTO
                    {
                        Id = d.Id,
                        POId = d.POId,
                        PurchaseOrder = new DTO.TrnPurchaseOrderDTO
                        {
                            Id = d.TrnPurchaseOrder_POId.Id,
                            BranchId = d.TrnPurchaseOrder_POId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnPurchaseOrder_POId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnPurchaseOrder_POId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnPurchaseOrder_POId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnPurchaseOrder_POId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnPurchaseOrder_POId.MstCurrency_CurrencyId.Currency
                            },
                            PONumber = d.TrnPurchaseOrder_POId.PONumber,
                            PODate = d.TrnPurchaseOrder_POId.PODate.ToShortDateString(),
                            ManualNumber = d.TrnPurchaseOrder_POId.ManualNumber,
                            DocumentReference = d.TrnPurchaseOrder_POId.DocumentReference,
                            SupplierId = d.TrnPurchaseOrder_POId.SupplierId,
                            Supplier = new DTO.MstArticleSupplierDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnPurchaseOrder_POId.MstArticle_SupplierId.ManualCode
                                },
                                Supplier = d.TrnPurchaseOrder_POId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnPurchaseOrder_POId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
                            },
                            TermId = d.TrnPurchaseOrder_POId.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.TrnPurchaseOrder_POId.MstTerm_TermId.ManualCode,
                                Term = d.TrnPurchaseOrder_POId.MstTerm_TermId.Term
                            },
                            DateNeeded = d.TrnPurchaseOrder_POId.DateNeeded.ToShortDateString(),
                            Remarks = d.TrnPurchaseOrder_POId.Remarks,
                            PRId = d.TrnPurchaseOrder_POId.PRId,
                            PurchaseRequest = new DTO.TrnPurchaseRequestDTO
                            {
                                PRNumber = d.TrnPurchaseOrder_POId.PRId != null ? d.TrnPurchaseOrder_POId.TrnPurchaseRequest_PRId.PRNumber : "",
                                ManualNumber = d.TrnPurchaseOrder_POId.PRId != null ? d.TrnPurchaseOrder_POId.TrnPurchaseRequest_PRId.ManualNumber : "",
                            },
                            RequestedByUserId = d.TrnPurchaseOrder_POId.RequestedByUserId,
                            RequestedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseOrder_POId.MstUser_RequestedByUserId.Username,
                                Fullname = d.TrnPurchaseOrder_POId.MstUser_RequestedByUserId.Fullname
                            },
                            PreparedByUserId = d.TrnPurchaseOrder_POId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseOrder_POId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnPurchaseOrder_POId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnPurchaseOrder_POId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseOrder_POId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnPurchaseOrder_POId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnPurchaseOrder_POId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseOrder_POId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnPurchaseOrder_POId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnPurchaseOrder_POId.Amount,
                            Status = d.TrnPurchaseOrder_POId.Status,
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
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            RRVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().RRVATId : 0,
                            RRVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxRate : 0
                            },
                            SIVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId : 0,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate : 0
                            },
                            WTAXId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId : 0,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate : 0
                            }
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Cost = d.Cost,
                        Amount = d.Amount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).ToListAsync();

                return StatusCode(200, purchaseOrderItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
