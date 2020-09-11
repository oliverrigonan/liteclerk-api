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
    public class MstArticleItemPriceAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemPriceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{articleId}")]
        public async Task<ActionResult> GetArticleItemPriceList(Int32 articleId)
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
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        PriceDescription = d.PriceDescription,
                        Price = d.Price
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemPrices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleItemPriceDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleItemPriceDTO articleItemPrice = await (
                    from d in _dbContext.MstArticleItemPrices
                    where d.Id == id
                    select new DTO.MstArticleItemPriceDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        PriceDescription = d.PriceDescription,
                        Price = d.Price
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, articleItemPrice);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleItemPrice([FromBody] DTO.MstArticleItemPriceDTO mstArticleItemPriceDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to add an item price.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an item price.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == mstArticleItemPriceDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add an item price if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemPriceDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemPriceDBSet newItemPrice = new DBSets.MstArticleItemPriceDBSet()
                {
                    ArticleId = mstArticleItemPriceDTO.ArticleId,
                    PriceDescription = mstArticleItemPriceDTO.PriceDescription,
                    Price = mstArticleItemPriceDTO.Price
                };

                _dbContext.MstArticleItemPrices.Add(newItemPrice);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateArticleItemPrice(int id, [FromBody] DTO.MstArticleItemPriceDTO mstArticleItemPriceDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to edit or update an item price.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an item price.");
                }

                DBSets.MstArticleItemPriceDBSet itemPrice = await (
                    from d in _dbContext.MstArticleItemPrices
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (itemPrice == null)
                {
                    return StatusCode(404, "Item price not found.");
                }

                if (itemPrice.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update an item price if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemPriceDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemPriceDBSet updateItemPrice = itemPrice;
                updateItemPrice.PriceDescription = mstArticleItemPriceDTO.PriceDescription;
                updateItemPrice.Price = mstArticleItemPriceDTO.Price;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteItemPrice(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to delete an item price.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an item price.");
                }

                DBSets.MstArticleItemPriceDBSet itemPrice = await (
                    from d in _dbContext.MstArticleItemPrices
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (itemPrice == null)
                {
                    return StatusCode(404, "Item price not found.");
                }

                if (itemPrice.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete an item price if the current item is locked.");
                }

                _dbContext.MstArticleItemPrices.Remove(itemPrice);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
