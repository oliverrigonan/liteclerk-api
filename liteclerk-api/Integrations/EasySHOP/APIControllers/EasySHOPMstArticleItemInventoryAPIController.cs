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

namespace liteclerk_api.Integrations.EasySHOP.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasySHOPMstArticleItemInventoryAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPMstArticleItemInventoryAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/byArticleItem/{articleId}")]
        public async Task<ActionResult> GetArticleItemInventoryListByArticleItem(Int32 articleId)
        {
            try
            {
                List<DTO.EasySHOPMstArticleItemInventoryDTO> articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.ArticleId == articleId
                    select new DTO.EasySHOPMstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        BranchCode = d.MstCompanyBranch_BranchId.ManualCode,
                        Branch = d.MstCompanyBranch_BranchId.Branch,
                        ManualItemCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                        ItemDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                        Category = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category : "",
                        Particulars = "",
                        Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : "",
                        Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price : 0,
                        Quantity = d.Quantity
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemInventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
