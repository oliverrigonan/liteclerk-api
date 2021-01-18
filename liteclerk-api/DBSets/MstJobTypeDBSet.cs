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
        public Boolean IsInventory { get; set; }

        public Boolean IsLocked { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<MstJobTypeAttachmentDBSet> MstJobTypeAttachments_JobTypeId { get; set; }
        public virtual ICollection<MstJobTypeDepartmentDBSet> MstJobTypeDepartments_JobTypeId { get; set; }
        public virtual ICollection<MstJobTypeInformationDBSet> MstJobTypeInformations_JobTypeId { get; set; }

        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_ItemJobTypeId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_ItemJobTypeId { get; set; }
    }
}
