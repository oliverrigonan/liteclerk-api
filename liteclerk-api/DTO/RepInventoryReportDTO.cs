using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepInventoryReportDTO
    {
        public Int32 Id { get; set; }
        public String Document { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
        public MstArticleItemInventoryDTO ItemInventory { get; set; }
        public MstArticleItemDTO Item { get; set; }
        public Decimal BegQuantity { get; set; }
        public Decimal InQuantity { get; set; }
        public Decimal OutQuantity { get; set; }
        public Decimal EndQuantity { get; set; }
        public Decimal CostAmount { get; set; }
        public Decimal SellingAmount { get; set; }
    }
}
