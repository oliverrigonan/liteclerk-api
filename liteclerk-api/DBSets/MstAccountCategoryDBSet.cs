using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstAccountCategoryDBSet
    {
        public Int32 Id { get; set; }
        public String AccountCategoryCode { get; set; }
        public String ManualCode { get; set; }
        public String AccountCategory { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public virtual ICollection<MstAccountTypeDBSet> MstAccountTypes_AccountCategory { get; set; }
    }
}
