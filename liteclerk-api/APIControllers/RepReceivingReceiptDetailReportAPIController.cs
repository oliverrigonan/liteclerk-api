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
    public class RepReceivingReceiptDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepReceivingReceiptDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetReceivingReceiptDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var receivingReceiptItems = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.TrnReceivingReceipt_RRId.RRDate >= Convert.ToDateTime(startDate)
                    && d.TrnReceivingReceipt_RRId.RRDate <= Convert.ToDateTime(endDate)
                    && d.TrnReceivingReceipt_RRId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnReceivingReceipt_RRId.BranchId == branchId
                    && d.TrnReceivingReceipt_RRId.IsLocked == true
                    && d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.TrnReceivingReceiptItemDTO
                    {
                        Id = d.Id,
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {
                            Id = d.TrnReceivingReceipt_RRId.Id,
                            BranchId = d.TrnReceivingReceipt_RRId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnReceivingReceipt_RRId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnReceivingReceipt_RRId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnReceivingReceipt_RRId.MstCurrency_CurrencyId.Currency
                            },
                            RRNumber = d.TrnReceivingReceipt_RRId.RRNumber,
                            RRDate = d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(),
                            ManualNumber = d.TrnReceivingReceipt_RRId.ManualNumber,
                            DocumentReference = d.TrnReceivingReceipt_RRId.DocumentReference,
                            SupplierId = d.TrnReceivingReceipt_RRId.SupplierId,
                            Supplier = new DTO.MstArticleSupplierDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.ManualCode
                                },
                                Supplier = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier
                            },
                            TermId = d.TrnReceivingReceipt_RRId.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.TrnReceivingReceipt_RRId.MstTerm_TermId.ManualCode,
                                Term = d.TrnReceivingReceipt_RRId.MstTerm_TermId.Term
                            },
                            Remarks = d.TrnReceivingReceipt_RRId.Remarks,
                            ReceivedByUserId = d.TrnReceivingReceipt_RRId.ReceivedByUserId,
                            ReceivedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_ReceivedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_ReceivedByUserId.Fullname
                            },
                            PreparedByUserId = d.TrnReceivingReceipt_RRId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnReceivingReceipt_RRId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnReceivingReceipt_RRId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnReceivingReceipt_RRId.Amount,
                            PaidAmount = d.TrnReceivingReceipt_RRId.PaidAmount,
                            AdjustmentAmount = d.TrnReceivingReceipt_RRId.AdjustmentAmount,
                            BalanceAmount = d.TrnReceivingReceipt_RRId.BalanceAmount,
                            Status = d.TrnReceivingReceipt_RRId.Status,
                            IsCancelled = d.TrnReceivingReceipt_RRId.IsCancelled,
                            IsPrinted = d.TrnReceivingReceipt_RRId.IsPrinted,
                            IsLocked = d.TrnReceivingReceipt_RRId.IsLocked,
                            CreatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_CreatedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_CreatedByUserId.Fullname
                            },
                            CreatedDateTime = d.TrnReceivingReceipt_RRId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                            UpdatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnReceivingReceipt_RRId.MstUser_UpdatedByUserId.Username,
                                Fullname = d.TrnReceivingReceipt_RRId.MstUser_UpdatedByUserId.Fullname
                            },
                            UpdatedDateTime = d.TrnReceivingReceipt_RRId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        POId = d.POId,
                        PurchaseOrder = new DTO.TrnPurchaseOrderDTO
                        {
                            PONumber = d.TrnPurchaseOrder_POId.PONumber,
                            ManualNumber = d.TrnPurchaseOrder_POId.ManualNumber
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
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description
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
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            ManualCode = d.MstTax_VATId.ManualCode,
                            TaxDescription = d.MstTax_VATId.TaxDescription
                        },
                        VATRate = d.VATRate,
                        VATAmount = d.VATAmount,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount,
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

                return StatusCode(200, receivingReceiptItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
