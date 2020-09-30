using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstTaxDBSet
    {
        public Int32 Id { get; set; }
        public String TaxCode { get; set; }
        public String ManualCode { get; set; }
        public String TaxDescription { get; set; }
        public Decimal TaxRate { get; set; }
        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_VATId { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_WTAXId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_RRVATId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_SIVATId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_WTAXId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_WTAXId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_VATId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_WTAXId { get; set; }
        public virtual ICollection<TrnPointOfSaleDBSet> TrnPointOfSales_TaxId { get; set; }
    }
}
