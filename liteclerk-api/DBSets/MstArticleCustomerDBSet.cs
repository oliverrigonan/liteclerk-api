using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleCustomerDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDBSet Article { get; set; }
        public String Customer { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 ReceivableAccountId { get; set; }
        public MstAccountDBSet ReceivableAccount { get; set; }
        public Int32 TermId { get; set; }
        public MstTermDBSet Term { get; set; }
        public Decimal CreditLimit { get; set; }
    }
}
