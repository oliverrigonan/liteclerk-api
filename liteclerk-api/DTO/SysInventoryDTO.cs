using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysInventoryDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public String InventoryDate { get; set; }

        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }

        public MstArticleItemDTO ArticleItem { get; set; }

        public Int32 ArticleItemInventoryId { get; set; }
        public MstArticleItemInventoryDTO ArticleItemInventory { get; set; }

        public Decimal QuantityIn { get; set; }
        public Decimal QuantityOut { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }

        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Int32? RRId { get; set; }
        public TrnReceivingReceiptDTO ReceivingReceipt { get; set; }

        public Int32? SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public Int32? INId { get; set; }
        public TrnStockInDTO StockIn { get; set; }

        public Int32? OTId { get; set; }
        public TrnStockOutDTO StockOut { get; set; }

        public Int32? STId { get; set; }
        public TrnStockTransferDTO StockTransfer { get; set; }

        public Int32? SWId { get; set; }
        public TrnStockWithdrawalDTO StockWithdrawal { get; set; }

        public Int32? ILId { get; set; }
        public TrnInventoryDTO Inventory { get; set; }
    }
}
