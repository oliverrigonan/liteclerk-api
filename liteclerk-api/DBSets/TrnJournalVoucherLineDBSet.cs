using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJournalVoucherLineDBSet
    {
        // Line document header relationship <Do not modify>
        public Int32 Id { get; set; }
        public Int32 JVId { get; set; }
        public virtual TrnJournalVoucherDBSet TrnJournalVoucher_JVId { get; set; }

        // Line fields


        // Line particular <Do not modify>
        public String Particulars { get; set; }
    }
}
