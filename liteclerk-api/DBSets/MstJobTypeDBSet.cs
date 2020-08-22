using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstJobTypeDBSet
    {
        public Int32 Id { get; set; }
        public String JobTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String JobType { get; set; }
        public Decimal TotalNumberOfDays { get; set; }
        public String Remarks { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstJobTypeAttachmentDBSet> MstJobTypeAttachments_JobType { get; set; }
        public virtual ICollection<MstJobTypeDepartmentDBSet> MstJobTypeDepartments_JobType { get; set; }
        public virtual ICollection<MstJobTypeInformationDBSet> MstJobTypeInformations_JobType { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_ItemJobType { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_ItemJobType { get; set; }
    }
}
