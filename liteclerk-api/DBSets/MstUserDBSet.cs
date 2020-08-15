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
        public MstCompanyDBSet Company { get; set; }
        public Int32? CompanyBranchId { get; set; }
        public MstCompanyBranchDBSet CompanyBranch { get; set; }
        public ICollection<MstCompanyDBSet> MstCompanies_CreatedByUser { get; set; }
        public ICollection<MstCompanyDBSet> MstCompanies_UpdatedByUser { get; set; }
        public ICollection<MstCurrencyDBSet> MstCurrencies_CreatedByUser { get; set; }
        public ICollection<MstCurrencyDBSet> MstCurrencies_UpdatedByUser { get; set; }
    }
}
