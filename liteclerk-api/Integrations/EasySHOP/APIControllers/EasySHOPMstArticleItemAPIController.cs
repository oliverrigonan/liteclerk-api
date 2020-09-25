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
using liteclerk_api.Integrations.EasySHOP.DTO;

namespace liteclerk_api.Integrations.EasySHOP.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasySHOPMstArticleItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPMstArticleItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/byCategory/{category}")]
        public async Task<ActionResult> GetArticleItemListByCategory(String category)
        {
            try
            {
                List<EasySHOPMstArticleItemDTO> lockedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.Category == category
                    select new EasySHOPMstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new EasySHOPMstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        Category = d.Category,
                        Price = d.Price,
                        UnitId = d.UnitId,
                        Unit = new EasySHOPMstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
