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
    public class TrnPurchaseRequestItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPurchaseRequestItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{PRId}")]
        public async Task<ActionResult> GetPurchaseRequestItemListByPurchaseRequest(Int32 PRId)
        {
            try
            {
                IEnumerable<DTO.TrnPurchaseRequestItemDTO> purchaseRequestItems = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == PRId
                    orderby d.Id descending
                    select new DTO.TrnPurchaseRequestItemDTO
                    {
                        Id = d.Id,
                        PRId = d.PRId,
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
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Cost = d.Cost,
                        Amount = d.Amount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).ToListAsync();

                return StatusCode(200, purchaseRequestItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPurchaseRequestItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnPurchaseRequestItemDTO purchaseRequestItem = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.Id == id
                    select new DTO.TrnPurchaseRequestItemDTO
                    {
                        Id = d.Id,
                        PRId = d.PRId,
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
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Cost = d.Cost,
                        Amount = d.Amount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, purchaseRequestItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPurchaseRequestItem([FromBody] DTO.TrnPurchaseRequestItemDTO trnPurchaseRequestItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a purchase request item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a purchase request item.");
                }

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == trnPurchaseRequestItemDTO.PRId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add purchase request items if the current purchase request is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.UnitId == trnPurchaseRequestItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnPurchaseRequestItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnPurchaseRequestItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnPurchaseRequestItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnPurchaseRequestItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnPurchaseRequestItemDBSet newPurchaseRequestItems = new DBSets.TrnPurchaseRequestItemDBSet()
                {
                    PRId = trnPurchaseRequestItemDTO.PRId,
                    ItemId = trnPurchaseRequestItemDTO.ItemId,
                    Particulars = trnPurchaseRequestItemDTO.Particulars,
                    Quantity = trnPurchaseRequestItemDTO.Quantity,
                    UnitId = trnPurchaseRequestItemDTO.UnitId,
                    Cost = trnPurchaseRequestItemDTO.Cost,
                    Amount = trnPurchaseRequestItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnPurchaseRequestItems.Add(newPurchaseRequestItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseRequestItemDBSet> purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == trnPurchaseRequestItemDTO.PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    amount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseRequestDBSet updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = amount;
                updatePurchaseRequest.UpdatedByUserId = loginUserId;
                updatePurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdatePurchaseRequestItem(Int32 id, [FromBody] DTO.TrnPurchaseRequestItemDTO trnPurchaseRequestItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a purchase request item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a purchase request item.");
                }

                DBSets.TrnPurchaseRequestItemDBSet purchaseRequestItem = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequestItem == null)
                {
                    return StatusCode(404, "Purchase request item not found.");
                }

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == trnPurchaseRequestItemDTO.PRId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update purchase request items if the current purchase request is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.UnitId == trnPurchaseRequestItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnPurchaseRequestItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnPurchaseRequestItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnPurchaseRequestItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnPurchaseRequestItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnPurchaseRequestItemDBSet updatePurchaseRequestItems = purchaseRequestItem;
                updatePurchaseRequestItems.PRId = trnPurchaseRequestItemDTO.PRId;
                updatePurchaseRequestItems.Particulars = trnPurchaseRequestItemDTO.Particulars;
                updatePurchaseRequestItems.Quantity = trnPurchaseRequestItemDTO.Quantity;
                updatePurchaseRequestItems.UnitId = trnPurchaseRequestItemDTO.UnitId;
                updatePurchaseRequestItems.Cost = trnPurchaseRequestItemDTO.Cost;
                updatePurchaseRequestItems.Amount = trnPurchaseRequestItemDTO.Amount;
                updatePurchaseRequestItems.BaseQuantity = baseQuantity;
                updatePurchaseRequestItems.BaseUnitId = item.UnitId;
                updatePurchaseRequestItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseRequestItemDBSet> purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == trnPurchaseRequestItemDTO.PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    amount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseRequestDBSet updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = amount;
                updatePurchaseRequest.UpdatedByUserId = loginUserId;
                updatePurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePurchaseRequestItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a purchase request item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a purchase request item.");
                }

                Int32 PRId = 0;

                DBSets.TrnPurchaseRequestItemDBSet purchaseRequestItem = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequestItem == null)
                {
                    return StatusCode(404, "Purchase request item not found.");
                }

                PRId = purchaseRequestItem.PRId;

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == PRId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete purchase request items if the current purchase request is locked.");
                }

                _dbContext.TrnPurchaseRequestItems.Remove(purchaseRequestItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseRequestItemDBSet> purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    amount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseRequestDBSet updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = amount;
                updatePurchaseRequest.UpdatedByUserId = loginUserId;
                updatePurchaseRequest.UpdatedDateTime = DateTime.Now;

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
