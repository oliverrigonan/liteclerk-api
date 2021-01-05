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
    public class RepDisbursementBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepDisbursementBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetDisbursementBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.CVId != null
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
                           Id = d.TrnDisbursement_CVId.Id,
                           BranchId = d.TrnDisbursement_CVId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.TrnDisbursement_CVId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnDisbursement_CVId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnDisbursement_CVId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnDisbursement_CVId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnDisbursement_CVId.MstCurrency_CurrencyId.Currency
                           },
                           CVNumber = d.TrnDisbursement_CVId.CVNumber,
                           CVDate = d.TrnDisbursement_CVId.CVDate.ToShortDateString(),
                           ManualNumber = d.TrnDisbursement_CVId.ManualNumber,
                           DocumentReference = d.TrnDisbursement_CVId.DocumentReference,
                           SupplierId = d.TrnDisbursement_CVId.SupplierId,
                           Supplier = new DTO.MstArticleSupplierDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnDisbursement_CVId.MstArticle_SupplierId.ManualCode
                               },
                               Supplier = d.TrnDisbursement_CVId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnDisbursement_CVId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
                           },
                           Payee = d.TrnDisbursement_CVId.Payee,
                           Remarks = d.TrnDisbursement_CVId.Remarks,
                           PayTypeId = d.TrnDisbursement_CVId.PayTypeId,
                           PayType = new DTO.MstPayTypeDTO
                           {
                               ManualCode = d.TrnDisbursement_CVId.MstPayType_PayTypeId.ManualCode,
                               PayType = d.TrnDisbursement_CVId.MstPayType_PayTypeId.PayType
                           },
                           CheckNumber = d.TrnDisbursement_CVId.CheckNumber,
                           CheckDate = d.TrnDisbursement_CVId.CheckDate != null ? Convert.ToDateTime(d.TrnDisbursement_CVId.CheckDate).ToShortDateString() : "",
                           CheckBank = d.TrnDisbursement_CVId.CheckBank,
                           IsCrossCheck = d.TrnDisbursement_CVId.IsCrossCheck,
                           BankId = d.TrnDisbursement_CVId.BankId,
                           Bank = new DTO.MstArticleBankDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnDisbursement_CVId.MstArticle_BankId.ManualCode
                               },
                               Bank = d.TrnDisbursement_CVId.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.TrnDisbursement_CVId.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
                           },
                           IsClear = d.TrnDisbursement_CVId.IsClear,
                           PreparedByUserId = d.TrnDisbursement_CVId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnDisbursement_CVId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnDisbursement_CVId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnDisbursement_CVId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnDisbursement_CVId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnDisbursement_CVId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnDisbursement_CVId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnDisbursement_CVId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnDisbursement_CVId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnDisbursement_CVId.Amount,
                           Status = d.TrnDisbursement_CVId.Status
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
