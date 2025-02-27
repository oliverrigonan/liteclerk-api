﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstCurrencyDBSet
    {
        public Int32 Id { get; set; }

        public String CurrencyCode { get; set; }
        public String ManualCode { get; set; }
        public String Currency { get; set; }
        public String Remarks { get; set; }

        public Boolean IsLocked { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<MstCurrencyExchangeDBSet> MstCurrencyExchanges_CurrencyId { get; set; }
        public virtual ICollection<MstCurrencyExchangeDBSet> MstCurrencyExchanges_ExchangeCurrencyId { get; set; }

        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_CurrencyId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_CurrencyId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_CurrencyId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_CurrencyId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_CurrencyId { get; set; }

        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_CurrencyId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_CurrencyId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_CurrencyId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_CurrencyId { get; set; }

        public virtual ICollection<TrnJobOrderDBSet> TrnJobOrders_CurrencyId { get; set; }
        public virtual ICollection<TrnStockInDBSet> TrnStockIns_CurrencyId { get; set; }
        public virtual ICollection<TrnStockOutDBSet> TrnStockOuts_CurrencyId { get; set; }
        public virtual ICollection<TrnStockTransferDBSet> TrnStockTransfers_CurrencyId { get; set; }
        public virtual ICollection<TrnStockWithdrawalDBSet> TrnStockWithdrawals_CurrencyId { get; set; }
        public virtual ICollection<TrnStockCountDBSet> TrnStockCounts_CurrencyId { get; set; }

        public virtual ICollection<TrnPurchaseRequestDBSet> TrnPurchaseRequests_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnPurchaseOrderDBSet> TrnPurchaseOrders_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnReceivingReceiptDBSet> TrnReceivingReceipts_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnDisbursementDBSet> TrnDisbursements_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnPayableMemoDBSet> TrnPayableMemos_ExchangeCurrencyId { get; set; }

        public virtual ICollection<TrnSalesOrderDBSet> TrnSalesOrders_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnSalesInvoiceDBSet> TrnSalesInvoices_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnCollectionDBSet> TrnCollections_ExchangeCurrencyId { get; set; }
        public virtual ICollection<TrnReceivableMemoDBSet> TrnReceivableMemos_ExchangeCurrencyId { get; set; }

        public virtual ICollection<TrnJournalVoucherDBSet> TrnJournalVouchers_CurrencyId { get; set; }
        public virtual ICollection<TrnInventoryDBSet> TrnInventories_CurrencyId { get; set; }

        public virtual ICollection<MstCompanyDBSet> MstCompanies_CurrencyId { get; set; }
    }
}
