﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJournalVoucherDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public String JVNumber { get; set; }
        public DateTime JVDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public String Remarks { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUserId { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUserId { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUserId { get; set; }

        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }

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

        public virtual ICollection<TrnJournalVoucherLineDBSet> TrnJournalVoucherLines_JVId { get; set; }
        public virtual ICollection<SysJournalEntryDBSet> SysJournalEntries_JVId { get; set; }
    }
}
