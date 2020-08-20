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
        private DBContext.LiteclerkDBContext _dbContext;

        public MstArticleCustomerAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleCustomerDTO>>> GetArticleCustomerList()
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
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        ReceivableAccountId = d.ReceivableAccountId,
                        ReceivableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ReceivableAccount.AccountCode,
                            ManualCode = d.MstAccount_ReceivableAccount.Account,
                            Account = d.MstAccount_ReceivableAccount.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_Term.TermCode,
                            ManualCode = d.MstTerm_Term.ManualCode,
                            Term = d.MstTerm_Term.Term
                        },
                        CreditLimit = d.CreditLimit,
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, articleCustomers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("locked/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleCustomerDTO>>> GetLockedArticleCustomerList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> lockedArticleCustomers = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_Article.IsLocked == true
                    select new DTO.MstArticleCustomerDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        ReceivableAccountId = d.ReceivableAccountId,
                        ReceivableAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ReceivableAccount.AccountCode,
                            ManualCode = d.MstAccount_ReceivableAccount.Account,
                            Account = d.MstAccount_ReceivableAccount.Account
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_Term.TermCode,
                            ManualCode = d.MstTerm_Term.ManualCode,
                            Term = d.MstTerm_Term.Term
                        },
                        CreditLimit = d.CreditLimit,
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleCustomers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
