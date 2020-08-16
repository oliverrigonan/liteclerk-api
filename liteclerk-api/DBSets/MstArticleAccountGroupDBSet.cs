using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleAccountGroupDBSet
    {
        public Int32 Id { get; set; }
        public String ArticleAccountGroupCode { get; set; }
        public String ManualCode { get; set; }
        public String ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public MstAccountDBSet AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public MstAccountDBSet SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public MstAccountDBSet CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public MstAccountDBSet ExpenseAccount { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_ArticleAccountGroup { get; set; }
    }
}
