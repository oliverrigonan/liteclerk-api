using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCurrencyDBSet
    {
        public Int32 Id { get; set; }
        public String CurrencyCode { get; set; }
        public String ManualCode { get; set; }
        public String Currency { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstCompanyDBSet> MstCompanies_Currency { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Currency { get; set; }
    }
}
