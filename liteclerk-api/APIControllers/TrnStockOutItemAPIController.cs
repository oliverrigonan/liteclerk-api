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
    public class TrnStockOutItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnStockOutItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{OTId}")]
        public async Task<ActionResult> GetStockOutItemListByStockOut(Int32 OTId)
        {
            try
            {
                IEnumerable<DTO.TrnStockOutItemDTO> stockOutItems = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == OTId
                    orderby d.Id descending
                    select new DTO.TrnStockOutItemDTO
                    {
                        Id = d.Id,
                        OTId = d.OTId,
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
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventoryId.InventoryCode
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

                return StatusCode(200, stockOutItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockOutItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockOutItemDTO stockOutItem = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.Id == id
                    select new DTO.TrnStockOutItemDTO
                    {
                        Id = d.Id,
                        OTId = d.OTId,
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
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventoryId.InventoryCode
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

                return StatusCode(200, stockOutItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockOutItem([FromBody] DTO.TrnStockOutItemDTO trnStockOutItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockOutDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock out item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock out item.");
                }

                DBSets.TrnStockOutDBSet stockOut = await (
                    from d in _dbContext.TrnStockOuts
                    where d.Id == trnStockOutItemDTO.OTId
                    select d
                ).FirstOrDefaultAsync();

                if (stockOut == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockOut.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add stock out items if the current stock out is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockOutItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockOutItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockOutItemDTO.ItemId
                    && d.UnitId == trnStockOutItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockOutItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockOutItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockOutItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockOutItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockOutItemDBSet newStockOutItems = new DBSets.TrnStockOutItemDBSet()
                {
                    OTId = trnStockOutItemDTO.OTId,
                    ItemId = trnStockOutItemDTO.ItemId,
                    ItemInventoryId = trnStockOutItemDTO.ItemInventoryId,
                    Particulars = trnStockOutItemDTO.Particulars,
                    Quantity = trnStockOutItemDTO.Quantity,
                    UnitId = trnStockOutItemDTO.UnitId,
                    Cost = trnStockOutItemDTO.Cost,
                    Amount = trnStockOutItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnStockOutItems.Add(newStockOutItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockOutItemDBSet> stockOutItemsByCurrentStockOut = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == trnStockOutItemDTO.OTId
                    select d
                ).ToListAsync();

                if (stockOutItemsByCurrentStockOut.Any())
                {
                    amount = stockOutItemsByCurrentStockOut.Sum(d => d.Amount);
                }

                DBSets.TrnStockOutDBSet updateStockOut = stockOut;
                updateStockOut.Amount = amount;
                updateStockOut.UpdatedByUserId = loginUserId;
                updateStockOut.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateStockOutItem(Int32 id, [FromBody] DTO.TrnStockOutItemDTO trnStockOutItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockOutDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a stock out item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a stock out item.");
                }

                DBSets.TrnStockOutItemDBSet stockOutItem = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockOutItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                DBSets.TrnStockOutDBSet stockOut = await (
                    from d in _dbContext.TrnStockOuts
                    where d.Id == trnStockOutItemDTO.OTId
                    select d
                ).FirstOrDefaultAsync();

                if (stockOut == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockOut.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update stock out items if the current stock out is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockOutItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockOutItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockOutItemDTO.ItemId
                    && d.UnitId == trnStockOutItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockOutItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockOutItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockOutItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockOutItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockOutItemDBSet updateStockOutItems = stockOutItem;
                updateStockOutItems.OTId = trnStockOutItemDTO.OTId;
                updateStockOutItems.ItemInventoryId = trnStockOutItemDTO.ItemInventoryId;
                updateStockOutItems.Particulars = trnStockOutItemDTO.Particulars;
                updateStockOutItems.Quantity = trnStockOutItemDTO.Quantity;
                updateStockOutItems.UnitId = trnStockOutItemDTO.UnitId;
                updateStockOutItems.Cost = trnStockOutItemDTO.Cost;
                updateStockOutItems.Amount = trnStockOutItemDTO.Amount;
                updateStockOutItems.BaseQuantity = baseQuantity;
                updateStockOutItems.BaseUnitId = item.UnitId;
                updateStockOutItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockOutItemDBSet> stockOutItemsByCurrentStockOut = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == trnStockOutItemDTO.OTId
                    select d
                ).ToListAsync();

                if (stockOutItemsByCurrentStockOut.Any())
                {
                    amount = stockOutItemsByCurrentStockOut.Sum(d => d.Amount);
                }

                DBSets.TrnStockOutDBSet updateStockOut = stockOut;
                updateStockOut.Amount = amount;
                updateStockOut.UpdatedByUserId = loginUserId;
                updateStockOut.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockOutItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityStockOutDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock out item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock out item.");
                }

                Int32 OTId = 0;

                DBSets.TrnStockOutItemDBSet stockOutItem = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockOutItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                OTId = stockOutItem.OTId;

                DBSets.TrnStockOutDBSet stockOut = await (
                    from d in _dbContext.TrnStockOuts
                    where d.Id == OTId
                    select d
                ).FirstOrDefaultAsync();

                if (stockOut == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockOut.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete stock out items if the current stock out is locked.");
                }

                _dbContext.TrnStockOutItems.Remove(stockOutItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockOutItemDBSet> stockOutItemsByCurrentStockOut = await (
                    from d in _dbContext.TrnStockOutItems
                    where d.OTId == OTId
                    select d
                ).ToListAsync();

                if (stockOutItemsByCurrentStockOut.Any())
                {
                    amount = stockOutItemsByCurrentStockOut.Sum(d => d.Amount);
                }

                DBSets.TrnStockOutDBSet updateStockOut = stockOut;
                updateStockOut.Amount = amount;
                updateStockOut.UpdatedByUserId = loginUserId;
                updateStockOut.UpdatedDateTime = DateTime.Now;

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
