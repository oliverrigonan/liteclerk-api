﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstAccountTypeDBSet
    {
        public Int32 Id { get; set; }
        public String AccountTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String AccountType { get; set; }
        public Int32 AccountCategoryId { get; set; }
        public virtual MstAccountCategoryDBSet MstAccountCategory_AccountCategory { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedByDateTime { get; set; }
        public virtual ICollection<MstAccountDBSet> MstAccounts_AccountType { get; set; }
    }
}
