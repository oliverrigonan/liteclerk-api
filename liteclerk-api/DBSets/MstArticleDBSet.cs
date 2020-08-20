using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleDBSet
    {
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Article { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public virtual MstArticleTypeDBSet MstArticleType_ArticleType { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_Article { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_Article { get; set; }
        public virtual ICollection<MstArticleItemUnitDBSet> MstArticleItemUnits_Article { get; set; }
        public virtual ICollection<MstArticleItemPriceDBSet> MstArticleItemPrices_Article { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Customer { get; set; }
        public virtual ICollection<MstArticleItemInventoryDBSet> MstArticleItemInventories_Article { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_Item { get; set; }
    }
}
