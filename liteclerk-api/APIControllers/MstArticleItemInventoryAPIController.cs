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
    public class MstArticleItemInventoryAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemInventoryAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetArticleItemInventoryList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemInventoryDTO> articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    select new DTO.MstArticleItemInventoryDTO
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
                            BarCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().BarCode : "",
                            Description = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().Description : "",
                            UnitId = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().UnitId : 0,
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstUnit_Unit.UnitCode : "",
                                ManualCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstUnit_Unit.ManualCode : "",
                                Unit = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstUnit_Unit.Unit : ""
                            },
                            Price = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().Price : 0,
                            SIVATId = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().SIVATId : 0,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_SIVAT.TaxCode : "",
                                ManualCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_SIVAT.ManualCode : "",
                                TaxDescription = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_SIVAT.TaxDescription : "",
                                TaxRate = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_SIVAT.TaxRate : 0
                            },
                            WTAXId = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().WTAXId : 0,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_WTAX.TaxCode : "",
                                ManualCode = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_WTAX.ManualCode : "",
                                TaxDescription = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_WTAX.TaxDescription : "",
                                TaxRate = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().MstTax_WTAX.TaxRate : 0
                            },
                            Kitting = d.MstArticle_Article.MstArticleItems_Article.Any() ? d.MstArticle_Article.MstArticleItems_Article.FirstOrDefault().Kitting : ""
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        InventoryCode = d.InventoryCode,
                        Quantity = d.Quantity,
                        Cost = d.Cost
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
