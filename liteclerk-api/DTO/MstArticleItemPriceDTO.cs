using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemPriceDTO
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleItemDTO ArticleItem { get; set; }

        public String PriceDescription { get; set; }
        public Decimal Price { get; set; }
    }
}
