using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Business
{
    public class SysAccountsReceivable
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysAccountsReceivable(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAccountsReceivable(Int32 SIId)
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
                    Decimal amount = salesInvoice.Amount;
                    Decimal paidAmount = 0;
                    Decimal adjustmentAmount = 0;

                    IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLine = await (
                        from d in _dbContext.TrnCollectionLines
                        where d.SIId == SIId
                        && d.TrnCollection_CIId.IsLocked == true
                        && d.TrnCollection_CIId.IsCancelled == false
                        select d
                    ).ToListAsync();

                    if (collectionLine.Any())
                    {
                        paidAmount = collectionLine.Sum(d => d.Amount);
                    }

                    Decimal balanceAmount = (amount - paidAmount) + adjustmentAmount;

                    DBSets.TrnSalesInvoiceDBSet updateSalesInvoice = salesInvoice;
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
