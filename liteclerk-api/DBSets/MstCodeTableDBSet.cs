﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCodeTableDBSet
    {
        public Int32 Id { get; set; }

        public String Code { get; set; }
        public String CodeValue { get; set; }
        public String Category { get; set; }
    }
}
