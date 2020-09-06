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
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_VAT { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_WTAX { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_RRVAT { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_SIVAT { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_WTAX { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_WTAX { get; set; }
    }
}
