using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnSalesOrderItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 SOId { get; set; }
        public virtual TrnSalesOrderDBSet TrnSalesOrder_SOId { get; set; }

        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_ItemId { get; set; }

        public Int32? ItemInventoryId { get; set; }
        public virtual MstArticleItemInventoryDBSet MstArticleItemInventory_ItemInventoryId { get; set; }

        public String Particulars { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }

        public Decimal Price { get; set; }

        public Int32 DiscountId { get; set; }
        public virtual MstDiscountDBSet MstDiscount_DiscountId { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }

        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }

        public Int32 VATId { get; set; }
        public virtual MstTaxDBSet MstTax_VATId { get; set; }
        public Decimal VATRate { get; set; }
        public Decimal VATAmount { get; set; }

        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAXId { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnitId { get; set; }
        public Decimal BaseNetPrice { get; set; }

        public DateTime LineTimeStamp { get; set; }
    }
}
