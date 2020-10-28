using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleBankDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String ArticleParticulars { get; set; }
        public String Bank { get; set; }
        public String AccountNumber { get; set; }
        public String TypeOfAccount { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public Int32 CashInBankAccountId { get; set; }
        public MstAccountDTO CashInBankAccount { get; set; }
        public Boolean IsLocked { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
