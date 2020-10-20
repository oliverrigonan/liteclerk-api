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
                var inventoryLedger = await (from d in _dbContext.TrnInventories
                                             where d.Id == ILId
                                             select d).FirstOrDefaultAsync();

                if (inventoryLedger != null)
                {
                    var otherArticle = await (from d in _dbContext.MstArticleOthers
                                              select d).FirstOrDefaultAsync();

                    if (otherArticle != null)
                    {
                        var inventories = await (from d in _dbContext.SysInventories
                                                 where d.ILId == ILId
                                                 && d.Amount != 0
                                                 select d).ToListAsync();

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
                                        ArticleId = otherArticle.ArticleId,
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
                                        ArticleId = otherArticle.ArticleId,
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
                var journalEntries = await (from d in _dbContext.SysJournalEntries
                                            where d.ILId == ILId
                                            select d).ToListAsync();

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

        public async Task InsertReceivingReceiptJournalEntry(Int32 RRId)
        {
            try
            {
                var receivingReceipt = await (from d in _dbContext.TrnReceivingReceipts
                                              where d.Id == RRId
                                              && d.Amount != 0
                                              select d).FirstOrDefaultAsync();

                if (receivingReceipt != null)
                {
                    var receivingReceiptItems = await (from d in _dbContext.TrnReceivingReceiptItems
                                                       where d.RRId == RRId
                                                       && d.Amount != 0
                                                       select d).ToListAsync();

                    if (receivingReceiptItems.Any())
                    {
                        var receivingReceiptItemExpenseAccounts = from d in receivingReceiptItems
                                                                  where d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                                                                  group d by new
                                                                  {
                                                                      d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().ExpenseAccountId
                                                                  } into g
                                                                  select new
                                                                  {
                                                                      g.Key.ExpenseAccountId,
                                                                      Amount = g.Sum(s => s.Amount - s.VATAmount)
                                                                  };

                        if (receivingReceiptItemExpenseAccounts.ToList().Any())
                        {
                            foreach (var receivingReceiptItemExpenseAccount in receivingReceiptItemExpenseAccounts)
                            {
                                DBSets.SysJournalEntryDBSet salesAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = receivingReceipt.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = receivingReceipt.SupplierId,
                                    AccountId = receivingReceiptItemExpenseAccount.ExpenseAccountId,
                                    DebitAmount = receivingReceiptItemExpenseAccount.Amount,
                                    CreditAmount = 0,
                                    Particulars = receivingReceipt.Remarks,
                                    RRId = receivingReceipt.Id,
                                    SIId = null,
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

                        var receivingReceiptItemVATAccounts = from d in receivingReceiptItems
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

                        if (receivingReceiptItemVATAccounts.ToList().Any())
                        {
                            foreach (var receivingReceiptItemVATAccount in receivingReceiptItemVATAccounts)
                            {
                                DBSets.SysJournalEntryDBSet VATAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = receivingReceipt.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = receivingReceipt.SupplierId,
                                    AccountId = receivingReceiptItemVATAccount.AccountId,
                                    DebitAmount = receivingReceiptItemVATAccount.VATAmount,
                                    CreditAmount = 0,
                                    Particulars = receivingReceipt.Remarks,
                                    RRId = receivingReceipt.Id,
                                    SIId = null,
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

                    DBSets.SysJournalEntryDBSet accountsPayableJournal = new DBSets.SysJournalEntryDBSet
                    {
                        BranchId = receivingReceipt.BranchId,
                        JournalEntryDate = DateTime.Today,
                        ArticleId = receivingReceipt.SupplierId,
                        AccountId = receivingReceipt.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId,
                        DebitAmount = 0,
                        CreditAmount = receivingReceipt.Amount,
                        Particulars = receivingReceipt.Remarks,
                        RRId = receivingReceipt.Id,
                        SIId = null,
                        CIId = null,
                        CVId = null,
                        PMId = null,
                        RMId = null,
                        JVId = null,
                        ILId = null
                    };

                    _dbContext.SysJournalEntries.Add(accountsPayableJournal);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteReceivingReceiptJournalEntry(Int32 RRId)
        {
            try
            {
                var journalEntries = await (from d in _dbContext.SysJournalEntries
                                            where d.RRId == RRId
                                            select d).ToListAsync();

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

        public async Task InsertDisbursementJournalEntry(Int32 CVId)
        {
            try
            {
                var disbursement = await (from d in _dbContext.TrnDisbursements
                                          where d.Id == CVId
                                          && d.Amount != 0
                                          select d).FirstOrDefaultAsync();

                if (disbursement != null)
                {
                    DBSets.SysJournalEntryDBSet accountsPayableJournal = new DBSets.SysJournalEntryDBSet
                    {
                        BranchId = disbursement.BranchId,
                        JournalEntryDate = DateTime.Today,
                        ArticleId = disbursement.SupplierId,
                        AccountId = disbursement.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId,
                        DebitAmount = disbursement.Amount,
                        CreditAmount = 0,
                        Particulars = disbursement.Remarks,
                        RRId = null,
                        SIId = null,
                        CIId = null,
                        CVId = disbursement.Id,
                        PMId = null,
                        RMId = null,
                        JVId = null,
                        ILId = null
                    };

                    _dbContext.SysJournalEntries.Add(accountsPayableJournal);
                    await _dbContext.SaveChangesAsync();

                    var disbursementLines = await (from d in _dbContext.TrnDisbursementLines
                                                   where d.CVId == CVId
                                                   && d.Amount != 0
                                                   select d).ToListAsync();

                    if (disbursementLines.Any())
                    {
                        var disbursementLineAccounts = from d in disbursementLines
                                                       group d by new
                                                       {
                                                           d.BranchId,
                                                           d.AccountId
                                                       } into g
                                                       select new
                                                       {
                                                           g.Key.BranchId,
                                                           g.Key.AccountId,
                                                           Amount = g.Sum(s => s.Amount)
                                                       };

                        if (disbursementLineAccounts.Any())
                        {
                            foreach (var disbursementLineAccount in disbursementLineAccounts)
                            {
                                DBSets.SysJournalEntryDBSet payTypeAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = disbursement.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = disbursement.SupplierId,
                                    AccountId = disbursementLineAccount.AccountId,
                                    DebitAmount = 0,
                                    CreditAmount = disbursementLineAccount.Amount,
                                    Particulars = disbursement.Remarks,
                                    RRId = null,
                                    SIId = null,
                                    CIId = null,
                                    CVId = disbursement.Id,
                                    PMId = null,
                                    RMId = null,
                                    JVId = null,
                                    ILId = null
                                };

                                _dbContext.SysJournalEntries.Add(payTypeAccountJournal);
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

        public async Task DeleteDisbursementJournalEntry(Int32 CVId)
        {
            try
            {
                var journalEntries = await (from d in _dbContext.SysJournalEntries
                                            where d.CVId == CVId
                                            select d).ToListAsync();

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

        public async Task InsertPayableMemoJournalEntry(Int32 PMId)
        {
            try
            {
                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == PMId
                    && d.Amount != 0
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemo != null)
                {
                    var payableMemoLines = await (
                        from d in _dbContext.TrnPayableMemoLines
                        where d.PMId == PMId
                        && d.Amount != 0
                        select d
                    ).ToListAsync();

                    if (payableMemoLines.Any())
                    {
                        var payableMemoLineAccounts = from d in payableMemoLines
                                                      group d by new
                                                      {
                                                          d.BranchId,
                                                          d.AccountId
                                                      } into g
                                                      select new
                                                      {
                                                          g.Key.BranchId,
                                                          g.Key.AccountId,
                                                          Amount = g.Sum(s => s.Amount)
                                                      };

                        if (payableMemoLineAccounts.Any())
                        {
                            foreach (var payableMemoLineAccount in payableMemoLineAccounts)
                            {
                                DBSets.SysJournalEntryDBSet payTypeAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = payableMemo.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = payableMemo.SupplierId,
                                    AccountId = payableMemoLineAccount.AccountId,
                                    DebitAmount = payableMemoLineAccount.Amount,
                                    CreditAmount = 0,
                                    Particulars = payableMemo.Remarks,
                                    RRId = null,
                                    SIId = null,
                                    CIId = null,
                                    CVId = null,
                                    PMId = payableMemo.Id,
                                    RMId = null,
                                    JVId = null,
                                    ILId = null
                                };

                                _dbContext.SysJournalEntries.Add(payTypeAccountJournal);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }

                    DBSets.SysJournalEntryDBSet accountsReceivableJournal = new DBSets.SysJournalEntryDBSet
                    {
                        BranchId = payableMemo.BranchId,
                        JournalEntryDate = DateTime.Today,
                        ArticleId = payableMemo.SupplierId,
                        AccountId = payableMemo.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId,
                        DebitAmount = 0,
                        CreditAmount = payableMemo.Amount,
                        Particulars = payableMemo.Remarks,
                        RRId = null,
                        SIId = null,
                        CIId = null,
                        CVId = null,
                        PMId = payableMemo.Id,
                        RMId = null,
                        JVId = null,
                        ILId = null
                    };

                    _dbContext.SysJournalEntries.Add(accountsReceivableJournal);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeletePayableMemoJournalEntry(Int32 PMId)
        {
            try
            {
                var journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.PMId == PMId
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
                var salesInvoice = await (from d in _dbContext.TrnSalesInvoices
                                          where d.Id == SIId
                                          && d.Amount != 0
                                          select d).FirstOrDefaultAsync();

                if (salesInvoice != null)
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

                    var salesInvoiceItems = await (from d in _dbContext.TrnSalesInvoiceItems
                                                   where d.SIId == SIId
                                                   && d.Amount != 0
                                                   select d).ToListAsync();

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
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteSalesInvoiceJournalEntry(Int32 SIId)
        {
            try
            {
                var journalEntries = await (from d in _dbContext.SysJournalEntries
                                            where d.SIId == SIId
                                            select d).ToListAsync();

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
                var collection = await (from d in _dbContext.TrnCollections
                                        where d.Id == CIId
                                        && d.Amount != 0
                                        select d).FirstOrDefaultAsync();

                if (collection != null)
                {
                    var collectionLines = await (from d in _dbContext.TrnCollectionLines
                                                 where d.CIId == CIId
                                                 && d.Amount != 0
                                                 select d).ToListAsync();

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
                    }

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
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteCollectionJournalEntry(Int32 CIId)
        {
            try
            {
                var journalEntries = await (from d in _dbContext.SysJournalEntries
                                            where d.CIId == CIId
                                            select d).ToListAsync();

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

        public async Task InsertReceivableMemoJournalEntry(Int32 RMId)
        {
            try
            {
                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == RMId
                    && d.Amount != 0
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemo != null)
                {
                    var receivableMemoLines = await (
                        from d in _dbContext.TrnReceivableMemoLines
                        where d.RMId == RMId
                        && d.Amount != 0
                        select d
                    ).ToListAsync();

                    if (receivableMemoLines.Any())
                    {
                        var receivableMemoLineAccounts = from d in receivableMemoLines
                                                         group d by new
                                                         {
                                                             d.BranchId,
                                                             d.AccountId
                                                         } into g
                                                         select new
                                                         {
                                                             g.Key.BranchId,
                                                             g.Key.AccountId,
                                                             Amount = g.Sum(s => s.Amount)
                                                         };

                        if (receivableMemoLineAccounts.Any())
                        {
                            foreach (var receivableMemoLineAccount in receivableMemoLineAccounts)
                            {
                                DBSets.SysJournalEntryDBSet payTypeAccountJournal = new DBSets.SysJournalEntryDBSet
                                {
                                    BranchId = receivableMemo.BranchId,
                                    JournalEntryDate = DateTime.Today,
                                    ArticleId = receivableMemo.CustomerId,
                                    AccountId = receivableMemoLineAccount.AccountId,
                                    DebitAmount = receivableMemoLineAccount.Amount,
                                    CreditAmount = 0,
                                    Particulars = receivableMemo.Remarks,
                                    RRId = null,
                                    SIId = null,
                                    CIId = null,
                                    CVId = null,
                                    PMId = null,
                                    RMId = receivableMemo.Id,
                                    JVId = null,
                                    ILId = null
                                };

                                _dbContext.SysJournalEntries.Add(payTypeAccountJournal);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }

                    DBSets.SysJournalEntryDBSet accountsReceivableJournal = new DBSets.SysJournalEntryDBSet
                    {
                        BranchId = receivableMemo.BranchId,
                        JournalEntryDate = DateTime.Today,
                        ArticleId = receivableMemo.CustomerId,
                        AccountId = receivableMemo.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId,
                        DebitAmount = 0,
                        CreditAmount = receivableMemo.Amount,
                        Particulars = receivableMemo.Remarks,
                        RRId = null,
                        SIId = null,
                        CIId = null,
                        CVId = null,
                        PMId = null,
                        RMId = receivableMemo.Id,
                        JVId = null,
                        ILId = null
                    };

                    _dbContext.SysJournalEntries.Add(accountsReceivableJournal);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteReceivableMemoJournalEntry(Int32 RMId)
        {
            try
            {
                var journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.RMId == RMId
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
