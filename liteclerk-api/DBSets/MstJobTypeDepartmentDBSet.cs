using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstJobTypeDepartmentDBSet
    {
        public Int32 Id { get; set; }
        public Int32 JobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_JobTypeId { get; set; }
        public Int32 JobDepartmentId { get; set; }
        public virtual MstJobDepartmentDBset MstJobDepartment_JobDepartmentId { get; set; }
        public Decimal NumberOfDays { get; set; }
        public Int32 SequenceNumber { get; set; }
        public Boolean IsRequired { get; set; }
    }
}
