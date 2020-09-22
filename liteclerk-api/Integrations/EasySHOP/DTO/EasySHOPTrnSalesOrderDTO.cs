using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasySHOP.DTO
{
    public class EasySHOPTrnSalesOrderDTO
    {
        public String SONumber { get; set; }
        public String SODate { get; set; }
        public String DocumentReference { get; set; }
        public String CustomerCode { get; set; }
        public String CustomerName { get; set; }
        public String Remarks { get; set; }
        public List<EasySHOPTrnSalesOrderItemDTO> ListSalesOrderItems { get; set; }
    }
}
