using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleTypeDBSet
    {
        public Int32 Id { get; set; }
        public String ArticleType { get; set; }
        public ICollection<MstArticleDBSet> MstArticles_ArticleType { get; set; }
    }
}
