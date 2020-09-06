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
        public virtual MstAccountTypeDBSet MstAccountType_AccountTypeId { get; set; }
        public Int32 AccountCashFlowId { get; set; }
        public virtual MstAccountCashFlowDBSet MstAccountCashFlow_AccountCashFlowId { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_ReceivableAccountId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_AssetAccountId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_SalesAccountId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_CostAccountId { get; set; }
        public virtual ICollection<MstArticleAccountGroupDBSet> MstArticleAccountGroups_ExpenseAccountId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_AssetAccountId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_SalesAccountId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_CostAccountId { get; set; }
        public virtual ICollection<MstArticleItemDBSet> MstArticleItems_ExpenseAccountId { get; set; }
        public virtual ICollection<MstPayTypeDBSet> MstPayTypes_AccountId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_AccountId { get; set; }
    }
}
