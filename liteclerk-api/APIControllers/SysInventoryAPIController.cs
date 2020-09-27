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
    public class SysInventoryAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysInventoryAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpGet("list/stockIn/{INId}")]
        public async Task<ActionResult> GetStockInInventoryList(Int32 INId)
        {
            try
            {
                IEnumerable<DTO.SysInventoryDTO> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.INId == INId
                    select new DTO.SysInventoryDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryDate = d.InventoryDate.ToShortDateString(),
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode : "",
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : "",
                            }
                        },
                        ArticleItemInventoryId = d.ArticleItemInventoryId,
                        ArticleItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ArticleItemInventoryId.InventoryCode
                        },
                        QuantityIn = d.QuantityIn,
                        QuantityOut = d.QuantityOut,
                        Quantity = d.Quantity,
                        Cost = d.Cost,
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {

                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {

                        },
                        INId = d.INId,
                        StockIn = new DTO.TrnStockInDTO
                        {

                        },
                        OTId = d.OTId,
                        StockOut = new DTO.TrnStockOutDTO
                        {

                        },
                        STId = d.STId,
                        StockTransfer = new DTO.TrnStockTransferDTO
                        {

                        },
                        SWId = d.SWId,
                        StockWithdrawal = new DTO.TrnStockWithdrawalDTO
                        {

                        },
                        ILId = d.ILId,
                        Inventory = new DTO.TrnInventoryDTO
                        {

                        }
                    }
                ).ToListAsync();

                return StatusCode(200, inventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/salesInvoice/{SIId}")]
        public async Task<ActionResult> GetSalesInvoiceInventoryList(Int32 SIId)
        {
            try
            {
                IEnumerable<DTO.SysInventoryDTO> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.SIId == SIId
                    select new DTO.SysInventoryDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryDate = d.InventoryDate.ToShortDateString(),
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode : "",
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : "",
                            }
                        },
                        ArticleItemInventoryId = d.ArticleItemInventoryId,
                        ArticleItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ArticleItemInventoryId.InventoryCode
                        },
                        QuantityIn = d.QuantityIn,
                        QuantityOut = d.QuantityOut,
                        Quantity = d.Quantity,
                        Cost = d.Cost,
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {

                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {

                        },
                        INId = d.INId,
                        StockIn = new DTO.TrnStockInDTO
                        {

                        },
                        OTId = d.OTId,
                        StockOut = new DTO.TrnStockOutDTO
                        {

                        },
                        STId = d.STId,
                        StockTransfer = new DTO.TrnStockTransferDTO
                        {

                        },
                        SWId = d.SWId,
                        StockWithdrawal = new DTO.TrnStockWithdrawalDTO
                        {

                        },
                        ILId = d.ILId,
                        Inventory = new DTO.TrnInventoryDTO
                        {

                        }
                    }
                ).ToListAsync();

                return StatusCode(200, inventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/fromInventoryLedger/{month}/{year}/{ILId}")]
        public async Task<ActionResult> GetInventoryListFromInventoryLedger(Int32 month, Int32 year, Int32 ILId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DBSets.SysInventoryDBSet> getInventoryLedgerInventories = await (
                    from d in _dbContext.SysInventories
                    where d.BranchId == loginUser.BranchId
                    && d.InventoryDate.Month == month
                    && d.InventoryDate.Year == year
                    select d
                ).ToListAsync();

                if (getInventoryLedgerInventories.Any())
                {
                    foreach (var getInventoryLedgerInventory in getInventoryLedgerInventories)
                    {
                        DBSets.SysInventoryDBSet updateInventoryILId = getInventoryLedgerInventory;
                        updateInventoryILId.ILId = ILId;

                        await _dbContext.SaveChangesAsync();
                    }
                }

                IEnumerable<DTO.SysInventoryDTO> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.InventoryDate.Month == month
                    && d.InventoryDate.Year == year
                    && d.ILId == ILId
                    select new DTO.SysInventoryDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryDate = d.InventoryDate.ToShortDateString(),
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode : "",
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : "",
                            }
                        },
                        ArticleItemInventoryId = d.ArticleItemInventoryId,
                        ArticleItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ArticleItemInventoryId.InventoryCode
                        },
                        QuantityIn = d.QuantityIn,
                        QuantityOut = d.QuantityOut,
                        Quantity = d.Quantity,
                        Cost = d.Cost,
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {

                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {

                        },
                        INId = d.INId,
                        StockIn = new DTO.TrnStockInDTO
                        {

                        },
                        OTId = d.OTId,
                        StockOut = new DTO.TrnStockOutDTO
                        {

                        },
                        STId = d.STId,
                        StockTransfer = new DTO.TrnStockTransferDTO
                        {

                        },
                        SWId = d.SWId,
                        StockWithdrawal = new DTO.TrnStockWithdrawalDTO
                        {

                        },
                        ILId = d.ILId,
                        Inventory = new DTO.TrnInventoryDTO
                        {

                        }
                    }
                ).ToListAsync();

                return StatusCode(200, inventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
