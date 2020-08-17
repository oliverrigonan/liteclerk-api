using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstTermDTO
    {
        public Int32 Id { get; set; }
        public String TermCode { get; set; }
        public String ManualCode { get; set; }
        public String Term { get; set; }
        public Decimal NumberOfDays { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedByDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedByDateTime { get; set; }
    }
}
