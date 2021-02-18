using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RepAccountLedgerAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepAccountLedgerAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}/byAccount/{accountId}")]
        public async Task<ActionResult> GetAccountLedgerList(String startDate, String endDate, Int32 companyId, Int32 branchId, Int32 accountId)
        {
            try
            {
                var journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                    && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.BranchId == branchId
                    && d.AccountId == accountId
                    select new DTO.SysJournalEntryDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        JournalEntryDate = d.JournalEntryDate.ToShortDateString(),
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Particulars = d.Particulars,
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {
                            RRNumber = d.RRId != null ? d.TrnReceivingReceipt_RRId.RRNumber : "",
                            RRDate = d.RRId != null ? d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString() : ""
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.SIId != null ? d.TrnSalesInvoice_SIId.SINumber : "",
                            SIDate = d.SIId != null ? d.TrnSalesInvoice_SIId.SIDate.ToShortDateString() : ""
                        },
                        CIId = d.CIId,
                        Collection = new DTO.TrnCollectionDTO
                        {
                            CINumber = d.CIId != null ? d.TrnCollection_CIId.CINumber : "",
                            CIDate = d.CIId != null ? d.TrnCollection_CIId.CIDate.ToShortDateString() : ""
                        },
                        CVId = d.CVId,
                        Disbursement = new DTO.TrnDisbursementDTO
                        {
                            CVNumber = d.CIId != null ? d.TrnDisbursement_CVId.CVNumber : "",
                            CVDate = d.CIId != null ? d.TrnDisbursement_CVId.CVDate.ToShortDateString() : ""
                        },
                        PMId = d.PMId,
                        PayableMemo = new DTO.TrnPayableMemoDTO
                        {
                            PMNumber = d.PMId != null ? d.TrnPayableMemo_PMId.PMNumber : "",
                            PMDate = d.PMId != null ? d.TrnPayableMemo_PMId.PMDate.ToShortDateString() : ""
                        },
                        RMId = d.RMId,
                        ReceivableMemo = new DTO.TrnReceivableMemoDTO
                        {
                            RMNumber = d.RMId != null ? d.TrnReceivableMemo_RMId.RMNumber : "",
                            RMDate = d.RMId != null ? d.TrnReceivableMemo_RMId.RMDate.ToShortDateString() : ""
                        },
                        JVId = d.JVId,
                        JournalVoucher = new DTO.TrnJournalVoucherDTO
                        {
                            JVNumber = d.RMId != null ? d.TrnJournalVoucher_JVId.JVNumber : "",
                            JVDate = d.RMId != null ? d.TrnJournalVoucher_JVId.JVDate.ToShortDateString() : ""
                        },
                        ILId = d.ILId,
                        InventoryLedger = new DTO.TrnInventoryDTO
                        {
                            ILNumber = d.RMId != null ? d.TrnInventory_ILId.ILNumber : "",
                            ILDate = d.RMId != null ? d.TrnInventory_ILId.ILDate.ToShortDateString() : ""
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, journalEntries);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
