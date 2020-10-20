using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysAccountsReceivableModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysAccountsReceivableModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAccountsReceivable(Int32 SIId)
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
                    Decimal amount = salesInvoice.Amount;
                    Decimal paidAmount = 0;
                    Decimal adjustmentAmount = 0;

                    var collectionLines = await (
                        from d in _dbContext.TrnCollectionLines
                        where d.SIId == SIId
                        && d.TrnCollection_CIId.IsLocked == true
                        && d.TrnCollection_CIId.IsCancelled == false
                        select d
                    ).ToListAsync();

                    if (collectionLines.Any())
                    {
                        paidAmount = collectionLines.Sum(d => d.Amount);
                    }

                    var receivableMemoLines = await (
                        from d in _dbContext.TrnReceivableMemoLines
                        where d.SIId == SIId
                        && d.TrnReceivableMemo_RMId.IsLocked == true
                        && d.TrnReceivableMemo_RMId.IsCancelled == false
                        select d
                    ).ToListAsync();

                    if (receivableMemoLines.Any())
                    {
                        adjustmentAmount = receivableMemoLines.Sum(d => d.Amount);
                    }

                    Decimal balanceAmount = (amount - paidAmount) + adjustmentAmount;

                    var updateSalesInvoice = salesInvoice;
                    updateSalesInvoice.PaidAmount = paidAmount;
                    updateSalesInvoice.AdjustmentAmount = adjustmentAmount;
                    updateSalesInvoice.BalanceAmount = balanceAmount;

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
