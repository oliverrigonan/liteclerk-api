using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstTermDBSet
    {
        public Int32 Id { get; set; }
        public String TermCode { get; set; }
        public String ManualCode { get; set; }
        public String Term { get; set; }
        public Decimal NumberOfDays { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_Term { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Term { get; set; }
    }
}
