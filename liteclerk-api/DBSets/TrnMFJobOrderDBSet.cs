using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnMFJobOrderDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public String JONumber { get; set; }
        public DateTime JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public DateTime DateScheduled { get; set; }
        public DateTime DateNeeded { get; set; }

        public Int32 CustomerId { get; set; }
        public virtual MstArticleDBSet MstArticle_CustomerId { get; set; }

        public String Accessories { get; set; }
        public String Engineer { get; set; }
        public String Complaint { get; set; }
        public String Remarks { get; set; }

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

        public virtual ICollection<TrnMFJobOrderLineDBSet> TrnMFJobOrderLine_MFJOId { get; set; }
        public virtual ICollection<TrnSalesInvoiceMFJOItemDBSet> TrnSalesInvoiceMFJOItem_MFJOSIId { get; set; }
    }
}
