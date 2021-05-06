﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnDisbursementLineDBSet
    {
        public Int32 Id { get; set; }

        public Int32 CVId { get; set; }
        public virtual TrnDisbursementDBSet TrnDisbursement_CVId { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32? RRId { get; set; }
        public virtual TrnReceivingReceiptDBSet TrnReceivingReceipt_RRId { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public String Particulars { get; set; }

        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAXId { get; set; }
        public Decimal WTAXRate { get; set; }
        public Decimal WTAXAmount { get; set; }
    }
}
