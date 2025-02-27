﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnReceivingReceiptItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 RRId { get; set; }
        public virtual TrnReceivingReceiptDBSet TrnReceivingReceipt_RRId { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 POId { get; set; }
        public virtual TrnPurchaseOrderDBSet TrnPurchaseOrder_POId { get; set; }

        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_ItemId { get; set; }

        public String Particulars { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }

        public Decimal Cost { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public Int32 VATId { get; set; }
        public virtual MstTaxDBSet MstTax_VATId { get; set; }
        public Decimal VATRate { get; set; }
        public Decimal VATAmount { get; set; }

        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAXId { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnitId { get; set; }
        public Decimal BaseCost { get; set; }
    }
}
