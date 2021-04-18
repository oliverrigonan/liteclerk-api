using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysFormTableColumnDTO
    {
        public Int32 Id { get; set; }

        public Int32 TableId { get; set; }
        public SysFormTableDTO Table { get; set; }

        public String Column { get; set; }
        public Boolean IsDisplayed { get; set; }
    }
}
