﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstJobDepartmentDBset
    {
        public Int32 Id { get; set; }
        public String JobDepartmentCode { get; set; }
        public String ManualCode { get; set; }
        public String JobDepartment { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUser { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public virtual ICollection<MstJobTypeDepartmentDBSet> MstJobTypeDepartments_JobDepartment { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_JobDepartment { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_StatusByUser { get; set; }
    }
}