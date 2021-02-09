using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleOtherDTO
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }

        public String ArticleManualCode { get; set; }
        public String ArticleParticulars { get; set; }

        public String Other { get; set; }
    }
}
