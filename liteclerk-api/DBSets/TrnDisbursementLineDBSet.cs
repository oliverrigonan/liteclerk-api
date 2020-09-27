using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnDisbursementLineDBSet
    {
        public Int32 Id { get; set; }

        public Int32 CVId { get; set; }
        public virtual TrnDisbursementDBSet TrnDisbursement_CVId { get; set; }

        public String Particulars { get; set; }
    }
}
