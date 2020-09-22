using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemComponentDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleItemDTO ArticleItem { get; set; }
        public Int32 ComponentArticleId { get; set; }
        public MstArticleItemDTO ComponentArticleItem { get; set; }
        public Decimal Quantity { get; set; }
    }
}
