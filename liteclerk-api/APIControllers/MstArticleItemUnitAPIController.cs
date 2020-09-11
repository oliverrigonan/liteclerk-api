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
                                ManualCode = d.MstArticle_ArticleId.ManualCode
                            },
                            SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
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

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleItemUnitDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleItemUnitDTO articleItemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.Id == id
                    select new DTO.MstArticleItemUnitDTO
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
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Multiplier = d.Multiplier
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, articleItemUnit);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleItemUnit([FromBody] DTO.MstArticleItemUnitDTO mstArticleItemUnitDTO)
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
                    return StatusCode(404, "No rights to add an item unit.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an item unit.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == mstArticleItemUnitDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (article.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add an item unit if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemUnitDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == mstArticleItemUnitDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleItemUnitDBSet newItemUnit = new DBSets.MstArticleItemUnitDBSet()
                {
                    ArticleId = mstArticleItemUnitDTO.ArticleId,
                    UnitId = mstArticleItemUnitDTO.UnitId,
                    Multiplier = mstArticleItemUnitDTO.Multiplier
                };

                _dbContext.MstArticleItemUnits.Add(newItemUnit);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateArticleItemUnit(int id, [FromBody] DTO.MstArticleItemUnitDTO mstArticleItemUnitDTO)
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
                    return StatusCode(404, "No rights to edit or update an item unit.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an item unit.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                if (itemUnit.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update an item unit if the current item is locked.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == mstArticleItemUnitDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == mstArticleItemUnitDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleItemUnitDBSet updateItemUnit = itemUnit;
                updateItemUnit.UnitId = mstArticleItemUnitDTO.UnitId;
                updateItemUnit.Multiplier = mstArticleItemUnitDTO.Multiplier;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteItemUnit(int id)
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
                    return StatusCode(404, "No rights to delete an item unit.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an item unit.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                if (itemUnit.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete an item unit if the current item is locked.");
                }

                _dbContext.MstArticleItemUnits.Remove(itemUnit);
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
