using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDBSet Article { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDBSet Unit { get; set; }
        public Boolean IsJob { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public MstArticleAccountGroupDBSet ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public MstAccountDBSet AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public MstAccountDBSet SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public MstAccountDBSet CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public MstAccountDBSet ExpenseAccount { get; set; }
    }
}
