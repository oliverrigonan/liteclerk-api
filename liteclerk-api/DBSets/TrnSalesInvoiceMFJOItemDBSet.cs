using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnSalesInvoiceMFJOItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }

        public Int32 MFJOId { get; set; }
        public virtual TrnMFJobOrderDBSet TrnMFJobOrder_MFJOItemId { get; set; }
    }
}
