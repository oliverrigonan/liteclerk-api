using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstPayTypeDBSet
    {
        public Int32 Id { get; set; }

        public String PayTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String PayType { get; set; }

        public Int32 AccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AccountId { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_PayTypeId { get; set; }
        public virtual ICollection<TrnCollectionLineDBSet> TrnCollectionLines_PayTypeId { get; set; }
    }
}
