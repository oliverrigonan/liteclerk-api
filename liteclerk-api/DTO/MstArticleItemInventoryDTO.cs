using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemInventoryDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
    }
}
