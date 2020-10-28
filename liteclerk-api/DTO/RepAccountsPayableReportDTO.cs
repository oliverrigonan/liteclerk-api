using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepAccountsPayableReportDTO
    {
        public MstCompanyBranchDTO Branch { get; set; }
        public TrnReceivingReceiptDTO ReceivingReceipt { get; set; }
        public String DueDate { get; set; }
        public Decimal BalanceAmount { get; set; }
        public Decimal CurrentAmount { get; set; }
        public Decimal Age30Amount { get; set; }
        public Decimal Age60Amount { get; set; }
        public Decimal Age90Amount { get; set; }
        public Decimal Age120Amount { get; set; }
    }
}
