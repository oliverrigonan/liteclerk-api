using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnCounterReceiptLineDBSet
    {
        public Int32 Id { get; set; }

        public Int32 CRId { get; set; }
        public virtual TrnCounterReceiptDBSet TrnCounterReceipt_CRId { get; set; }

        public String Particulars { get; set; }
    }
}
