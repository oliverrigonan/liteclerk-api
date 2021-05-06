using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnDisbursementLineDTO
    {
        public Int32 Id { get; set; }

        public Int32 CVId { get; set; }
        public TrnDisbursementDTO Disbursement { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }

        public Int32? RRId { get; set; }
        public TrnReceivingReceiptDTO ReceivingReceipt { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public String Particulars { get; set; }

        public Int32 WTAXId { get; set; }
        public MstTaxDTO WTAX { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }
    }
}
