using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnStockCountItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 SCId { get; set; }
        public virtual TrnStockCountDBSet TrnStockCount_SCId { get; set; }

        public String Particulars { get; set; }
    }
}
