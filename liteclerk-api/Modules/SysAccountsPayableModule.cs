using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysAccountsPayableModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysAccountsPayableModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAccountsPayable(Int32 RRId)
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
                    Decimal amount = receivingReceipt.Amount;
                    Decimal paidAmount = 0;
                    Decimal adjustmentAmount = 0;

                    IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLine = await (
                        from d in _dbContext.TrnDisbursementLines
                        where d.RRId == RRId
                        && d.TrnDisbursement_CVId.IsLocked == true
                        && d.TrnDisbursement_CVId.IsCancelled == false
                        select d
                    ).ToListAsync();

                    if (disbursementLine.Any())
                    {
                        paidAmount = disbursementLine.Sum(d => d.Amount);
                    }

                    Decimal balanceAmount = (amount - paidAmount) + adjustmentAmount;

                    DBSets.TrnReceivingReceiptDBSet updateReceivingReceipt = receivingReceipt;
                    updateReceivingReceipt.PaidAmount = paidAmount;
                    updateReceivingReceipt.AdjustmentAmount = adjustmentAmount;
                    updateReceivingReceipt.BalanceAmount = balanceAmount;

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
