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
    public class MstArticleItemComponentAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemComponentAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{articleId}")]
        public async Task<ActionResult> GetArticleItemComponentList(Int32 articleId)
        {
            try
            {
                IEnumerable<DTO.MstArticleItemComponentDTO> articleItemComponents = await (
                    from d in _dbContext.MstArticleItemComponents
                    where d.ArticleId == articleId
                    select new DTO.MstArticleItemComponentDTO
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
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        ComponentArticleId = d.ComponentArticleId,
                        ComponentArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ComponentArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                            Description = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        Quantity = d.Quantity
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemComponents);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleItemComponentDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleItemComponentDTO articleItemComponent = await (
                    from d in _dbContext.MstArticleItemComponents
                    where d.Id == id
                    select new DTO.MstArticleItemComponentDTO
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
                        ComponentArticleId = d.ComponentArticleId,
                        ComponentArticleItem = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ComponentArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ComponentArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        Quantity = d.Quantity
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, articleItemComponent);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleItemComponent([FromBody] DTO.MstArticleItemComponentDTO mstArticleItemComponentDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add an item component.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an item component.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == mstArticleItemComponentDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add an item component if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemComponentDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemDBSet componentArticleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemComponentDTO.ComponentArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (componentArticleItem == null)
                {
                    return StatusCode(404, "Component item not found.");
                }

                DBSets.MstArticleItemComponentDBSet newItemComponent = new DBSets.MstArticleItemComponentDBSet()
                {
                    ArticleId = mstArticleItemComponentDTO.ArticleId,
                    ComponentArticleId = mstArticleItemComponentDTO.ComponentArticleId,
                    Quantity = mstArticleItemComponentDTO.Quantity
                };

                _dbContext.MstArticleItemComponents.Add(newItemComponent);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateArticleItemComponent(int id, [FromBody] DTO.MstArticleItemComponentDTO mstArticleItemComponentDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update an item component.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an item component.");
                }

                DBSets.MstArticleItemComponentDBSet articleItemComponent = await (
                    from d in _dbContext.MstArticleItemComponents
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (articleItemComponent == null)
                {
                    return StatusCode(404, "Item component not found.");
                }

                if (articleItemComponent.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update an item component if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemComponentDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemDBSet componentArticleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemComponentDTO.ComponentArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (componentArticleItem == null)
                {
                    return StatusCode(404, "Component item not found.");
                }

                DBSets.MstArticleItemComponentDBSet updateItemComponent = articleItemComponent;
                updateItemComponent.ArticleId = mstArticleItemComponentDTO.ArticleId;
                updateItemComponent.ComponentArticleId = mstArticleItemComponentDTO.ComponentArticleId;
                updateItemComponent.Quantity = mstArticleItemComponentDTO.Quantity;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteItemComponent(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete an item component.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an item component.");
                }

                DBSets.MstArticleItemComponentDBSet itemComponent = await (
                    from d in _dbContext.MstArticleItemComponents
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (itemComponent == null)
                {
                    return StatusCode(404, "Item component not found.");
                }

                if (itemComponent.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete an item component if the current item is locked.");
                }

                _dbContext.MstArticleItemComponents.Remove(itemComponent);
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
