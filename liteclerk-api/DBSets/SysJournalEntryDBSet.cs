using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysJournalEntryDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public DateTime JournalEntryDate { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }

        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }

        public String Particulars { get; set; }

        public Int32? RRId { get; set; }
        public virtual TrnReceivingReceiptDBSet TrnReceivingReceipt_RRId { get; set; }
        public Int32? SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }
        public Int32? CIId { get; set; }
        public virtual TrnCollectionDBSet TrnCollection_CIId { get; set; }
        public Int32? CVId { get; set; }
        public virtual TrnDisbursementDBSet TrnDisbursement_CVId { get; set; }
        public Int32? PMId { get; set; }
        public virtual TrnPayableMemoDBSet TrnPayableMemo_PMId { get; set; }
        public Int32? RMId { get; set; }
        public virtual TrnReceivableMemoDBSet TrnReceivableMemo_RMId { get; set; }
        public Int32? JVId { get; set; }
        public virtual TrnJournalVoucherDBSet TrnJournalVoucher_JVId { get; set; }
        public Int32? ILId { get; set; }
        public virtual TrnInventoryDBSet TrnInventory_ILId { get; set; }
    }
}
