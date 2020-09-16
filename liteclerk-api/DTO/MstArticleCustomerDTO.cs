using liteclerk_api.DBSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleCustomerDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String Customer { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 ReceivableAccountId { get; set; }
        public MstAccountDTO ReceivableAccount { get; set; }
        public Int32 TermId { get; set; }
        public MstTermDTO Term { get; set; }
        public Decimal CreditLimit { get; set; }
        public Boolean IsLocked { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
