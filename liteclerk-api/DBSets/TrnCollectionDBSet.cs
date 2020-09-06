using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnCollectionDBSet
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_Branch { get; set; }
        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_Currency { get; set; }
        public String CINumber { get; set; }
        public DateTime CIDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public Int32 CustomerId { get; set; }
        public virtual MstArticleDBSet MstArticle_Customer { get; set; }
        public String Remarks { get; set; }
        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUser { get; set; }
        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUser { get; set; }
        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUser { get; set; }
        public Decimal Amount { get; set; }
        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_Collection { get; set; }
    }
}
