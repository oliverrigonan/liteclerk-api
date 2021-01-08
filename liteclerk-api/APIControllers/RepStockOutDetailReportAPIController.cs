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
    public class RepStockOutDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepStockOutDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetStockOutDetailReport(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var stockOutItems = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.TrnStockOut_OTId.OTDate >= Convert.ToDateTime(startDate)
                    && d.TrnStockOut_OTId.OTDate <= Convert.ToDateTime(endDate)
                    && d.TrnStockOut_OTId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnStockOut_OTId.BranchId == branchId
                    && d.TrnStockOut_OTId.IsLocked == true
                    select new DTO.TrnStockOutItemDTO
                    {
                        Id = d.Id,
                        OTId = d.OTId,
                        StockOut = new DTO.TrnStockOutDTO
                        {
                            Id = d.TrnStockOut_OTId.Id,
                            BranchId = d.TrnStockOut_OTId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnStockOut_OTId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnStockOut_OTId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnStockOut_OTId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnStockOut_OTId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnStockOut_OTId.MstCurrency_CurrencyId.Currency
                            },
                            OTNumber = d.TrnStockOut_OTId.OTNumber,
                            OTDate = d.TrnStockOut_OTId.OTDate.ToShortDateString(),
                            ManualNumber = d.TrnStockOut_OTId.ManualNumber,
                            DocumentReference = d.TrnStockOut_OTId.DocumentReference,
                            AccountId = d.TrnStockOut_OTId.AccountId,
                            Account = new DTO.MstAccountDTO
                            {
                                ManualCode = d.TrnStockOut_OTId.MstAccount_AccountId.ManualCode,
                                Account = d.TrnStockOut_OTId.MstAccount_AccountId.Account
                            },
                            ArticleId = d.TrnStockOut_OTId.ArticleId,
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.TrnStockOut_OTId.MstArticle_ArticleId.ManualCode,
                                Article = d.TrnStockOut_OTId.MstArticle_ArticleId.Article
                            },
                            Remarks = d.TrnStockOut_OTId.Remarks,
                            PreparedByUserId = d.TrnStockOut_OTId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockOut_OTId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnStockOut_OTId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnStockOut_OTId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockOut_OTId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnStockOut_OTId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnStockOut_OTId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockOut_OTId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnStockOut_OTId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnStockOut_OTId.Amount,
                            Status = d.TrnStockOut_OTId.Status
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

                return StatusCode(200, stockOutItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

    }
}
