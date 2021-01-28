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
    public class RepPurchaseRequestDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepPurchaseRequestDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetPurchaseRequestDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var purchaseRequestItems = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.TrnPurchaseRequest_PRId.PRDate >= Convert.ToDateTime(startDate)
                    && d.TrnPurchaseRequest_PRId.PRDate <= Convert.ToDateTime(endDate)
                    && d.TrnPurchaseRequest_PRId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnPurchaseRequest_PRId.BranchId == branchId
                    && d.TrnPurchaseRequest_PRId.IsLocked == true
                    && d.TrnPurchaseRequest_PRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.TrnPurchaseRequestItemDTO
                    {
                        Id = d.Id,
                        PRId = d.PRId,
                        PurchaseRequest = new DTO.TrnPurchaseRequestDTO
                        {
                            Id = d.TrnPurchaseRequest_PRId.Id,
                            BranchId = d.TrnPurchaseRequest_PRId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnPurchaseRequest_PRId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnPurchaseRequest_PRId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnPurchaseRequest_PRId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnPurchaseRequest_PRId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnPurchaseRequest_PRId.MstCurrency_CurrencyId.Currency
                            },
                            PRNumber = d.TrnPurchaseRequest_PRId.PRNumber,
                            PRDate = d.TrnPurchaseRequest_PRId.PRDate.ToShortDateString(),
                            ManualNumber = d.TrnPurchaseRequest_PRId.ManualNumber,
                            DocumentReference = d.TrnPurchaseRequest_PRId.DocumentReference,
                            SupplierId = d.TrnPurchaseRequest_PRId.SupplierId,
                            Supplier = new DTO.MstArticleSupplierDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnPurchaseRequest_PRId.MstArticle_SupplierId.ManualCode
                                },
                                Supplier = d.TrnPurchaseRequest_PRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier,
                            },
                            TermId = d.TrnPurchaseRequest_PRId.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.TrnPurchaseRequest_PRId.MstTerm_TermId.ManualCode,
                                Term = d.TrnPurchaseRequest_PRId.MstTerm_TermId.Term
                            },
                            DateNeeded = d.TrnPurchaseRequest_PRId.DateNeeded.ToShortDateString(),
                            Remarks = d.TrnPurchaseRequest_PRId.Remarks,
                            RequestedByUserId = d.TrnPurchaseRequest_PRId.RequestedByUserId,
                            RequestedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_RequestedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_RequestedByUserId.Fullname
                            },
                            PreparedByUserId = d.TrnPurchaseRequest_PRId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnPurchaseRequest_PRId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnPurchaseRequest_PRId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnPurchaseRequest_PRId.Amount,
                            Status = d.TrnPurchaseRequest_PRId.Status,
                            IsCancelled = d.TrnPurchaseRequest_PRId.IsCancelled,
                            IsPrinted = d.TrnPurchaseRequest_PRId.IsPrinted,
                            IsLocked = d.TrnPurchaseRequest_PRId.IsLocked,
                            CreatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_CreatedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_CreatedByUserId.Fullname
                            },
                            CreatedDateTime = d.TrnPurchaseRequest_PRId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                            UpdatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnPurchaseRequest_PRId.MstUser_UpdatedByUserId.Username,
                                Fullname = d.TrnPurchaseRequest_PRId.MstUser_UpdatedByUserId.Fullname
                            },
                            UpdatedDateTime = d.TrnPurchaseRequest_PRId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
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

                return StatusCode(200, purchaseRequestItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
