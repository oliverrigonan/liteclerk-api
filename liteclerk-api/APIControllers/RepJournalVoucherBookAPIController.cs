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
    public class RepJournalVoucherBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepJournalVoucherBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetJournalVoucherBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.JVId != null
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

                       },
                       JVId = d.JVId,
                       JournalVoucher = new DTO.TrnJournalVoucherDTO
                       {
                           Id = d.TrnJournalVoucher_JVId.Id,
                           BranchId = d.TrnJournalVoucher_JVId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.TrnJournalVoucher_JVId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnJournalVoucher_JVId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnJournalVoucher_JVId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnJournalVoucher_JVId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnJournalVoucher_JVId.MstCurrency_CurrencyId.Currency
                           },
                           JVNumber = d.TrnJournalVoucher_JVId.JVNumber,
                           JVDate = d.TrnJournalVoucher_JVId.JVDate.ToShortDateString(),
                           ManualNumber = d.TrnJournalVoucher_JVId.ManualNumber,
                           DocumentReference = d.TrnJournalVoucher_JVId.DocumentReference,
                           Remarks = d.TrnJournalVoucher_JVId.Remarks,
                           PreparedByUserId = d.TrnJournalVoucher_JVId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnJournalVoucher_JVId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnJournalVoucher_JVId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnJournalVoucher_JVId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnJournalVoucher_JVId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnJournalVoucher_JVId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnJournalVoucher_JVId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnJournalVoucher_JVId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnJournalVoucher_JVId.MstUser_ApprovedByUserId.Fullname
                           },
                           DebitAmount = d.TrnJournalVoucher_JVId.DebitAmount,
                           CreditAmount = d.TrnJournalVoucher_JVId.CreditAmount,
                           Status = d.TrnJournalVoucher_JVId.Status
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
