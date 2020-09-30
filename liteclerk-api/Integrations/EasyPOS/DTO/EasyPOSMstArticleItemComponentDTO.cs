using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSMstArticleItemComponentDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public EasyPOSMstArticleItemDTO ArticleItem { get; set; }
        public Int32 ComponentArticleId { get; set; }
        public EasyPOSMstArticleItemDTO ComponentArticleItem { get; set; }
        public Decimal Quantity { get; set; }
    }
}
