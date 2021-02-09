using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepStockCardDTO
    {
        public String Document { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
        public MstArticleItemDTO Item { get; set; }
        public Int32? RRId { get; set; }
        public Int32? SIId { get; set; }
        public Int32? INId { get; set; }
        public Int32? OTId { get; set; }
        public Int32? STId { get; set; }
        public Int32? SWId { get; set; }
        public Int32? ILId { get; set; }
        public Decimal InQuantity { get; set; }
        public Decimal OutQuantity { get; set; }
        public Decimal BalanceQuantity { get; set; }
        public Decimal RunningQuantity { get; set; }
        public MstUnitDTO Unit { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    }
}
