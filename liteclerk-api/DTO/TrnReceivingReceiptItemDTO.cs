﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnReceivingReceiptItemDTO
    {
        public Int32 Id { get; set; }

        public Int32 RRId { get; set; }
        public TrnReceivingReceiptDTO ReceivingReceipt { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public Int32 POId { get; set; }
        public TrnPurchaseOrderDTO PurchaseOrder { get; set; }

        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }

        public String Particulars { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }

        public Decimal Cost { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public Int32 VATId { get; set; }
        public MstTaxDTO VAT { get; set; }
        public Decimal VATRate { get; set; }
        public Decimal VATAmount { get; set; }

        public Int32 WTAXId { get; set; }
        public MstTaxDTO WTAX { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public MstUnitDTO BaseUnit { get; set; }
        public Decimal BaseCost { get; set; }
    }
}
