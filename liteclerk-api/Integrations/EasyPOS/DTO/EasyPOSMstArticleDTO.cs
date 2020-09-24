using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSMstArticleDTO
    {
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Article { get; set; }
        public String ImageURL { get; set; }
    }
}
