using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnCollectionLineDBSet
    {
        public Int32 Id { get; set; }
        public Int32 CIId { get; set; }
        public virtual TrnCollectionDBSet TrnCollection_CIId { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }
        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }
        public Int32? SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }
        public Decimal Amount { get; set; }
        public Int32 PayTypeId { get; set; }
        public virtual MstPayTypeDBSet MstPayType_PayTypeId { get; set; }
        public String Particulars { get; set; }
        public String CheckNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Int32 BankId { get; set; }
        public virtual MstArticleDBSet MstArticle_BankId { get; set; }
        public Boolean IsClear { get; set; }
        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAXId { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }
    }
}
