using liteclerk_api.DBSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnMFJobOrderDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public String JONumber { get; set; }
        public String JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }
        public String DateScheduled { get; set; }
        public String DateNeeded { get; set; }

        public Int32 CustomerId { get; set; }
        public MstArticleDTO Customer { get; set; }

        public String Accessories { get; set; }
        public String Engineer { get; set; }
        public String Complaint { get; set; }
        public String Remarks { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public MstUserDTO PreparedByUser { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public MstUserDTO CheckedByUser { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public MstUserDTO ApprovedByUser { get; set; }

        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
