using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasySHOP.DTO
{
    public class EasySHOPMstCompanyBranchDTO
    {
        public Int32 Id { get; set; }
        public String Company { get; set; }
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String TaxNumber { get; set; }
    }
}
