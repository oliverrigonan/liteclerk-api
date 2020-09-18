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
    public class TrnStockInItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnStockInItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{INId}")]
        public async Task<ActionResult> GetStockInItemListByStockIn(Int32 INId)
        {
            try
            {
                IEnumerable<DTO.TrnStockInItemDTO> stockInItems = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == INId
                    orderby d.Id descending
                    select new DTO.TrnStockInItemDTO
                    {
                        Id = d.Id,
                        INId = d.INId,
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
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Cost = d.Cost,
                        Amount = d.Amount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnitId.UnitCode,
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).ToListAsync();

                return StatusCode(200, stockInItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockInItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockInItemDTO stockInItem = await (
                    from d in _dbContext.TrnStockInItems
                    where d.Id == id
                    select new DTO.TrnStockInItemDTO
                    {
                        Id = d.Id,
                        INId = d.INId,
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
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Cost = d.Cost,
                        Amount = d.Amount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnitId.UnitCode,
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, stockInItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockInItem([FromBody] DTO.TrnStockInItemDTO trnStockInItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockInDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock in item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock in item.");
                }

                DBSets.TrnStockInDBSet stockIn = await (
                    from d in _dbContext.TrnStockIns
                    where d.Id == trnStockInItemDTO.INId
                    select d
                ).FirstOrDefaultAsync();

                if (stockIn == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockIn.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add stock in items if the current stock in is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockInItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockInItemDTO.ItemId
                    && d.UnitId == trnStockInItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockInItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockInItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockInItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockInItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockInItemDBSet newStockInItems = new DBSets.TrnStockInItemDBSet()
                {
                    INId = trnStockInItemDTO.INId,
                    ItemId = trnStockInItemDTO.ItemId,
                    Particulars = trnStockInItemDTO.Particulars,
                    Quantity = trnStockInItemDTO.Quantity,
                    UnitId = trnStockInItemDTO.UnitId,
                    Cost = trnStockInItemDTO.Cost,
                    Amount = trnStockInItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnStockInItems.Add(newStockInItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockInItemDBSet> stockInItemsByCurrentStockIn = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == trnStockInItemDTO.INId
                    select d
                ).ToListAsync();

                if (stockInItemsByCurrentStockIn.Any())
                {
                    amount = stockInItemsByCurrentStockIn.Sum(d => d.Amount);
                }

                DBSets.TrnStockInDBSet updateStockIn = stockIn;
                updateStockIn.Amount = amount;
                updateStockIn.UpdatedByUserId = loginUserId;
                updateStockIn.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateStockInItem(Int32 id, [FromBody] DTO.TrnStockInItemDTO trnStockInItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockInDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a stock in item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a stock in item.");
                }

                DBSets.TrnStockInItemDBSet stockInItem = await (
                    from d in _dbContext.TrnStockInItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockInItem == null)
                {
                    return StatusCode(404, "Stock in item not found.");
                }

                DBSets.TrnStockInDBSet stockIn = await (
                    from d in _dbContext.TrnStockIns
                    where d.Id == trnStockInItemDTO.INId
                    select d
                ).FirstOrDefaultAsync();

                if (stockIn == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockIn.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update stock in items if the current stock in is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockInItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockInItemDTO.ItemId
                    && d.UnitId == trnStockInItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockInItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockInItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockInItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockInItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockInItemDBSet updateStockInItems = stockInItem;
                updateStockInItems.INId = trnStockInItemDTO.INId;
                updateStockInItems.Particulars = trnStockInItemDTO.Particulars;
                updateStockInItems.Quantity = trnStockInItemDTO.Quantity;
                updateStockInItems.UnitId = trnStockInItemDTO.UnitId;
                updateStockInItems.Cost = trnStockInItemDTO.Cost;
                updateStockInItems.Amount = trnStockInItemDTO.Amount;
                updateStockInItems.BaseQuantity = baseQuantity;
                updateStockInItems.BaseUnitId = item.UnitId;
                updateStockInItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockInItemDBSet> stockInItemsByCurrentStockIn = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == trnStockInItemDTO.INId
                    select d
                ).ToListAsync();

                if (stockInItemsByCurrentStockIn.Any())
                {
                    amount = stockInItemsByCurrentStockIn.Sum(d => d.Amount);
                }

                DBSets.TrnStockInDBSet updateStockIn = stockIn;
                updateStockIn.Amount = amount;
                updateStockIn.UpdatedByUserId = loginUserId;
                updateStockIn.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockInItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityStockInDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock in item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock in item.");
                }

                Int32 INId = 0;

                DBSets.TrnStockInItemDBSet stockInItem = await (
                    from d in _dbContext.TrnStockInItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockInItem == null)
                {
                    return StatusCode(404, "Stock in item not found.");
                }

                INId = stockInItem.INId;

                DBSets.TrnStockInDBSet stockIn = await (
                    from d in _dbContext.TrnStockIns
                    where d.Id == INId
                    select d
                ).FirstOrDefaultAsync();

                if (stockIn == null)
                {
                    return StatusCode(404, "Stock in not found.");
                }

                if (stockIn.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete stock in items if the current stock in is locked.");
                }

                _dbContext.TrnStockInItems.Remove(stockInItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockInItemDBSet> stockInItemsByCurrentStockIn = await (
                    from d in _dbContext.TrnStockInItems
                    where d.INId == INId
                    select d
                ).ToListAsync();

                if (stockInItemsByCurrentStockIn.Any())
                {
                    amount = stockInItemsByCurrentStockIn.Sum(d => d.Amount);
                }

                DBSets.TrnStockInDBSet updateStockIn = stockIn;
                updateStockIn.Amount = amount;
                updateStockIn.UpdatedByUserId = loginUserId;
                updateStockIn.UpdatedDateTime = DateTime.Now;

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
