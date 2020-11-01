using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBContext
{
    public class LiteclerkDBContext : DbContext
    {
        public LiteclerkDBContext(DbContextOptions<LiteclerkDBContext> options) : base(options)
        {
            AppDomain.CurrentDomain.SetData("LiteclerkDataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
        public virtual DbSet<DBSets.MstCodeTableDBSet> MstCodeTables { get; set; }
        public virtual DbSet<DBSets.MstUserDBSet> MstUsers { get; set; }
        public virtual DbSet<DBSets.MstUserBranchDBSet> MstUserBranches { get; set; }
        public virtual DbSet<DBSets.MstUserJobDepartmentDBSet> MstUserJobDepartments { get; set; }
        public virtual DbSet<DBSets.MstUserFormDBSet> MstUserForms { get; set; }
        public virtual DbSet<DBSets.MstCurrencyDBSet> MstCurrencies { get; set; }
        public virtual DbSet<DBSets.MstAccountCashFlowDBSet> MstAccountCashFlows { get; set; }
        public virtual DbSet<DBSets.MstAccountCategoryDBSet> MstAccountCategories { get; set; }
        public virtual DbSet<DBSets.MstAccountTypeDBSet> MstAccountTypes { get; set; }
        public virtual DbSet<DBSets.MstAccountDBSet> MstAccounts { get; set; }
        public virtual DbSet<DBSets.MstAccountArticleTypeDBSet> MstAccountArticleTypes { get; set; }
        public virtual DbSet<DBSets.MstCompanyDBSet> MstCompanies { get; set; }
        public virtual DbSet<DBSets.MstCompanyBranchDBSet> MstCompanyBranches { get; set; }
        public virtual DbSet<DBSets.MstTermDBSet> MstTerms { get; set; }
        public virtual DbSet<DBSets.MstUnitDBSet> MstUnits { get; set; }
        public virtual DbSet<DBSets.MstDiscountDBSet> MstDiscounts { get; set; }
        public virtual DbSet<DBSets.MstTaxDBSet> MstTaxes { get; set; }
        public virtual DbSet<DBSets.MstPayTypeDBSet> MstPayTypes { get; set; }
        public virtual DbSet<DBSets.MstArticleTypeDBSet> MstArticleTypes { get; set; }
        public virtual DbSet<DBSets.MstArticleDBSet> MstArticles { get; set; }
        public virtual DbSet<DBSets.MstArticleAccountGroupDBSet> MstArticleAccountGroups { get; set; }
        public virtual DbSet<DBSets.MstArticleItemDBSet> MstArticleItems { get; set; }
        public virtual DbSet<DBSets.MstArticleItemUnitDBSet> MstArticleItemUnits { get; set; }
        public virtual DbSet<DBSets.MstArticleItemPriceDBSet> MstArticleItemPrices { get; set; }
        public virtual DbSet<DBSets.MstArticleItemComponentDBSet> MstArticleItemComponents { get; set; }
        public virtual DbSet<DBSets.MstArticleCustomerDBSet> MstArticleCustomers { get; set; }
        public virtual DbSet<DBSets.MstArticleSupplierDBSet> MstArticleSuppliers { get; set; }
        public virtual DbSet<DBSets.MstArticleBankDBSet> MstArticleBanks { get; set; }
        public virtual DbSet<DBSets.MstArticleItemInventoryDBSet> MstArticleItemInventories { get; set; }
        public virtual DbSet<DBSets.MstJobDepartmentDBset> MstJobDepartments { get; set; }
        public virtual DbSet<DBSets.MstJobTypeDBSet> MstJobTypes { get; set; }
        public virtual DbSet<DBSets.MstJobTypeAttachmentDBSet> MstJobTypeAttachments { get; set; }
        public virtual DbSet<DBSets.MstJobTypeDepartmentDBSet> MstJobTypeDepartments { get; set; }
        public virtual DbSet<DBSets.MstJobTypeInformationDBSet> MstJobTypeInformations { get; set; }
        public virtual DbSet<DBSets.TrnSalesOrderDBSet> TrnSalesOrders { get; set; }
        public virtual DbSet<DBSets.TrnSalesOrderItemDBSet> TrnSalesOrderItems { get; set; }
        public virtual DbSet<DBSets.TrnSalesInvoiceDBSet> TrnSalesInvoices { get; set; }
        public virtual DbSet<DBSets.TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems { get; set; }
        public virtual DbSet<DBSets.TrnJobOrderDBSet> TrnJobOrders { get; set; }
        public virtual DbSet<DBSets.TrnJobOrderAttachmentDBSet> TrnJobOrderAttachments { get; set; }
        public virtual DbSet<DBSets.TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments { get; set; }
        public virtual DbSet<DBSets.TrnJobOrderInformationDBSet> TrnJobOrderInformations { get; set; }
        public virtual DbSet<DBSets.TrnCollectionDBSet> TrnCollections { get; set; }
        public virtual DbSet<DBSets.TrnCollectionLineDBSet> TrnCollectionLines { get; set; }
        public virtual DbSet<DBSets.TrnStockInDBSet> TrnStockIns { get; set; }
        public virtual DbSet<DBSets.TrnStockInItemDBSet> TrnStockInItems { get; set; }
        public virtual DbSet<DBSets.SysFormDBSet> SysForms { get; set; }
        public virtual DbSet<DBSets.SysProductionDBSet> SysProductions { get; set; }
        public virtual DbSet<DBSets.TrnPurchaseRequestDBSet> TrnPurchaseRequests { get; set; }
        public virtual DbSet<DBSets.TrnPurchaseRequestItemDBSet> TrnPurchaseRequestItems { get; set; }
        public virtual DbSet<DBSets.TrnPurchaseOrderDBSet> TrnPurchaseOrders { get; set; }
        public virtual DbSet<DBSets.TrnPurchaseOrderItemDBSet> TrnPurchaseOrderItems { get; set; }
        public virtual DbSet<DBSets.TrnReceivingReceiptDBSet> TrnReceivingReceipts { get; set; }
        public virtual DbSet<DBSets.TrnReceivingReceiptItemDBSet> TrnReceivingReceiptItems { get; set; }
        public virtual DbSet<DBSets.TrnStockOutDBSet> TrnStockOuts { get; set; }
        public virtual DbSet<DBSets.TrnStockOutItemDBSet> TrnStockOutItems { get; set; }
        public virtual DbSet<DBSets.TrnStockTransferDBSet> TrnStockTransfers { get; set; }
        public virtual DbSet<DBSets.TrnStockTransferItemDBSet> TrnStockTransferItems { get; set; }
        public virtual DbSet<DBSets.SysInventoryDBSet> SysInventories { get; set; }
        public virtual DbSet<DBSets.TrnPointOfSaleDBSet> TrnPointOfSales { get; set; }
        public virtual DbSet<DBSets.TrnInventoryDBSet> TrnInventories { get; set; }
        public virtual DbSet<DBSets.TrnDisbursementDBSet> TrnDisbursements { get; set; }
        public virtual DbSet<DBSets.TrnDisbursementLineDBSet> TrnDisbursementLines { get; set; }
        public virtual DbSet<DBSets.TrnPayableMemoDBSet> TrnPayableMemos { get; set; }
        public virtual DbSet<DBSets.TrnPayableMemoLineDBSet> TrnPayableMemoLines { get; set; }
        public virtual DbSet<DBSets.TrnReceivableMemoDBSet> TrnReceivableMemos { get; set; }
        public virtual DbSet<DBSets.TrnReceivableMemoLineDBSet> TrnReceivableMemoLines { get; set; }
        public virtual DbSet<DBSets.TrnJournalVoucherDBSet> TrnJournalVouchers { get; set; }
        public virtual DbSet<DBSets.TrnJournalVoucherLineDBSet> TrnJournalVoucherLines { get; set; }
        public virtual DbSet<DBSets.SysJournalEntryDBSet> SysJournalEntries { get; set; }
        public virtual DbSet<DBSets.MstArticleOtherDBSet> MstArticleOthers { get; set; }
        public virtual DbSet<DBSets.TrnStockCountDBSet> TrnStockCounts { get; set; }
        public virtual DbSet<DBSets.TrnStockCountItemDBSet> TrnStockCountItems { get; set; }
        public virtual DbSet<DBSets.TrnStockWithdrawalDBSet> TrnStockWithdrawals { get; set; }
        public virtual DbSet<DBSets.TrnStockWithdrawalItemDBSet> TrnStockWithdrawalItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DBModelBuilder.MstCodeTableModelBuilder.CreateMstCodeTableModel(modelBuilder);
            DBModelBuilder.MstUserModelBuilder.CreateMstUserModel(modelBuilder);
            DBModelBuilder.MstUserBranchModelBuilder.CreateMstUserBranchModel(modelBuilder);
            DBModelBuilder.MstUserJobDepartmentModelBuilder.CreateMstUserJobDepartmentModel(modelBuilder);
            DBModelBuilder.MstUserFormModelBuilder.CreateMstUserFormModel(modelBuilder);
            DBModelBuilder.MstCurrencyModelBuilder.CreateMstCurrencyModel(modelBuilder);
            DBModelBuilder.MstAccountCashFlowModelBuilder.CreateMstAccountCashFlowModel(modelBuilder);
            DBModelBuilder.MstAccountCategoryModelBuilder.CreateMstAccountCategoryModel(modelBuilder);
            DBModelBuilder.MstAccountTypeModelBuilder.CreateMstAccountTypeModel(modelBuilder);
            DBModelBuilder.MstAccountModelBuilder.CreateMstAccountModel(modelBuilder);
            DBModelBuilder.MstAccountArticleTypeModelBuilder.CreateMstAccountArticleTypeModel(modelBuilder);
            DBModelBuilder.MstCompanyModelBuilder.CreateMstCompanyModel(modelBuilder);
            DBModelBuilder.MstCompanyBranchModelBuilder.CreateMstCompanyBranchModel(modelBuilder);
            DBModelBuilder.MstTermModelBuilder.CreateMstTermModel(modelBuilder);
            DBModelBuilder.MstUnitModelBuilder.CreateMstUnitModel(modelBuilder);
            DBModelBuilder.MstDiscountModelBuilder.CreateMstDiscountModel(modelBuilder);
            DBModelBuilder.MstTaxModelBuilder.CreateMstTaxModel(modelBuilder);
            DBModelBuilder.MstPayTypeModelBuilder.CreateMstPayTypeModel(modelBuilder);
            DBModelBuilder.MstArticleTypeModelBuilder.CreateMstArticleTypeModel(modelBuilder);
            DBModelBuilder.MstArticleModelBuilder.CreateMstArticleModel(modelBuilder);
            DBModelBuilder.MstArticleAccountGroupModelBuilder.CreateMstArticleAccountGroupModel(modelBuilder);
            DBModelBuilder.MstArticleCustomerModelBuilder.CreateMstArticleCustomerModel(modelBuilder);
            DBModelBuilder.MstArticleItemModelBuilder.CreateMstArticleItemModel(modelBuilder);
            DBModelBuilder.MstArticleItemUnitModelBuilder.CreateMstArticleItemUnitModel(modelBuilder);
            DBModelBuilder.MstArticleItemPriceModelBuilder.CreateMstArticleItemPriceModel(modelBuilder);
            DBModelBuilder.MstArticleItemComponentModelBuilder.CreateMstArticleItemComponentModel(modelBuilder);
            DBModelBuilder.MstArticleSupplierModelBuilder.CreateMstArticleSupplierModel(modelBuilder);
            DBModelBuilder.MstArticleBankModelBuilder.CreateMstArticleBankModel(modelBuilder);
            DBModelBuilder.MstArticleItemInventoryModelBuilder.CreateMstArticleItemInventoryModel(modelBuilder);
            DBModelBuilder.MstJobDepartmentModelBuilder.CreateMstJobDepartmentModel(modelBuilder);
            DBModelBuilder.MstJobTypeModelBuilder.CreateMstJobTypeModel(modelBuilder);
            DBModelBuilder.MstJobTypeAttachmentModelBuilder.CreateMstJobTypeAttachmentModel(modelBuilder);
            DBModelBuilder.MstJobTypeDepartmentModelBuilder.CreateMstJobTypeDepartmentModel(modelBuilder);
            DBModelBuilder.MstJobTypeInformationModelBuilder.CreateMstJobTypeInformationModel(modelBuilder);
            DBModelBuilder.TrnSalesOrderModelBuilder.CreateTrnSalesOrderModel(modelBuilder);
            DBModelBuilder.TrnSalesOrderItemModelBuilder.CreateTrnSalesOrderItemModel(modelBuilder);
            DBModelBuilder.TrnSalesInvoiceModelBuilder.CreateTrnSalesInvoiceModel(modelBuilder);
            DBModelBuilder.TrnSalesInvoiceItemModelBuilder.CreateTrnSalesInvoiceItemModel(modelBuilder);
            DBModelBuilder.TrnJobOrderModelBuilder.CreateTrnJobOrderModel(modelBuilder);
            DBModelBuilder.TrnJobOrderDepartmentModelBuilder.CreateTrnJobOrderDepartmentModel(modelBuilder);
            DBModelBuilder.TrnJobOrderAttachmentModelBuilder.CreateTrnJobOrderAttachmentModel(modelBuilder);
            DBModelBuilder.TrnJobOrderInformationModelBuilder.CreateTrnJobOrderInformationModel(modelBuilder);
            DBModelBuilder.TrnCollectionModelBuilder.CreateTrnCollectionModel(modelBuilder);
            DBModelBuilder.TrnCollectionLineModelBuilder.CreateTrnCollectionLineModel(modelBuilder);
            DBModelBuilder.TrnStockInModelBuilder.CreateTrnStockInModel(modelBuilder);
            DBModelBuilder.TrnStockInItemModelBuilder.CreateTrnStockInItemModel(modelBuilder);
            DBModelBuilder.SysFormModelBuilder.CreateSysFormModel(modelBuilder);
            DBModelBuilder.SysProductionModelBuilder.CreateSysProductionModel(modelBuilder);
            DBModelBuilder.TrnPurchaseRequestModelBuilder.CreateTrnPurchaseRequestModel(modelBuilder);
            DBModelBuilder.TrnPurchaseRequestItemModelBuilder.CreateTrnPurchaseRequestItemModel(modelBuilder);
            DBModelBuilder.TrnPurchaseOrderModelBuilder.CreateTrnPurchaseOrderModel(modelBuilder);
            DBModelBuilder.TrnPurchaseOrderItemModelBuilder.CreateTrnPurchaseOrderItemModel(modelBuilder);
            DBModelBuilder.TrnReceivingReceiptModelBuilder.CreateTrnReceivingReceiptModel(modelBuilder);
            DBModelBuilder.TrnReceivingReceiptItemModelBuilder.CreateTrnReceivingReceiptItemModel(modelBuilder);
            DBModelBuilder.TrnStockOutModelBuilder.CreateTrnStockOutModel(modelBuilder);
            DBModelBuilder.TrnStockOutItemModelBuilder.CreateTrnStockOutItemModel(modelBuilder);
            DBModelBuilder.TrnStockTransferModelBuilder.CreateTrnStockTransferModel(modelBuilder);
            DBModelBuilder.TrnStockTransferItemModelBuilder.CreateTrnStockTransferItemModel(modelBuilder);
            DBModelBuilder.TrnStockWithdrawalModelBuilder.CreateTrnStockWithdrawalModel(modelBuilder);
            DBModelBuilder.TrnStockWithdrawalItemModelBuilder.CreateTrnStockWithdrawalItemModel(modelBuilder);
            DBModelBuilder.SysInventoryModelBuilder.CreateSysInventoryModel(modelBuilder);
            DBModelBuilder.TrnPointOfSaleModelBuilder.CreateTrnPointOfSaleModel(modelBuilder);
            DBModelBuilder.TrnInventoryModelBuilder.CreateTrnInventoryModel(modelBuilder);
            DBModelBuilder.TrnDisbursementModelBuilder.CreateTrnDisbursementModel(modelBuilder);
            DBModelBuilder.TrnDisbursementLineModelBuilder.CreateTrnDisbursementLineModel(modelBuilder);
            DBModelBuilder.TrnPayableMemoModelBuilder.CreateTrnPayableMemoModel(modelBuilder);
            DBModelBuilder.TrnPayableMemoLineModelBuilder.CreateTrnPayableMemoLineModel(modelBuilder);
            DBModelBuilder.TrnReceivableMemoModelBuilder.CreateTrnReceivableMemoModel(modelBuilder);
            DBModelBuilder.TrnReceivableMemoLineModelBuilder.CreateTrnReceivableMemoLineModel(modelBuilder);
            DBModelBuilder.TrnJournalVoucherModelBuilder.CreateTrnJournalVoucherModel(modelBuilder);
            DBModelBuilder.TrnJournalVoucherLineModelBuilder.CreateTrnJournalVoucherLineModel(modelBuilder);
            DBModelBuilder.SysJournalEntryModelBuilder.CreateSysJournalEntryModel(modelBuilder);
            DBModelBuilder.MstArticleOtherModelBuilder.CreateMstArticleOtherModel(modelBuilder);
            DBModelBuilder.TrnStockCountModelBuilder.CreateTrnStockCountModel(modelBuilder);
            DBModelBuilder.TrnStockCountItemModelBuilder.CreateTrnStockCountItemModel(modelBuilder);
        }
    }
}
