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
    public class TrnStockTransferItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnStockTransferItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{STId}")]
        public async Task<ActionResult> GetStockTransferItemListByStockTransfer(Int32 STId)
        {
            try
            {
                IEnumerable<DTO.TrnStockTransferItemDTO> stockTransferItems = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == STId
                    orderby d.Id descending
                    select new DTO.TrnStockTransferItemDTO
                    {
                        Id = d.Id,
                        STId = d.STId,
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

                return StatusCode(200, stockTransferItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockTransferItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockTransferItemDTO stockTransferItem = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.Id == id
                    select new DTO.TrnStockTransferItemDTO
                    {
                        Id = d.Id,
                        STId = d.STId,
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

                return StatusCode(200, stockTransferItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockTransferItem([FromBody] DTO.TrnStockTransferItemDTO trnStockTransferItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockTransferDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock transfer item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock transfer item.");
                }

                DBSets.TrnStockTransferDBSet stockTransfer = await (
                    from d in _dbContext.TrnStockTransfers
                    where d.Id == trnStockTransferItemDTO.STId
                    select d
                ).FirstOrDefaultAsync();

                if (stockTransfer == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockTransfer.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add stock transfer items if the current stock transfer is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockTransferItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockTransferItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockTransferItemDTO.ItemId
                    && d.UnitId == trnStockTransferItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockTransferItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockTransferItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockTransferItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockTransferItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockTransferItemDBSet newStockTransferItems = new DBSets.TrnStockTransferItemDBSet()
                {
                    STId = trnStockTransferItemDTO.STId,
                    ItemId = trnStockTransferItemDTO.ItemId,
                    ItemInventoryId = trnStockTransferItemDTO.ItemInventoryId,
                    Particulars = trnStockTransferItemDTO.Particulars,
                    Quantity = trnStockTransferItemDTO.Quantity,
                    UnitId = trnStockTransferItemDTO.UnitId,
                    Cost = trnStockTransferItemDTO.Cost,
                    Amount = trnStockTransferItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnStockTransferItems.Add(newStockTransferItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockTransferItemDBSet> stockTransferItemsByCurrentStockTransfer = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == trnStockTransferItemDTO.STId
                    select d
                ).ToListAsync();

                if (stockTransferItemsByCurrentStockTransfer.Any())
                {
                    amount = stockTransferItemsByCurrentStockTransfer.Sum(d => d.Amount);
                }

                DBSets.TrnStockTransferDBSet updateStockTransfer = stockTransfer;
                updateStockTransfer.Amount = amount;
                updateStockTransfer.UpdatedByUserId = loginUserId;
                updateStockTransfer.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateStockTransferItem(Int32 id, [FromBody] DTO.TrnStockTransferItemDTO trnStockTransferItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockTransferDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a stock transfer item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a stock transfer item.");
                }

                DBSets.TrnStockTransferItemDBSet stockTransferItem = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockTransferItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                DBSets.TrnStockTransferDBSet stockTransfer = await (
                    from d in _dbContext.TrnStockTransfers
                    where d.Id == trnStockTransferItemDTO.STId
                    select d
                ).FirstOrDefaultAsync();

                if (stockTransfer == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockTransfer.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update stock transfer items if the current stock transfer is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockTransferItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockTransferItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockTransferItemDTO.ItemId
                    && d.UnitId == trnStockTransferItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockTransferItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockTransferItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockTransferItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockTransferItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockTransferItemDBSet updateStockTransferItems = stockTransferItem;
                updateStockTransferItems.STId = trnStockTransferItemDTO.STId;
                updateStockTransferItems.ItemInventoryId = trnStockTransferItemDTO.ItemInventoryId;
                updateStockTransferItems.Particulars = trnStockTransferItemDTO.Particulars;
                updateStockTransferItems.Quantity = trnStockTransferItemDTO.Quantity;
                updateStockTransferItems.UnitId = trnStockTransferItemDTO.UnitId;
                updateStockTransferItems.Cost = trnStockTransferItemDTO.Cost;
                updateStockTransferItems.Amount = trnStockTransferItemDTO.Amount;
                updateStockTransferItems.BaseQuantity = baseQuantity;
                updateStockTransferItems.BaseUnitId = item.UnitId;
                updateStockTransferItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockTransferItemDBSet> stockTransferItemsByCurrentStockTransfer = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == trnStockTransferItemDTO.STId
                    select d
                ).ToListAsync();

                if (stockTransferItemsByCurrentStockTransfer.Any())
                {
                    amount = stockTransferItemsByCurrentStockTransfer.Sum(d => d.Amount);
                }

                DBSets.TrnStockTransferDBSet updateStockTransfer = stockTransfer;
                updateStockTransfer.Amount = amount;
                updateStockTransfer.UpdatedByUserId = loginUserId;
                updateStockTransfer.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockTransferItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityStockTransferDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock transfer item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock transfer item.");
                }

                Int32 STId = 0;

                DBSets.TrnStockTransferItemDBSet stockTransferItem = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockTransferItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                STId = stockTransferItem.STId;

                DBSets.TrnStockTransferDBSet stockTransfer = await (
                    from d in _dbContext.TrnStockTransfers
                    where d.Id == STId
                    select d
                ).FirstOrDefaultAsync();

                if (stockTransfer == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockTransfer.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete stock transfer items if the current stock transfer is locked.");
                }

                _dbContext.TrnStockTransferItems.Remove(stockTransferItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockTransferItemDBSet> stockTransferItemsByCurrentStockTransfer = await (
                    from d in _dbContext.TrnStockTransferItems
                    where d.STId == STId
                    select d
                ).ToListAsync();

                if (stockTransferItemsByCurrentStockTransfer.Any())
                {
                    amount = stockTransferItemsByCurrentStockTransfer.Sum(d => d.Amount);
                }

                DBSets.TrnStockTransferDBSet updateStockTransfer = stockTransfer;
                updateStockTransfer.Amount = amount;
                updateStockTransfer.UpdatedByUserId = loginUserId;
                updateStockTransfer.UpdatedDateTime = DateTime.Now;

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
