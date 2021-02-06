using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnJobOrderInformationDTO
    {
        public Int32 Id { get; set; }

        public Int32 JOId { get; set; }
        public TrnJobOrderDTO JobOrder { get; set; }

        public String InformationCode { get; set; }
        public String InformationGroup { get; set; }
        public String Value { get; set; }
        public String Particulars { get; set; }
        public Boolean IsPrinted { get; set; }

        public Int32 InformationByUserId { get; set; }
        public MstUserDTO InformationByUser { get; set; }
        public String InformationUpdatedDateTime { get; set; }
    }
}
