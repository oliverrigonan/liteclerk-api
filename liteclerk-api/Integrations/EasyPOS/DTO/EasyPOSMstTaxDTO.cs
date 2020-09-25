using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSMstTaxDTO
    {
        public Int32 Id { get; set; }
        public String ManualCode { get; set; }
        public String TaxDescription { get; set; }
        public Decimal TaxRate { get; set; }
    }
}
