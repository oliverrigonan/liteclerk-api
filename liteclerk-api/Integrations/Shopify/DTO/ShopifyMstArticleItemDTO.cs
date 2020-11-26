using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.Shopify.DTO
{
    public class ShopifyMstArticleItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public ShopifyMstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public ShopifyMstUnitDTO Unit { get; set; }
        public String Category { get; set; }
        public Decimal Price { get; set; }
    }
}
