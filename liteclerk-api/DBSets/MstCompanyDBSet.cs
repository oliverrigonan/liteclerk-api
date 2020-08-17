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
        public Int32 CurrencyId { get; set; }
        public MstCurrencyDBSet MstCurrency_Currency { get; set; }
        public String CostMethod { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public ICollection<MstCompanyBranchDBSet> MstCompanyBranches_Company { get; set; }
        public ICollection<MstUserDBSet> MstUsers_Company { get; set; }
    }
}
