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
        public MstAccountDTO AssetAccount { get; set; }

        public Int32 SalesAccountId { get; set; }
        public MstAccountDTO SalesAccount { get; set; }

        public Int32 CostAccountId { get; set; }
        public MstAccountDTO CostAccount { get; set; }

        public Int32 ExpenseAccountId { get; set; }
        public MstAccountDTO ExpenseAccount { get; set; }

        public Int32 ArticleTypeId { get; set; }
        public MstArticleTypeDTO ArticleType { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
