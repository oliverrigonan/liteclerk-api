using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleDBSet
    {
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Article { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public virtual MstArticleTypeDBSet MstArticleType_ArticleTypeId { get; set; }
        public String ImageURL { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_ArticleId { get; set; }
        public virtual ICollection<MstArticleItemUnitDBSet> MstArticleItemUnits_ArticleId { get; set; }
        public virtual ICollection<MstArticleItemPriceDBSet> MstArticleItemPrices_ArticleId { get; set; }
        public virtual ICollection<MstArticleItemComponentDBSet> MstArticleItemComponents_ArticleId { get; set; }
        public virtual ICollection<MstArticleItemComponentDBSet> MstArticleItemComponents_ComponentArticleId { get; set; }
        public virtual ICollection<MstArticleItemInventoryDBSet> MstArticleItemInventories_ArticleId { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_ArticleId { get; set; }
        public virtual ICollection<MstArticleSupplierDBSet> MstArticleSuppliers_ArticleId { get; set; }
        public virtual ICollection<MstArticleBankDBSet> MstArticleBanks_ArticleId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CustomerId { get; set; }
        public virtual ICollection<TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems_ItemId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_ItemId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_CustomerId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_ArticleId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_BankId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_CustomerId { get; set; }
        public virtual ICollection<TrnSalesOrderItemDBSet> TrnSalesOrderItems_ItemId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_ArticleId { get; set; }
        public virtual ICollection<TrnStockInItemDBSet> TrnStockInItems_ItemId { get; set; }
        public virtual ICollection<SysInventoryDBSet> SysInventories_ArticleId { get; set; }
        public virtual ICollection<TrnPointOfSaleDBSet> TrnPointOfSales_CustomerId { get; set; }
        public virtual ICollection<TrnPointOfSaleDBSet> TrnPointOfSales_ItemId { get; set; }
        public virtual ICollection<SysJournalEntryDBSet> SysJournalEntries_ArticleId { get; set; }
        public virtual ICollection<MstArticleOtherDBSet> MstArticleOthers_ArticleId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_ArticleId { get; set; }
        public virtual ICollection<TrnStockOutItemDBSet> TrnStockOutItems_ItemId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_ArticleId { get; set; }
        public virtual ICollection<TrnStockTransferItemDBSet> TrnStockTransferItems_ItemId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_SupplierId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_SupplierId { get; set; }
        public virtual ICollection<TrnPurchaseRequestItemDBSet> TrnPurchaseRequestItems_ItemId { get; set; }
        public virtual ICollection<TrnPurchaseOrderItemDBSet> TrnPurchaseOrderItems_ItemId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_SupplierId { get; set; }
        public virtual ICollection<TrnReceivingReceiptItemDBSet> TrnReceivingReceiptItems_ItemId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_SupplierId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_BankId { get; set; }
        public virtual ICollection<TrnDisbursementLineDBSet> TrnDisbursementLines_ArticleId { get; set; }
    }
}
