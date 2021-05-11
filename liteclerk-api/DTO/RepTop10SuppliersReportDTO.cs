using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepTop10SuppliersReportDTO
    {
        public Int32 SupplierId { get; set; }
        public MstArticleSupplierDTO Supplier { get; set; }

        public Decimal Amount { get; set; }
    }
}
