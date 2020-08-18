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
        public virtual MstAccountTypeDBSet MstAccountType_AccountType { get; set; }
        public Int32 AccountCashFlowId { get; set; }
        public virtual MstAccountCashFlowDBSet MstAccountCashFlow_AccountCashFlow { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_ReceivableAccount { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_AssetAccount { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_SalesAccount { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CostAccount { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_ExpenseAccount { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_AssetAccount { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_SalesAccount { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_CostAccount { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_ExpenseAccount { get; set; }
    }
}
