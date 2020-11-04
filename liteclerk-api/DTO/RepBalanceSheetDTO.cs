using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class RepBalanceSheetDTO
    {
        public Int32 Id { get; set; }
        public String Document { get; set; }
        public MstAccountCategoryDTO AccountCategory { get; set; }
        public MstAccountTypeDTO AccountType { get; set; }
        public MstAccountDTO Account { get; set; }
        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }
        public Decimal Balance { get; set; }
    }
}
