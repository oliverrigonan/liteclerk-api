using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstCompanyBranchDTO
    {
        public Int32 Id { get; set; }
        public String BranchCode { get; set; }
        public String ManualCode { get; set; }
        public Int32 CompanyId { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String TIN { get; set; }
    }
}
