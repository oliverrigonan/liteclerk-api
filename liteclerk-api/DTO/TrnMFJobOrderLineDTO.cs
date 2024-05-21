using liteclerk_api.DBSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnMFJobOrderLineDTO
    {
        public Int32 Id { get; set; }

        public Int32 MFJOId { get; set; }
        public TrnMFJobOrderDBSet MFJobOrder { get; set; }

        public String Description { get; set; }
        public String Brand { get; set; }
        public String Serial { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
    }
}
