using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstUnitDBSet
    {
        public Int32 Id { get; set; }
        public String UnitCode { get; set; }
        public String ManualCode { get; set; }
        public String Unit { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_Unit { get; set; }
        public virtual ICollection<MstArticleItemUnitDBSet> MstArticleItemUnits_Unit { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_Unit { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_BaseUnit { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_Unit { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_BaseUnit { get; set; }
    }
}
