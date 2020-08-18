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

        }

        public virtual DbSet<DBSets.MstUserDBSet> MstUsers { get; set; }
        public virtual DbSet<DBSets.MstCurrencyDBSet> MstCurrencies { get; set; }
        public virtual DbSet<DBSets.MstAccountCashFlowDBSet> MstAccountCashFlows { get; set; }
        public virtual DbSet<DBSets.MstAccountCategoryDBSet> MstAccountCategories { get; set; }
        public virtual DbSet<DBSets.MstAccountTypeDBSet> MstAccountTypes { get; set; }
        public virtual DbSet<DBSets.MstAccountDBSet> MstAccounts { get; set; }
        public virtual DbSet<DBSets.MstCompanyDBSet> MstCompanies { get; set; }
        public virtual DbSet<DBSets.MstCompanyBranchDBSet> MstCompanyBranches { get; set; }
        public virtual DbSet<DBSets.MstTermDBSet> MstTerms { get; set; }
        public virtual DbSet<DBSets.MstUnitDBSet> MstUnits { get; set; }
        public virtual DbSet<DBSets.MstDiscountDBSet> MstDiscounts { get; set; }
        public virtual DbSet<DBSets.MstTaxDBSet> MstTaxes { get; set; }
        public virtual DbSet<DBSets.MstArticleTypeDBSet> MstArticleTypes { get; set; }
        public virtual DbSet<DBSets.MstArticleDBSet> MstArticles { get; set; }
        public virtual DbSet<DBSets.MstArticleAccountGroupDBSet> MstArticleAccountGroups { get; set; }
        public virtual DbSet<DBSets.MstArticleCustomerDBSet> MstArticleCustomers { get; set; }
        public virtual DbSet<DBSets.MstArticleItemDBSet> MstArticleItems { get; set; }
        public virtual DbSet<DBSets.MstArticleItemInventoryDBSet> MstArticleItemInventories { get; set; }
        public virtual DbSet<DBSets.MstJobTypeDBSet> MstJobTypes { get; set; }
        public virtual DbSet<DBSets.TrnSalesInvoiceDBSet> TrnSalesInvoices { get; set; }
        public virtual DbSet<DBSets.TrnSalesInvoiceItemDBSet> TrnSalesInvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DBModelBuilder.MstUserModelBuilder.CreateMstUserModel(modelBuilder);
            DBModelBuilder.MstCurrencyModelBuilder.CreateMstCurrencyModel(modelBuilder);
            DBModelBuilder.MstAccountCashFlowModelBuilder.CreateMstAccountCashFlowModel(modelBuilder);
            DBModelBuilder.MstAccountCategoryModelBuilder.CreateMstAccountCategoryModel(modelBuilder);
            DBModelBuilder.MstAccountTypeModelBuilder.CreateMstAccountTypeModel(modelBuilder);
            DBModelBuilder.MstAccountModelBuilder.CreateMstAccountModel(modelBuilder);
            DBModelBuilder.MstCompanyModelBuilder.CreateMstCompanyModel(modelBuilder);
            DBModelBuilder.MstCompanyBranchModelBuilder.CreateMstCompanyBranchModel(modelBuilder);
            DBModelBuilder.MstTermModelBuilder.CreateMstTermModel(modelBuilder);
            DBModelBuilder.MstUnitModelBuilder.CreateMstUnitModel(modelBuilder);
            DBModelBuilder.MstDiscountModelBuilder.CreateMstDiscountModel(modelBuilder);
            DBModelBuilder.MstTaxModelBuilder.CreateMstTaxModel(modelBuilder);
            DBModelBuilder.MstArticleTypeModelBuilder.CreateMstArticleTypeModel(modelBuilder);
            DBModelBuilder.MstArticleModelBuilder.CreateMstArticleModel(modelBuilder);
            DBModelBuilder.MstArticleAccountGroupModelBuilder.CreateMstArticleAccountGroupModel(modelBuilder);
            DBModelBuilder.MstArticleCustomerModelBuilder.CreateMstArticleCustomerModel(modelBuilder);
            DBModelBuilder.MstArticleItemModelBuilder.CreateMstArticleItemModel(modelBuilder);
            DBModelBuilder.MstArticleItemInventoryModelBuilder.CreateMstArticleItemInventoryModel(modelBuilder);
            DBModelBuilder.MstJobTypeModelBuilder.CreateMstJobTypeModel(modelBuilder);
            DBModelBuilder.TrnSalesInvoiceModelBuilder.CreateTrnSalesInvoiceModel(modelBuilder);
            DBModelBuilder.TrnSalesInvoiceItemModelBuilder.CreateTrnSalesInvoiceItemModel(modelBuilder);
        }
    }
}
