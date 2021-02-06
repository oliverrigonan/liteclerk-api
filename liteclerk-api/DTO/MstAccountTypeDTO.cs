using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstAccountTypeDTO
    {
        public Int32 Id { get; set; }

        public String AccountTypeCode { get; set; }
        public String ManualCode { get; set; }
        public String AccountType { get; set; }

        public Int32 AccountCategoryId { get; set; }
        public MstAccountCategoryDTO AccountCategory { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
