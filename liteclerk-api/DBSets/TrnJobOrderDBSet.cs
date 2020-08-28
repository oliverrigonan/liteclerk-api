using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJobOrderDBSet
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_Branch { get; set; }
        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_Currency { get; set; }
        public String JONumber { get; set; }
        public DateTime JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public DateTime DateScheduled { get; set; }
        public DateTime DateNeeded { get; set; }
        public Int32? SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SalesInvoice { get; set; }
        public Int32? SIItemId { get; set; }
        public virtual TrnSalesInvoiceItemDBSet TrnSalesInvoiceItem_SalesInvoiceItem { get; set; }
        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_Item { get; set; }
        public Int32 ItemJobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_ItemJobType { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_Unit { get; set; }
        public String Remarks { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnit { get; set; }
        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUser { get; set; }
        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUser { get; set; }
        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUser { get; set; }
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
        public virtual ICollection<TrnJobOrderAttachmentDBSet> TrnJobOrderAttachments_JobOrder { get; set; }
        public virtual ICollection<TrnJobOrderInformationDBSet> TrnJobOrderInformations_JobOrder { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_JobOrder { get; set; }
    }
}
