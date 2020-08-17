using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstAccountDBSet
    {
        public Int32 Id { get; set; }
        public String AccountCode { get; set; }
        public String ManualCode { get; set; }
        public String Account { get; set; }
        public Int32 AccountTypeId { get; set; }
        public MstAccountTypeDBSet MstAccountType_AccountType { get; set; }
        public Int32 AccountCashFlowId { get; set; }
        public MstAccountCashFlowDBSet MstAccountCashFlow_AccountCashFlow { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public ICollection<MstArticleCustomerDBSet> MstArticleCustomers_ReceivableAccount { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_AssetAccount { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_SalesAccount { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CostAccount { get; set; }
        public ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_ExpenseAccount { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_AssetAccount { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_SalesAccount { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_CostAccount { get; set; }
        public ICollection<MstArticleItemDBSet> MstArticleItems_ExpenseAccount { get; set; }
    }
}
