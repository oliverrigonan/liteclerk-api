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

        public virtual ICollection<MstArticleDBSet> MstArticles_ArticleTypeId { get; set; }
        public virtual ICollection<MstAccountArticleTypeDBSet> MstAccountArticleTypes_ArticleTypeId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_ArticleTypeId { get; set; }
    }
}
