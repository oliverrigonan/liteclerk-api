using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstAccountArticleTypeDTO
    {
        public Int32 Id { get; set; }

        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Int32 ArticleTypeId { get; set; }
        public MstArticleTypeDTO ArticleType { get; set; }
    }
}
