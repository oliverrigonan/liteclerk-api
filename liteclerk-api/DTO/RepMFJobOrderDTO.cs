using liteclerk_api.DBSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepMFJobOrderDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public Int32 NoOfDays { get; set; }
        public MstCompanyBranchDTO Branch { get; set; }

        public String SINumber { get; set; }
        public TrnSalesInvoiceMFJOItemDTO MFJobOrder { get; set; }

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

        public String Status { get; set; }

        public String Description { get; set; }
        public String Brand { get; set; }
        public String Serial { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
    }
}
