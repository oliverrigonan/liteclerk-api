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

        public async Task UpdateArticleInventory(Int32 articleInventoryId)
        {
            try
            {
                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == articleInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory != null)
                {
                    Decimal quantity = 0;
                    Decimal cost = 0;
                    Decimal amount = 0;

                    List<DBSets.SysInventoryDBSet> inventories = await (
                        from d in _dbContext.SysInventories
                        where d.ArticleItemInventoryId == articleInventoryId
                        select d
                    ).ToListAsync();

                    if (inventories.Any())
                    {
                        quantity = inventories.Sum(d => d.Quantity);

                        var lastPurchaseCost = from d in inventories
                                               where d.RRId != null
                                               select d;

                        if (lastPurchaseCost.Any())
                        {
                            cost = lastPurchaseCost.OrderByDescending(d => d.Id).FirstOrDefault().Cost;
                        }
                        else
                        {
                            var stockInCost = from d in inventories
                                              where d.INId != null
                                              select d;

                            if (stockInCost.Any())
                            {
                                cost = stockInCost.FirstOrDefault().Cost;
                            }
                        }

                        amount = quantity * cost;
                    }

                    DBSets.MstArticleItemInventoryDBSet updateItemInventory = itemInventory;
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
                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == RRId
                     select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt != null)
                {
                    List<DBSets.TrnReceivingReceiptItemDBSet> receivingReceiptItems = await (
                        from d in _dbContext.TrnReceivingReceiptItems
                        where d.RRId == RRId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (receivingReceiptItems.Any())
                    {
                        foreach (var receivingReceiptItem in receivingReceiptItems)
                        {
                            DBSets.MstArticleItemDBSet item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == receivingReceiptItem.ItemId
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleInventoryId = 0;

                                Decimal quantity = receivingReceiptItem.BaseQuantity;
                                Decimal cost = receivingReceiptItem.BaseCost;
                                Decimal amount = receivingReceiptItem.BaseQuantity * receivingReceiptItem.BaseCost;

                                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.ArticleId == receivingReceiptItem.ItemId
                                     && d.BranchId == receivingReceipt.BranchId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    articleInventoryId = itemInventory.Id;
                                }
                                else
                                {
                                    DBSets.MstArticleItemInventoryDBSet newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                    {
                                        ArticleId = receivingReceiptItem.ItemId,
                                        BranchId = receivingReceipt.BranchId,
                                        InventoryCode = "RR-" + receivingReceipt.RRNumber,
                                        Quantity = quantity,
                                        Cost = cost,
                                        Amount = amount
                                    };

                                    _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                    await _dbContext.SaveChangesAsync();

                                    articleInventoryId = newItemInventory.Id;
                                }

                                if (articleInventoryId != 0)
                                {
                                    DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = receivingReceipt.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = receivingReceiptItem.ItemId,
                                        ArticleItemInventoryId = articleInventoryId,
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

                                    await UpdateArticleInventory(articleInventoryId);

                                    if (receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any())
                                    {
                                        if (receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Kitting == "COMPONENT")
                                        {
                                            List<DBSets.MstArticleItemComponentDBSet> articleItemComponents = await (
                                                from d in _dbContext.MstArticleItemComponents
                                                where d.ArticleId == receivingReceiptItem.ItemId
                                                select d
                                            ).ToListAsync();

                                            if (articleItemComponents.Any())
                                            {
                                                foreach (var articleItemComponent in articleItemComponents)
                                                {
                                                    DBSets.MstArticleItemInventoryDBSet componentItemInventory = await (
                                                         from d in _dbContext.MstArticleItemInventories
                                                         where d.ArticleId == articleItemComponent.ComponentArticleId
                                                         && d.BranchId == receivingReceipt.BranchId
                                                         select d
                                                    ).FirstOrDefaultAsync();

                                                    if (componentItemInventory != null)
                                                    {
                                                        Int32 componentItemInventoryId = componentItemInventory.Id;

                                                        Decimal componentQuantity = articleItemComponent.Quantity * receivingReceiptItem.BaseQuantity;
                                                        Decimal componentCost = componentItemInventory.Cost;
                                                        Decimal componentAmount = (articleItemComponent.Quantity * receivingReceiptItem.BaseQuantity) * componentItemInventory.Cost;

                                                        DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                        {
                                                            BranchId = receivingReceipt.BranchId,
                                                            InventoryDate = DateTime.Today,
                                                            ArticleId = articleItemComponent.ComponentArticleId,
                                                            ArticleItemInventoryId = componentItemInventoryId,
                                                            AccountId = articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ?
                                                                        articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().ExpenseAccountId : 0,
                                                            QuantityIn = componentQuantity,
                                                            QuantityOut = 0,
                                                            Quantity = componentQuantity,
                                                            Cost = componentCost,
                                                            Amount = componentAmount,
                                                            Particulars = receivingReceiptItem.Particulars,
                                                            RRId = RRId,
                                                            SIId = null,
                                                            INId = null,
                                                            OTId = null,
                                                            STId = null,
                                                            SWId = null
                                                        };

                                                        _dbContext.SysInventories.Add(newComponentInventory);
                                                        await _dbContext.SaveChangesAsync();

                                                        await UpdateArticleInventory(componentItemInventoryId);
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
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.RRId == RRId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnReceivingReceiptItemDBSet> receivingReceiptItems = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == RRId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                    select d
                ).ToListAsync();

                if (receivingReceiptItems.Any() == true)
                {
                    foreach (var receivingReceiptItem in receivingReceiptItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == receivingReceiptItem.ItemId
                            && d.BranchId == receivingReceiptItem.TrnReceivingReceipt_RRId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
                DBSets.TrnStockInDBSet stockIn = await (
                     from d in _dbContext.TrnStockIns
                     where d.Id == INId
                     select d
                ).FirstOrDefaultAsync();

                if (stockIn != null)
                {
                    List<DBSets.TrnStockInItemDBSet> stockInItems = await (
                        from d in _dbContext.TrnStockInItems
                        where d.INId == INId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockInItems.Any())
                    {
                        foreach (var stockInItem in stockInItems)
                        {
                            Int32 articleInventoryId = 0;

                            Decimal quantity = stockInItem.BaseQuantity;
                            Decimal cost = stockInItem.BaseCost;
                            Decimal amount = stockInItem.BaseQuantity * stockInItem.BaseCost;

                            DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                 from d in _dbContext.MstArticleItemInventories
                                 where d.ArticleId == stockInItem.ItemId
                                 && d.BranchId == stockIn.BranchId
                                 select d
                            ).FirstOrDefaultAsync();

                            if (itemInventory != null)
                            {
                                articleInventoryId = itemInventory.Id;
                            }
                            else
                            {
                                DBSets.MstArticleItemInventoryDBSet newItemInventory = new DBSets.MstArticleItemInventoryDBSet()
                                {
                                    ArticleId = stockInItem.ItemId,
                                    BranchId = stockIn.BranchId,
                                    InventoryCode = "IN-" + stockIn.INNumber,
                                    Quantity = quantity,
                                    Cost = cost,
                                    Amount = amount
                                };

                                _dbContext.MstArticleItemInventories.Add(newItemInventory);
                                await _dbContext.SaveChangesAsync();

                                articleInventoryId = newItemInventory.Id;
                            }

                            if (articleInventoryId != 0)
                            {
                                DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                {
                                    BranchId = stockIn.BranchId,
                                    InventoryDate = DateTime.Today,
                                    ArticleId = stockInItem.ItemId,
                                    ArticleItemInventoryId = articleInventoryId,
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

                                await UpdateArticleInventory(articleInventoryId);

                                if (stockInItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any())
                                {
                                    if (stockInItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Kitting == "COMPONENT")
                                    {
                                        List<DBSets.MstArticleItemComponentDBSet> articleItemComponents = await (
                                            from d in _dbContext.MstArticleItemComponents
                                            where d.ArticleId == stockInItem.ItemId
                                            select d
                                        ).ToListAsync();

                                        if (articleItemComponents.Any())
                                        {
                                            foreach (var articleItemComponent in articleItemComponents)
                                            {
                                                DBSets.MstArticleItemInventoryDBSet componentItemInventory = await (
                                                     from d in _dbContext.MstArticleItemInventories
                                                     where d.ArticleId == articleItemComponent.ComponentArticleId
                                                     && d.BranchId == stockIn.BranchId
                                                     select d
                                                ).FirstOrDefaultAsync();

                                                if (componentItemInventory != null)
                                                {
                                                    Int32 componentItemInventoryId = componentItemInventory.Id;

                                                    Decimal componentQuantity = articleItemComponent.Quantity * stockInItem.BaseQuantity;
                                                    Decimal componentCost = componentItemInventory.Cost;
                                                    Decimal componentAmount = (articleItemComponent.Quantity * stockInItem.BaseQuantity) * componentItemInventory.Cost;

                                                    DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = stockIn.BranchId,
                                                        InventoryDate = DateTime.Today,
                                                        ArticleId = articleItemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentItemInventoryId,
                                                        AccountId = stockIn.AccountId,
                                                        QuantityIn = componentQuantity,
                                                        QuantityOut = 0,
                                                        Quantity = componentQuantity,
                                                        Cost = componentCost,
                                                        Amount = componentAmount,
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

                                                    await UpdateArticleInventory(componentItemInventoryId);
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
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.INId == INId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnStockInItemDBSet> stockInItems = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == INId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                    select d
                ).ToListAsync();

                if (stockInItems.Any() == true)
                {
                    foreach (var stockInItem in stockInItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockInItem.ItemId
                            && d.BranchId == stockInItem.TrnStockIn_INId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
                DBSets.TrnStockOutDBSet stockOut = await (
                     from d in _dbContext.TrnStockOuts
                     where d.Id == OTId
                     select d
                ).FirstOrDefaultAsync();

                if (stockOut != null)
                {
                    List<DBSets.TrnStockOutItemDBSet> stockOutItems = await (
                        from d in _dbContext.TrnStockOutItems
                        where d.OTId == OTId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockOutItems.Any())
                    {
                        foreach (var stockOutItem in stockOutItems)
                        {
                            Int32 articleInventoryId = Convert.ToInt32(stockOutItem.ItemInventoryId);

                            DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                 from d in _dbContext.MstArticleItemInventories
                                 where d.Id == articleInventoryId
                                 select d
                            ).FirstOrDefaultAsync();

                            if (itemInventory != null)
                            {
                                Decimal quantity = stockOutItem.BaseQuantity;
                                Decimal cost = stockOutItem.BaseCost;
                                Decimal amount = stockOutItem.BaseQuantity * stockOutItem.BaseCost;

                                DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                {
                                    BranchId = stockOut.BranchId,
                                    InventoryDate = DateTime.Today,
                                    ArticleId = stockOutItem.ItemId,
                                    ArticleItemInventoryId = articleInventoryId,
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockOutInventory(Int32 OTId)
        {
            try
            {
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.OTId == OTId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnStockOutItemDBSet> stockOutItems = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == OTId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                    select d
                ).ToListAsync();

                if (stockOutItems.Any() == true)
                {
                    foreach (var stockOutItem in stockOutItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockOutItem.ItemId
                            && d.BranchId == stockOutItem.TrnStockOut_OTId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
                DBSets.TrnStockTransferDBSet stockTransfer = await (
                     from d in _dbContext.TrnStockTransfers
                     where d.Id == STId
                     select d
                ).FirstOrDefaultAsync();

                if (stockTransfer != null)
                {
                    List<DBSets.TrnStockTransferItemDBSet> stockTransferItems = await (
                        from d in _dbContext.TrnStockTransferItems
                        where d.STId == STId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockTransferItems.Any())
                    {
                        foreach (var stockTransferItem in stockTransferItems)
                        {
                            Int32 articleInventoryId = Convert.ToInt32(stockTransferItem.ItemInventoryId);

                            DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                 from d in _dbContext.MstArticleItemInventories
                                 where d.Id == articleInventoryId
                                 select d
                            ).FirstOrDefaultAsync();

                            if (itemInventory != null)
                            {
                                Decimal quantity = stockTransferItem.BaseQuantity;
                                Decimal cost = stockTransferItem.BaseCost;
                                Decimal amount = stockTransferItem.BaseQuantity * stockTransferItem.BaseCost;

                                DBSets.SysInventoryDBSet newOutInventory = new DBSets.SysInventoryDBSet()
                                {
                                    BranchId = stockTransfer.BranchId,
                                    InventoryDate = DateTime.Today,
                                    ArticleId = stockTransferItem.ItemId,
                                    ArticleItemInventoryId = articleInventoryId,
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

                                DBSets.SysInventoryDBSet newInInventory = new DBSets.SysInventoryDBSet()
                                {
                                    BranchId = stockTransfer.ToBranchId,
                                    InventoryDate = DateTime.Today,
                                    ArticleId = stockTransferItem.ItemId,
                                    ArticleItemInventoryId = articleInventoryId,
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockTransferInventory(Int32 STId)
        {
            try
            {
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.STId == STId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnStockTransferItemDBSet> stockTransferItems = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == STId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                    select d
                ).ToListAsync();

                if (stockTransferItems.Any() == true)
                {
                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockTransferItem.ItemId
                            && d.BranchId == stockTransferItem.TrnStockTransfer_STId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == SIId
                     select d
                ).FirstOrDefaultAsync();

                if (salesInvoice != null)
                {
                    List<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItems = await (
                        from d in _dbContext.TrnSalesInvoiceItems
                        where d.SIId == SIId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                        && d.ItemInventoryId != null
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (salesInvoiceItems.Any() == true)
                    {
                        foreach (var salesInvoiceItem in salesInvoiceItems)
                        {
                            DBSets.MstArticleItemDBSet item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == salesInvoiceItem.ItemId
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleInventoryId = Convert.ToInt32(salesInvoiceItem.ItemInventoryId);

                                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.Id == articleInventoryId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    Decimal quantity = salesInvoiceItem.BaseQuantity;
                                    Decimal cost = itemInventory.Cost;
                                    Decimal amount = salesInvoiceItem.BaseQuantity * itemInventory.Cost;

                                    DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = salesInvoice.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = salesInvoiceItem.ItemId,
                                        ArticleItemInventoryId = articleInventoryId,
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

                                    await UpdateArticleInventory(articleInventoryId);

                                    if (item.Kitting == "COMPONENT")
                                    {
                                        List<DBSets.MstArticleItemComponentDBSet> articleItemComponents = await (
                                            from d in _dbContext.MstArticleItemComponents
                                            where d.ArticleId == item.ArticleId
                                            select d
                                        ).ToListAsync();

                                        if (articleItemComponents.Any())
                                        {
                                            foreach (var articleItemComponent in articleItemComponents)
                                            {
                                                DBSets.MstArticleItemInventoryDBSet componentItemInventory = await (
                                                     from d in _dbContext.MstArticleItemInventories
                                                     where d.ArticleId == articleItemComponent.ComponentArticleId
                                                     && d.BranchId == salesInvoice.BranchId
                                                     select d
                                                ).FirstOrDefaultAsync();

                                                if (componentItemInventory != null)
                                                {
                                                    Int32 componentItemInventoryId = componentItemInventory.Id;

                                                    Decimal componentQuantity = articleItemComponent.Quantity * salesInvoiceItem.BaseQuantity;
                                                    Decimal componentCost = componentItemInventory.Cost;
                                                    Decimal componentAmount = (articleItemComponent.Quantity * salesInvoiceItem.BaseQuantity) * componentItemInventory.Cost;

                                                    DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = salesInvoice.BranchId,
                                                        InventoryDate = DateTime.Today,
                                                        ArticleId = articleItemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentItemInventoryId,
                                                        AccountId = articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ?
                                                                    articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().CostAccountId : 0,
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

                                                    await UpdateArticleInventory(componentItemInventoryId);
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
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.SIId == SIId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == SIId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                       d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                    && d.ItemInventoryId != null
                    select d
                ).ToListAsync();

                if (salesInvoiceItems.Any() == true)
                {
                    foreach (var salesInvoiceItem in salesInvoiceItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == salesInvoiceItem.ItemId
                            && d.BranchId == salesInvoiceItem.TrnSalesInvoice_SIId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == SWId
                     select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawal != null)
                {
                    List<DBSets.TrnStockWithdrawalItemDBSet> stockWithdrawalItems = await (
                        from d in _dbContext.TrnStockWithdrawalItems
                        where d.SWId == SWId
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (stockWithdrawalItems.Any())
                    {
                        foreach (var stockWithdrawalItem in stockWithdrawalItems)
                        {
                            DBSets.MstArticleItemDBSet item = await (
                                from d in _dbContext.MstArticleItems
                                where d.ArticleId == stockWithdrawalItem.ItemId
                                && d.MstArticle_ArticleId.IsLocked == true
                                select d
                            ).FirstOrDefaultAsync();

                            if (item != null)
                            {
                                Int32 articleInventoryId = Convert.ToInt32(stockWithdrawalItem.ItemInventoryId);

                                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                                     from d in _dbContext.MstArticleItemInventories
                                     where d.Id == articleInventoryId
                                     select d
                                ).FirstOrDefaultAsync();

                                if (itemInventory != null)
                                {
                                    Decimal quantity = stockWithdrawalItem.BaseQuantity;
                                    Decimal cost = stockWithdrawalItem.BaseCost;
                                    Decimal amount = stockWithdrawalItem.BaseQuantity * stockWithdrawalItem.BaseCost;

                                    DBSets.SysInventoryDBSet newOutInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockWithdrawal.BranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockWithdrawalItem.ItemId,
                                        ArticleItemInventoryId = articleInventoryId,
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

                                    DBSets.SysInventoryDBSet newInInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = stockWithdrawal.FromBranchId,
                                        InventoryDate = DateTime.Today,
                                        ArticleId = stockWithdrawalItem.ItemId,
                                        ArticleItemInventoryId = articleInventoryId,
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteStockWithdrawalInventory(Int32 SWId)
        {
            try
            {
                List<DBSets.SysInventoryDBSet> inventories = await (
                    from d in _dbContext.SysInventories
                    where d.SWId == SWId
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    _dbContext.SysInventories.RemoveRange(inventories);
                    await _dbContext.SaveChangesAsync();
                }

                List<DBSets.TrnStockWithdrawalItemDBSet> stockWithdrawalItems = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == SWId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any()
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true
                    select d
                ).ToListAsync();

                if (stockWithdrawalItems.Any() == true)
                {
                    foreach (var stockWithdrawalItem in stockWithdrawalItems)
                    {
                        List<DBSets.MstArticleItemInventoryDBSet> itemInventories = await (
                            from d in _dbContext.MstArticleItemInventories
                            where d.ArticleId == stockWithdrawalItem.ItemId
                            && d.BranchId == stockWithdrawalItem.TrnStockWithdrawal_SWId.BranchId
                            select d
                        ).ToListAsync();

                        if (itemInventories.Any())
                        {
                            foreach (var itemInventory in itemInventories)
                            {
                                Int32 articleInventoryId = itemInventory.Id;
                                await UpdateArticleInventory(articleInventoryId);
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
