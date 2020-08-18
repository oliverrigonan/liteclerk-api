﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnSalesInvoiceDTO
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 CurrencyId { get; set; }
        public MstCurrencyDTO Currency { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public Int32 CustomerId { get; set; }
        public MstArticleDTO Customer { get; set; }
        public Int32 TermId { get; set; }
        public MstTermDTO Term { get; set; }
        public String DateNeeded { get; set; }
        public String Remarks { get; set; }
        public Int32 SoldByUserId { get; set; }
        public MstUserDTO SoldByUser { get; set; }
        public Int32 PreparedByUserId { get; set; }
        public MstUserDTO PreparedByUser { get; set; }
        public Int32 CheckedByUserId { get; set; }
        public MstUserDTO CheckedByUser { get; set; }
        public Int32 ApprovedByUserId { get; set; }
        public MstUserDTO ApprovedByUser { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
