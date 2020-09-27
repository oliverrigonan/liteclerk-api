using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnPurchaseRequestItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 PRId { get; set; }
        public virtual TrnPurchaseRequestDBSet TrnPurchaseRequest_PRId { get; set; }

        public String Particulars { get; set; }
    }
}
