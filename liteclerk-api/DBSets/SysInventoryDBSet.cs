using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysInventoryDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public DateTime InventoryDate { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public Int32 ArticleItemInventoryId { get; set; }
        public virtual MstArticleItemInventoryDBSet MstArticleItemInventory_ArticleItemInventoryId { get; set; }

        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }

        public Decimal QuantityIn { get; set; }
        public Decimal QuantityOut { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }

        public String Particulars { get; set; }

        public Int32? RRId { get; set; }
        public virtual TrnReceivingReceiptDBSet TrnReceivingReceipt_RRId { get; set; }

        public Int32? SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }

        public Int32? INId { get; set; }
        public virtual TrnStockInDBSet TrnStockIn_INId { get; set; }

        public Int32? OTId { get; set; }
        public virtual TrnStockOutDBSet TrnStockOut_OTId { get; set; }

        public Int32? STId { get; set; }
        public virtual TrnStockTransferDBSet TrnStockTransfer_STId { get; set; }

        public Int32? SWId { get; set; }
        public virtual TrnStockWithdrawalDBSet TrnStockWithdrawal_SWId { get; set; }

        public Int32? ILId { get; set; }
        public virtual TrnInventoryDBSet TrnInventory_ILId { get; set; }
    }
}
