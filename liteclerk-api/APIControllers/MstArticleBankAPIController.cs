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
    public class MstArticleBankAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleBankAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetArticleBankList()
        {
            try
            {
                IEnumerable<DTO.MstArticleBankDTO> articleBanks = await (
                    from d in _dbContext.MstArticleBanks
                    select new DTO.MstArticleBankDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        Bank = d.Bank,
                        AccountNumber = d.AccountNumber,
                        TypeOfAccount = d.TypeOfAccount,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        CashInBankAccountId = d.CashInBankAccountId,
                        CashInBankAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CashInBankAccountId.AccountCode,
                            ManualCode = d.MstAccount_CashInBankAccountId.Account,
                            Account = d.MstAccount_CashInBankAccountId.Account
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

                return StatusCode(200, articleBanks);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedArticleBankList()
        {
            try
            {
                IEnumerable<DTO.MstArticleBankDTO> lockedArticleBanks = await (
                    from d in _dbContext.MstArticleBanks
                    where d.MstArticle_ArticleId.IsLocked == true
                    select new DTO.MstArticleBankDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        Bank = d.Bank,
                        AccountNumber = d.AccountNumber,
                        TypeOfAccount = d.TypeOfAccount,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        CashInBankAccountId = d.CashInBankAccountId,
                        CashInBankAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CashInBankAccountId.AccountCode,
                            ManualCode = d.MstAccount_CashInBankAccountId.Account,
                            Account = d.MstAccount_CashInBankAccountId.Account
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

                return StatusCode(200, lockedArticleBanks);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleBankDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleBankDTO producedArticleBank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.Id == id
                    select new DTO.MstArticleBankDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        Bank = d.Bank,
                        AccountNumber = d.AccountNumber,
                        TypeOfAccount = d.TypeOfAccount,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        CashInBankAccountId = d.CashInBankAccountId,
                        CashInBankAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CashInBankAccountId.AccountCode,
                            ManualCode = d.MstAccount_CashInBankAccountId.Account,
                            Account = d.MstAccount_CashInBankAccountId.Account
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

                return StatusCode(200, producedArticleBank);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleBank()
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
                    && d.SysForm_FormId.Form == "SetupBankList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a bank.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a bank.");
                }

                DBSets.MstAccountDBSet cashInBankAccount = await (
                    from d in _dbContext.MstAccounts
                    select d
                ).FirstOrDefaultAsync();

                if (cashInBankAccount == null)
                {
                    return StatusCode(404, "Cash in bank account not found.");
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
                    ArticleTypeId = 1,
                    Article = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticles.Add(newArticle);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleBankDBSet newArticleBank = new DBSets.MstArticleBankDBSet()
                {
                    ArticleId = newArticle.Id,
                    Bank = "",
                    AccountNumber = "",
                    TypeOfAccount = "",
                    Address = "",
                    ContactPerson = "",
                    ContactNumber = "",
                    CashInBankAccountId = cashInBankAccount.Id
                };

                _dbContext.MstArticleBanks.Add(newArticleBank);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newArticleBank.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveArticleBank(Int32 id, [FromBody] DTO.MstArticleBankDTO mstArticleBankDTO)
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
                    && d.SysForm_FormId.Form == "SetupBankDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a bank.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a bank.");
                }

                DBSets.MstArticleBankDBSet articleBank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleBank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                if (articleBank.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a bank that is locked.");
                }

                DBSets.MstAccountDBSet cashInBankAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleBankDTO.CashInBankAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (cashInBankAccount == null)
                {
                    return StatusCode(404, "Cash in bank account not found.");
                }

                DBSets.MstArticleBankDBSet saveArticleBank = articleBank;
                saveArticleBank.Bank = mstArticleBankDTO.Bank;
                saveArticleBank.AccountNumber = mstArticleBankDTO.AccountNumber;
                saveArticleBank.TypeOfAccount = mstArticleBankDTO.TypeOfAccount;
                saveArticleBank.Address = mstArticleBankDTO.Address;
                saveArticleBank.ContactPerson = mstArticleBankDTO.ContactPerson;
                saveArticleBank.ContactNumber = mstArticleBankDTO.ContactNumber;
                saveArticleBank.CashInBankAccountId = mstArticleBankDTO.CashInBankAccountId;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleBank.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet saveArticle = article;
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
        public async Task<ActionResult> LockArticleBank(Int32 id, [FromBody] DTO.MstArticleBankDTO mstArticleBankDTO)
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
                    && d.SysForm_FormId.Form == "SetupBankDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a bank.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a bank.");
                }

                DBSets.MstArticleBankDBSet articleBank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleBank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                if (articleBank.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a bank that is locked.");
                }

                DBSets.MstAccountDBSet cashInBankAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleBankDTO.CashInBankAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (cashInBankAccount == null)
                {
                    return StatusCode(404, "Cash in bank account not found.");
                }

                DBSets.MstArticleBankDBSet lockArticleBank = articleBank;
                lockArticleBank.Bank = mstArticleBankDTO.Bank;
                lockArticleBank.AccountNumber = mstArticleBankDTO.AccountNumber;
                lockArticleBank.TypeOfAccount = mstArticleBankDTO.TypeOfAccount;
                lockArticleBank.Address = mstArticleBankDTO.Address;
                lockArticleBank.ContactPerson = mstArticleBankDTO.ContactPerson;
                lockArticleBank.ContactNumber = mstArticleBankDTO.ContactNumber;
                lockArticleBank.CashInBankAccountId = mstArticleBankDTO.CashInBankAccountId;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleBank.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet lockArticle = article;
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
        public async Task<ActionResult> UnlockArticleBank(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupBankDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a bank.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a bank.");
                }

                DBSets.MstArticleBankDBSet articleBank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleBank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                if (articleBank.MstArticle_ArticleId.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a bank that is unlocked.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleBank.ArticleId
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
        public async Task<ActionResult> DeleteArticleBank(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupBankList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a bank.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a bank.");
                }

                DBSets.MstArticleBankDBSet articleBank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleBank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                if (articleBank.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a bank that is locked.");
                }

                _dbContext.MstArticleBanks.Remove(articleBank);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleBank.ArticleId
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
