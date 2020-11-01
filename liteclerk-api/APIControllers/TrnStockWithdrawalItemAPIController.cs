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
    public class TrnStockWithdrawalItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnStockWithdrawalItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{SWId}")]
        public async Task<ActionResult> GetStockWithdrawalItemListByStockWithdrawal(Int32 SWId)
        {
            try
            {
                IEnumerable<DTO.TrnStockWithdrawalItemDTO> stockWithdrawalItems = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == SWId
                    orderby d.Id descending
                    select new DTO.TrnStockWithdrawalItemDTO
                    {
                        Id = d.Id,
                        SWId = d.SWId,
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

                return StatusCode(200, stockWithdrawalItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockWithdrawalItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockWithdrawalItemDTO stockWithdrawalItem = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.Id == id
                    select new DTO.TrnStockWithdrawalItemDTO
                    {
                        Id = d.Id,
                        SWId = d.SWId,
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

                return StatusCode(200, stockWithdrawalItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockWithdrawalItem([FromBody] DTO.TrnStockWithdrawalItemDTO trnStockWithdrawalItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock withdrawal item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock withdrawal item.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.Id == trnStockWithdrawalItemDTO.SWId
                    select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add stock withdrawal items if the current stock withdrawal is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockWithdrawalItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockWithdrawalItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockWithdrawalItemDTO.ItemId
                    && d.UnitId == trnStockWithdrawalItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockWithdrawalItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockWithdrawalItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockWithdrawalItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockWithdrawalItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockWithdrawalItemDBSet newStockWithdrawalItems = new DBSets.TrnStockWithdrawalItemDBSet()
                {
                    SWId = trnStockWithdrawalItemDTO.SWId,
                    ItemId = trnStockWithdrawalItemDTO.ItemId,
                    ItemInventoryId = trnStockWithdrawalItemDTO.ItemInventoryId,
                    Particulars = trnStockWithdrawalItemDTO.Particulars,
                    Quantity = trnStockWithdrawalItemDTO.Quantity,
                    UnitId = trnStockWithdrawalItemDTO.UnitId,
                    Cost = trnStockWithdrawalItemDTO.Cost,
                    Amount = trnStockWithdrawalItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnStockWithdrawalItems.Add(newStockWithdrawalItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockWithdrawalItemDBSet> stockWithdrawalItemsByCurrentStockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == trnStockWithdrawalItemDTO.SWId
                    select d
                ).ToListAsync();

                if (stockWithdrawalItemsByCurrentStockWithdrawal.Any())
                {
                    amount = stockWithdrawalItemsByCurrentStockWithdrawal.Sum(d => d.Amount);
                }

                DBSets.TrnStockWithdrawalDBSet updateStockWithdrawal = stockWithdrawal;
                updateStockWithdrawal.Amount = amount;
                updateStockWithdrawal.UpdatedByUserId = loginUserId;
                updateStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateStockWithdrawalItem(Int32 id, [FromBody] DTO.TrnStockWithdrawalItemDTO trnStockWithdrawalItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a stock withdrawal item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a stock withdrawal item.");
                }

                DBSets.TrnStockWithdrawalItemDBSet stockWithdrawalItem = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawalItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.Id == trnStockWithdrawalItemDTO.SWId
                    select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update stock withdrawal items if the current stock withdrawal is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnStockWithdrawalItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                    from d in _dbContext.MstArticleItemInventories
                    where d.Id == trnStockWithdrawalItemDTO.ItemInventoryId
                    select d
                ).FirstOrDefaultAsync();

                if (itemInventory == null)
                {
                    return StatusCode(404, "Inventory code not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnStockWithdrawalItemDTO.ItemId
                    && d.UnitId == trnStockWithdrawalItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnStockWithdrawalItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnStockWithdrawalItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnStockWithdrawalItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnStockWithdrawalItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnStockWithdrawalItemDBSet updateStockWithdrawalItems = stockWithdrawalItem;
                updateStockWithdrawalItems.SWId = trnStockWithdrawalItemDTO.SWId;
                updateStockWithdrawalItems.ItemInventoryId = trnStockWithdrawalItemDTO.ItemInventoryId;
                updateStockWithdrawalItems.Particulars = trnStockWithdrawalItemDTO.Particulars;
                updateStockWithdrawalItems.Quantity = trnStockWithdrawalItemDTO.Quantity;
                updateStockWithdrawalItems.UnitId = trnStockWithdrawalItemDTO.UnitId;
                updateStockWithdrawalItems.Cost = trnStockWithdrawalItemDTO.Cost;
                updateStockWithdrawalItems.Amount = trnStockWithdrawalItemDTO.Amount;
                updateStockWithdrawalItems.BaseQuantity = baseQuantity;
                updateStockWithdrawalItems.BaseUnitId = item.UnitId;
                updateStockWithdrawalItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockWithdrawalItemDBSet> stockWithdrawalItemsByCurrentStockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == trnStockWithdrawalItemDTO.SWId
                    select d
                ).ToListAsync();

                if (stockWithdrawalItemsByCurrentStockWithdrawal.Any())
                {
                    amount = stockWithdrawalItemsByCurrentStockWithdrawal.Sum(d => d.Amount);
                }

                DBSets.TrnStockWithdrawalDBSet updateStockWithdrawal = stockWithdrawal;
                updateStockWithdrawal.Amount = amount;
                updateStockWithdrawal.UpdatedByUserId = loginUserId;
                updateStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockWithdrawalItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock withdrawal item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock withdrawal item.");
                }

                Int32 SWId = 0;

                DBSets.TrnStockWithdrawalItemDBSet stockWithdrawalItem = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawalItem == null)
                {
                    return StatusCode(404, "Stock out item not found.");
                }

                SWId = stockWithdrawalItem.SWId;

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.Id == SWId
                    select d
                ).FirstOrDefaultAsync();

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete stock withdrawal items if the current stock withdrawal is locked.");
                }

                _dbContext.TrnStockWithdrawalItems.Remove(stockWithdrawalItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnStockWithdrawalItemDBSet> stockWithdrawalItemsByCurrentStockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawalItems
                    where d.SWId == SWId
                    select d
                ).ToListAsync();

                if (stockWithdrawalItemsByCurrentStockWithdrawal.Any())
                {
                    amount = stockWithdrawalItemsByCurrentStockWithdrawal.Sum(d => d.Amount);
                }

                DBSets.TrnStockWithdrawalDBSet updateStockWithdrawal = stockWithdrawal;
                updateStockWithdrawal.Amount = amount;
                updateStockWithdrawal.UpdatedByUserId = loginUserId;
                updateStockWithdrawal.UpdatedDateTime = DateTime.Now;

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
