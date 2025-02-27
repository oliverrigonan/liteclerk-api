﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnStockWithdrawalDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public Int32 CurrencyId { get; set; }
        public MstCurrencyDTO Currency { get; set; }

        public String SWNumber { get; set; }
        public String SWDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public Int32 CustomerId { get; set; }
        public MstArticleCustomerDTO Customer { get; set; }

        public Int32 FromBranchId { get; set; }
        public MstCompanyBranchDTO FromBranch { get; set; }

        public Int32 SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }
        public String Remarks { get; set; }

        public Int32 ReceivedByUserId { get; set; }
        public MstUserDTO ReceivedByUser { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public MstUserDTO PreparedByUser { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public MstUserDTO CheckedByUser { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public MstUserDTO ApprovedByUser { get; set; }

        public Decimal Amount { get; set; }

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
