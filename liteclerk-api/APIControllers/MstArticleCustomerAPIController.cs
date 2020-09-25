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
    public class MstArticleCustomerAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleCustomerAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetArticleCustomerList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> articleCustomers = await (
                    from d in _dbContext.MstArticleCustomers
                    select new DTO.MstArticleCustomerDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        ReceivableAccountId = d.ReceivableAccountId,
                        ReceivableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ReceivableAccountId.AccountCode,
                            ManualCode = d.MstAccount_ReceivableAccountId.Account,
                            Account = d.MstAccount_ReceivableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        CreditLimit = d.CreditLimit,
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

                return StatusCode(200, articleCustomers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedArticleCustomerList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> lockedArticleCustomers = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select new DTO.MstArticleCustomerDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        ReceivableAccountId = d.ReceivableAccountId,
                        ReceivableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ReceivableAccountId.AccountCode,
                            ManualCode = d.MstAccount_ReceivableAccountId.Account,
                            Account = d.MstAccount_ReceivableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        CreditLimit = d.CreditLimit,
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

                return StatusCode(200, lockedArticleCustomers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleCustomerDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleCustomerDTO producedArticleCustomer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.Id == id
                    select new DTO.MstArticleCustomerDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        ReceivableAccountId = d.ReceivableAccountId,
                        ReceivableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ReceivableAccountId.AccountCode,
                            ManualCode = d.MstAccount_ReceivableAccountId.ManualCode,
                            Account = d.MstAccount_ReceivableAccountId.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_TermId.TermCode,
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        CreditLimit = d.CreditLimit,
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

                return StatusCode(200, producedArticleCustomer);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleCustomer()
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
                    && d.SysForm_FormId.Form == "SetupCustomerList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a customer.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a customer.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Receivable account not found.");
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
                    where d.ArticleTypeId == 2
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
                    ImageURL = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticles.Add(newArticle);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleCustomerDBSet newArticleCustomer = new DBSets.MstArticleCustomerDBSet()
                {
                    ArticleId = newArticle.Id,
                    Customer = "",
                    Address = "",
                    ContactPerson = "",
                    ContactNumber = "",
                    ReceivableAccountId = receivableAccount.Id,
                    TermId = term.Id,
                    CreditLimit = 0
                };

                _dbContext.MstArticleCustomers.Add(newArticleCustomer);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newArticleCustomer.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveArticleCustomer(Int32 id, [FromBody] DTO.MstArticleCustomerDTO mstArticleCustomerDTO)
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
                    && d.SysForm_FormId.Form == "SetupCustomerDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a customer.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a customer.");
                }

                DBSets.MstArticleCustomerDBSet articleCustomer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleCustomer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                if (articleCustomer.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a customer that is locked.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleCustomerDTO.ReceivableAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Receivable account not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == mstArticleCustomerDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstArticleCustomerDBSet saveArticleCustomer = articleCustomer;
                saveArticleCustomer.Customer = mstArticleCustomerDTO.Customer;
                saveArticleCustomer.Address = mstArticleCustomerDTO.Address;
                saveArticleCustomer.ContactPerson = mstArticleCustomerDTO.ContactPerson;
                saveArticleCustomer.ContactNumber = mstArticleCustomerDTO.ContactNumber;
                saveArticleCustomer.ReceivableAccountId = mstArticleCustomerDTO.ReceivableAccountId;
                saveArticleCustomer.TermId = mstArticleCustomerDTO.TermId;
                saveArticleCustomer.CreditLimit = mstArticleCustomerDTO.CreditLimit;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleCustomer.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet saveArticle = article;
                saveArticle.ManualCode = mstArticleCustomerDTO.ArticleManualCode;
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
        public async Task<ActionResult> LockArticleCustomer(Int32 id, [FromBody] DTO.MstArticleCustomerDTO mstArticleCustomerDTO)
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
                    && d.SysForm_FormId.Form == "SetupCustomerDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a customer.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a customer.");
                }

                DBSets.MstArticleCustomerDBSet articleCustomer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleCustomer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                if (articleCustomer.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a customer that is locked.");
                }

                DBSets.MstAccountDBSet receivableAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleCustomerDTO.ReceivableAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (receivableAccount == null)
                {
                    return StatusCode(404, "Receivable account not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == mstArticleCustomerDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstArticleCustomerDBSet lockArticleCustomer = articleCustomer;
                lockArticleCustomer.Customer = mstArticleCustomerDTO.Customer;
                lockArticleCustomer.Address = mstArticleCustomerDTO.Address;
                lockArticleCustomer.ContactPerson = mstArticleCustomerDTO.ContactPerson;
                lockArticleCustomer.ContactNumber = mstArticleCustomerDTO.ContactNumber;
                lockArticleCustomer.ReceivableAccountId = mstArticleCustomerDTO.ReceivableAccountId;
                lockArticleCustomer.TermId = mstArticleCustomerDTO.TermId;
                lockArticleCustomer.CreditLimit = mstArticleCustomerDTO.CreditLimit;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleCustomer.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet lockArticle = article;
                lockArticle.ManualCode = mstArticleCustomerDTO.ArticleManualCode;
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
        public async Task<ActionResult> UnlockArticleCustomer(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupCustomerDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a customer.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a customer.");
                }

                DBSets.MstArticleCustomerDBSet articleCustomer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleCustomer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                if (articleCustomer.MstArticle_ArticleId.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a customer that is unlocked.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleCustomer.ArticleId
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
        public async Task<ActionResult> DeleteArticleCustomer(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupCustomerList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a customer.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a customer.");
                }

                DBSets.MstArticleCustomerDBSet articleCustomer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleCustomer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                if (articleCustomer.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a customer that is locked.");
                }

                _dbContext.MstArticleCustomers.Remove(articleCustomer);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleCustomer.ArticleId
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
