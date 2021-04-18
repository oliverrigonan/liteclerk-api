using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class SysFormTableColumnDBSet
    {
        public Int32 Id { get; set; }

        public Int32 TableId { get; set; }
        public virtual SysFormTableDBSet SysFormTable_TableId { get; set; }

        public String Column { get; set; }
        public Boolean IsDisplayed { get; set; }
    }
}
