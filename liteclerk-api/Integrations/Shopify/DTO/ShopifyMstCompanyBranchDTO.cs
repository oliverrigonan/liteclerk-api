using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.Shopify.DTO
{
    public class ShopifyMstCompanyBranchDTO
    {
        public Int32 Id { get; set; }
        public String ManualCode { get; set; }
        public Int32 CompanyId { get; set; }
        public ShopifyMstCompanyDTO Company { get; set; }
        public String Branch { get; set; }
    }
}
