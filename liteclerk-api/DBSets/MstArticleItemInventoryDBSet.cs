using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemInventoryDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_ItemInventoryId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_ItemInventoryId { get; set; }
        public virtual ICollection<SysInventoryDBSet> SysInventories_ArticleItemInventoryId { get; set; }
    }
}
