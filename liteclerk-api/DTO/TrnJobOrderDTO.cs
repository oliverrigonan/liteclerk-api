﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnJobOrderDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public Int32 CurrencyId { get; set; }
        public MstCurrencyDTO Currency { get; set; }

        public String JONumber { get; set; }
        public String JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public String DateScheduled { get; set; }
        public String DateNeeded { get; set; }

        public Int32? SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public Int32? SIItemId { get; set; }
        public TrnSalesInvoiceItemDTO SalesInvoiceItem { get; set; }

        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }

        public Int32 ItemJobTypeId { get; set; }
        public MstJobTypeDTO ItemJobType { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }

        public String Remarks { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public MstUnitDTO BaseUnit { get; set; }

        public String CurrentDepartment { get; set; }
        public String CurrentDepartmentStatus { get; set; }
        public String CurrentDepartmentUserFullName { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public MstUserDTO PreparedByUser { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public MstUserDTO CheckedByUser { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public MstUserDTO ApprovedByUser { get; set; }

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
