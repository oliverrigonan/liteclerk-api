using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstJobTypeDepartmentDTO
    {
        public Int32 Id { get; set; }

        public Int32 JobTypeId { get; set; }
        public MstJobTypeDTO JobType { get; set; }

        public Int32 JobDepartmentId { get; set; }
        public MstJobDepartmentDTO JobDepartment { get; set; }

        public Decimal NumberOfDays { get; set; }
        public Int32 SequenceNumber { get; set; }
        public Boolean IsRequired { get; set; }
    }
}
