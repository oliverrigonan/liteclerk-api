using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnStockOutItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 OTId { get; set; }
        public virtual TrnStockOutDBSet TrnStockOut_OTId { get; set; }

        public String Particulars { get; set; }
    }
}
