using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnSalesInvoiceItemDTO
    {
        public Int32 Id { get; set; }

        public Int32 SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }

        public Int32? ItemInventoryId { get; set; }
        public MstArticleItemInventoryDTO ItemInventory { get; set; }

        public Int32? ItemJobTypeId { get; set; }
        public MstJobTypeDTO ItemJobType { get; set; }

        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }

        public Decimal Price { get; set; }

        public Int32 DiscountId { get; set; }
        public MstDiscountDTO Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }

        public Decimal NetPrice { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public Int32 VATId { get; set; }
        public MstTaxDTO VAT { get; set; }
        public Decimal VATRate { get; set; }
        public Decimal VATAmount { get; set; }

        public Int32 WTAXId { get; set; }
        public MstTaxDTO WTAX { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public MstUnitDTO BaseUnit { get; set; }
        public Decimal BaseNetPrice { get; set; }

        public String LineTimeStamp { get; set; }
    }
}
