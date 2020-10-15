using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstUserDBSet
    {
        public Int32 Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Fullname { get; set; }
        public Int32? CompanyId { get; set; }
        public virtual MstCompanyDBSet MstCompany_CompanyId { get; set; }
        public Int32? BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsLocked { get; set; }
        public virtual ICollection<MstCompanyDBSet> MstCompanies_CreatedByUserId { get; set; }
        public virtual ICollection<MstCompanyDBSet> MstCompanies_UpdatedByUserId { get; set; }
        public virtual ICollection<MstCurrencyDBSet> MstCurrencies_CreatedByUserId { get; set; }
        public virtual ICollection<MstCurrencyDBSet> MstCurrencies_UpdatedByUserId { get; set; }
        public virtual ICollection<MstArticleDBSet> MstArticles_CreatedByUserId { get; set; }
        public virtual ICollection<MstArticleDBSet> MstArticles_UpdatedByUserId { get; set; }
        public virtual ICollection<MstTermDBSet> MstTerms_CreatedByUserId { get; set; }
        public virtual ICollection<MstTermDBSet> MstTerms_UpdatedByUserId { get; set; }
        public virtual ICollection<MstAccountDBSet> MstAccounts_CreatedByUserId { get; set; }
        public virtual ICollection<MstAccountDBSet> MstAccounts_UpdatedByUserId { get; set; }
        public virtual ICollection<MstAccountTypeDBSet> MstAccountTypes_CreatedByUserId { get; set; }
        public virtual ICollection<MstAccountTypeDBSet> MstAccountTypes_UpdatedByUserId { get; set; }
        public virtual ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_CreatedByUserId { get; set; }
        public virtual ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_UpdatedByUserId { get; set; }
        public virtual ICollection<MstAccountCategoryDBSet> MstAccountCategories_CreatedByUserId { get; set; }
        public virtual ICollection<MstAccountCategoryDBSet> MstAccountCategories_UpdatedByUserId { get; set; }
        public virtual ICollection<MstUnitDBSet> MstUnits_CreatedByUserId { get; set; }
        public virtual ICollection<MstUnitDBSet> MstUnits_UpdatedByUserId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CreatedByUserId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_SoldByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_PreparedByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CheckedByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CreatedByUserId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_UpdatedByUserId { get; set; }
        public virtual ICollection<MstJobTypeDBSet> MstJobTypes_CreatedByUserId { get; set; }
        public virtual ICollection<MstJobTypeDBSet> MstJobTypes_UpdatedByUserId { get; set; }
        public virtual ICollection<MstDiscountDBSet> MstDiscounts_CreatedByUserId { get; set; }
        public virtual ICollection<MstDiscountDBSet> MstDiscounts_UpdatedByUserId { get; set; }
        public virtual ICollection<MstTaxDBSet> MstTaxes_CreatedByUserId { get; set; }
        public virtual ICollection<MstTaxDBSet> MstTaxes_UpdatedByUserId { get; set; }
        public virtual ICollection<MstJobDepartmentDBset> MstJobDepartments_CreatedByUserId { get; set; }
        public virtual ICollection<MstJobDepartmentDBset> MstJobDepartments_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_PreparedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_CheckedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_CreatedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnJobOrderInformationDBSet> TrnJobOrderInformations_InformationByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_StatusByUserId { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_AssignedToUserId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_PreparedByUserId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_CheckedByUserId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_CreatedByUserId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_UpdatedByUserId { get; set; }
        public virtual ICollection<MstPayTypeDBSet> MstPayTypes_CreatedByUserId { get; set; }
        public virtual ICollection<MstPayTypeDBSet> MstPayTypes_UpdatedByUserId { get; set; }
        public virtual ICollection<MstUserBranchDBSet> MstUserBranches_UserId { get; set; }
        public virtual ICollection<MstUserJobDepartmentDBSet> MstUserJobDepartments_UserId { get; set; }
        public virtual ICollection<MstUserFormDBSet> MstUserForms_UserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_SoldByUserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_PreparedByUserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_CheckedByUserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_CreatedByUserId { get; set; }
        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_UpdatedByUserId { get; set; }
        public virtual ICollection<SysProductionDBSet> SysProductions_UserId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_PreparedByUserId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_CheckedByUserId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_CreatedByUserId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_ReceivedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_PreparedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_CheckedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_CreatedByUserId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_PreparedByUserId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_CheckedByUserId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_CreatedByUserId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_PreparedByUserId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_CheckedByUserId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_CreatedByUserId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_PreparedByUserId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_CheckedByUserId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_CreatedByUserId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnPointOfSaleDBSet> TrnPointOfSales_CashierUserId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_PreparedByUserId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_CheckedByUserId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_CreatedByUserId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_PreparedByUserId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_CheckedByUserId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_CreatedByUserId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_PreparedByUserId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_CheckedByUserId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_CreatedByUserId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_PreparedByUserId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_CheckedByUserId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_CreatedByUserId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_PreparedByUserId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_CheckedByUserId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_CreatedByUserId { get; set; }
        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_RequestedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_PreparedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_CheckedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_CreatedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_UpdatedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_RequestedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_PreparedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_CheckedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_ApprovedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_CreatedByUserId { get; set; }
        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_UpdatedByUserId { get; set; }
    }
}
