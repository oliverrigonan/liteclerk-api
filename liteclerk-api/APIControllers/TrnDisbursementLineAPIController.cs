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
    public class TrnDisbursementLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnDisbursementLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{CVId}")]
        public async Task<ActionResult> GetDisbursementLineListByDisbursement(Int32 CVId)
        {
            try
            {
                IEnumerable<DTO.TrnDisbursementLineDTO> disbursementLines = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == CVId
                    orderby d.Id descending
                    select new DTO.TrnDisbursementLineDTO
                    {
                        Id = d.Id,
                        CVId = d.CVId,
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
                        Particulars = d.Particulars,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount
                    }
                ).ToListAsync();

                return StatusCode(200, disbursementLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetDisbursementLineDetail(Int32 id)
        {
            try
            {
                DTO.TrnDisbursementLineDTO disbursementLine = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.Id == id
                    select new DTO.TrnDisbursementLineDTO
                    {
                        Id = d.Id,
                        CVId = d.CVId,
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
                        Particulars = d.Particulars,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, disbursementLine);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddDisbursementLine([FromBody] DTO.TrnDisbursementLineDTO trnDisbursementLineDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a disbursement line.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a disbursement line.");
                }

                DBSets.TrnDisbursementDBSet disbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == trnDisbursementLineDTO.CVId
                    select d
                ).FirstOrDefaultAsync();

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add disbursement line if the current disbursement is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnDisbursementLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnDisbursementLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnDisbursementLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (trnDisbursementLineDTO.RRId != null)
                {
                    DBSets.TrnReceivingReceiptDBSet salesInvoice = await (
                        from d in _dbContext.TrnReceivingReceipts
                        where d.Id == trnDisbursementLineDTO.RRId
                        && d.IsLocked == true
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                    {
                        return StatusCode(404, "Sales invoice not found.");
                    }
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnDisbursementLineDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.TrnDisbursementLineDBSet newDisbursementLines = new DBSets.TrnDisbursementLineDBSet()
                {
                    CVId = trnDisbursementLineDTO.CVId,
                    BranchId = trnDisbursementLineDTO.BranchId,
                    AccountId = trnDisbursementLineDTO.AccountId,
                    ArticleId = trnDisbursementLineDTO.ArticleId,
                    RRId = trnDisbursementLineDTO.RRId,
                    Amount = trnDisbursementLineDTO.Amount,
                    Particulars = trnDisbursementLineDTO.Particulars,
                    WTAXId = trnDisbursementLineDTO.WTAXId,
                    WTAXRate = trnDisbursementLineDTO.WTAXRate,
                    WTAXAmount = trnDisbursementLineDTO.WTAXAmount
                };

                _dbContext.TrnDisbursementLines.Add(newDisbursementLines);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == trnDisbursementLineDTO.CVId
                    select d
                ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    amount = disbursementLinesByCurrentDisbursement.Sum(d => d.Amount);
                }

                DBSets.TrnDisbursementDBSet updateDisbursement = disbursement;
                updateDisbursement.Amount = amount;
                updateDisbursement.UpdatedByUserId = loginUserId;
                updateDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateDisbursementLine(Int32 id, [FromBody] DTO.TrnDisbursementLineDTO trnDisbursementLineDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a disbursement line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a disbursement line.");
                }

                DBSets.TrnDisbursementLineDBSet disbursementLine = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (disbursementLine == null)
                {
                    return StatusCode(404, "Disbursement line not found.");
                }

                DBSets.TrnDisbursementDBSet disbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == trnDisbursementLineDTO.CVId
                    select d
                ).FirstOrDefaultAsync();

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update disbursement line if the current disbursement is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnDisbursementLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnDisbursementLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnDisbursementLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (trnDisbursementLineDTO.RRId != null)
                {
                    DBSets.TrnReceivingReceiptDBSet salesInvoice = await (
                        from d in _dbContext.TrnReceivingReceipts
                        where d.Id == trnDisbursementLineDTO.RRId
                        && d.IsLocked == true
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                    {
                        return StatusCode(404, "Sales invoice not found.");
                    }
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnDisbursementLineDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.TrnDisbursementLineDBSet updateDisbursementLines = disbursementLine;
                updateDisbursementLines.CVId = trnDisbursementLineDTO.CVId;
                updateDisbursementLines.BranchId = trnDisbursementLineDTO.BranchId;
                updateDisbursementLines.AccountId = trnDisbursementLineDTO.AccountId;
                updateDisbursementLines.ArticleId = trnDisbursementLineDTO.ArticleId;
                updateDisbursementLines.RRId = trnDisbursementLineDTO.RRId;
                updateDisbursementLines.Amount = trnDisbursementLineDTO.Amount;
                updateDisbursementLines.Particulars = trnDisbursementLineDTO.Particulars;
                updateDisbursementLines.WTAXId = trnDisbursementLineDTO.WTAXId;
                updateDisbursementLines.WTAXRate = trnDisbursementLineDTO.WTAXRate;
                updateDisbursementLines.WTAXAmount = trnDisbursementLineDTO.WTAXAmount;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == trnDisbursementLineDTO.CVId
                    select d
                ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    amount = disbursementLinesByCurrentDisbursement.Sum(d => d.Amount);
                }

                DBSets.TrnDisbursementDBSet updateDisbursement = disbursement;
                updateDisbursement.Amount = amount;
                updateDisbursement.UpdatedByUserId = loginUserId;
                updateDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteDisbursementLine(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a disbursement line.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a disbursement line.");
                }

                Int32 CVId = 0;

                DBSets.TrnDisbursementLineDBSet disbursementLine = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (disbursementLine == null)
                {
                    return StatusCode(404, "Disbursement line not found.");
                }

                CVId = disbursementLine.CVId;

                DBSets.TrnDisbursementDBSet disbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == CVId
                    select d
                ).FirstOrDefaultAsync();

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete disbursement line if the current disbursement is locked.");
                }

                _dbContext.TrnDisbursementLines.Remove(disbursementLine);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == CVId
                    select d
                ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    amount = disbursementLinesByCurrentDisbursement.Sum(d => d.Amount);
                }

                DBSets.TrnDisbursementDBSet updateDisbursement = disbursement;
                updateDisbursement.Amount = amount;
                updateDisbursement.UpdatedByUserId = loginUserId;
                updateDisbursement.UpdatedDateTime = DateTime.Now;

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
