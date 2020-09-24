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
    public class EasyPOSMstArticleSupplierAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSMstArticleSupplierAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/locked/byUpdatedDateTime/{updatedDateTime}")]
        public async Task<ActionResult> GetLockedArticleSupplierList(String updatedDateTime)
        {
            try
            {
                IEnumerable<EasyPOSMstArticleSupplierDTO> lockedArticleSuppliers = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.MstArticle_ArticleId.UpdatedDateTime == Convert.ToDateTime(updatedDateTime)
                    select new EasyPOSMstArticleSupplierDTO
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
                        Supplier = d.Supplier,
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
                        TermId = d.TermId,
                        Term = new EasyPOSMstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleSuppliers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
