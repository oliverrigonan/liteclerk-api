﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnStockWithdrawalItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 SWId { get; set; }
        public virtual TrnStockWithdrawalDBSet TrnStockWithdrawal_SWId { get; set; }
        
        public String Particulars { get; set; }
    }
}
