using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Boolean IsJob { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public String ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public String AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public String SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public String CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public String ExpenseAccount { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedByDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedByDateTime { get; set; }
    }
}
