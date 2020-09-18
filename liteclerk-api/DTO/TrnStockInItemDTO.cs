using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnStockInItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 INId { get; set; }
        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public MstUnitDTO BaseUnit { get; set; }
        public Decimal BaseCost { get; set; }
    }
}
