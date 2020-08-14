using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstUser
    {
        public Int32 Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Fullname { get; set; }
        public ICollection<MstCompany> CreatedByUserCompanies { get; set; }
        public ICollection<MstCompany> UpdatedByUserCompanies { get; set; }
        public ICollection<MstCurrency> CreatedByUserCurrencies { get; set; }
        public ICollection<MstCurrency> UpdatedByUserCurrencies { get; set; }
    }
}
