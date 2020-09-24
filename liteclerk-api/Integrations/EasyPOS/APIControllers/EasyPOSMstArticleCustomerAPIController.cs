using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using liteclerk_api.Integrations.EasyPOS.DTO;

namespace liteclerk_api.Integrations.EasyPOS.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasyPOSMstArticleCustomerAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSMstArticleCustomerAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/locked/byUpdatedDateTime/{updatedDateTime}")]
        public async Task<ActionResult> GetLockedArticleCustomerList(String updatedDateTime)
        {
            try
            {
                IEnumerable<EasyPOSMstArticleCustomerDTO> lockedArticleCustomers = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.MstArticle_ArticleId.UpdatedDateTime == Convert.ToDateTime(updatedDateTime)
                    select new EasyPOSMstArticleCustomerDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new EasyPOSMstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        Customer = d.Customer,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        TermId = d.TermId,
                        Term = new EasyPOSMstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        CreditLimit = d.CreditLimit
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
