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
        public virtual MstCompanyDBSet MstCompany_Company { get; set; }
        public Int32? BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_Branch { get; set; }
        public Boolean IsActive { get; set; }
        public virtual ICollection<MstCompanyDBSet> MstCompanies_CreatedByUser { get; set; }
        public virtual ICollection<MstCompanyDBSet> MstCompanies_UpdatedByUser { get; set; }
        public virtual ICollection<MstCurrencyDBSet> MstCurrencies_CreatedByUser { get; set; }
        public virtual ICollection<MstCurrencyDBSet> MstCurrencies_UpdatedByUser { get; set; }
        public virtual ICollection<MstArticleDBSet> MstArticles_CreatedByUser { get; set; }
        public virtual ICollection<MstArticleDBSet> MstArticles_UpdatedByUser { get; set; }
        public virtual ICollection<MstTermDBSet> MstTerms_CreatedByUser { get; set; }
        public virtual ICollection<MstTermDBSet> MstTerms_UpdatedByUser { get; set; }
        public virtual ICollection<MstAccountDBSet> MstAccounts_CreatedByUser { get; set; }
        public virtual ICollection<MstAccountDBSet> MstAccounts_UpdatedByUser { get; set; }
        public virtual ICollection<MstAccountTypeDBSet> MstAccountTypes_CreatedByUser { get; set; }
        public virtual ICollection<MstAccountTypeDBSet> MstAccountTypes_UpdatedByUser { get; set; }
        public virtual ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_CreatedByUser { get; set; }
        public virtual ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_UpdatedByUser { get; set; }
        public virtual ICollection<MstAccountCategoryDBSet> MstAccountCategories_CreatedByUser { get; set; }
        public virtual ICollection<MstAccountCategoryDBSet> MstAccountCategories_UpdatedByUser { get; set; }
        public virtual ICollection<MstUnitDBSet> MstUnits_CreatedByUser { get; set; }
        public virtual ICollection<MstUnitDBSet> MstUnits_UpdatedByUser { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CreatedByUser { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_UpdatedByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_SoldByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_PreparedByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CheckedByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_ApprovedByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CreatedByUser { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_UpdatedByUser { get; set; }
        public virtual ICollection<MstJobTypeDBSet> MstJobTypes_CreatedByUser { get; set; }
        public virtual ICollection<MstJobTypeDBSet> MstJobTypes_UpdatedByUser { get; set; }
        public virtual ICollection<MstDiscountDBSet> MstDiscounts_CreatedByUser { get; set; }
        public virtual ICollection<MstDiscountDBSet> MstDiscounts_UpdatedByUser { get; set; }
        public virtual ICollection<MstTaxDBSet> MstTaxes_CreatedByUser { get; set; }
        public virtual ICollection<MstTaxDBSet> MstTaxes_UpdatedByUser { get; set; }
        public virtual ICollection<MstJobDepartmentDBset> MstJobDepartments_CreatedByUser { get; set; }
        public virtual ICollection<MstJobDepartmentDBset> MstJobDepartments_UpdatedByUser { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_PreparedByUser { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_CheckedByUser { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_ApprovedByUser { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_CreatedByUser { get; set; }
        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_UpdatedByUser { get; set; }
        public virtual ICollection<TrnJobOrderInformationDBSet> TrnJobOrderInformations_InformationByUser { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_StatusByUser { get; set; }
    }
}
