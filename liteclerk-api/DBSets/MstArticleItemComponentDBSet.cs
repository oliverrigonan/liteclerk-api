using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemComponentDBSet
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32 ComponentArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ComponentArticleId { get; set; }

        public Decimal Quantity { get; set; }
    }
}
