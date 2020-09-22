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
                                String IVNumber = "0000000001";
                                DBSets.SysInventoryDBSet lastInventory = await (
                                    from d in _dbContext.SysInventories
                                    where d.BranchId == stockIn.BranchId
                                    orderby d.Id descending
                                    select d
                                ).FirstOrDefaultAsync();

                                if (lastInventory != null)
                                {
                                    Int32 lastIVNumber = Convert.ToInt32(lastInventory.IVNumber) + 0000000001;
                                    IVNumber = PadZeroes(lastIVNumber, 10);
                                }

                                DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                {
                                    BranchId = stockIn.BranchId,
                                    IVNumber = IVNumber,
                                    IVDate = DateTime.Today,
                                    ArticleId = stockInItem.ItemId,
                                    ArticleItemInventoryId = articleInventoryId,
                                    QuantityIn = quantity,
                                    QuantityOut = 0,
                                    Quantity = quantity,
                                    Cost = cost,
                                    Amount = amount,
                                    Particulars = stockInItem.Particulars,
                                    AccountId = stockInItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                stockInItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().AssetAccountId : 0,
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

                                                    String componentIVNumber = "0000000001";
                                                    DBSets.SysInventoryDBSet lastComponentInventory = await (
                                                        from d in _dbContext.SysInventories
                                                        where d.BranchId == stockIn.BranchId
                                                        orderby d.Id descending
                                                        select d
                                                    ).FirstOrDefaultAsync();

                                                    if (lastComponentInventory != null)
                                                    {
                                                        Int32 lastComponentIVNumber = Convert.ToInt32(lastComponentInventory.IVNumber) + 0000000001;
                                                        componentIVNumber = PadZeroes(lastComponentIVNumber, 10);
                                                    }

                                                    DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = stockIn.BranchId,
                                                        IVNumber = componentIVNumber,
                                                        IVDate = DateTime.Today,
                                                        ArticleId = articleItemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentItemInventoryId,
                                                        QuantityIn = componentQuantity,
                                                        QuantityOut = 0,
                                                        Quantity = componentQuantity,
                                                        Cost = componentCost,
                                                        Amount = componentAmount,
                                                        Particulars = stockInItem.Particulars,
                                                        AccountId = articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ?
                                                                    articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().AssetAccountId : 0,
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
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                       d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
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
                        && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                           d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                        && d.ItemInventoryId != null
                        && d.BaseQuantity > 0
                        select d
                    ).ToListAsync();

                    if (salesInvoiceItems.Any() == true)
                    {
                        foreach (var salesInvoiceItem in salesInvoiceItems)
                        {
                            if (salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any())
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

                                    String IVNumber = "0000000001";
                                    DBSets.SysInventoryDBSet lastInventory = await (
                                        from d in _dbContext.SysInventories
                                        where d.BranchId == salesInvoice.BranchId
                                        orderby d.Id descending
                                        select d
                                    ).FirstOrDefaultAsync();

                                    if (lastInventory != null)
                                    {
                                        Int32 lastIVNumber = Convert.ToInt32(lastInventory.IVNumber) + 0000000001;
                                        IVNumber = PadZeroes(lastIVNumber, 10);
                                    }

                                    DBSets.SysInventoryDBSet newInventory = new DBSets.SysInventoryDBSet()
                                    {
                                        BranchId = salesInvoice.BranchId,
                                        IVNumber = IVNumber,
                                        IVDate = DateTime.Today,
                                        ArticleId = salesInvoiceItem.ItemId,
                                        ArticleItemInventoryId = articleInventoryId,
                                        QuantityIn = 0,
                                        QuantityOut = quantity,
                                        Quantity = quantity * -1,
                                        Cost = cost,
                                        Amount = amount * -1,
                                        Particulars = salesInvoiceItem.Particulars,
                                        AccountId = salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                    salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().AssetAccountId : 0,
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

                                    if (salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Kitting == "COMPONENT")
                                    {
                                        List<DBSets.MstArticleItemComponentDBSet> articleItemComponents = await (
                                            from d in _dbContext.MstArticleItemComponents
                                            where d.ArticleId == salesInvoiceItem.MstArticle_ItemId.Id
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

                                                    String componentIVNumber = "0000000001";
                                                    DBSets.SysInventoryDBSet lastComponentInventory = await (
                                                        from d in _dbContext.SysInventories
                                                        where d.BranchId == salesInvoice.BranchId
                                                        orderby d.Id descending
                                                        select d
                                                    ).FirstOrDefaultAsync();

                                                    if (lastComponentInventory != null)
                                                    {
                                                        Int32 lastComponentIVNumber = Convert.ToInt32(lastComponentInventory.IVNumber) + 0000000001;
                                                        componentIVNumber = PadZeroes(lastComponentIVNumber, 10);
                                                    }

                                                    DBSets.SysInventoryDBSet newComponentInventory = new DBSets.SysInventoryDBSet()
                                                    {
                                                        BranchId = salesInvoice.BranchId,
                                                        IVNumber = componentIVNumber,
                                                        IVDate = DateTime.Today,
                                                        ArticleId = articleItemComponent.ComponentArticleId,
                                                        ArticleItemInventoryId = componentItemInventoryId,
                                                        QuantityIn = 0,
                                                        QuantityOut = componentQuantity,
                                                        Quantity = componentQuantity * -1,
                                                        Cost = componentCost,
                                                        Amount = componentAmount * -1,
                                                        Particulars = salesInvoiceItem.Particulars,
                                                        AccountId = articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ?
                                                                    articleItemComponent.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().SalesAccountId : 0,
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
    }
}
