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
    public class MstArticleItemUnitAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemUnitAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{articleId}")]
        public async Task<ActionResult> GetArticleItemUnitList(Int32 articleId)
        {
            try
            {
                IEnumerable<DTO.MstArticleItemUnitDTO> articleItemUnits = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == articleId
                    select new DTO.MstArticleItemUnitDTO
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
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Multiplier = d.Multiplier
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemUnits);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
