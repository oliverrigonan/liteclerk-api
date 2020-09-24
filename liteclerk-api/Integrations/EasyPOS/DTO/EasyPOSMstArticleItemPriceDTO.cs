using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSMstArticleItemPriceDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public EasyPOSMstArticleItemDTO ArticleItem { get; set; }
        public String PriceDescription { get; set; }
        public Decimal Price { get; set; }
    }
}
