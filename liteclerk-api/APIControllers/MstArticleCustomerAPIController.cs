using System;
using System.Collections.Generic;
using System.Linq;
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
                            Article = d.MstArticle_ArticleId.Article
                        },
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
                            Article = d.MstArticle_ArticleId.Article
                        },
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
    }
}
