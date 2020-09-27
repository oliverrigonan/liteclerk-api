using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJournalVoucherLineDBSet
    {
        public Int32 Id { get; set; }

        public Int32 JVId { get; set; }
        public virtual TrnJournalVoucherDBSet TrnJournalVoucher_JVId { get; set; }

        public String Particulars { get; set; }
    }
}
