using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.Shopify.DTO
{
    public class ShopifyMstArticleItemInventoryDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public ShopifyMstArticleItemDTO ArticleItem { get; set; }
        public Int32 BranchId { get; set; }
        public ShopifyMstCompanyBranchDTO Branch { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    }
}
