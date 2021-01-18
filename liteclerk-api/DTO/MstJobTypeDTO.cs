﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstJobTypeDTO
    {
        public Int32 Id { get; set; }
        public String JobTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String JobType { get; set; }
        public Decimal TotalNumberOfDays { get; set; }
        public String Remarks { get; set; }
        public Boolean IsInventory { get; set; }
        public Boolean IsLocked { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
