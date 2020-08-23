using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstTermDBSet
    {
        [Column(Order = 0)]
        public Int32 Id { get; set; }

        [Column(Order = 1)]
        public String TermCode { get; set; }

        [Column(Order = 2)]
        public String ManualCode { get; set; }

        [Column(Order = 3)]
        public String Term { get; set; }

        [Column(Order = 4)]
        public Decimal NumberOfDays { get; set; }

        [Column(Order = 5)]
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }

        [Column(Order = 6)]
        public DateTime CreatedDateTime { get; set; }

        [Column(Order = 8)]
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }

        [Column(Order = 7)]
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstArticleCustomerDBSet> MstArticleCustomers_Term { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_Term { get; set; }
    }
}
