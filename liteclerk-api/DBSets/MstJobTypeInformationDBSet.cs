using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstJobTypeInformationDBSet
    {
        public Int32 Id { get; set; }

        public Int32 JobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_JobTypeId { get; set; }

        public String InformationCode { get; set; }
        public String InformationGroup { get; set; }
        public Boolean IsPrinted { get; set; }
    }
}
