using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysProductionDBSet
    {
        public Int32 Id { get; set; }
        public String PNNumber { get; set; }
        public DateTime PNDate { get; set; }
        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }
        public String Status { get; set; }
        public String Particulars { get; set; }
        public DateTime ProductionTimeStamp { get; set; }
        public Int32 UserId { get; set; }
        public virtual MstUserDBSet MstUser_UserId { get; set; }

        // Job Relationship <Do not modify>
        public Int32? JODepartmentId { get; set; }
        public virtual TrnJobOrderDepartmentDBSet TrnJobOrderDepartment_JODepartmentId { get; set; }
    }
}
