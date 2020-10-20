using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnReceivableMemoLineDBSet
    {
        public Int32 Id { get; set; }

        public Int32 RMId { get; set; }
        public virtual TrnReceivableMemoDBSet TrnReceivableMemo_RMId { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32 SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }

        public Decimal Amount { get; set; }

        public String Particulars { get; set; }
    }
}
