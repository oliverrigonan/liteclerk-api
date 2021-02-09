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
    public class RepStockCardAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepStockCardAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byItem/{itemId}/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetStockCardList(Int32 itemId, String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                List<DTO.RepStockCardDTO> stockCardList = new List<DTO.RepStockCardDTO>();
                Decimal runningBalance = 0;

                var beginningInventories = await (
                    from d in _dbContext.SysInventories
                    where d.ArticleId == itemId
                    && d.InventoryDate < Convert.ToDateTime(startDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.MstArticleItemInventory_ArticleItemInventoryId.BranchId == branchId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (beginningInventories.Any() == true)
                {
                    stockCardList.Add(new DTO.RepStockCardDTO()
                    {
                        Document = "Beginning Balance",
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = beginningInventories.FirstOrDefault().MstCompanyBranch_BranchId.ManualCode,
                            Branch = beginningInventories.FirstOrDefault().MstCompanyBranch_BranchId.Branch
                        },
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = beginningInventories.FirstOrDefault().MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = beginningInventories.FirstOrDefault().MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = beginningInventories.FirstOrDefault().MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = beginningInventories.FirstOrDefault().MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description
                        },
                        RRId = beginningInventories.FirstOrDefault().RRId,
                        SIId = beginningInventories.FirstOrDefault().SIId,
                        INId = beginningInventories.FirstOrDefault().INId,
                        OTId = beginningInventories.FirstOrDefault().OTId,
                        STId = beginningInventories.FirstOrDefault().STId,
                        SWId = beginningInventories.FirstOrDefault().SWId,
                        ILId = beginningInventories.FirstOrDefault().ILId,
                        InQuantity = beginningInventories.Sum(s => s.QuantityIn),
                        OutQuantity = beginningInventories.Sum(s => s.QuantityOut),
                        BalanceQuantity = beginningInventories.Sum(s => s.Quantity),
                        RunningQuantity = beginningInventories.Sum(s => s.Quantity),
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = beginningInventories.FirstOrDefault().MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode,
                            Unit = beginningInventories.FirstOrDefault().MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit
                        },
                        Cost = beginningInventories.FirstOrDefault().MstArticleItemInventory_ArticleItemInventoryId.Cost,
                        Amount = beginningInventories.Sum(s => s.Amount)
                    });

                    runningBalance += beginningInventories.Sum(s => s.Quantity);
                }

                var currentInventories = await (
                    from d in _dbContext.SysInventories
                    where d.ArticleId == itemId
                    && d.InventoryDate >= Convert.ToDateTime(startDate)
                    && d.InventoryDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.MstArticleItemInventory_ArticleItemInventoryId.BranchId == branchId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (currentInventories.Any() == true)
                {
                    foreach (var currentInventory in currentInventories)
                    {
                        runningBalance += currentInventory.Quantity;

                        String document = "";
                        if (currentInventory.RRId != null) { document = "RR-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnReceivingReceipt_RRId.RRNumber; }
                        if (currentInventory.SIId != null) { document = "SI-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnSalesInvoice_SIId.SINumber; }
                        if (currentInventory.INId != null) { document = "IN-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnStockIn_INId.INNumber; }
                        if (currentInventory.OTId != null) { document = "OT-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnStockOut_OTId.OTNumber; }
                        if (currentInventory.STId != null) { document = "ST-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnStockTransfer_STId.STNumber; }
                        if (currentInventory.SWId != null) { document = "SW-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnStockWithdrawal_SWId.SWNumber; }
                        if (currentInventory.ILId != null) { document = "IL-" + currentInventory.MstCompanyBranch_BranchId.ManualCode + "-" + currentInventory.TrnInventory_ILId.ILNumber; }

                        stockCardList.Add(new DTO.RepStockCardDTO()
                        {
                            Document = document,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = currentInventory.MstCompanyBranch_BranchId.ManualCode,
                                Branch = currentInventory.MstCompanyBranch_BranchId.Branch
                            },
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = currentInventory.MstArticle_ArticleId.ManualCode
                                },
                                SKUCode = currentInventory.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                                BarCode = currentInventory.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                                Description = currentInventory.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description
                            },
                            RRId = currentInventory.RRId,
                            SIId = currentInventory.SIId,
                            INId = currentInventory.INId,
                            OTId = currentInventory.OTId,
                            STId = currentInventory.STId,
                            SWId = currentInventory.SWId,
                            ILId = currentInventory.ILId,
                            InQuantity = currentInventory.QuantityIn,
                            OutQuantity = currentInventory.QuantityOut,
                            BalanceQuantity = currentInventory.Quantity,
                            RunningQuantity = runningBalance,
                            Unit = new DTO.MstUnitDTO
                            {
                                ManualCode = currentInventory.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode,
                                Unit = currentInventory.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit
                            },
                            Cost = currentInventory.Cost,
                            Amount = currentInventory.Amount
                        });
                    }
                }

                return StatusCode(200, await Task.FromResult(stockCardList));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
