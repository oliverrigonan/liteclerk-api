using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSTrnStockInItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 INId { get; set; }
        public Int32 ItemId { get; set; }
        public EasyPOSMstArticleItemDTO Item { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public EasyPOSMstUnitDTO Unit { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    }
}
