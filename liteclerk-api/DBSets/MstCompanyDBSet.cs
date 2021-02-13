using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCompanyDBSet
    {
        public Int32 Id { get; set; }

        public String CompanyCode { get; set; }
        public String ManualCode { get; set; }
        public String Company { get; set; }
        public String Address { get; set; }
        public String TIN { get; set; }
        public String ImageURL { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public String CostMethod { get; set; }

        public Int32? IncomeAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_IncomeAccountId { get; set; }

        public Int32? SalesInvoiceCheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_SalesInvoiceCheckedByUserId { get; set; }

        public Int32? SalesInvoiceApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_SalesInvoiceApprovedByUserId { get; set; }

        public Boolean IsLocked { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<MstCompanyBranchDBSet> MstCompanyBranches_CompanyId { get; set; }
        public virtual ICollection<MstUserDBSet> MstUsers_CompanyId { get; set; }
    }
}
