using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnJobOrderDepartmentDTO
    {
        public Int32 Id { get; set; }
        public Int32 JOId { get; set; }
        public TrnJobOrderDTO JobOrder { get; set; }
        public Int32 JobDepartmentId { get; set; }
        public MstJobDepartmentDTO JobDepartment { get; set; }
        public String Particulars { get; set; }
        public String Status { get; set; }
        public Int32 StatusByUserId { get; set; }
        public MstUserDTO StatusByUser { get; set; }
        public String StatusUpdatedDateTime { get; set; }
    }
}
