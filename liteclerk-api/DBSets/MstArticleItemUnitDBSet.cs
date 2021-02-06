using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemUnitDBSet
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }

        public Decimal Multiplier { get; set; }
    }
}
