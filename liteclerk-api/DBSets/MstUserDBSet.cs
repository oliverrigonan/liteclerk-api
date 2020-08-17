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
        public Int32 CompanyId { get; set; }
        public MstCompanyDBSet MstCompany_Company { get; set; }
        public Int32 BranchId { get; set; }
        public MstCompanyBranchDBSet MstCompanyBranch_Branch { get; set; }
        public Boolean IsActive { get; set; }
        public ICollection<MstCompanyDBSet> MstCompanies_CreatedByUser { get; set; }
        public ICollection<MstCompanyDBSet> MstCompanies_UpdatedByUser { get; set; }
        public ICollection<MstCurrencyDBSet> MstCurrencies_CreatedByUser { get; set; }
        public ICollection<MstCurrencyDBSet> MstCurrencies_UpdatedByUser { get; set; }
        public ICollection<MstArticleDBSet> MstArticles_CreatedByUser { get; set; }
        public ICollection<MstArticleDBSet> MstArticles_UpdatedByUser { get; set; }
        public ICollection<MstTermDBSet> MstTerms_CreatedByUser { get; set; }
        public ICollection<MstTermDBSet> MstTerms_UpdatedByUser { get; set; }
        public ICollection<MstAccountDBSet> MstAccounts_CreatedByUser { get; set; }
        public ICollection<MstAccountDBSet> MstAccounts_UpdatedByUser { get; set; }
        public ICollection<MstAccountTypeDBSet> MstAccountTypes_CreatedByUser { get; set; }
        public ICollection<MstAccountTypeDBSet> MstAccountTypes_UpdatedByUser { get; set; }
        public ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_CreatedByUser { get; set; }
        public ICollection<MstAccountCashFlowDBSet> MstAccountCashFlows_UpdatedByUser { get; set; }
        public ICollection<MstAccountCategoryDBSet> MstAccountCategories_CreatedByUser { get; set; }
        public ICollection<MstAccountCategoryDBSet> MstAccountCategories_UpdatedByUser { get; set; }
        public ICollection<MstUnitDBSet> MstUnits_CreatedByUser { get; set; }
        public ICollection<MstUnitDBSet> MstUnits_UpdatedByUser { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CreatedByUser { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_UpdatedByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_SoldByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_PreparedByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CheckedByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_ApprovedByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CreatedByUser { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_UpdatedByUser { get; set; }
    }
}
