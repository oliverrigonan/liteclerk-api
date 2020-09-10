using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstUserFormDBSet
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public virtual MstUserDBSet MstUser_UserId { get; set; }
        public Int32 FormId { get; set; }
        public virtual SysFormDBSet SysForm_FormId { get; set; }
        public Boolean CanAdd { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanLock { get; set; }
        public Boolean CanUnlock { get; set; }
        public Boolean CanCancel { get; set; }
        public Boolean CanPrint { get; set; }
    }
}
