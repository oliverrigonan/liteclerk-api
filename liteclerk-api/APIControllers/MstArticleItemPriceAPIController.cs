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
    public class MstArticleItemPriceAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemPriceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{articleId}")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemPriceDTO>>> GetArticleItemPriceList(Int32 articleId)
        {
            try
            {
                IEnumerable<DTO.MstArticleItemPriceDTO> articleItemPrices = await (
                    from d in _dbContext.MstArticleItemPrices
                    where d.ArticleId == articleId
                    select new DTO.MstArticleItemPriceDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Article.ManualCode
                            },
                            SKUCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        PriceDescription = d.PriceDescription,
                        Price = d.Price
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemPrices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
