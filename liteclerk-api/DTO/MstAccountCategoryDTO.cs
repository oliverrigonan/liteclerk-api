using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstAccountCategoryDTO
    {
        public Int32 Id { get; set; }
        public String AccountCategoryCode { get; set; }
        public String ManualCode { get; set; }
        public String AccountCategory { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
