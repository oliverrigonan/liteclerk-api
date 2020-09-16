using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysProductionDTO
    {
        public Int32 Id { get; set; }
        public Int32 JOId { get; set; }
        public String JONumber { get; set; }
        public String JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public String DateScheduled { get; set; }
        public String DateNeeded { get; set; }
        public Int32? SIId { get; set; }
        public TrnSalesInvoiceDTO SalesInvoice { get; set; }
        public Int32 ItemId { get; set; }
        public MstArticleItemDTO Item { get; set; }
        public Int32 ItemJobTypeId { get; set; }
        public MstJobTypeDTO ItemJobType { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }
        public String Remarks { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public MstUnitDTO BaseUnit { get; set; }
        public String Status { get; set; }
    }
}
