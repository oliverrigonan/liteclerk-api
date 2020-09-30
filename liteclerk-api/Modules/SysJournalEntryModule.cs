using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysJournalEntryModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysJournalEntryModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertInventoryLedgerJournalEntry(Int32 ILId)
        {
            try
            {
                DBSets.TrnInventoryDBSet inventoryLedger = await (
                    from d in _dbContext.TrnInventories
                    where d.Id == ILId
                    select d
                ).FirstOrDefaultAsync();

                if (inventoryLedger != null)
                {
                    DBSets.MstArticleOtherDBSet articleOther = await (
                      from d in _dbContext.MstArticleOthers
                      select d
                    ).FirstOrDefaultAsync();

                    if (articleOther != null)
                    {
                        List<DBSets.SysInventoryDBSet> inventories = await (
                            from d in _dbContext.SysInventories
                            where d.ILId == ILId
                            select d
                        ).ToListAsync();

                        if (inventories.Any())
                        {
                            var inventoryAccounts = from d in inventories
                                                    where d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                                                    group d by new
                                                    {
                                                        d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().AssetAccountId
                                                    } into g
                                                    select new
                                                    {
                                                        g.Key.AssetAccountId,
                                                        Amount = g.Sum(s => s.Amount)
                                                    };

                            if (inventoryAccounts.ToList().Any())
                            {
                                foreach (var inventoryAccount in inventoryAccounts)
                                {
                                    Decimal debitAmount = inventoryAccount.Amount;
                                    Decimal creditAmount = 0;

                                    if (inventoryAccount.Amount < 0)
                                    {
                                        debitAmount = 0;
                                        creditAmount = inventoryAccount.Amount;
                                    }

                                    DBSets.SysJournalEntryDBSet inventoryAccountJournal = new DBSets.SysJournalEntryDBSet
                                    {
                                        BranchId = inventoryLedger.BranchId,
                                        JournalEntryDate = DateTime.Today,
                                        ArticleId = articleOther.ArticleId,
                                        AccountId = inventoryAccount.AssetAccountId,
                                        DebitAmount = debitAmount,
                                        CreditAmount = creditAmount,
                                        Particulars = inventoryLedger.Remarks,
                                        RRId = null,
                                        SIId = null,
                                        CIId = null,
                                        CVId = null,
                                        PMId = null,
                                        RMId = null,
                                        JVId = null,
                                        ILId = inventoryLedger.Id
                                    };

                                    _dbContext.SysJournalEntries.Add(inventoryAccountJournal);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            var expenseAccounts = from d in inventories
                                                  group d by new
                                                  {
                                                      d.AccountId
                                                  } into g
                                                  select new
                                                  {
                                                      g.Key.AccountId,
                                                      Amount = g.Sum(s => s.Amount)
                                                  };

                            if (expenseAccounts.ToList().Any())
                            {
                                foreach (var expenseAccount in expenseAccounts)
                                {
                                    Decimal debitAmount = 0;
                                    Decimal creditAmount = expenseAccount.Amount;

                                    if (expenseAccount.Amount < 0)
                                    {
                                        debitAmount = expenseAccount.Amount * -1;
                                        creditAmount = 0;
                                    }

                                    DBSets.SysJournalEntryDBSet expenseAccountJournal = new DBSets.SysJournalEntryDBSet
                                    {
                                        BranchId = inventoryLedger.BranchId,
                                        JournalEntryDate = DateTime.Today,
                                        ArticleId = articleOther.ArticleId,
                                        AccountId = expenseAccount.AccountId,
                                        DebitAmount = debitAmount,
                                        CreditAmount = creditAmount,
                                        Particulars = inventoryLedger.Remarks,
                                        RRId = null,
                                        SIId = null,
                                        CIId = null,
                                        CVId = null,
                                        PMId = null,
                                        RMId = null,
                                        JVId = null,
                                        ILId = inventoryLedger.Id
                                    };

                                    _dbContext.SysJournalEntries.Add(expenseAccountJournal);
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

        public async Task DeleteInventoryLedgerJournalEntry(Int32 ILId)
        {
            try
            {
                IEnumerable<DBSets.SysJournalEntryDBSet> journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.ILId == ILId
                    select d
                ).ToListAsync();

                if (journalEntries.Any())
                {
                    _dbContext.SysJournalEntries.RemoveRange(journalEntries);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertSalesInvoiceJournalEntry(Int32 SIId)
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
                    if (salesInvoice.Amount != 0)
                    {
                        DBSets.SysJournalEntryDBSet accountsReceivableJournal = new DBSets.SysJournalEntryDBSet
                        {
                            BranchId = salesInvoice.BranchId,
                            JournalEntryDate = DateTime.Today,
                            ArticleId = salesInvoice.CustomerId,
                            AccountId = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId,
                            DebitAmount = salesInvoice.Amount,
                            CreditAmount = 0,
                            Particulars = salesInvoice.Remarks,
                            RRId = null,
                            SIId = salesInvoice.Id,
                            CIId = null,
                            CVId = null,
                            PMId = null,
                            RMId = null,
                            JVId = null,
                            ILId = null
                        };

                        _dbContext.SysJournalEntries.Add(accountsReceivableJournal);
                        await _dbContext.SaveChangesAsync();

                        IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItems = await (
                           from d in _dbContext.TrnSalesInvoiceItems
                           where d.SIId == SIId
                           select d
                        ).ToListAsync();

                        if (salesInvoiceItems.Any())
                        {
                            var salesInvoiceItemSalesAccounts = from d in salesInvoiceItems
                                                                where d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                                                                group d by new
                                                                {
                                                                    d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SalesAccountId
                                                                } into g
                                                                select new
                                                                {
                                                                    g.Key.SalesAccountId,
                                                                    Amount = g.Sum(s => s.Amount - s.VATAmount)
                                                                };

                            if (salesInvoiceItemSalesAccounts.ToList().Any())
                            {
                                foreach (var salesInvoiceItemSalesAccount in salesInvoiceItemSalesAccounts)
                                {
                                    DBSets.SysJournalEntryDBSet salesAccountJournal = new DBSets.SysJournalEntryDBSet
                                    {
                                        BranchId = salesInvoice.BranchId,
                                        JournalEntryDate = DateTime.Today,
                                        ArticleId = salesInvoice.CustomerId,
                                        AccountId = salesInvoiceItemSalesAccount.SalesAccountId,
                                        DebitAmount = 0,
                                        CreditAmount = salesInvoiceItemSalesAccount.Amount,
                                        Particulars = salesInvoice.Remarks,
                                        RRId = null,
                                        SIId = salesInvoice.Id,
                                        CIId = null,
                                        CVId = null,
                                        PMId = null,
                                        RMId = null,
                                        JVId = null,
                                        ILId = null
                                    };

                                    _dbContext.SysJournalEntries.Add(salesAccountJournal);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            var salesInvoiceItemVATAccounts = from d in salesInvoiceItems
                                                              where d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                                                              group d by new
                                                              {
                                                                  d.MstTax_VATId.AccountId
                                                              } into g
                                                              select new
                                                              {
                                                                  g.Key.AccountId,
                                                                  VATAmount = g.Sum(s => s.VATAmount)
                                                              };

                            if (salesInvoiceItemVATAccounts.ToList().Any())
                            {
                                foreach (var salesInvoiceItemVATAccount in salesInvoiceItemVATAccounts)
                                {
                                    DBSets.SysJournalEntryDBSet VATAccountJournal = new DBSets.SysJournalEntryDBSet
                                    {
                                        BranchId = salesInvoice.BranchId,
                                        JournalEntryDate = DateTime.Today,
                                        ArticleId = salesInvoice.CustomerId,
                                        AccountId = salesInvoiceItemVATAccount.AccountId,
                                        DebitAmount = 0,
                                        CreditAmount = salesInvoiceItemVATAccount.VATAmount,
                                        Particulars = salesInvoice.Remarks,
                                        RRId = null,
                                        SIId = salesInvoice.Id,
                                        CIId = null,
                                        CVId = null,
                                        PMId = null,
                                        RMId = null,
                                        JVId = null,
                                        ILId = null
                                    };

                                    _dbContext.SysJournalEntries.Add(VATAccountJournal);
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

        public async Task DeleteSalesInvoiceJournalEntry(Int32 SIId)
        {
            try
            {
                IEnumerable<DBSets.SysJournalEntryDBSet> journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.SIId == SIId
                    select d
                ).ToListAsync();

                if (journalEntries.Any())
                {
                    _dbContext.SysJournalEntries.RemoveRange(journalEntries);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertCollectionJournalEntry(Int32 CIId)
        {
            try
            {
                DBSets.TrnCollectionDBSet collection = await (
                  from d in _dbContext.TrnCollections
                  where d.Id == CIId
                  select d
                ).FirstOrDefaultAsync();

                if (collection != null)
                {
                    IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLines = await (
                       from d in _dbContext.TrnCollectionLines
                       where d.CIId == CIId
                       select d
                    ).ToListAsync();

                    if (collectionLines.Any())
                    {
                        var collectionLinesPayTypeAccounts = from d in collectionLines
                                                             where d.Amount > 0
                                                             group d by new
                                                             {
                                                                 d.BranchId,
                                                                 d.MstPayType_PayTypeId.AccountId
                                                             } into g
                                                             select new
                                                             {
                                                                 g.Key.BranchId,
                                                                 g.Key.AccountId,
                                                                 Amount = g.Sum(s => s.Amount)
                                                             };

                        if (collectionLinesPayTypeAccounts.Any())
                        {
                            foreach (var collectionLinesPayTypeAccount in collectionLinesPayTypeAccounts)
                            {
                                DBSets.SysJournalEntryDBSet payTypeAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = collection.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = collection.CustomerId,
                                    AccountId = collectionLinesPayTypeAccount.AccountId,
                                    DebitAmount = collectionLinesPayTypeAccount.Amount,
                                    CreditAmount = 0,
                                    Particulars = collection.Remarks,
                                    RRId = null,
                                    SIId = null,
                                    CIId = collection.Id,
                                    CVId = null,
                                    PMId = null,
                                    RMId = null,
                                    JVId = null,
                                    ILId = null
                                };

                                _dbContext.SysJournalEntries.Add(payTypeAccountJournal);
                                await _dbContext.SaveChangesAsync();
                            }
                        }

                        if (collection.Amount > 0)
                        {
                            DBSets.SysJournalEntryDBSet accountsReceivableJournal = new DBSets.SysJournalEntryDBSet
                            {
                                BranchId = collection.BranchId,
                                JournalEntryDate = DateTime.Today,
                                ArticleId = collection.CustomerId,
                                AccountId = collection.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId,
                                DebitAmount = 0,
                                CreditAmount = collection.Amount,
                                Particulars = collection.Remarks,
                                RRId = null,
                                SIId = null,
                                CIId = collection.Id,
                                CVId = null,
                                PMId = null,
                                RMId = null,
                                JVId = null,
                                ILId = null
                            };

                            _dbContext.SysJournalEntries.Add(accountsReceivableJournal);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteCollectionJournalEntry(Int32 CIId)
        {
            try
            {
                IEnumerable<DBSets.SysJournalEntryDBSet> journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.CIId == CIId
                    select d
                ).ToListAsync();

                if (journalEntries.Any())
                {
                    _dbContext.SysJournalEntries.RemoveRange(journalEntries);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
