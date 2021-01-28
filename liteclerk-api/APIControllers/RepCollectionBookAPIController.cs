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
    public class RepCollectionBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepCollectionBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetCollectionBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.CIId != null
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
                           Id = d.TrnCollection_CIId.Id,
                           BranchId = d.TrnCollection_CIId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.TrnCollection_CIId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnCollection_CIId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnCollection_CIId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnCollection_CIId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnCollection_CIId.MstCurrency_CurrencyId.Currency
                           },
                           CINumber = d.TrnCollection_CIId.CINumber,
                           CIDate = d.TrnCollection_CIId.CIDate.ToShortDateString(),
                           ManualNumber = d.TrnCollection_CIId.ManualNumber,
                           DocumentReference = d.TrnCollection_CIId.DocumentReference,
                           CustomerId = d.TrnCollection_CIId.CustomerId,
                           Customer = new DTO.MstArticleCustomerDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnCollection_CIId.MstArticle_CustomerId.ManualCode
                               },
                               Customer = d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                           },
                           Remarks = d.TrnCollection_CIId.Remarks,
                           PreparedByUserId = d.TrnCollection_CIId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnCollection_CIId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnCollection_CIId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnCollection_CIId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnCollection_CIId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnCollection_CIId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnCollection_CIId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnCollection_CIId.Amount,
                           Status = d.TrnCollection_CIId.Status
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
