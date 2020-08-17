﻿using System;
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
        public MstArticleTypeDBSet MstArticleType_ArticleType { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public ICollection<MstArticleCustomerDBSet> MstArticleCustomers_Article { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_Article { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Customer { get; set; }
    }
}
