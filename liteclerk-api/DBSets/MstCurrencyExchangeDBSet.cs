using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCurrencyExchangeDBSet
    {
        public Int32 Id { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public Int32 ExchangeCurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_ExchangeCurrencyId { get; set; }

        public DateTime ExchangeDate { get; set; }
        public Decimal ExchangeRate { get; set; }
    }
}
