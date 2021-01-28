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
    public class RepReceivingReceiptBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepReceivingReceiptBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetReceivingReceiptBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.RRId != null
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
                           Id = d.TrnReceivingReceipt_RRId.Id,
                           BranchId = d.TrnReceivingReceipt_RRId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnReceivingReceipt_RRId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnReceivingReceipt_RRId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnReceivingReceipt_RRId.MstCurrency_CurrencyId.Currency
                           },
                           RRNumber = d.TrnReceivingReceipt_RRId.RRNumber,
                           RRDate = d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(),
                           ManualNumber = d.TrnReceivingReceipt_RRId.ManualNumber,
                           DocumentReference = d.TrnReceivingReceipt_RRId.DocumentReference,
                           SupplierId = d.TrnReceivingReceipt_RRId.SupplierId,
                           Supplier = new DTO.MstArticleSupplierDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.ManualCode
                               },
                               Supplier = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
                               PayableAccountId = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId : 0,
                               PayableAccount = new DTO.MstAccountDTO
                               {
                                   ManualCode = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.ManualCode : "",
                                   Account = d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnReceivingReceipt_RRId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.Account : ""
                               }
                           },
                           TermId = d.TrnReceivingReceipt_RRId.TermId,
                           Term = new DTO.MstTermDTO
                           {
                               ManualCode = d.TrnReceivingReceipt_RRId.MstTerm_TermId.ManualCode,
                               Term = d.TrnReceivingReceipt_RRId.MstTerm_TermId.Term
                           },
                           Remarks = d.TrnReceivingReceipt_RRId.Remarks,
                           ReceivedByUserId = d.TrnReceivingReceipt_RRId.ReceivedByUserId,
                           ReceivedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivingReceipt_RRId.MstUser_ReceivedByUserId.Username,
                               Fullname = d.TrnReceivingReceipt_RRId.MstUser_ReceivedByUserId.Fullname
                           },
                           PreparedByUserId = d.TrnReceivingReceipt_RRId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivingReceipt_RRId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnReceivingReceipt_RRId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnReceivingReceipt_RRId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivingReceipt_RRId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnReceivingReceipt_RRId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnReceivingReceipt_RRId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnReceivingReceipt_RRId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnReceivingReceipt_RRId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnReceivingReceipt_RRId.Amount,
                           PaidAmount = d.TrnReceivingReceipt_RRId.PaidAmount,
                           AdjustmentAmount = d.TrnReceivingReceipt_RRId.AdjustmentAmount,
                           BalanceAmount = d.TrnReceivingReceipt_RRId.BalanceAmount,
                           Status = d.TrnReceivingReceipt_RRId.Status,
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
