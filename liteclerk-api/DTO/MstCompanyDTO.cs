using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstCompanyDTO
    {
        public Int32 Id { get; set; }
        public String CompanyCode { get; set; }
        public String ManualCode { get; set; }
        public String Company { get; set; }
        public String Address { get; set; }
        public String TIN { get; set; }
        public Int32 CurrencyId { get; set; }
        public String CostMethod { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedByDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedByDateTime { get; set; }
    }
}
