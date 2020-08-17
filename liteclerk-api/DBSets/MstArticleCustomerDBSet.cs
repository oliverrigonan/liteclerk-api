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
        public virtual MstArticleDBSet MstArticle_Article { get; set; }
        public String Customer { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 ReceivableAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_ReceivableAccount { get; set; }
        public Int32 TermId { get; set; }
        public virtual MstTermDBSet MstTerm_Term { get; set; }
        public Decimal CreditLimit { get; set; }
    }
}
