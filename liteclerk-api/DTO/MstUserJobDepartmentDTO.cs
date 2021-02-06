using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstUserJobDepartmentDTO
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }
        public MstUserDTO User { get; set; }

        public Int32 JobDepartmentId { get; set; }
        public MstJobDepartmentDTO JobDepartment { get; set; }
    }
}
