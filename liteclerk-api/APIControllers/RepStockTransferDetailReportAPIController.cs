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
    public class RepStockTransferDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepStockTransferDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetStockTransferDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var stockTransferItems = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.TrnStockTransfer_STId.STDate >= Convert.ToDateTime(startDate)
                    && d.TrnStockTransfer_STId.STDate <= Convert.ToDateTime(endDate)
                    && d.TrnStockTransfer_STId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnStockTransfer_STId.BranchId == branchId
                    && d.TrnStockTransfer_STId.IsLocked == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.TrnStockTransferItemDTO
                    {
                        Id = d.Id,
                        STId = d.STId,
                        StockTransfer = new DTO.TrnStockTransferDTO
                        {
                            Id = d.TrnStockTransfer_STId.Id,
                            BranchId = d.TrnStockTransfer_STId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnStockTransfer_STId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnStockTransfer_STId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnStockTransfer_STId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnStockTransfer_STId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnStockTransfer_STId.MstCurrency_CurrencyId.Currency
                            },
                            STNumber = d.TrnStockTransfer_STId.STNumber,
                            STDate = d.TrnStockTransfer_STId.STDate.ToShortDateString(),
                            ManualNumber = d.TrnStockTransfer_STId.ManualNumber,
                            DocumentReference = d.TrnStockTransfer_STId.DocumentReference,
                            ToBranchId = d.TrnStockTransfer_STId.ToBranchId,
                            ToBranch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnStockTransfer_STId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnStockTransfer_STId.MstCompanyBranch_BranchId.Branch
                            },
                            AccountId = d.TrnStockTransfer_STId.AccountId,
                            Account = new DTO.MstAccountDTO
                            {
                                ManualCode = d.TrnStockTransfer_STId.MstAccount_AccountId.ManualCode,
                                Account = d.TrnStockTransfer_STId.MstAccount_AccountId.Account
                            },
                            ArticleId = d.TrnStockTransfer_STId.ArticleId,
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.TrnStockTransfer_STId.MstArticle_ArticleId.ManualCode,
                                Article = d.TrnStockTransfer_STId.MstArticle_ArticleId.Article
                            },
                            Remarks = d.TrnStockTransfer_STId.Remarks,
                            PreparedByUserId = d.TrnStockTransfer_STId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockTransfer_STId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnStockTransfer_STId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnStockTransfer_STId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockTransfer_STId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnStockTransfer_STId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnStockTransfer_STId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockTransfer_STId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnStockTransfer_STId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnStockTransfer_STId.Amount,
                            Status = d.TrnStockTransfer_STId.Status,
                            IsCancelled = d.TrnStockTransfer_STId.IsCancelled,
                            IsPrinted = d.TrnStockTransfer_STId.IsPrinted,
                            IsLocked = d.TrnStockTransfer_STId.IsLocked,
                            CreatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockTransfer_STId.MstUser_CreatedByUserId.Username,
                                Fullname = d.TrnStockTransfer_STId.MstUser_CreatedByUserId.Fullname
                            },
                            CreatedDateTime = d.TrnStockTransfer_STId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                            UpdatedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockTransfer_STId.MstUser_UpdatedByUserId.Username,
                                Fullname = d.TrnStockTransfer_STId.MstUser_UpdatedByUserId.Fullname
                            },
                            UpdatedDateTime = d.TrnStockTransfer_STId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
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

                return StatusCode(200, stockTransferItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
