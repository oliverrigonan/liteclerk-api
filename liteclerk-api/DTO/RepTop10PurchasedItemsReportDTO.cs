using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepTop10PurchasedItemsReportDTO
    {
        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }

        public Decimal Quantity { get; set; }
    }
}
