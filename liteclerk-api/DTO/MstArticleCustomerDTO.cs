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
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Customer { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 ReceivableAccountId { get; set; }
        public String ReceivableAccount { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public Decimal CreditLimit { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedByDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedByDateTime { get; set; }
    }
}
