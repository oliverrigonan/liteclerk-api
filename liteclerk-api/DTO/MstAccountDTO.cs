using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstAccountDTO
    {
        public Int32 Id { get; set; }
        public String AccountCode { get; set; }
        public String ManualCode { get; set; }
        public String Account { get; set; }
        public Int32 AccountTypeId { get; set; }
        public Int32 AccountCashFlowId { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
