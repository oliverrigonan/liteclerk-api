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
    public class RepPayableMemoBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepPayableMemoBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetPayableMemoBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.PMId != null
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
                           Id = d.Id,
                           BranchId = d.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnPayableMemo_PMId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               ManualCode = d.TrnPayableMemo_PMId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnPayableMemo_PMId.MstCurrency_CurrencyId.Currency
                           },
                           PMNumber = d.TrnPayableMemo_PMId.PMNumber,
                           PMDate = d.TrnPayableMemo_PMId.PMDate.ToShortDateString(),
                           ManualNumber = d.TrnPayableMemo_PMId.ManualNumber,
                           DocumentReference = d.TrnPayableMemo_PMId.DocumentReference,
                           SupplierId = d.TrnPayableMemo_PMId.SupplierId,
                           Supplier = new DTO.MstArticleSupplierDTO
                           {
                               Article = new DTO.MstArticleDTO
                               {
                                   ManualCode = d.TrnPayableMemo_PMId.MstArticle_SupplierId.ManualCode
                               },
                               Supplier = d.TrnPayableMemo_PMId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnPayableMemo_PMId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
                           },
                           Remarks = d.TrnPayableMemo_PMId.Remarks,
                           PreparedByUserId = d.TrnPayableMemo_PMId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnPayableMemo_PMId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnPayableMemo_PMId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnPayableMemo_PMId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnPayableMemo_PMId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnPayableMemo_PMId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnPayableMemo_PMId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnPayableMemo_PMId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnPayableMemo_PMId.MstUser_ApprovedByUserId.Fullname
                           },
                           Amount = d.TrnPayableMemo_PMId.Amount,
                           Status = d.TrnPayableMemo_PMId.Status
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
