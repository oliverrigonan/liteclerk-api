using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnJournalVoucherLineDTO
    {
        public Int32 Id { get; set; }

        public Int32 JVId { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }

        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }

        public String Particulars { get; set; }
    }
}
