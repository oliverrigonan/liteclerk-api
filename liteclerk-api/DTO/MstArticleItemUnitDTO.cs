using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemUnitDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }
        public Decimal Mutliplier { get; set; }
    }
}
