using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnCollectionLineDTO
    {
        public Int32 Id { get; set; }
        public Int32 CIId { get; set; }
        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }
        public Int32? SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }
        public Int32 PayTypeId { get; set; }
        public MstPayTypeDTO PayType { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Int32 BankId { get; set; }
        public MstArticleBankDTO Bank { get; set; }
        public Boolean IsClear { get; set; }
        public Int32 WTAXId { get; set; }
        public MstTaxDTO WTAX { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }
    }
}
