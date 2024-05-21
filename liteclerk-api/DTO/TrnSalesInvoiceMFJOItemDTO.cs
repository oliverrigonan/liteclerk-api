using liteclerk_api.DBSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnSalesInvoiceMFJOItemDTO
    {
        public Int32 Id { get; set; }

        public Int32 SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }

        public Int32 MFJOId { get; set; }
        public TrnMFJobOrderDTO MFJobOrder { get; set; }
    }
}
