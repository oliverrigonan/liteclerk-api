using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnDisbursementDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public String CVNumber { get; set; }
        public DateTime CVDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public Int32 SupplierId { get; set; }
        public virtual MstArticleDBSet MstArticle_SupplierId { get; set; }

        public String Payee { get; set; }
        public String Remarks { get; set; }

        public Int32 PayTypeId { get; set; }
        public virtual MstPayTypeDBSet MstPayType_PayTypeId { get; set; }

        public String CheckNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Boolean IsCrossCheck { get; set; }

        public Int32 BankId { get; set; }
        public virtual MstArticleDBSet MstArticle_BankId { get; set; }

        public Boolean IsClear { get; set; }

        public Decimal Amount { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUserId { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUserId { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUserId { get; set; }

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

        public virtual ICollection<TrnDisbursementLineDBSet> TrnDisbursementLines_CVId { get; set; }
        public virtual ICollection<SysJournalEntryDBSet> SysJournalEntries_CVId { get; set; }
    }
}
