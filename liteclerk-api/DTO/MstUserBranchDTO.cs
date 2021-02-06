using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstUserBranchDTO
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }
        public MstUserDTO User { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
    }
}
