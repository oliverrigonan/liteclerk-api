using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnStockCountItemDTO
    {
        public Int32 Id { get; set; }

        public Int32 SCId { get; set; }

        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }

        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
    }
}
