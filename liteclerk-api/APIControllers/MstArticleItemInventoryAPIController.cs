using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.BranchId == loginUser.BranchId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.MstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            Category = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category,
                            UnitId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().UnitId,
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode,
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit
                            },
                            Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price,
                            SIVATId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate
                            },
                            WTAXId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate
                            },
                            Kitting = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Kitting,
                            IsInventory = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
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

        [HttpGet("list/paginated/{column}/{skip}/{take}")]
        public async Task<ActionResult> GetPaginatedArticleItemInventoryList(String column, Int32 skip, Int32 take, String keywords)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.BranchId == loginUser.BranchId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    && (
                        keywords == "" || String.IsNullOrEmpty(keywords) ? true :
                        column == "All" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode.Contains(keywords) ||
                                          d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description.Contains(keywords) ||
                                          d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category.Contains(keywords) ||
                                          d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit.Contains(keywords) ||
                                          d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Kitting.Contains(keywords) :
                        column == "Barcode" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode.Contains(keywords) :
                        column == "Description" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description.Contains(keywords) :
                        column == "Category" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category.Contains(keywords) :
                        column == "Unit" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit.Contains(keywords) :
                        column == "Kitting" ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Kitting.Contains(keywords) : true
                    )
                    select new DTO.MstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            Category = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Category,
                            UnitId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().UnitId,
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode,
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit
                            },
                            Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price,
                            SIVATId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate
                            },
                            WTAXId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate
                            },
                            Kitting = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Kitting,
                            IsInventory = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryCode = d.InventoryCode,
                        Quantity = d.Quantity,
                        Cost = d.Cost
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemInventories.OrderByDescending(d => d.Id).Skip(skip).Take(take));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byArticleItem/{articleId}")]
        public async Task<ActionResult> GetArticleItemInventoryListByArticleItem(Int32 articleId)
        {
            try
            {
                var articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.ArticleId == articleId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.MstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            UnitId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().UnitId,
                            Unit = new DTO.MstUnitDTO
                            {
                                UnitCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.UnitCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode,
                                Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit
                            },
                            Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price,
                            SIVATId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate
                            },
                            WTAXId = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode,
                                ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode,
                                TaxDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription,
                                TaxRate = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate
                            },
                            Kitting = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Kitting
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        InventoryCode = d.InventoryCode,
                        Quantity = d.Quantity,
                        Cost = d.Cost,
                        Amount = d.Amount
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
