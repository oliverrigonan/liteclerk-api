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
    public class EasySHOPMstArticleItemInventoryAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPMstArticleItemInventoryAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("detail/byBarCode/{barCode}/byBranchManualCode/{branchManualCode}")]
        public async Task<ActionResult> GetArticleItemInventoryListByArticleItem(String barCode, String branchManualCode)
        {
            try
            {
                EasySHOPMstArticleItemInventoryDTO articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.MstCompanyBranch_BranchId.ManualCode == branchManualCode
                    && (d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ?
                    d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : d.MstArticle_ArticleId.ManualCode) == barCode
                    select new EasySHOPMstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new EasySHOPMstArticleItemDTO
                        {
                            Article = new EasySHOPMstArticleDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.ManualCode,
                                Article = d.MstArticle_ArticleId.Article,
                                ImageURL = d.MstArticle_ArticleId.ImageURL
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            Category = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category : "",
                            Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price : 0,
                            UnitId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().UnitId : 0,
                            Unit = new EasySHOPMstUnitDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : ""
                            }
                        },
                        BranchId = d.BranchId,
                        Branch = new EasySHOPMstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryCode = d.InventoryCode,
                        Quantity = d.Quantity,
                        Cost = d.Cost,
                        Amount = d.Amount
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, articleItemInventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
