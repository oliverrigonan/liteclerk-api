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
    public class TrnPayableMemoLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPayableMemoLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{PMId}")]
        public async Task<ActionResult> GetPayableMemoLineListByPayableMemo(Int32 PMId)
        {
            try
            {
                var payableMemoLines = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == PMId
                    orderby d.Id descending
                    select new DTO.TrnPayableMemoLineDTO
                    {
                        Id = d.Id,
                        PMId = d.PMId,
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
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {
                            RRNumber = d.TrnReceivingReceipt_RRId.RRNumber,
                            RRDate = d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(),
                            ManualNumber = d.TrnReceivingReceipt_RRId.ManualNumber,
                            DocumentReference = d.TrnReceivingReceipt_RRId.DocumentReference
                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars
                    }
                ).ToListAsync();

                return StatusCode(200, payableMemoLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPayableMemoLineDetail(Int32 id)
        {
            try
            {
                var payableMemoLine = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.Id == id
                    select new DTO.TrnPayableMemoLineDTO
                    {
                        Id = d.Id,
                        PMId = d.PMId,
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
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {
                            RRNumber = d.TrnReceivingReceipt_RRId.RRNumber,
                            RRDate = d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(),
                            ManualNumber = d.TrnReceivingReceipt_RRId.ManualNumber,
                            DocumentReference = d.TrnReceivingReceipt_RRId.DocumentReference
                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, payableMemoLine);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPayableMemoLine([FromBody] DTO.TrnPayableMemoLineDTO trnPayableMemoLineDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
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

                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == trnPayableMemoLineDTO.PMId
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemo == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add payable memo line if the current payable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnPayableMemoLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnPayableMemoLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnPayableMemoLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == trnPayableMemoLineDTO.RRId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                var newPayableMemoLines = new DBSets.TrnPayableMemoLineDBSet()
                {
                    PMId = trnPayableMemoLineDTO.PMId,
                    BranchId = trnPayableMemoLineDTO.BranchId,
                    AccountId = trnPayableMemoLineDTO.AccountId,
                    ArticleId = trnPayableMemoLineDTO.ArticleId,
                    RRId = trnPayableMemoLineDTO.RRId,
                    Amount = trnPayableMemoLineDTO.Amount,
                    Particulars = trnPayableMemoLineDTO.Particulars
                };

                _dbContext.TrnPayableMemoLines.Add(newPayableMemoLines);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == trnPayableMemoLineDTO.PMId
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    amount = payableMemoLinesByCurrentPayableMemo.Sum(d => d.Amount);
                }

                var updatePayableMemo = payableMemo;
                updatePayableMemo.Amount = amount;
                updatePayableMemo.UpdatedByUserId = loginUserId;
                updatePayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdatePayableMemoLine(Int32 id, [FromBody] DTO.TrnPayableMemoLineDTO trnPayableMemoLineDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
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

                var payableMemoLine = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemoLine == null)
                {
                    return StatusCode(404, "Payable memo line not found.");
                }

                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == trnPayableMemoLineDTO.PMId
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemo == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update payable memo line if the current payable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnPayableMemoLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnPayableMemoLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnPayableMemoLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == trnPayableMemoLineDTO.RRId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                var updatePayableMemoLines = payableMemoLine;
                updatePayableMemoLines.PMId = trnPayableMemoLineDTO.PMId;
                updatePayableMemoLines.BranchId = trnPayableMemoLineDTO.BranchId;
                updatePayableMemoLines.AccountId = trnPayableMemoLineDTO.AccountId;
                updatePayableMemoLines.ArticleId = trnPayableMemoLineDTO.ArticleId;
                updatePayableMemoLines.RRId = trnPayableMemoLineDTO.RRId;
                updatePayableMemoLines.Amount = trnPayableMemoLineDTO.Amount;
                updatePayableMemoLines.Particulars = trnPayableMemoLineDTO.Particulars;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == trnPayableMemoLineDTO.PMId
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    amount = payableMemoLinesByCurrentPayableMemo.Sum(d => d.Amount);
                }

                var updatePayableMemo = payableMemo;
                updatePayableMemo.Amount = amount;
                updatePayableMemo.UpdatedByUserId = loginUserId;
                updatePayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePayableMemoLine(int id)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
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

                Int32 PMId = 0;

                var payableMemoLine = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemoLine == null)
                {
                    return StatusCode(404, "Payable memo line not found.");
                }

                PMId = payableMemoLine.PMId;

                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == PMId
                    select d
                ).FirstOrDefaultAsync();

                if (payableMemo == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete payable memo line if the current payable memo is locked.");
                }

                _dbContext.TrnPayableMemoLines.Remove(payableMemoLine);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == PMId
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    amount = payableMemoLinesByCurrentPayableMemo.Sum(d => d.Amount);
                }

                var updatePayableMemo = payableMemo;
                updatePayableMemo.Amount = amount;
                updatePayableMemo.UpdatedByUserId = loginUserId;
                updatePayableMemo.UpdatedDateTime = DateTime.Now;

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
