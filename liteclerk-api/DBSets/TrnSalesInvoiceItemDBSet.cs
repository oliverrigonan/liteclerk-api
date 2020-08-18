using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnSalesInvoiceItemDBSet
    {
        public Int32 Id { get; set; }
        public Int32 SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SalesInvoice { get; set; }
        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_Item { get; set; }
        public Int32? ItemInventoryId { get; set; }
        public virtual MstArticleItemInventoryDBSet MstArticleItemInventory_ItemInventory { get; set; }
        public Int32? ItemJobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_ItemJobType { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_Unit { get; set; }
        public Decimal Price { get; set; }
        public Int32 DiscountId { get; set; }
        public virtual MstDiscountDBSet MstDiscount_Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }
        public Int32 VATId { get; set; }
        public virtual MstTaxDBSet MstTax_VAT { get; set; }
        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAX { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnit { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseNetPrice { get; set; }
        public DateTime LineTimeStamp { get; set; }
    }
}
