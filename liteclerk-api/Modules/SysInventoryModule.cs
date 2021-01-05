using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysInventoryModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysInventoryModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateArticleInventory(Int32 articleItemInventoryId)
        {
            try
            {
                var itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == articleItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory != null)
                {
                    Decimal quantity = 0;
                    Decimal cost = 0;
                    Decimal amount = 0;

                    var inventories = await (
                        from d in _dbContext.SysInventories
                        where d.ArticleItemInventoryId == articleItemInventoryId
                        select d
                    ).ToListAsync();

                    if (inventories.Any() == true)
                    {
                        quantity = inventories.Sum(d => d.Quantity);

                        var lastPurchaseCost = from d in inventories
                                               where d.RRId != null
                                               select d;

                        if (lastPurchaseCost.Any() == true)
                        {
                            cost = lastPurchaseCost.OrderByDescending(d => d.Id).FirstOrDefault().Cost;
                        }
                        else
                        {
                            var stockInCost = from d in inventories
                                              where d.INId != null
                                              select d;

                            if (stockInCost.Any() == true)
                            {
                                cost = stockInCost.FirstOrDefault().Cost;
                            }
                        }

                        amount = quantity * cost;
                    }

                    var updateItemInventory = itemInventory;
                    updateItemInventory.Quantity = quantity;
                    updateItemInventory.Cost = cost;
                    updateItemInventory.Amount = amount;

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertReceivingReceiptInventory(Int32 RRId)
        {
            try
            {
                var receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == RRId
                     select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt != null)
                {
                    var receivingReceiptItems = await (
                        from d in _dbContext.TrnReceivingReceiptItems
                        where d.RRId == RRId
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (receivingReceiptItems.Any() == true)
                    {
                        foreach (var receivingReceiptItem in receivingReceiptItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == receivingReceiptItem.ItemId
                                && d.IsInventory == true
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleItemInventoryId = 0;

                                Decimal quantity = receivingReceiptItem.BaseQuantity;
                                Decimal cost = receivingReceiptItem.BaseCost;
                                Decimal amount = quantity * cost;

                                var itemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.ArticleId == receivingReceiptItem.ItemId
                                     && d.BranchId == receivingReceipt.BranchId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    articleItemInventoryId = itemInventory.Id;
                                }
                                else
                                {
                                    String inventoryCode = "RR-" + receivingReceipt.MstCompanyBranch_BranchId.BranchCode + "-" + receivingReceipt.RRNumber;

                                    var newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                    {
                                        ArticleId = receivingReceiptItem.ItemId,
                                        BranchId = receivingReceipt.BranchId,
                                        InventoryCode = inventoryCode,
                                        Quantity = quantity,
                                        Cost = cost,
                                        Amount = amount
                                    };

                                    _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                    await _dbContext.SaveChangesAsync();

                                    articleItemInventoryId = newItemInventory.Id;
                                }

                                if (articleItemInventoryId != 0)
                                {
                                    var newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = receivingReceipt.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = receivingReceiptItem.ItemId,
                                        ArticleItemInventoryId = articleItemInventoryId,
                                        AccountId = item.ExpenseAccountId,
                                        QuantityIn = quantity,
                                        QuantityOut = 0,
                                        Quantity = quantity,
                                        Cost = cost,
                                        Amount = amount,
                                        Particulars = receivingReceiptItem.Particulars,
                                        RRId = RRId,
                                        SIId = null,
                                        INId = null,
                                        OTId = null,
                                        STId = null,
                                        SWId = null
                                    };

                                    _dbContext.SysInventories.Add(newInventory);
                                    await _dbContext.SaveChangesAsync();

                                    await UpdateArticleInventory(articleItemInventoryId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteReceivingReceiptInventory(Int32 RRId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.RRId == RRId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var receivingReceiptItems = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == RRId
                    select d
                ).ToListAsync();

                if (receivingReceiptItems.Any() == true)
                {
                    foreach (var receivingReceiptItem in receivingReceiptItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == receivingReceiptItem.ItemId
                            && d.BranchId == receivingReceiptItem.TrnReceivingReceipt_RRId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertStockInInventory(Int32 INId)
        {
            try
            {
                var stockIn = await (
                     from d in _dbContext.TrnStockIns
                     where d.Id == INId
                     select d
                ).FirstOrDefaultAsync();

                if (stockIn != null)
                {
                    var stockInItems = await (
                        from d in _dbContext.TrnStockInItems
                        where d.INId == INId
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockInItems.Any() == true)
                    {
                        foreach (var stockInItem in stockInItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == stockInItem.ItemId
                                && d.IsInventory == true
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleItemInventoryId = 0;

                                Decimal quantity = stockInItem.BaseQuantity;
                                Decimal cost = stockInItem.BaseCost;
                                Decimal amount = quantity * cost;

                                var itemInventory = await (
                                    from d in _dbContext.MstArticleItemInventories
                                    where d.ArticleId == stockInItem.ItemId
                                    && d.BranchId == stockIn.BranchId
                                    select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    articleItemInventoryId = itemInventory.Id;
                                }
                                else
                                {
                                    String inventoryCode = "IN-" + stockIn.MstCompanyBranch_BranchId.BranchCode + "-" + stockIn.INNumber;

                                    var newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                    {
                                        ArticleId = stockInItem.ItemId,
                                        BranchId = stockIn.BranchId,
                                        InventoryCode = inventoryCode,
                                        Quantity = quantity,
                                        Cost = cost,
                                        Amount = amount
                                    };

                                    _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                    await _dbContext.SaveChangesAsync();

                                    articleItemInventoryId = newItemInventory.Id;
                                }

                                if (articleItemInventoryId != 0)
                                {
                                    var newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockIn.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockInItem.ItemId,
                                        ArticleItemInventoryId = articleItemInventoryId,
                                        AccountId = stockIn.AccountId,
                                        QuantityIn = quantity,
                                        QuantityOut = 0,
                                        Quantity = quantity,
                                        Cost = cost,
                                        Amount = amount,
                                        Particulars = stockInItem.Particulars,
                                        RRId = null,
                                        SIId = null,
                                        INId = INId,
                                        OTId = null,
                                        STId = null,
                                        SWId = null
                                    };

                                    _dbContext.SysInventories.Add(newInventory);
                                    await _dbContext.SaveChangesAsync();

                                    await UpdateArticleInventory(articleItemInventoryId);

                                    if (stockInItem.JOId != null && item.Kitting == "PRODUCED")
                                    {
                                        var itemComponents = await (
                                            from d in _dbContext.MstArticleItemComponents
                                            where d.ArticleId == stockInItem.ItemId
                                            select d
                                        ).ToListAsync();

                                        if (itemComponents.Any() == true)
                                        {
                                            foreach (var itemComponent in itemComponents)
                                            {
                                                var componentItemInventory = await (
                                                     from d in _dbContext.MstArticleItemInventories
                                                     where d.ArticleId == itemComponent.ComponentArticleId
                                                     && d.BranchId == stockIn.BranchId
                                                     select d
                                                ).FirstOrDefaultAsync();

                                                if (componentItemInventory != null)
                                                {
                                                    Int32 componentArticleItemInventoryId = componentItemInventory.Id;

                                                    Decimal componentQuantity = itemComponent.Quantity * stockInItem.BaseQuantity;
                                                    Decimal componentCost = componentItemInventory.Cost;
                                                    Decimal componentAmount = componentQuantity * componentCost;

                                                    var newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = stockIn.BranchId,
                                                        InventoryDate = DateTime.Today,
                                                        ArticleId = itemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentArticleItemInventoryId,
                                                        AccountId = stockIn.AccountId,
                                                        QuantityIn = 0,
                                                        QuantityOut = componentQuantity,
                                                        Quantity = componentQuantity * -1,
                                                        Cost = componentCost,
                                                        Amount = componentAmount * -1,
                                                        Particulars = stockInItem.Particulars,
                                                        RRId = null,
                                                        SIId = null,
                                                        INId = INId,
                                                        OTId = null,
                                                        STId = null,
                                                        SWId = null
                                                    };

                                                    _dbContext.SysInventories.Add(newComponentInventory);
                                                    await _dbContext.SaveChangesAsync();

                                                    await UpdateArticleInventory(componentArticleItemInventoryId);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockInInventory(Int32 INId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.INId == INId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var stockInItems = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == INId
                    select d
                ).ToListAsync();

                if (stockInItems.Any() == true)
                {
                    foreach (var stockInItem in stockInItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockInItem.ItemId
                            && d.BranchId == stockInItem.TrnStockIn_INId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertStockOutInventory(Int32 OTId)
        {
            try
            {
                var stockOut = await (
                     from d in _dbContext.TrnStockOuts
                     where d.Id == OTId
                     select d
                ).FirstOrDefaultAsync();

                if (stockOut != null)
                {
                    var stockOutItems = await (
                        from d in _dbContext.TrnStockOutItems
                        where d.OTId == OTId
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockOutItems.Any() == true)
                    {
                        foreach (var stockOutItem in stockOutItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == stockOutItem.ItemId
                                && d.IsInventory == true
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleItemInventoryId = Convert.ToInt32(stockOutItem.ItemInventoryId);

                                var itemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.Id == articleItemInventoryId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    Decimal quantity = stockOutItem.BaseQuantity;
                                    Decimal cost = stockOutItem.BaseCost;
                                    Decimal amount = quantity * cost;

                                    var newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockOut.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockOutItem.ItemId,
                                        ArticleItemInventoryId = articleItemInventoryId,
                                        AccountId = stockOut.AccountId,
                                        QuantityIn = 0,
                                        QuantityOut = quantity,
                                        Quantity = quantity * -1,
                                        Cost = cost,
                                        Amount = amount * -1,
                                        Particulars = stockOutItem.Particulars,
                                        RRId = null,
                                        SIId = null,
                                        INId = null,
                                        OTId = OTId,
                                        STId = null,
                                        SWId = null
                                    };

                                    _dbContext.SysInventories.Add(newInventory);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockOutInventory(Int32 OTId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.OTId == OTId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var stockOutItems = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == OTId
                    select d
                ).ToListAsync();

                if (stockOutItems.Any() == true)
                {
                    foreach (var stockOutItem in stockOutItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockOutItem.ItemId
                            && d.BranchId == stockOutItem.TrnStockOut_OTId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertStockTransferInventory(Int32 STId)
        {
            try
            {
                var stockTransfer = await (
                     from d in _dbContext.TrnStockTransfers
                     where d.Id == STId
                     select d
                ).FirstOrDefaultAsync();

                if (stockTransfer != null)
                {
                    var stockTransferItems = await (
                        from d in _dbContext.TrnStockTransferItems
                        where d.STId == STId
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockTransferItems.Any() == true)
                    {
                        foreach (var stockTransferItem in stockTransferItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == stockTransferItem.ItemId
                                && d.IsInventory == true
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 outArticleItemInventoryId = Convert.ToInt32(stockTransferItem.ItemInventoryId);

                                Decimal quantity = stockTransferItem.BaseQuantity;
                                Decimal cost = stockTransferItem.BaseCost;
                                Decimal amount = quantity * cost;

                                var outItemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.Id == outArticleItemInventoryId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (outItemInventory != null)
                                {
                                    var newOutInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockTransfer.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockTransferItem.ItemId,
                                        ArticleItemInventoryId = outArticleItemInventoryId,
                                        AccountId = stockTransfer.AccountId,
                                        QuantityIn = 0,
                                        QuantityOut = quantity,
                                        Quantity = quantity * -1,
                                        Cost = cost,
                                        Amount = amount * -1,
                                        Particulars = stockTransferItem.Particulars,
                                        RRId = null,
                                        SIId = null,
                                        INId = null,
                                        OTId = null,
                                        STId = STId,
                                        SWId = null
                                    };

                                    _dbContext.SysInventories.Add(newOutInventory);
                                    await _dbContext.SaveChangesAsync();

                                    Int32 inArticleItemInventoryId = 0;

                                    var inItemInventory = await (
                                         from d in _dbContext.MstArticleItemInventories
                                         where d.ArticleId == stockTransferItem.ItemId
                                         && d.BranchId == stockTransfer.ToBranchId
                                         select d
                                    ).FirstOrDefaultAsync();

                                    if (inItemInventory != null)
                                    {
                                        inArticleItemInventoryId = inItemInventory.Id;
                                    }
                                    else
                                    {
                                        String inventoryCode = "ST-" + stockTransfer.MstCompanyBranch_ToBranchId.BranchCode + "-" + stockTransfer.STNumber;

                                        var newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                        {
                                            ArticleId = stockTransferItem.ItemId,
                                            BranchId = stockTransfer.ToBranchId,
                                            InventoryCode = inventoryCode,
                                            Quantity = quantity,
                                            Cost = cost,
                                            Amount = amount
                                        };

                                        _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                        await _dbContext.SaveChangesAsync();

                                        inArticleItemInventoryId = newItemInventory.Id;
                                    }

                                    if (inArticleItemInventoryId != 0)
                                    {
                                        var newInInventory = new DBSets.SysInventoryDBSet()
                                        {
                                            BranchId = stockTransfer.ToBranchId,
                                            InventoryDate = DateTime.Today,
                                            ArticleId = stockTransferItem.ItemId,
                                            ArticleItemInventoryId = inArticleItemInventoryId,
                                            AccountId = stockTransfer.AccountId,
                                            QuantityIn = quantity,
                                            QuantityOut = 0,
                                            Quantity = quantity,
                                            Cost = cost,
                                            Amount = amount,
                                            Particulars = stockTransferItem.Particulars,
                                            RRId = null,
                                            SIId = null,
                                            INId = null,
                                            OTId = null,
                                            STId = STId,
                                            SWId = null
                                        };

                                        _dbContext.SysInventories.Add(newInInventory);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockTransferInventory(Int32 STId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.STId == STId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var stockTransferItems = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == STId
                    select d
                ).ToListAsync();

                if (stockTransferItems.Any() == true)
                {
                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockTransferItem.ItemId
                            && d.BranchId == stockTransferItem.TrnStockTransfer_STId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertSalesInvoiceInventory(Int32 SIId)
        {
            try
            {
                var salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == SIId
                     select d
                ).FirstOrDefaultAsync();

                if (salesInvoice != null)
                {
                    var salesInvoiceItems = await (
                        from d in _dbContext.TrnSalesInvoiceItems
                        where d.SIId == SIId
                        select d
                    ).ToListAsync();

                    if (salesInvoiceItems.Any() == true)
                    {
                        foreach (var salesInvoiceItem in salesInvoiceItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == salesInvoiceItem.ItemId
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                if (item.IsInventory == true)
                                {
                                    Int32 articleItemInventoryId = 0;
                                    if (salesInvoiceItem.ItemInventoryId != null)
                                    {
                                        articleItemInventoryId = Convert.ToInt32(salesInvoiceItem.ItemInventoryId);
                                    }

                                    if (articleItemInventoryId != 0)
                                    {
                                        var itemInventory = await (
                                             from d in _dbContext.MstArticleItemInventories
                                             where d.Id == articleItemInventoryId
                                             select d
                                        ).FirstOrDefaultAsync();

                                        if (itemInventory != null)
                                        {
                                            Decimal quantity = salesInvoiceItem.BaseQuantity;
                                            Decimal cost = itemInventory.Cost;
                                            Decimal amount = quantity * cost;

                                            var newInventory = new DBSets.SysInventoryDBSet()
                                            {
                                                BranchId = salesInvoice.BranchId,
                                                InventoryDate = DateTime.Today,
                                                ArticleId = salesInvoiceItem.ItemId,
                                                ArticleItemInventoryId = articleItemInventoryId,
                                                AccountId = item.CostAccountId,
                                                QuantityIn = 0,
                                                QuantityOut = quantity,
                                                Quantity = quantity * -1,
                                                Cost = cost,
                                                Amount = amount * -1,
                                                Particulars = salesInvoiceItem.Particulars,
                                                RRId = null,
                                                SIId = SIId,
                                                INId = null,
                                                OTId = null,
                                                STId = null,
                                                SWId = null
                                            };

                                            _dbContext.SysInventories.Add(newInventory);
                                            await _dbContext.SaveChangesAsync();

                                            await UpdateArticleInventory(articleItemInventoryId);
                                        }
                                    }
                                }
                                else
                                {
                                    if (item.Kitting == "COMPONENT")
                                    {
                                        var itemComponents = await (
                                            from d in _dbContext.MstArticleItemComponents
                                            where d.ArticleId == salesInvoiceItem.ItemId
                                            select d
                                        ).ToListAsync();

                                        if (itemComponents.Any() == true)
                                        {
                                            foreach (var itemComponent in itemComponents)
                                            {
                                                var componentItemInventory = await (
                                                     from d in _dbContext.MstArticleItemInventories
                                                     where d.ArticleId == itemComponent.ComponentArticleId
                                                     && d.BranchId == salesInvoice.BranchId
                                                     select d
                                                ).FirstOrDefaultAsync();

                                                if (componentItemInventory != null)
                                                {
                                                    Int32 componentArticleItemInventoryId = componentItemInventory.Id;
                                                    Int32 componentItemAccountId = itemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() == true ?
                                                                                   itemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().CostAccountId : 0;

                                                    Decimal componentQuantity = itemComponent.Quantity * salesInvoiceItem.BaseQuantity;
                                                    Decimal componentCost = componentItemInventory.Cost;
                                                    Decimal componentAmount = componentQuantity * componentCost;

                                                    DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = salesInvoice.BranchId,
                                                        InventoryDate = DateTime.Today,
                                                        ArticleId = itemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentArticleItemInventoryId,
                                                        AccountId = componentItemAccountId,
                                                        QuantityIn = 0,
                                                        QuantityOut = componentQuantity,
                                                        Quantity = componentQuantity * -1,
                                                        Cost = componentCost,
                                                        Amount = componentAmount * -1,
                                                        Particulars = salesInvoiceItem.Particulars,
                                                        RRId = null,
                                                        SIId = SIId,
                                                        INId = null,
                                                        OTId = null,
                                                        STId = null,
                                                        SWId = null
                                                    };

                                                    _dbContext.SysInventories.Add(newComponentInventory);
                                                    await _dbContext.SaveChangesAsync();

                                                    await UpdateArticleInventory(componentArticleItemInventoryId);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteSalesInvoiceInventory(Int32 SIId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.SIId == SIId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == SIId
                    && d.ItemInventoryId != null
                    select d
                ).ToListAsync();

                if (salesInvoiceItems.Any() == true)
                {
                    foreach (var salesInvoiceItem in salesInvoiceItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == salesInvoiceItem.ItemId
                            && d.BranchId == salesInvoiceItem.TrnSalesInvoice_SIId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertStockWithdrawalInventory(Int32 SWId)
        {
            try
            {
                var stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == SWId
                     select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawal != null)
                {
                    var stockWithdrawalItems = await (
                        from d in _dbContext.TrnStockWithdrawalItems
                        where d.SWId == SWId
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockWithdrawalItems.Any() == true)
                    {
                        foreach (var stockWithdrawalItem in stockWithdrawalItems)
                        {
                            var item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == stockWithdrawalItem.ItemId
                                && d.IsInventory == true
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 outArticleItemInventoryId = Convert.ToInt32(stockWithdrawalItem.ItemInventoryId);

                                Decimal quantity = stockWithdrawalItem.BaseQuantity;
                                Decimal cost = stockWithdrawalItem.BaseCost;
                                Decimal amount = quantity * cost;

                                var outItemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.Id == outArticleItemInventoryId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (outItemInventory != null)
                                {
                                    var newOutInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockWithdrawal.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockWithdrawalItem.ItemId,
                                        ArticleItemInventoryId = outArticleItemInventoryId,
                                        AccountId = item.ExpenseAccountId,
                                        QuantityIn = 0,
                                        QuantityOut = quantity,
                                        Quantity = quantity * -1,
                                        Cost = cost,
                                        Amount = amount * -1,
                                        Particulars = stockWithdrawalItem.Particulars,
                                        RRId = null,
                                        SIId = null,
                                        INId = null,
                                        OTId = null,
                                        STId = null,
                                        SWId = SWId
                                    };

                                    _dbContext.SysInventories.Add(newOutInventory);
                                    await _dbContext.SaveChangesAsync();

                                    Int32 inArticleItemInventoryId = 0;

                                    var inItemInventory = await (
                                         from d in _dbContext.MstArticleItemInventories
                                         where d.ArticleId == stockWithdrawalItem.ItemId
                                         && d.BranchId == stockWithdrawal.FromBranchId
                                         select d
                                    ).FirstOrDefaultAsync();

                                    if (inItemInventory != null)
                                    {
                                        inArticleItemInventoryId = inItemInventory.Id;
                                    }
                                    else
                                    {
                                        String inventoryCode = "SW-" + stockWithdrawal.MstCompanyBranch_FromBranchId.BranchCode + "-" + stockWithdrawal.SWNumber;

                                        var newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                        {
                                            ArticleId = stockWithdrawalItem.ItemId,
                                            BranchId = stockWithdrawal.FromBranchId,
                                            InventoryCode = inventoryCode,
                                            Quantity = quantity,
                                            Cost = cost,
                                            Amount = amount
                                        };

                                        _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                        await _dbContext.SaveChangesAsync();

                                        inArticleItemInventoryId = newItemInventory.Id;
                                    }

                                    if (inArticleItemInventoryId != 0)
                                    {
                                        var newInInventory = new DBSets.SysInventoryDBSet()
                                        {
                                            BranchId = stockWithdrawal.FromBranchId,
                                            InventoryDate = DateTime.Today,
                                            ArticleId = stockWithdrawalItem.ItemId,
                                            ArticleItemInventoryId = inArticleItemInventoryId,
                                            AccountId = item.ExpenseAccountId,
                                            QuantityIn = quantity,
                                            QuantityOut = 0,
                                            Quantity = quantity,
                                            Cost = cost,
                                            Amount = amount,
                                            Particulars = stockWithdrawalItem.Particulars,
                                            RRId = null,
                                            SIId = null,
                                            INId = null,
                                            OTId = null,
                                            STId = null,
                                            SWId = SWId
                                        };

                                        _dbContext.SysInventories.Add(newInInventory);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockWithdrawalInventory(Int32 SWId)
        {
            try
            {
                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.SWId == SWId
                    select d
                ).ToListAsync();

                if (inventories.Any() == true)
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                var stockWithdrawalItems = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == SWId
                    select d
                ).ToListAsync();

                if (stockWithdrawalItems.Any() == true)
                {
                    foreach (var stockWithdrawalItem in stockWithdrawalItems)
                    {
                        var itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockWithdrawalItem.ItemId
                            && d.BranchId == stockWithdrawalItem.TrnStockWithdrawal_SWId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any() == true)
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleItemInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleItemInventoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
