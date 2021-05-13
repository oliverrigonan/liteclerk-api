using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepTop10IncomeReportDTO
    {
        public Int32 AccountId { get; set; }
        public MstAccountDTO Account { get; set; }

        public Decimal Amount { get; set; }
    }
}
