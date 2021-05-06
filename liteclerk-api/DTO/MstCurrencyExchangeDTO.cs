using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstCurrencyExchangeDTO
    {
        public Int32 Id { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDTO Currency { get; set; }

        public Int32 ExchangeCurrencyId { get; set; }
        public virtual MstCurrencyDTO ExchangeCurrency { get; set; }

        public String ExchangeDate { get; set; }
        public Decimal ExchangeRate { get; set; }
    }
}
