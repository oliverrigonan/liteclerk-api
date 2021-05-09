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
    public class TrnSalesOrderItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnSalesOrderItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{SOId}")]
        public async Task<ActionResult> GetSalesOrderItemListBySalesOrder(Int32 SOId)
        {
            try
            {
                var salesOrderItems = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.SOId == SOId
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    orderby d.Id descending
                    select new DTO.TrnSalesOrderItemDTO
                    {
                        Id = d.Id,
                        SOId = d.SOId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory
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
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            DiscountCode = d.MstDiscount_DiscountId.DiscountCode,
                            ManualCode = d.MstDiscount_DiscountId.ManualCode,
                            Discount = d.MstDiscount_DiscountId.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_VATId.TaxCode,
                            ManualCode = d.MstTax_VATId.ManualCode,
                            TaxDescription = d.MstTax_VATId.TaxDescription
                        },
                        VATRate = d.VATRate,
                        VATAmount = d.VATAmount,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnitId.UnitCode,
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, salesOrderItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetSalesOrderItemDetail(Int32 id)
        {
            try
            {
                var salesOrderItem = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.Id == id
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select new DTO.TrnSalesOrderItemDTO
                    {
                        Id = d.Id,
                        SOId = d.SOId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory
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
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            DiscountCode = d.MstDiscount_DiscountId.DiscountCode,
                            ManualCode = d.MstDiscount_DiscountId.ManualCode,
                            Discount = d.MstDiscount_DiscountId.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_VATId.TaxCode,
                            ManualCode = d.MstTax_VATId.ManualCode,
                            TaxDescription = d.MstTax_VATId.TaxDescription
                        },
                        VATRate = d.VATRate,
                        VATAmount = d.VATAmount,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnitId.UnitCode,
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, salesOrderItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddSalesOrderItem([FromBody] DTO.TrnSalesOrderItemDTO trnSalesOrderItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a sales order item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a sales order item.");
                }

                var salesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.Id == trnSalesOrderItemDTO.SOId
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add sales order items if the current sales order is locked.");
                }

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnSalesOrderItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (trnSalesOrderItemDTO.ItemInventoryId != null)
                {
                    var itemInventory = await (
                        from d in _dbContext.MstArticleItemInventories
                        where d.Id == trnSalesOrderItemDTO.ItemInventoryId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemInventory == null)
                    {
                        return StatusCode(404, "Inventory code not found.");
                    }
                }

                var discount = await (
                    from d in _dbContext.MstDiscounts
                    where d.Id == trnSalesOrderItemDTO.DiscountId
                    select d
                ).FirstOrDefaultAsync();

                if (discount == null)
                {
                    return StatusCode(404, "Discount not found.");
                }

                var VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesOrderItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                var WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesOrderItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                var itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnSalesOrderItemDTO.ItemId
                    && d.UnitId == trnSalesOrderItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnSalesOrderItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnSalesOrderItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseNetPrice = trnSalesOrderItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseNetPrice = trnSalesOrderItemDTO.Amount / baseQuantity;
                }

                Decimal exchangeRate = salesOrder.ExchangeRate;
                Decimal baseAmount = trnSalesOrderItemDTO.Amount;

                if (exchangeRate > 0)
                {
                    baseAmount = trnSalesOrderItemDTO.Amount * exchangeRate;
                }

                var newSalesOrderItems = new DBSets.TrnSalesOrderItemDBSet()
                {
                    SOId = trnSalesOrderItemDTO.SOId,
                    ItemId = trnSalesOrderItemDTO.ItemId,
                    ItemInventoryId = trnSalesOrderItemDTO.ItemInventoryId,
                    Particulars = trnSalesOrderItemDTO.Particulars,
                    Quantity = trnSalesOrderItemDTO.Quantity,
                    UnitId = trnSalesOrderItemDTO.UnitId,
                    Price = trnSalesOrderItemDTO.Price,
                    DiscountId = trnSalesOrderItemDTO.DiscountId,
                    DiscountRate = trnSalesOrderItemDTO.DiscountRate,
                    DiscountAmount = trnSalesOrderItemDTO.DiscountAmount,
                    NetPrice = trnSalesOrderItemDTO.NetPrice,
                    Amount = trnSalesOrderItemDTO.Amount,
                    BaseAmount = baseAmount,
                    VATId = trnSalesOrderItemDTO.VATId,
                    VATRate = trnSalesOrderItemDTO.VATRate,
                    VATAmount = trnSalesOrderItemDTO.VATAmount,
                    WTAXId = trnSalesOrderItemDTO.WTAXId,
                    WTAXRate = trnSalesOrderItemDTO.WTAXRate,
                    WTAXAmount = trnSalesOrderItemDTO.WTAXAmount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseNetPrice = baseNetPrice,
                    LineTimeStamp = DateTime.Now
                };

                _dbContext.TrnSalesOrderItems.Add(newSalesOrderItems);
                await _dbContext.SaveChangesAsync();

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var salesOrderItemsByCurrentSalesOrder = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.SOId == trnSalesOrderItemDTO.SOId
                    select d
                ).ToListAsync();

                if (salesOrderItemsByCurrentSalesOrder.Any())
                {
                    totalAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.Amount);
                    totalBaseAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.BaseAmount);
                }

                var updateSalesOrder = salesOrder;
                updateSalesOrder.Amount = totalAmount;
                updateSalesOrder.BaseAmount = totalBaseAmount;
                updateSalesOrder.UpdatedByUserId = loginUserId;
                updateSalesOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateSalesOrderItem(Int32 id, [FromBody] DTO.TrnSalesOrderItemDTO trnSalesOrderItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a sales order item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a sales order item.");
                }

                var salesOrderItem = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrderItem == null)
                {
                    return StatusCode(404, "Sales order item not found.");
                }

                var salesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.Id == trnSalesOrderItemDTO.SOId
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update sales order items if the current sales order is locked.");
                }

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnSalesOrderItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (trnSalesOrderItemDTO.ItemInventoryId != null)
                {
                    var itemInventory = await (
                        from d in _dbContext.MstArticleItemInventories
                        where d.Id == trnSalesOrderItemDTO.ItemInventoryId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemInventory == null)
                    {
                        return StatusCode(404, "Inventory code not found.");
                    }
                }

                var discount = await (
                    from d in _dbContext.MstDiscounts
                    where d.Id == trnSalesOrderItemDTO.DiscountId
                    select d
                ).FirstOrDefaultAsync();

                if (discount == null)
                {
                    return StatusCode(404, "Discount not found.");
                }

                var VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesOrderItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                var WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesOrderItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                var itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnSalesOrderItemDTO.ItemId
                    && d.UnitId == trnSalesOrderItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnSalesOrderItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnSalesOrderItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseNetPrice = trnSalesOrderItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseNetPrice = trnSalesOrderItemDTO.Amount / baseQuantity;
                }

                Decimal exchangeRate = salesOrder.ExchangeRate;
                Decimal baseAmount = trnSalesOrderItemDTO.Amount;

                if (exchangeRate > 0)
                {
                    baseAmount = trnSalesOrderItemDTO.Amount * exchangeRate;
                }

                var updateSalesOrderItems = salesOrderItem;
                updateSalesOrderItems.SOId = trnSalesOrderItemDTO.SOId;
                updateSalesOrderItems.ItemInventoryId = trnSalesOrderItemDTO.ItemInventoryId;
                updateSalesOrderItems.Particulars = trnSalesOrderItemDTO.Particulars;
                updateSalesOrderItems.Quantity = trnSalesOrderItemDTO.Quantity;
                updateSalesOrderItems.UnitId = trnSalesOrderItemDTO.UnitId;
                updateSalesOrderItems.Price = trnSalesOrderItemDTO.Price;
                updateSalesOrderItems.DiscountId = trnSalesOrderItemDTO.DiscountId;
                updateSalesOrderItems.DiscountRate = trnSalesOrderItemDTO.DiscountRate;
                updateSalesOrderItems.DiscountAmount = trnSalesOrderItemDTO.DiscountAmount;
                updateSalesOrderItems.NetPrice = trnSalesOrderItemDTO.NetPrice;
                updateSalesOrderItems.Amount = trnSalesOrderItemDTO.Amount;
                updateSalesOrderItems.BaseAmount = baseAmount;
                updateSalesOrderItems.VATId = trnSalesOrderItemDTO.VATId;
                updateSalesOrderItems.VATRate = trnSalesOrderItemDTO.VATRate;
                updateSalesOrderItems.VATAmount = trnSalesOrderItemDTO.VATAmount;
                updateSalesOrderItems.WTAXId = trnSalesOrderItemDTO.WTAXId;
                updateSalesOrderItems.WTAXRate = trnSalesOrderItemDTO.WTAXRate;
                updateSalesOrderItems.WTAXAmount = trnSalesOrderItemDTO.WTAXAmount;
                updateSalesOrderItems.BaseQuantity = baseQuantity;
                updateSalesOrderItems.BaseUnitId = item.UnitId;
                updateSalesOrderItems.BaseNetPrice = baseNetPrice;
                updateSalesOrderItems.LineTimeStamp = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var salesOrderItemsByCurrentSalesOrder = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.SOId == trnSalesOrderItemDTO.SOId
                    select d
                ).ToListAsync();

                if (salesOrderItemsByCurrentSalesOrder.Any())
                {
                    totalAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.Amount);
                    totalBaseAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.BaseAmount);
                }

                var updateSalesOrder = salesOrder;
                updateSalesOrder.Amount = totalAmount;
                updateSalesOrder.BaseAmount = totalBaseAmount;
                updateSalesOrder.UpdatedByUserId = loginUserId;
                updateSalesOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteSalesOrderItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a sales order item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a sales order item.");
                }

                Int32 SOId = 0;

                var salesOrderItem = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrderItem == null)
                {
                    return StatusCode(404, "Sales order item not found.");
                }

                SOId = salesOrderItem.SOId;

                var salesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.Id == SOId
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete sales order items if the current sales order is locked.");
                }

                _dbContext.TrnSalesOrderItems.Remove(salesOrderItem);
                await _dbContext.SaveChangesAsync();

                Decimal totalAmount = 0;
                Decimal totalBaseAmount = 0;

                var salesOrderItemsByCurrentSalesOrder = await (
                    from d in _dbContext.TrnSalesOrderItems
                    where d.SOId == SOId
                    select d
                ).ToListAsync();

                if (salesOrderItemsByCurrentSalesOrder.Any())
                {
                    totalAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.Amount);
                    totalBaseAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.BaseAmount);
                }

                var updateSalesOrder = salesOrder;
                updateSalesOrder.Amount = totalAmount;
                updateSalesOrder.BaseAmount = totalBaseAmount;
                updateSalesOrder.UpdatedByUserId = loginUserId;
                updateSalesOrder.UpdatedDateTime = DateTime.Now;

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
