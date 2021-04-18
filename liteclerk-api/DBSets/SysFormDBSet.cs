using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysFormDBSet
    {
        public Int32 Id { get; set; }

        public String Form { get; set; }
        public String Description { get; set; }

        public virtual ICollection<MstUserFormDBSet> MstUserForms_FormId { get; set; }
        public virtual ICollection<SysFormTableDBSet> SysFormTables_FormId { get; set; }
    }
}
