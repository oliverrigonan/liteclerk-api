using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCompanyBranchDBSet
    {
        public Int32 Id { get; set; }
        public String BranchCode { get; set; }
        public String ManualCode { get; set; }
        public Int32 CompanyId { get; set; }
        public virtual MstCompanyDBSet MstCompany_CompanyId { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String TIN { get; set; }
        public virtual ICollection<MstUserDBSet> MstUsers_BranchId { get; set; }
        public virtual ICollection<MstArticleItemInventoryDBSet> MstArticleItemInventories_BranchId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_BranchId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_BranchId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_BranchId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_BranchId { get; set; }
        public virtual ICollection<MstUserBranchDBSet> MstUserBranches_BranchId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_BranchId { get; set; }
        public virtual ICollection<SysProductionDBSet> SysProductions_BranchId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_BranchId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_BranchId { get; set; }
        public virtual ICollection<TrnReceivingReceiptItemDBSet> TrnReceivingReceiptItems_BranchId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_BranchId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_BranchId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_ToBranchId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_BranchId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_FromBranchId { get; set; }
        public virtual ICollection<SysInventoryDBSet> SysInventories_BranchId { get; set; }
        public virtual ICollection<TrnPointOfSaleDBSet> TrnPointOfSales_BranchId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_BranchId { get; set; }
        public virtual ICollection<SysJournalEntryDBSet> SysJournalEntries_BranchId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_BranchId { get; set; }
        public virtual ICollection<TrnDisbursementLineDBSet> TrnDisbursementLines_BranchId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_BranchId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_BranchId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_BranchId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_BranchId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_BranchId { get; set; }
        public virtual ICollection<TrnReceivableMemoLineDBSet> TrnReceivableMemoLines_BranchId { get; set; }
        public virtual ICollection<TrnPayableMemoLineDBSet> TrnPayableMemoLines_BranchId { get; set; }
        public virtual ICollection<TrnStockCountDBSet> TrnStockCounts_BranchId { get; set; }
        public virtual ICollection<TrnJournalVoucherLineDBSet> TrnJournalVoucherLines_BranchId { get; set; }
    }
}
