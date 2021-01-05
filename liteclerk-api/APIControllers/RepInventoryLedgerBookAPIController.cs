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
    public class RepInventoryLedgerBookAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepInventoryLedgerBookAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetInventoryLedgerBookList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var journalEntries = await (
                   from d in _dbContext.SysJournalEntries
                   where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                   && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                   && d.BranchId == branchId
                   && d.ILId != null
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

                       },
                       ILId = d.ILId,
                       InventoryLedger = new DTO.TrnInventoryDTO
                       {
                           Id = d.TrnInventory_ILId.Id,
                           BranchId = d.TrnInventory_ILId.BranchId,
                           Branch = new DTO.MstCompanyBranchDTO
                           {
                               BranchCode = d.TrnInventory_ILId.MstCompanyBranch_BranchId.BranchCode,
                               ManualCode = d.TrnInventory_ILId.MstCompanyBranch_BranchId.ManualCode,
                               Branch = d.TrnInventory_ILId.MstCompanyBranch_BranchId.Branch
                           },
                           CurrencyId = d.TrnInventory_ILId.CurrencyId,
                           Currency = new DTO.MstCurrencyDTO
                           {
                               CurrencyCode = d.TrnInventory_ILId.MstCurrency_CurrencyId.CurrencyCode,
                               ManualCode = d.TrnInventory_ILId.MstCurrency_CurrencyId.ManualCode,
                               Currency = d.TrnInventory_ILId.MstCurrency_CurrencyId.Currency
                           },
                           ILNumber = d.TrnInventory_ILId.ILNumber,
                           ILDate = d.TrnInventory_ILId.ILDate.ToShortDateString(),
                           ManualNumber = d.TrnInventory_ILId.ManualNumber,
                           DocumentReference = d.TrnInventory_ILId.DocumentReference,
                           Month = d.TrnInventory_ILId.Month,
                           Year = d.TrnInventory_ILId.Year,
                           Remarks = d.TrnInventory_ILId.Remarks,
                           PreparedByUserId = d.TrnInventory_ILId.PreparedByUserId,
                           PreparedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnInventory_ILId.MstUser_PreparedByUserId.Username,
                               Fullname = d.TrnInventory_ILId.MstUser_PreparedByUserId.Fullname
                           },
                           CheckedByUserId = d.TrnInventory_ILId.CheckedByUserId,
                           CheckedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnInventory_ILId.MstUser_CheckedByUserId.Username,
                               Fullname = d.TrnInventory_ILId.MstUser_CheckedByUserId.Fullname
                           },
                           ApprovedByUserId = d.TrnInventory_ILId.ApprovedByUserId,
                           ApprovedByUser = new DTO.MstUserDTO
                           {
                               Username = d.TrnInventory_ILId.MstUser_ApprovedByUserId.Username,
                               Fullname = d.TrnInventory_ILId.MstUser_ApprovedByUserId.Fullname
                           },
                           Status = d.TrnInventory_ILId.Status
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
