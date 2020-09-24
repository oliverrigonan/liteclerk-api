using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasySHOP.DTO
{
    public class EasySHOPMstArticleCustomerDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public EasySHOPMstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String Customer { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
    }
}
