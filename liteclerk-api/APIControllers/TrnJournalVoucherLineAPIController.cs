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
    public class TrnJournalVoucherLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnJournalVoucherLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{JVId}")]
        public async Task<ActionResult> GetJournalVoucherLineListByJournalVoucher(Int32 JVId)
        {
            try
            {
                var journalVoucherLines = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.JVId == JVId
                    orderby d.Id descending
                    select new DTO.TrnJournalVoucherLineDTO
                    {
                        Id = d.Id,
                        JVId = d.JVId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Particulars = d.Particulars
                    }
                ).ToListAsync();

                return StatusCode(200, journalVoucherLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJournalVoucherLineDetail(Int32 id)
        {
            try
            {
                var journalVoucherLine = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.Id == id
                    select new DTO.TrnJournalVoucherLineDTO
                    {
                        Id = d.Id,
                        JVId = d.JVId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Particulars = d.Particulars
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, journalVoucherLine);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJournalVoucherLine([FromBody] DTO.TrnJournalVoucherLineDTO trnJournalVoucherLineDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a payable memo line.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a payable memo line.");
                }

                var journalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.Id == trnJournalVoucherLineDTO.JVId
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucher == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add payable memo line if the current payable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnJournalVoucherLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnJournalVoucherLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnJournalVoucherLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var newJournalVoucherLines = new DBSets.TrnJournalVoucherLineDBSet()
                {
                    JVId = trnJournalVoucherLineDTO.JVId,
                    BranchId = trnJournalVoucherLineDTO.BranchId,
                    AccountId = trnJournalVoucherLineDTO.AccountId,
                    ArticleId = trnJournalVoucherLineDTO.ArticleId,
                    DebitAmount = trnJournalVoucherLineDTO.DebitAmount,
                    CreditAmount = trnJournalVoucherLineDTO.CreditAmount,
                    Particulars = trnJournalVoucherLineDTO.Particulars
                };

                _dbContext.TrnJournalVoucherLines.Add(newJournalVoucherLines);
                await _dbContext.SaveChangesAsync();

                Decimal debitAmount = 0;
                Decimal creditAmount = 0;

                var journalVoucherLinesByCurrentJournalVoucher = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.JVId == trnJournalVoucherLineDTO.JVId
                    select d
                ).ToListAsync();

                if (journalVoucherLinesByCurrentJournalVoucher.Any())
                {
                    debitAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.DebitAmount);
                    creditAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.CreditAmount);
                }

                var updateJournalVoucher = journalVoucher;
                updateJournalVoucher.DebitAmount = debitAmount;
                updateJournalVoucher.CreditAmount = creditAmount;
                updateJournalVoucher.UpdatedByUserId = loginUserId;
                updateJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateJournalVoucherLine(Int32 id, [FromBody] DTO.TrnJournalVoucherLineDTO trnJournalVoucherLineDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a payable memo line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a payable memo line.");
                }

                var journalVoucherLine = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucherLine == null)
                {
                    return StatusCode(404, "Payable memo line not found.");
                }

                var journalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.Id == trnJournalVoucherLineDTO.JVId
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucher == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update payable memo line if the current payable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnJournalVoucherLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnJournalVoucherLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnJournalVoucherLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var updateJournalVoucherLines = journalVoucherLine;
                updateJournalVoucherLines.JVId = trnJournalVoucherLineDTO.JVId;
                updateJournalVoucherLines.BranchId = trnJournalVoucherLineDTO.BranchId;
                updateJournalVoucherLines.AccountId = trnJournalVoucherLineDTO.AccountId;
                updateJournalVoucherLines.ArticleId = trnJournalVoucherLineDTO.ArticleId;
                updateJournalVoucherLines.DebitAmount = trnJournalVoucherLineDTO.DebitAmount;
                updateJournalVoucherLines.CreditAmount = trnJournalVoucherLineDTO.CreditAmount;
                updateJournalVoucherLines.Particulars = trnJournalVoucherLineDTO.Particulars;

                await _dbContext.SaveChangesAsync();

                Decimal debitAmount = 0;
                Decimal creditAmount = 0;

                var journalVoucherLinesByCurrentJournalVoucher = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.JVId == trnJournalVoucherLineDTO.JVId
                    select d
                ).ToListAsync();

                if (journalVoucherLinesByCurrentJournalVoucher.Any())
                {
                    debitAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.DebitAmount);
                    creditAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.CreditAmount);
                }

                var updateJournalVoucher = journalVoucher;
                updateJournalVoucher.DebitAmount = debitAmount;
                updateJournalVoucher.CreditAmount = creditAmount;
                updateJournalVoucher.UpdatedByUserId = loginUserId;
                updateJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJournalVoucherLine(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a payable memo line.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a payable memo line.");
                }

                Int32 JVId = 0;

                var journalVoucherLine = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucherLine == null)
                {
                    return StatusCode(404, "Payable memo line not found.");
                }

                JVId = journalVoucherLine.JVId;

                var journalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.Id == JVId
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucher == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete payable memo line if the current payable memo is locked.");
                }

                _dbContext.TrnJournalVoucherLines.Remove(journalVoucherLine);
                await _dbContext.SaveChangesAsync();

                Decimal debitAmount = 0;
                Decimal creditAmount = 0;

                var journalVoucherLinesByCurrentJournalVoucher = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.JVId == JVId
                    select d
                ).ToListAsync();

                if (journalVoucherLinesByCurrentJournalVoucher.Any())
                {
                    debitAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.DebitAmount);
                    creditAmount = journalVoucherLinesByCurrentJournalVoucher.Sum(d => d.CreditAmount);
                }

                var updateJournalVoucher = journalVoucher;
                updateJournalVoucher.DebitAmount = debitAmount;
                updateJournalVoucher.CreditAmount = creditAmount;
                updateJournalVoucher.UpdatedByUserId = loginUserId;
                updateJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
