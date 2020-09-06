using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleBankDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }
        public String Bank { get; set; }
        public String AccountNumber { get; set; }
        public String TypeOfAccount { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 CashInBankAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_CashInBankAccountId { get; set; }
    }
}
