using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleAccountGroupDTO
    {
        public Int32 Id { get; set; }
        public String ArticleAccountGroupCode { get; set; }
        public String ManualCode { get; set; }
        public String ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public Int32 SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
