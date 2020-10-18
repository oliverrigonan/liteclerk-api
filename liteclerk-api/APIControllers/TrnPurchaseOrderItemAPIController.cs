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
    public class TrnPurchaseOrderItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPurchaseOrderItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{POId}")]
        public async Task<ActionResult> GetPurchaseOrderItemListByPurchaseOrder(Int32 POId)
        {
            try
            {
                IEnumerable<DTO.TrnPurchaseOrderItemDTO> purchaseOrderItems = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.POId == POId
                    orderby d.Id descending
                    select new DTO.TrnPurchaseOrderItemDTO
                    {
                        Id = d.Id,
                        POId = d.POId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            RRVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().RRVATId : 0,
                            RRVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxRate : 0
                            },
                            SIVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId : 0,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate : 0
                            },
                            WTAXId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId : 0,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate : 0
                            }
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

                return StatusCode(200, purchaseOrderItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPurchaseOrderItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnPurchaseOrderItemDTO purchaseOrderItem = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.Id == id
                    select new DTO.TrnPurchaseOrderItemDTO
                    {
                        Id = d.Id,
                        POId = d.POId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                            RRVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().RRVATId : 0,
                            RRVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxRate : 0
                            },
                            SIVATId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId : 0,
                            SIVAT = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxRate : 0
                            },
                            WTAXId = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().WTAXId : 0,
                            WTAX = new DTO.MstTaxDTO
                            {
                                TaxCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxCode : "",
                                ManualCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.ManualCode : "",
                                TaxDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxDescription : "",
                                TaxRate = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_WTAXId.TaxRate : 0
                            }
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

                return StatusCode(200, purchaseOrderItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPurchaseOrderItem([FromBody] DTO.TrnPurchaseOrderItemDTO trnPurchaseOrderItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a purchase order item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a purchase order item.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == trnPurchaseOrderItemDTO.POId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add purchase order items if the current purchase order is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseOrderItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnPurchaseOrderItemDTO.ItemId
                    && d.UnitId == trnPurchaseOrderItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnPurchaseOrderItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnPurchaseOrderItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnPurchaseOrderItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnPurchaseOrderItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnPurchaseOrderItemDBSet newPurchaseOrderItems = new DBSets.TrnPurchaseOrderItemDBSet()
                {
                    POId = trnPurchaseOrderItemDTO.POId,
                    ItemId = trnPurchaseOrderItemDTO.ItemId,
                    Particulars = trnPurchaseOrderItemDTO.Particulars,
                    Quantity = trnPurchaseOrderItemDTO.Quantity,
                    UnitId = trnPurchaseOrderItemDTO.UnitId,
                    Cost = trnPurchaseOrderItemDTO.Cost,
                    Amount = trnPurchaseOrderItemDTO.Amount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnPurchaseOrderItems.Add(newPurchaseOrderItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseOrderItemDBSet> purchaseOrderItemsByCurrentPurchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.POId == trnPurchaseOrderItemDTO.POId
                    select d
                ).ToListAsync();

                if (purchaseOrderItemsByCurrentPurchaseOrder.Any())
                {
                    amount = purchaseOrderItemsByCurrentPurchaseOrder.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseOrderDBSet updatePurchaseOrder = purchaseOrder;
                updatePurchaseOrder.Amount = amount;
                updatePurchaseOrder.UpdatedByUserId = loginUserId;
                updatePurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdatePurchaseOrderItem(Int32 id, [FromBody] DTO.TrnPurchaseOrderItemDTO trnPurchaseOrderItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a purchase order item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a purchase order item.");
                }

                DBSets.TrnPurchaseOrderItemDBSet purchaseOrderItem = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrderItem == null)
                {
                    return StatusCode(404, "Purchase order item not found.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == trnPurchaseOrderItemDTO.POId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update purchase order items if the current purchase order is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseOrderItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnPurchaseOrderItemDTO.ItemId
                    && d.UnitId == trnPurchaseOrderItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnPurchaseOrderItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnPurchaseOrderItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnPurchaseOrderItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnPurchaseOrderItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnPurchaseOrderItemDBSet updatePurchaseOrderItems = purchaseOrderItem;
                updatePurchaseOrderItems.POId = trnPurchaseOrderItemDTO.POId;
                updatePurchaseOrderItems.Particulars = trnPurchaseOrderItemDTO.Particulars;
                updatePurchaseOrderItems.Quantity = trnPurchaseOrderItemDTO.Quantity;
                updatePurchaseOrderItems.UnitId = trnPurchaseOrderItemDTO.UnitId;
                updatePurchaseOrderItems.Cost = trnPurchaseOrderItemDTO.Cost;
                updatePurchaseOrderItems.Amount = trnPurchaseOrderItemDTO.Amount;
                updatePurchaseOrderItems.BaseQuantity = baseQuantity;
                updatePurchaseOrderItems.BaseUnitId = item.UnitId;
                updatePurchaseOrderItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseOrderItemDBSet> purchaseOrderItemsByCurrentPurchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.POId == trnPurchaseOrderItemDTO.POId
                    select d
                ).ToListAsync();

                if (purchaseOrderItemsByCurrentPurchaseOrder.Any())
                {
                    amount = purchaseOrderItemsByCurrentPurchaseOrder.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseOrderDBSet updatePurchaseOrder = purchaseOrder;
                updatePurchaseOrder.Amount = amount;
                updatePurchaseOrder.UpdatedByUserId = loginUserId;
                updatePurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePurchaseOrderItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a purchase order item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a purchase order item.");
                }

                Int32 POId = 0;

                DBSets.TrnPurchaseOrderItemDBSet purchaseOrderItem = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrderItem == null)
                {
                    return StatusCode(404, "Purchase order item not found.");
                }

                POId = purchaseOrderItem.POId;

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == POId
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete purchase order items if the current purchase order is locked.");
                }

                _dbContext.TrnPurchaseOrderItems.Remove(purchaseOrderItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnPurchaseOrderItemDBSet> purchaseOrderItemsByCurrentPurchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrderItems
                    where d.POId == POId
                    select d
                ).ToListAsync();

                if (purchaseOrderItemsByCurrentPurchaseOrder.Any())
                {
                    amount = purchaseOrderItemsByCurrentPurchaseOrder.Sum(d => d.Amount);
                }

                DBSets.TrnPurchaseOrderDBSet updatePurchaseOrder = purchaseOrder;
                updatePurchaseOrder.Amount = amount;
                updatePurchaseOrder.UpdatedByUserId = loginUserId;
                updatePurchaseOrder.UpdatedDateTime = DateTime.Now;

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
