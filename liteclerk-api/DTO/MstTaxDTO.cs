using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstTaxDTO
    {
        public Int32 Id { get; set; }

        public String TaxCode { get; set; }
        public String ManualCode { get; set; }
        public String TaxDescription { get; set; }
        public Decimal TaxRate { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
