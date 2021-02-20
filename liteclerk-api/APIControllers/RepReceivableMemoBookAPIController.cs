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
    public class RepReceivableMemoBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepReceivableMemoBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetReceivableMemoBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.RMId != null
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

                       },
                       SIId = d.SIId,
                       SalesInvoice = new DTO.TrnSalesInvoiceDTO
                       {

                       },
                       CIId = d.CIId,
                       Collection = new DTO.TrnCollectionDTO
                       {

                       },
                       CVId = d.CVId,
                       Disbursement = new DTO.TrnDisbursementDTO
                       {

                       },
                       PMId = d.PMId,
                       PayableMemo = new DTO.TrnPayableMemoDTO
                       {
                           
                       },
                       RMId = d.RMId,
                       ReceivableMemo = new DTO.TrnReceivableMemoDTO
                       {
                           Id = d.Id,
                           BranchId = d.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.TrnReceivableMemo_RMId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnReceivableMemo_RMId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnReceivableMemo_RMId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnReceivableMemo_RMId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnReceivableMemo_RMId.MstCurrency_CurrencyId.Currency
                           },
                           RMNumber = d.TrnReceivableMemo_RMId.RMNumber,
                           RMDate = d.TrnReceivableMemo_RMId.RMDate.ToShortDateString(),
                           ManualNumber = d.TrnReceivableMemo_RMId.ManualNumber,
                           DocumentReference = d.TrnReceivableMemo_RMId.DocumentReference,
                           CustomerId = d.TrnReceivableMemo_RMId.CustomerId,
                           Customer = new DTO.MstArticleCustomerDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnReceivableMemo_RMId.MstArticle_CustomerId.ManualCode
                               },
                               Customer = d.TrnReceivableMemo_RMId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnReceivableMemo_RMId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                           },
                           Remarks = d.TrnReceivableMemo_RMId.Remarks,
                           PreparedByUserId = d.TrnReceivableMemo_RMId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivableMemo_RMId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnReceivableMemo_RMId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnReceivableMemo_RMId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivableMemo_RMId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnReceivableMemo_RMId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnReceivableMemo_RMId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivableMemo_RMId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnReceivableMemo_RMId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnReceivableMemo_RMId.Amount,
                           Status = d.TrnReceivableMemo_RMId.Status
                       },
                       JVId = d.JVId,
                       JournalVoucher = new DTO.TrnJournalVoucherDTO
                       {

                       },
                       ILId = d.ILId,
                       InventoryLedger = new DTO.TrnInventoryDTO
                       {

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
