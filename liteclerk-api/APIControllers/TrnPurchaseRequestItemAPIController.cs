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
                var purchaseRequestItems = await (
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
                        BaseAmount = d.BaseAmount,
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
                var purchaseRequestItem = await (
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
                        BaseAmount = d.BaseAmount,
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

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
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

                var purchaseRequest = await (
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

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var itemUnit = await (
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

                Decimal exchangeRate = purchaseRequest.ExchangeRate;
                Decimal baseAmount = trnPurchaseRequestItemDTO.Amount;

                if (exchangeRate > 0)
                {
                    baseAmount = trnPurchaseRequestItemDTO.Amount * exchangeRate;
                }

                var newPurchaseRequestItems = new DBSets.TrnPurchaseRequestItemDBSet()
                {
                    PRId = trnPurchaseRequestItemDTO.PRId,
                    ItemId = trnPurchaseRequestItemDTO.ItemId,
                    Particulars = trnPurchaseRequestItemDTO.Particulars,
                    Quantity = trnPurchaseRequestItemDTO.Quantity,
                    UnitId = trnPurchaseRequestItemDTO.UnitId,
                    Cost = trnPurchaseRequestItemDTO.Cost,
                    Amount = trnPurchaseRequestItemDTO.Amount,
                    BaseAmount = baseAmount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnPurchaseRequestItems.Add(newPurchaseRequestItems);
                await _dbContext.SaveChangesAsync();

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == trnPurchaseRequestItemDTO.PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    totalAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                    totalBaseAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.BaseAmount);
                }

                var updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = totalAmount;
                updatePurchaseRequest.BaseAmount = totalBaseAmount;
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

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
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

                var purchaseRequestItem = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequestItem == null)
                {
                    return StatusCode(404, "Purchase request item not found.");
                }

                var purchaseRequest = await (
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

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnPurchaseRequestItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var itemUnit = await (
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

                Decimal exchangeRate = purchaseRequest.ExchangeRate;
                Decimal baseAmount = trnPurchaseRequestItemDTO.Amount;

                if (exchangeRate > 0)
                {
                    baseAmount = trnPurchaseRequestItemDTO.Amount * exchangeRate;
                }

                var updatePurchaseRequestItems = purchaseRequestItem;
                updatePurchaseRequestItems.PRId = trnPurchaseRequestItemDTO.PRId;
                updatePurchaseRequestItems.Particulars = trnPurchaseRequestItemDTO.Particulars;
                updatePurchaseRequestItems.Quantity = trnPurchaseRequestItemDTO.Quantity;
                updatePurchaseRequestItems.UnitId = trnPurchaseRequestItemDTO.UnitId;
                updatePurchaseRequestItems.Cost = trnPurchaseRequestItemDTO.Cost;
                updatePurchaseRequestItems.Amount = trnPurchaseRequestItemDTO.Amount;
                updatePurchaseRequestItems.BaseAmount = baseAmount;
                updatePurchaseRequestItems.BaseQuantity = baseQuantity;
                updatePurchaseRequestItems.BaseUnitId = item.UnitId;
                updatePurchaseRequestItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == trnPurchaseRequestItemDTO.PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    totalAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                    totalBaseAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.BaseAmount);
                }

                var updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = totalAmount;
                updatePurchaseRequest.BaseAmount = totalBaseAmount;
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

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
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

                var purchaseRequestItem = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseRequestItem == null)
                {
                    return StatusCode(404, "Purchase request item not found.");
                }

                PRId = purchaseRequestItem.PRId;

                var purchaseRequest = await (
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

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var purchaseRequestItemsByCurrentPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequestItems
                    where d.PRId == PRId
                    select d
                ).ToListAsync();

                if (purchaseRequestItemsByCurrentPurchaseRequest.Any())
                {
                    totalAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.Amount);
                    totalBaseAmount = purchaseRequestItemsByCurrentPurchaseRequest.Sum(d => d.BaseAmount);
                }

                var updatePurchaseRequest = purchaseRequest;
                updatePurchaseRequest.Amount = totalAmount;
                updatePurchaseRequest.BaseAmount = totalBaseAmount;
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
