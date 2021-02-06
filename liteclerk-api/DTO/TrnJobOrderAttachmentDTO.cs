using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnJobOrderAttachmentDTO
    {
        public Int32 Id { get; set; }

        public Int32 JOId { get; set; }
        public TrnJobOrderDTO JobOrder { get; set; }

        public String AttachmentCode { get; set; }
        public String AttachmentType { get; set; }
        public String AttachmentURL { get; set; }
        public String Particulars { get; set; }
        public Boolean IsPrinted { get; set; }
    }
}
