using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using liteclerk_api.Integrations.EasyPOS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.Integrations.EasyPOS.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasyPOSMstArticleItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSMstArticleItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/locked/byUpdatedDateTime/{updatedDateTime}")]
        public async Task<ActionResult> GetLockedArticleItemList(String updatedDateTime)
        {
            try
            {
                List<EasyPOSMstArticleItemDTO> lockedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.MstArticle_ArticleId.UpdatedDateTime.Date == Convert.ToDateTime(updatedDateTime)
                    select new EasyPOSMstArticleItemDTO
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
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        Price = d.Price,
                        UnitId = d.UnitId,
                        Unit = new EasyPOSMstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        RRVATId = d.RRVATId,
                        RRVAT = new EasyPOSMstTaxDTO
                        {
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new EasyPOSMstTaxDTO
                        {
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        ArticleItemPrices = d.MstArticle_ArticleId.MstArticleItemPrices_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItemPrices_ArticleId.Where(i => i.ArticleId == d.ArticleId).Select(i => new EasyPOSMstArticleItemPriceDTO
                        {
                            Id = i.Id,
                            ArticleId = i.ArticleId,
                            PriceDescription = i.PriceDescription,
                            Price = i.Price
                        }).ToList() : new List<EasyPOSMstArticleItemPriceDTO>()
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
