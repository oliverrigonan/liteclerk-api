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
    public class TrnReceivableMemoLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnReceivableMemoLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{RMId}")]
        public async Task<ActionResult> GetReceivableMemoLineListByReceivableMemo(Int32 RMId)
        {
            try
            {
                var receivableMemoLines = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == RMId
                    orderby d.Id descending
                    select new DTO.TrnReceivableMemoLineDTO
                    {
                        Id = d.Id,
                        RMId = d.RMId,
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
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars
                    }
                ).ToListAsync();

                return StatusCode(200, receivableMemoLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetReceivableMemoLineDetail(Int32 id)
        {
            try
            {
                var receivableMemoLine = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.Id == id
                    select new DTO.TrnReceivableMemoLineDTO
                    {
                        Id = d.Id,
                        RMId = d.RMId,
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
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, receivableMemoLine);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddReceivableMemoLine([FromBody] DTO.TrnReceivableMemoLineDTO trnReceivableMemoLineDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a receivable memo line.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a receivable memo line.");
                }

                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == trnReceivableMemoLineDTO.RMId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemo == null)
                {
                    return StatusCode(404, "Receivable memo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add receivable memo line if the current receivable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnReceivableMemoLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnReceivableMemoLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnReceivableMemoLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnReceivableMemoLineDTO.SIId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                var newReceivableMemoLines = new DBSets.TrnReceivableMemoLineDBSet()
                {
                    RMId = trnReceivableMemoLineDTO.RMId,
                    BranchId = trnReceivableMemoLineDTO.BranchId,
                    AccountId = trnReceivableMemoLineDTO.AccountId,
                    ArticleId = trnReceivableMemoLineDTO.ArticleId,
                    SIId = trnReceivableMemoLineDTO.SIId,
                    Amount = trnReceivableMemoLineDTO.Amount,
                    Particulars = trnReceivableMemoLineDTO.Particulars
                };

                _dbContext.TrnReceivableMemoLines.Add(newReceivableMemoLines);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == trnReceivableMemoLineDTO.RMId
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    amount = receivableMemoLinesByCurrentReceivableMemo.Sum(d => d.Amount);
                }

                var updateReceivableMemo = receivableMemo;
                updateReceivableMemo.Amount = amount;
                updateReceivableMemo.UpdatedByUserId = loginUserId;
                updateReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateReceivableMemoLine(Int32 id, [FromBody] DTO.TrnReceivableMemoLineDTO trnReceivableMemoLineDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a receivable memo line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a receivable memo line.");
                }

                var receivableMemoLine = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemoLine == null)
                {
                    return StatusCode(404, "Receivable memo line not found.");
                }

                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == trnReceivableMemoLineDTO.RMId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemo == null)
                {
                    return StatusCode(404, "Receivable memo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update receivable memo line if the current receivable memo is locked.");
                }

                var branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnReceivableMemoLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnReceivableMemoLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnReceivableMemoLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnReceivableMemoLineDTO.SIId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                var updateReceivableMemoLines = receivableMemoLine;
                updateReceivableMemoLines.RMId = trnReceivableMemoLineDTO.RMId;
                updateReceivableMemoLines.BranchId = trnReceivableMemoLineDTO.BranchId;
                updateReceivableMemoLines.AccountId = trnReceivableMemoLineDTO.AccountId;
                updateReceivableMemoLines.ArticleId = trnReceivableMemoLineDTO.ArticleId;
                updateReceivableMemoLines.SIId = trnReceivableMemoLineDTO.SIId;
                updateReceivableMemoLines.Amount = trnReceivableMemoLineDTO.Amount;
                updateReceivableMemoLines.Particulars = trnReceivableMemoLineDTO.Particulars;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == trnReceivableMemoLineDTO.RMId
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    amount = receivableMemoLinesByCurrentReceivableMemo.Sum(d => d.Amount);
                }

                var updateReceivableMemo = receivableMemo;
                updateReceivableMemo.Amount = amount;
                updateReceivableMemo.UpdatedByUserId = loginUserId;
                updateReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReceivableMemoLine(int id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a receivable memo line.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a receivable memo line.");
                }

                Int32 RMId = 0;

                var receivableMemoLine = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemoLine == null)
                {
                    return StatusCode(404, "Receivable memo line not found.");
                }

                RMId = receivableMemoLine.RMId;

                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == RMId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableMemo == null)
                {
                    return StatusCode(404, "Receivable memo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete receivable memo line if the current receivable memo is locked.");
                }

                _dbContext.TrnReceivableMemoLines.Remove(receivableMemoLine);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == RMId
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    amount = receivableMemoLinesByCurrentReceivableMemo.Sum(d => d.Amount);
                }

                var updateReceivableMemo = receivableMemo;
                updateReceivableMemo.Amount = amount;
                updateReceivableMemo.UpdatedByUserId = loginUserId;
                updateReceivableMemo.UpdatedDateTime = DateTime.Now;

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
