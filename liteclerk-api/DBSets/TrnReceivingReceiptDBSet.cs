using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnReceivingReceiptDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public String RRNumber { get; set; }
        public DateTime RRDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public Int32 SupplierId { get; set; }
        public virtual MstArticleDBSet MstArticle_SupplierId { get; set; }

        public Int32 TermId { get; set; }
        public virtual MstTermDBSet MstTerm_TermId { get; set; }

        public String Remarks { get; set; }

        public Int32 ReceivedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ReceivedByUserId { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUserId { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUserId { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUserId { get; set; }

        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }

        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<TrnReceivingReceiptItemDBSet> TrnReceivingReceiptItems_RRId { get; set; }
        public virtual ICollection<TrnDisbursementLineDBSet> TrnDisbursementLines_RRId { get; set; }
        public virtual ICollection<TrnPayableMemoLineDBSet> TrnPayableMemoLines_RRId { get; set; }

        public virtual ICollection<SysInventoryDBSet> SysInventories_RRId { get; set; }
        public virtual ICollection<SysJournalEntryDBSet> SysJournalEntries_RRId { get; set; }
    }
}
