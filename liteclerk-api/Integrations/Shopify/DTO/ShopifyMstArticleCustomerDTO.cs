using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.Shopify.DTO
{
    public class ShopifyMstArticleCustomerDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public ShopifyMstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String Customer { get; set; }
    }
}
