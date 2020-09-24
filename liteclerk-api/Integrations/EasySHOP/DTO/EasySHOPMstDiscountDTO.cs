﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasySHOP.DTO
{
    public class EasySHOPMstDiscountDTO
    {
        public Int32 Id { get; set; }
        public String DiscountCode { get; set; }
        public String ManualCode { get; set; }
        public String Discount { get; set; }
        public Decimal DiscountRate { get; set; }
    }
}
