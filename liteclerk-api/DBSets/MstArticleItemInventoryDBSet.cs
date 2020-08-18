﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemInventoryDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_Article { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_Branch { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_ItemInventory { get; set; }
    }
}
