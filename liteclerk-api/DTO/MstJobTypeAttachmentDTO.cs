using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstJobTypeAttachmentDTO
    {
        public Int32 Id { get; set; }

        public Int32 JobTypeId { get; set; }
        public MstJobTypeDTO JobType { get; set; }

        public String AttachmentCode { get; set; }
        public String AttachmentType { get; set; }
        public Boolean IsPrinted { get; set; }
    }
}
