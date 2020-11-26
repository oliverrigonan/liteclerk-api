using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.Shopify.DTO
{
    public class ShopifyTrnSalesOrderDTO
    {
        public Int32 Id { get; set; }
        public String BranchManualCode { get; set; }
        public String SONumber { get; set; }
        public String SODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public Int32 CustomerId { get; set; }
        public ShopifyMstArticleCustomerDTO Customer { get; set; }
        public String CustomerManualCode { get; set; }
        public String CustomerName { get; set; }
        public String Remarks { get; set; }
        public List<ShopifyTrnSalesOrderItemDTO> SalesOrderItems { get; set; }
    }
}
