using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSTrnStockInDTO
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public EasyPOSMstCompanyBranchDTO Branch { get; set; }
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public String Remarks { get; set; }
        public List<EasyPOSTrnStockInItemDTO> StockInItems { get; set; }
    }
}
