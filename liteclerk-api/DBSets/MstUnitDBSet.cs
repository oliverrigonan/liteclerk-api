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
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_UnitId { get; set; }
        public virtual ICollection<MstArticleItemUnitDBSet> MstArticleItemUnits_UnitId { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_UnitId { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_BaseUnitId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_UnitId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_BaseUnitId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_UnitId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_BaseUnitId { get; set; }
        public virtual ICollection<TrnStockInItemDBSet> TrnStockInItems_UnitId { get; set; }
        public virtual ICollection<TrnStockInItemDBSet> TrnStockInItems_BaseUnitId { get; set; }
    }
}
