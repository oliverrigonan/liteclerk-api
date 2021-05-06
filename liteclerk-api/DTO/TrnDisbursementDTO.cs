using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class TrnDisbursementDTO
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public  MstCompanyBranchDTO Branch { get; set; }

        public Int32 CurrencyId { get; set; }
        public  MstCurrencyDTO Currency { get; set; }

        public Int32 ExchangeCurrencyId { get; set; }
        public MstCurrencyDTO ExchangeCurrency { get; set; }

        public Decimal ExchangeRate { get; set; }

        public String CVNumber { get; set; }
        public String CVDate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public Int32 SupplierId { get; set; }
        public MstArticleSupplierDTO Supplier { get; set; }

        public String Payee { get; set; }
        public String Remarks { get; set; }

        public Int32 PayTypeId { get; set; }
        public  MstPayTypeDTO PayType { get; set; }

        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public Boolean IsCrossCheck { get; set; }

        public Int32 BankId { get; set; }
        public MstArticleBankDTO Bank { get; set; }

        public Boolean IsClear { get; set; }

        public Decimal Amount { get; set; }
        public Decimal BaseAmount { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public  MstUserDTO PreparedByUser { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public  MstUserDTO CheckedByUser { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public  MstUserDTO ApprovedByUser { get; set; }

        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
