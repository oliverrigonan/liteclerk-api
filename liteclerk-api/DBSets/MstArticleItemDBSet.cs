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
        public MstArticleDBSet MstArticle_Article { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDBSet MstUnit_Unit { get; set; }
        public Boolean IsJob { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public MstArticleAccountGroupDBSet MstArticleAccountGroup_ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public MstAccountDBSet MstAccount_AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public MstAccountDBSet MstAccount_SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public MstAccountDBSet MstAccount_CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public MstAccountDBSet MstAccount_ExpenseAccount { get; set; }
    }
}
