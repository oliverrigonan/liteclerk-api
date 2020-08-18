using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleDTO
    {
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Article { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedUserFullname { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedUserFullname { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
