using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnPointOfSaleDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public String TerminalCode { get; set; }

        public DateTime POSDate { get; set; }
        public String POSNumber { get; set; }
        public String OrderNumber { get; set; }

        public String CustomerCode { get; set; }
        public Int32? CustomerId { get; set; }
        public virtual MstArticleDBSet MstArticle_CustomerId { get; set; }

        public String ItemCode { get; set; }
        public Int32? ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_ItemId { get; set; }

        public String Particulars { get; set; }

        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }

        public String TaxCode { get; set; }
        public Int32? TaxId { get; set; }
        public virtual MstTaxDBSet MstTax_TaxId { get; set; }
        public Decimal TaxAmount { get; set; }

        public String CashierUserCode { get; set; }
        public Int32? CashierUserId { get; set; }
        public virtual MstUserDBSet MstUser_CashierUserId { get; set; }

        public DateTime TimeStamp { get; set; }
        public String PostCode { get; set; }
    }
}
