﻿using System;
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
        public virtual MstAccountDBSet MstAccount_AssetAccountId { get; set; }

        public Int32 SalesAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_SalesAccountId { get; set; }

        public Int32 CostAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_CostAccountId { get; set; }

        public Int32 ExpenseAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_ExpenseAccountId { get; set; }

        public Int32 ArticleTypeId { get; set; }
        public virtual MstArticleTypeDBSet MstArticleType_ArticleTypeId { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_ArticleAccountGroupId { get; set; }
    }
}
