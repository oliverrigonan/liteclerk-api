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
    public class RepSalesInvoiceBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepSalesInvoiceBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetSalesInvoiceBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.SIId != null
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
                           Id = d.TrnSalesInvoice_SIId.Id,
                           BranchId = d.TrnSalesInvoice_SIId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnSalesInvoice_SIId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnSalesInvoice_SIId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnSalesInvoice_SIId.MstCurrency_CurrencyId.Currency
                           },
                           SINumber = d.TrnSalesInvoice_SIId.SINumber,
                           SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                           ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                           DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                           CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                           Customer = new DTO.MstArticleCustomerDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                               },
                               Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                           },
                           TermId = d.TrnSalesInvoice_SIId.TermId,
                           Term = new DTO.MstTermDTO
                           {
                               ManualCode = d.TrnSalesInvoice_SIId.MstTerm_TermId.ManualCode,
                               Term = d.TrnSalesInvoice_SIId.MstTerm_TermId.Term
                           },
                           DateNeeded = d.TrnSalesInvoice_SIId.DateNeeded.ToShortDateString(),
                           Remarks = d.TrnSalesInvoice_SIId.Remarks,
                           SoldByUserId = d.TrnSalesInvoice_SIId.SoldByUserId,
                           SoldByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnSalesInvoice_SIId.MstUser_SoldByUserId.Username,
                               Fullname = d.TrnSalesInvoice_SIId.MstUser_SoldByUserId.Fullname
                           },
                           PreparedByUserId = d.TrnSalesInvoice_SIId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnSalesInvoice_SIId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnSalesInvoice_SIId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnSalesInvoice_SIId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnSalesInvoice_SIId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnSalesInvoice_SIId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnSalesInvoice_SIId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnSalesInvoice_SIId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnSalesInvoice_SIId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnSalesInvoice_SIId.Amount,
                           PaidAmount = d.TrnSalesInvoice_SIId.PaidAmount,
                           AdjustmentAmount = d.TrnSalesInvoice_SIId.AdjustmentAmount,
                           BalanceAmount = d.TrnSalesInvoice_SIId.BalanceAmount,
                           Status = d.TrnSalesInvoice_SIId.Status
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
