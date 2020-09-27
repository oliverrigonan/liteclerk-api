using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnPurchaseOrderItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 POId { get; set; }
        public virtual TrnPurchaseOrderDBSet TrnPurchaseOrder_POId { get; set; }

        public String Particulars { get; set; }
    }
}
