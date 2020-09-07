using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstAccountArticleTypeDBSet
    {
        public Int32 Id { get; set; }
        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public virtual MstArticleTypeDBSet MstArticleType_ArticleTypeId { get; set; }
    }
}
