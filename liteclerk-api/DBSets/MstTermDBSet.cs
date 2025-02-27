﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstTermDBSet
    {
        public Int32 Id { get; set; }

        public String TermCode { get; set; }
        public String ManualCode { get; set; }
        public String Term { get; set; }
        public Decimal NumberOfDays { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_TermId { get; set; }
        public virtual ICollection<MstArticleSupplierDBSet> MstArticleSuppliers_TermId { get; set; }

        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_TermId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_TermId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_TermId { get; set; }

        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_TermId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_TermId { get; set; }
    }
}
