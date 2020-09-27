﻿using System;
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
    public class SysJournalEntryAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysJournalEntryAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/inventoryLedger/{ILId}")]
        public async Task<ActionResult> GetStockInJournalEntryList(Int32 ILId)
        {
            try
            {
                IEnumerable<DTO.SysJournalEntryDTO> journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.ILId == ILId
                    select new DTO.SysJournalEntryDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        JournalEntryDate = d.JournalEntryDate.ToShortDateString(),
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
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
