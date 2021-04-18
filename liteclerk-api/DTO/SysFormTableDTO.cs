using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysFormTableDTO
    {
        public Int32 Id { get; set; }

        public Int32 FormId { get; set; }
        public SysFormDTO Form { get; set; }

        public String Table { get; set; }
    }
}
