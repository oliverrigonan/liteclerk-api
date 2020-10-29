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
    public class TrnStockCountItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnStockCountItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{SCId}")]
        public async Task<ActionResult> GetStockCountItemListByStockCount(Int32 SCId)
        {
            try
            {
                IEnumerable<DTO.TrnStockCountItemDTO> stockCountItems = await (
                    from d in _dbContext.TrnStockCountItems
                    where d.SCId == SCId
                    orderby d.Id descending
                    select new DTO.TrnStockCountItemDTO
                    {
                        Id = d.Id,
                        SCId = d.SCId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity
                    }
                ).ToListAsync();

                return StatusCode(200, stockCountItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockCountItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockCountItemDTO stockCountItem = await (
                    from d in _dbContext.TrnStockCountItems
                    where d.Id == id
                    select new DTO.TrnStockCountItemDTO
                    {
                        Id = d.Id,
                        SCId = d.SCId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, stockCountItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockCountItem([FromBody] DTO.TrnStockCountItemDTO trnStockCountItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockCountDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock count item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock count item.");
                }

                DBSets.TrnStockCountDBSet stockCount = await (
                    from d in _dbContext.TrnStockCounts
                    where d.Id == trnStockCountItemDTO.SCId
                    select d
                ).FirstOrDefaultAsync();

                if (stockCount == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockCount.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add stock count items if the current stock count is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockCountItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.TrnStockCountItemDBSet newStockCountItems = new DBSets.TrnStockCountItemDBSet()
                {
                    SCId = trnStockCountItemDTO.SCId,
                    ItemId = trnStockCountItemDTO.ItemId,
                    Particulars = trnStockCountItemDTO.Particulars,
                    Quantity = trnStockCountItemDTO.Quantity
                };

                _dbContext.TrnStockCountItems.Add(newStockCountItems);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateStockCountItem(Int32 id, [FromBody] DTO.TrnStockCountItemDTO trnStockCountItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockCountDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a stock count item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a stock count item.");
                }

                DBSets.TrnStockCountItemDBSet stockCountItem = await (
                    from d in _dbContext.TrnStockCountItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockCountItem == null)
                {
                    return StatusCode(404, "Stock in item not found.");
                }

                DBSets.TrnStockCountDBSet stockCount = await (
                    from d in _dbContext.TrnStockCounts
                    where d.Id == trnStockCountItemDTO.SCId
                    select d
                ).FirstOrDefaultAsync();

                if (stockCount == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockCount.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update stock count items if the current stock count is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockCountItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.TrnStockCountItemDBSet updateStockCountItems = stockCountItem;
                updateStockCountItems.SCId = trnStockCountItemDTO.SCId;
                updateStockCountItems.Particulars = trnStockCountItemDTO.Particulars;
                updateStockCountItems.Quantity = trnStockCountItemDTO.Quantity;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockCountItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityStockCountDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock count item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock count item.");
                }

                Int32 SCId = 0;

                DBSets.TrnStockCountItemDBSet stockCountItem = await (
                    from d in _dbContext.TrnStockCountItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockCountItem == null)
                {
                    return StatusCode(404, "Stock in item not found.");
                }

                SCId = stockCountItem.SCId;

                DBSets.TrnStockCountDBSet stockCount = await (
                    from d in _dbContext.TrnStockCounts
                    where d.Id == SCId
                    select d
                ).FirstOrDefaultAsync();

                if (stockCount == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockCount.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete stock count items if the current stock count is locked.");
                }

                _dbContext.TrnStockCountItems.Remove(stockCountItem);
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
