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
                var receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == RRId
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt != null)
                {
                    Decimal amount = receivingReceipt.Amount;
                    Decimal baseAmount = receivingReceipt.BaseAmount;
                    Decimal paidAmount = 0;
                    Decimal basePaidAmount = 0;
                    Decimal adjustmentAmount = 0;
                    Decimal baseAdjustmentAmount = 0;

                    var disbursementLine = await (
                         from d in _dbContext.TrnDisbursementLines
                         where d.RRId == RRId
                         && d.TrnDisbursement_CVId.IsLocked == true
                         && d.TrnDisbursement_CVId.IsCancelled == false
                         select d
                     ).ToListAsync();

                    if (disbursementLine.Any())
                    {
                        paidAmount = disbursementLine.Sum(d => d.Amount);
                        basePaidAmount = disbursementLine.Sum(d => d.Amount) * receivingReceipt.ExchangeRate;
                    }

                    var payableMemoLines = await (
                        from d in _dbContext.TrnPayableMemoLines
                        where d.RRId == RRId
                        && d.TrnPayableMemo_PMId.IsLocked == true
                        && d.TrnPayableMemo_PMId.IsCancelled == false
                        select d
                    ).ToListAsync();

                    if (payableMemoLines.Any())
                    {
                        adjustmentAmount = payableMemoLines.Sum(d => d.Amount);
                        baseAdjustmentAmount = payableMemoLines.Sum(d => d.BaseAmount);
                    }

                    Decimal balanceAmount = (amount - paidAmount) + adjustmentAmount;
                    Decimal baseBalanceAmount = (baseAmount - basePaidAmount) + baseAdjustmentAmount;

                    var updateReceivingReceipt = receivingReceipt;
                    updateReceivingReceipt.PaidAmount = paidAmount;
                    updateReceivingReceipt.BasePaidAmount = basePaidAmount;
                    updateReceivingReceipt.AdjustmentAmount = adjustmentAmount;
                    updateReceivingReceipt.BaseAdjustmentAmount = baseAdjustmentAmount;
                    updateReceivingReceipt.BalanceAmount = balanceAmount;
                    updateReceivingReceipt.BaseBalanceAmount = baseBalanceAmount;

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
