using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJobOrderDepartmentDBSet
    {
        public Int32 Id { get; set; }
        public Int32 JOId { get; set; }
        public virtual TrnJobOrderDBSet TrnJobOrder_JobOrder { get; set; }
        public Int32 JobDepartmentId { get; set; }
        public virtual MstJobDepartmentDBset MstJobDepartment_JobDepartment { get; set; }
        public String Particulars { get; set; }
        public String Status { get; set; }
        public Int32 StatusByUserId { get; set; }
        public virtual MstUserDBSet MstUser_StatusByUser { get; set; }
        public DateTime StatusUpdatedDateTime { get; set; }
    }
}
