﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCurrencyDBSet
    {
        public Int32 Id { get; set; }
        public String CurrencyCode { get; set; }
        public String ManualCode { get; set; }
        public String Currency { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public ICollection<MstCompanyDBSet> MstCompanies_Currency { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Currency { get; set; }
    }
}
