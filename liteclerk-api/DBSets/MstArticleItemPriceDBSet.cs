using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemPriceDBSet
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public String PriceDescription { get; set; }
        public Decimal Price { get; set; }
    }
}
