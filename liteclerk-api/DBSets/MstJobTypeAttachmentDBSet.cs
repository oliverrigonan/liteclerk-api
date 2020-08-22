using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstJobTypeAttachmentDBSet
    {
        public Int32 Id { get; set; }
        public Int32 JobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_JobType { get; set; }
        public String AttachmentCode { get; set; }
        public String AttachmentType { get; set; }
        public Boolean IsPrinted { get; set; }
    }
}
