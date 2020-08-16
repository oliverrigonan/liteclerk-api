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
        public MstCompanyDBSet Company { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String TIN { get; set; }
        public ICollection<MstUserDBSet> MstUsers_CompanyBranch { get; set; }
        public ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Branch { get; set; }

    }
}
