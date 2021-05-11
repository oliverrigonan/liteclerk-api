using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepTop10SellingBranchesReportDTO
    {
        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
        
        public Decimal Amount { get; set; }
    }
}
