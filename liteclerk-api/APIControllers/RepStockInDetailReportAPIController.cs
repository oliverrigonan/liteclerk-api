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
    public class RepStockInDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepStockInDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetStockInDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var stockInItems = await (
                    from d in _dbContext.TrnStockInItems
                    where d.TrnStockIn_INId.INDate >= Convert.ToDateTime(startDate)
                    && d.TrnStockIn_INId.INDate <= Convert.ToDateTime(endDate)
                    && d.TrnStockIn_INId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnStockIn_INId.BranchId == branchId
                    && d.TrnStockIn_INId.IsLocked == true
                    select new DTO.TrnStockInItemDTO
                    {
                        Id = d.Id,
                        INId = d.INId,
                        StockIn = new DTO.TrnStockInDTO
                        {
                            Id = d.TrnStockIn_INId.Id,
                            BranchId = d.TrnStockIn_INId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnStockIn_INId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnStockIn_INId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnStockIn_INId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnStockIn_INId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnStockIn_INId.MstCurrency_CurrencyId.Currency
                            },
                            INNumber = d.TrnStockIn_INId.INNumber,
                            INDate = d.TrnStockIn_INId.INDate.ToShortDateString(),
                            ManualNumber = d.TrnStockIn_INId.ManualNumber,
                            DocumentReference = d.TrnStockIn_INId.DocumentReference,
                            AccountId = d.TrnStockIn_INId.AccountId,
                            Account = new DTO.MstAccountDTO
                            {
                                ManualCode = d.TrnStockIn_INId.MstAccount_AccountId.ManualCode,
                                Account = d.TrnStockIn_INId.MstAccount_AccountId.Account
                            },
                            ArticleId = d.TrnStockIn_INId.ArticleId,
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.TrnStockIn_INId.MstArticle_ArticleId.ManualCode,
                                Article = d.TrnStockIn_INId.MstArticle_ArticleId.Article
                            },
                            Remarks = d.TrnStockIn_INId.Remarks,
                            PreparedByUserId = d.TrnStockIn_INId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockIn_INId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnStockIn_INId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnStockIn_INId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockIn_INId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnStockIn_INId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnStockIn_INId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnStockIn_INId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnStockIn_INId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnStockIn_INId.Amount,
                            Status = d.TrnStockIn_INId.Status
                        },
                        ItemId = d.ItemId,
                        JOId = d.TrnJobOrder_JOId.Id,
                        JobOrder = new DTO.TrnJobOrderDTO
                        {
                            JONumber = d.TrnJobOrder_JOId.JONumber,
                            JODate = d.TrnJobOrder_JOId.JODate.ToShortDateString(),
                            ManualNumber = d.TrnJobOrder_JOId.ManualNumber
                        },
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

                return StatusCode(200, stockInItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
