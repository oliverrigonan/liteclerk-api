using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstJobTypeDTO
    {
        public Int32 Id { get; set; }
        public String JobTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String JobType { get; set; }
        public Decimal TotalNumberOfDays { get; set; }
        public String Remarks { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedByDateTime { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedByDateTime { get; set; }
    }
}
