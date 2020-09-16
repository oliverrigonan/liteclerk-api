using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstUserDTO
    {
        public Int32 Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Fullname { get; set; }
        public Int32? CompanyId { get; set; }
        public MstCompanyDTO Company { get; set; }
        public Int32? BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsLocked { get; set; }
    }
}
