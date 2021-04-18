using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysFormTableDBSet
    {
        public Int32 Id { get; set; }

        public Int32 FormId { get; set; }
        public virtual SysFormDBSet SysForm_FormId { get; set; }

        public String Table { get; set; }

        public virtual ICollection<SysFormTableColumnDBSet> SysFormTableColumns_TableId { get; set; }
    }
}