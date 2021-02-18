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
    public class RepTrialBalanceAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTrialBalanceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}")]
        public async Task<ActionResult> GetTrialBalanceList(String startDate, String endDate, Int32 companyId)
        {
            try
            {
                List<DTO.SysJournalEntryDTO> newJournals = new List<DTO.SysJournalEntryDTO>();

                var journals = await (
                    from d in _dbContext.SysJournalEntries
                    where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                    && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    select d
                ).ToListAsync();

                if (journals.Any() == true)
                {
                    var groupedJournals = from d in journals
                                          group d by new
                                          {
                                              Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company,
                                              BranchId = d.BranchId,
                                              BranchManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                              Branch = d.MstCompanyBranch_BranchId.Branch,
                                              AccountId = d.AccountId,
                                              AccountManualCode = d.MstAccount_AccountId.ManualCode,
                                              Account = d.MstAccount_AccountId.Account
                                          } into g
                                          select new
                                          {
                                              Company = g.Key.Company,
                                              BranchId = g.Key.BranchId,
                                              BranchManualCode = g.Key.BranchManualCode,
                                              Branch = g.Key.Branch,
                                              AccountId = g.Key.AccountId,
                                              AccountManualCode = g.Key.AccountManualCode,
                                              Account = g.Key.Account,
                                              TotalDebitAmount = g.Sum(s => s.DebitAmount),
                                              TotalCreditAmount = g.Sum(s => s.CreditAmount)
                                          };

                    foreach (var groupedJournal in groupedJournals.ToList())
                    {
                        newJournals.Add(new DTO.SysJournalEntryDTO
                        {
                            Id = 0,
                            BranchId = groupedJournal.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = groupedJournal.BranchManualCode,
                                Branch = groupedJournal.Branch,
                                Company = new DTO.MstCompanyDTO
                                {
                                    ManualCode = "",
                                    Company = groupedJournal.Company
                                }
                            },
                            JournalEntryDate = "",
                            ArticleId = 0,
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = "",
                                Article = ""
                            },
                            AccountId = groupedJournal.AccountId,
                            Account = new DTO.MstAccountDTO
                            {
                                ManualCode = groupedJournal.AccountManualCode,
                                Account = groupedJournal.Account
                            },
                            DebitAmount = groupedJournal.TotalDebitAmount,
                            CreditAmount = groupedJournal.TotalCreditAmount,
                            Particulars = "",
                            RRId = null,
                            ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                            {

                            },
                            SIId = null,
                            SalesInvoice = new DTO.TrnSalesInvoiceDTO
                            {

                            },
                            CIId = null,
                            Collection = new DTO.TrnCollectionDTO
                            {

                            },
                            CVId = null,
                            Disbursement = new DTO.TrnDisbursementDTO
                            {

                            },
                            PMId = null,
                            PayableMemo = new DTO.TrnPayableMemoDTO
                            {

                            },
                            RMId = null,
                            ReceivableMemo = new DTO.TrnReceivableMemoDTO
                            {

                            },
                            JVId = null,
                            JournalVoucher = new DTO.TrnJournalVoucherDTO
                            {

                            },
                            ILId = null,
                            InventoryLedger = new DTO.TrnInventoryDTO
                            {

                            }
                        });
                    }
                }

                return StatusCode(200, await Task.FromResult(newJournals));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
