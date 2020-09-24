using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasySHOP.DTO
{
    public class EasySHOPTrnSalesOrderItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 SOId { get; set; }
        public Int32 ItemId { get; set; }
        public EasySHOPMstArticleItemDTO Item { get; set; }
        public String ItemBarCode { get; set; }
        public Int32? ItemInventoryId { get; set; }
        public EasySHOPMstArticleItemInventoryDTO ItemInventory { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public EasySHOPMstUnitDTO Unit { get; set; }
        public Decimal Price { get; set; }
        public Int32 DiscountId { get; set; }
        public EasySHOPMstDiscountDTO Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }
    }
}
