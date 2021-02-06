using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysJournalEntryDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public String JournalEntryDate { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }

        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }

        public String Particulars { get; set; }

        public Int32? RRId { get; set; }
        public TrnReceivingReceiptDTO ReceivingReceipt { get; set; }

        public Int32? SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public Int32? CIId { get; set; }
        public TrnCollectionDTO Collection { get; set; }

        public Int32? CVId { get; set; }
        public TrnDisbursementDTO Disbursement { get; set; }

        public Int32? PMId { get; set; }
        public TrnPayableMemoDTO PayableMemo { get; set; }

        public Int32? RMId { get; set; }
        public TrnReceivableMemoDTO ReceivableMemo { get; set; }

        public Int32? JVId { get; set; }
        public TrnJournalVoucherDTO JournalVoucher { get; set; }

        public Int32? ILId { get; set; }
        public TrnInventoryDTO InventoryLedger { get; set; }
    }
}
