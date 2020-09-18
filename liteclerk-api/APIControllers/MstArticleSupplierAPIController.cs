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
    public class MstArticleSupplierAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleSupplierAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetArticleSupplierList()
        {
            try
            {
                IEnumerable<DTO.MstArticleSupplierDTO> articleSuppliers = await (
                    from d in _dbContext.MstArticleSuppliers
                    select new DTO.MstArticleSupplierDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Supplier = d.Supplier,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        PayableAccountId = d.PayableAccountId,
                        PayableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_PayableAccountId.AccountCode,
                            ManualCode = d.MstAccount_PayableAccountId.Account,
                            Account = d.MstAccount_PayableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, articleSuppliers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedArticleSupplierList()
        {
            try
            {
                IEnumerable<DTO.MstArticleSupplierDTO> lockedArticleSuppliers = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select new DTO.MstArticleSupplierDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Supplier = d.Supplier,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        PayableAccountId = d.PayableAccountId,
                        PayableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_PayableAccountId.AccountCode,
                            ManualCode = d.MstAccount_PayableAccountId.Account,
                            Account = d.MstAccount_PayableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleSuppliers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleSupplierDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleSupplierDTO producedArticleSupplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.Id == id
                    select new DTO.MstArticleSupplierDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Supplier = d.Supplier,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        PayableAccountId = d.PayableAccountId,
                        PayableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_PayableAccountId.AccountCode,
                            ManualCode = d.MstAccount_PayableAccountId.ManualCode,
                            Account = d.MstAccount_PayableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, producedArticleSupplier);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleSupplier()
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
                    && d.SysForm_FormId.Form == "SetupSupplierList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a supplier.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a supplier.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Payable account not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                String articleCode = "0000000001";
                DBSets.MstArticleDBSet lastArticle = await (
                    from d in _dbContext.MstArticles
                    where d.ArticleTypeId == 1
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastArticle != null)
                {
                    Int32 lastArticleCode = Convert.ToInt32(lastArticle.ArticleCode) + 0000000001;
                    articleCode = PadZeroes(lastArticleCode, 10);
                }

                DBSets.MstArticleDBSet newArticle = new DBSets.MstArticleDBSet()
                {
                    ArticleCode = articleCode,
                    ManualCode = articleCode,
                    ArticleTypeId = 2,
                    Article = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticles.Add(newArticle);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleSupplierDBSet newArticleSupplier = new DBSets.MstArticleSupplierDBSet()
                {
                    ArticleId = newArticle.Id,
                    Supplier = "",
                    Address = "",
                    ContactPerson = "",
                    ContactNumber = "",
                    PayableAccountId = receivableAccount.Id,
                    TermId = term.Id
                };

                _dbContext.MstArticleSuppliers.Add(newArticleSupplier);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newArticleSupplier.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveArticleSupplier(Int32 id, [FromBody] DTO.MstArticleSupplierDTO mstArticleSupplierDTO)
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
                    && d.SysForm_FormId.Form == "SetupSupplierDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a supplier.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a supplier.");
                }

                DBSets.MstArticleSupplierDBSet articleSupplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleSupplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                if (articleSupplier.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a supplier that is locked.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleSupplierDTO.PayableAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Payable account not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == mstArticleSupplierDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstArticleSupplierDBSet saveArticleSupplier = articleSupplier;
                saveArticleSupplier.Supplier = mstArticleSupplierDTO.Supplier;
                saveArticleSupplier.Address = mstArticleSupplierDTO.Address;
                saveArticleSupplier.ContactPerson = mstArticleSupplierDTO.ContactPerson;
                saveArticleSupplier.ContactNumber = mstArticleSupplierDTO.ContactNumber;
                saveArticleSupplier.PayableAccountId = mstArticleSupplierDTO.PayableAccountId;
                saveArticleSupplier.TermId = mstArticleSupplierDTO.TermId;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleSupplier.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet saveArticle = article;
                saveArticle.ManualCode = mstArticleSupplierDTO.ArticleManualCode;
                saveArticle.UpdatedByUserId = loginUserId;
                saveArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockArticleSupplier(Int32 id, [FromBody] DTO.MstArticleSupplierDTO mstArticleSupplierDTO)
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
                    && d.SysForm_FormId.Form == "SetupSupplierDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a supplier.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a supplier.");
                }

                DBSets.MstArticleSupplierDBSet articleSupplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleSupplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                if (articleSupplier.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a supplier that is locked.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleSupplierDTO.PayableAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Payable account not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == mstArticleSupplierDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstArticleSupplierDBSet lockArticleSupplier = articleSupplier;
                lockArticleSupplier.Supplier = mstArticleSupplierDTO.Supplier;
                lockArticleSupplier.Address = mstArticleSupplierDTO.Address;
                lockArticleSupplier.ContactPerson = mstArticleSupplierDTO.ContactPerson;
                lockArticleSupplier.ContactNumber = mstArticleSupplierDTO.ContactNumber;
                lockArticleSupplier.PayableAccountId = mstArticleSupplierDTO.PayableAccountId;
                lockArticleSupplier.TermId = mstArticleSupplierDTO.TermId;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleSupplier.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet lockArticle = article;
                lockArticle.ManualCode = mstArticleSupplierDTO.ArticleManualCode;
                lockArticle.IsLocked = true;
                lockArticle.UpdatedByUserId = loginUserId;
                lockArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockArticleSupplier(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupSupplierDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a supplier.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a supplier.");
                }

                DBSets.MstArticleSupplierDBSet articleSupplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleSupplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                if (articleSupplier.MstArticle_ArticleId.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a supplier that is unlocked.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleSupplier.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet unlockArticle = article;
                unlockArticle.IsLocked = false;
                unlockArticle.UpdatedByUserId = loginUserId;
                unlockArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteArticleSupplier(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupSupplierList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a supplier.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a supplier.");
                }

                DBSets.MstArticleSupplierDBSet articleSupplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleSupplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                if (articleSupplier.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a supplier that is locked.");
                }

                _dbContext.MstArticleSuppliers.Remove(articleSupplier);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleSupplier.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                _dbContext.MstArticles.Remove(article);
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
