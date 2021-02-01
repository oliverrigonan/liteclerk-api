using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysProductionDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public String PNNumber { get; set; }
        public String PNDate { get; set; }
        public String Status { get; set; }
        public String Particulars { get; set; }
        public String ProductionTimeStamp { get; set; }

        public Int32 UserId { get; set; }
        public MstUserDTO User { get; set; }

        public Int32? JODepartmentId { get; set; }
        public TrnJobOrderDepartmentDTO JODepartment { get; set; }
    }
}
