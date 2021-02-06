using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstUserBranchDBSet
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }
        public virtual MstUserDBSet MstUser_UserId { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }
    }
}
