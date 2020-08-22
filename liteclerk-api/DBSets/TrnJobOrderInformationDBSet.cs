using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJobOrderInformationDBSet
    {
        public Int32 Id { get; set; }
        public Int32 JOId { get; set; }
        public virtual TrnJobOrderDBSet TrnJobOrder_JobOrder { get; set; }
        public String InformationCode { get; set; }
        public String InformationGroup { get; set; }
        public String Value { get; set; }
        public String Particulars { get; set; }
        public Boolean IsPrinted { get; set; }
        public Int32 InformationByUserId { get; set; }
        public virtual MstUserDBSet MstUser_InformationByUser { get; set; }
        public DateTime InformationUpdatedDateTime { get; set; }
    }
}
