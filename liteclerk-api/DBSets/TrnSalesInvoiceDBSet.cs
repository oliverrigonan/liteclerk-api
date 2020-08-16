using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnSalesInvoiceDBSet
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public MstCompanyBranchDBSet Branch { get; set; }
        public Int32 CurrencyId { get; set; }
        public MstCurrencyDBSet Currency { get; set; }
        public String SINumber { get; set; }
        public DateTime SIDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public Int32 CustomerId { get; set; }
        public MstArticleDBSet Customer { get; set; }
        public Int32 TermId { get; set; }
        public MstTermDBSet Term { get; set; }
        public DateTime DateNeeded { get; set; }
        public String Remarks { get; set; }
        public Int32 SoldByUserId { get; set; }
        public MstUserDBSet SoldByUser { get; set; }
        public Int32 PreparedByUserId { get; set; }
        public MstUserDBSet PreparedByUser { get; set; }
        public Int32 CheckedByUserId { get; set; }
        public MstUserDBSet CheckedByUser { get; set; }
        public Int32 ApprovedByUserId { get; set; }
        public MstUserDBSet ApprovedByUser { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
    }
}
