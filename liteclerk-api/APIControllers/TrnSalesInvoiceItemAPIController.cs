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
    public class TrnSalesInvoiceItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnSalesInvoiceItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list/{SIId}")]
        public async Task<ActionResult> GetSalesInvoiceItemListBySalesInvoice(Int32 SIId)
        {
            try
            {
                IEnumerable<DTO.TrnSalesInvoiceItemDTO> salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == SIId
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceItemDTO
                    {
                        Id = d.Id,
                        SIId = d.SIId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Item.ManualCode
                            },
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventory.InventoryCode
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_ItemJobType.JobTypeCode,
                            ManualCode = d.MstJobType_ItemJobType.ManualCode,
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            DiscountCode = d.MstDiscount_Discount.DiscountCode,
                            ManualCode = d.MstDiscount_Discount.ManualCode,
                            Discount = d.MstDiscount_Discount.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_VAT.TaxCode,
                            ManualCode = d.MstTax_VAT.ManualCode,
                            TaxDescription = d.MstTax_VAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnit.UnitCode,
                            ManualCode = d.MstUnit_BaseUnit.ManualCode,
                            Unit = d.MstUnit_BaseUnit.Unit
                        },
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoiceItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetSalesInvoiceItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnSalesInvoiceItemDTO salesInvoiceItem = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.Id == id
                    select new DTO.TrnSalesInvoiceItemDTO
                    {
                        Id = d.Id,
                        SIId = d.SIId,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_Item.ManualCode
                            },
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : ""
                        },
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventory.InventoryCode
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_ItemJobType.JobTypeCode,
                            ManualCode = d.MstJobType_ItemJobType.ManualCode,
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            DiscountCode = d.MstDiscount_Discount.DiscountCode,
                            ManualCode = d.MstDiscount_Discount.ManualCode,
                            Discount = d.MstDiscount_Discount.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_VAT.TaxCode,
                            ManualCode = d.MstTax_VAT.ManualCode,
                            TaxDescription = d.MstTax_VAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_BaseUnit.UnitCode,
                            ManualCode = d.MstUnit_BaseUnit.ManualCode,
                            Unit = d.MstUnit_BaseUnit.Unit
                        },
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, salesInvoiceItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddSalesInvoiceItem([FromBody] DTO.TrnSalesInvoiceItemDTO trnSalesInvoiceItemDTO)
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

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnSalesInvoiceItemDTO.SIId
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add sales invoice items if the current sales invoice is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnSalesInvoiceItemDTO.ItemId
                    && d.MstArticle_Article.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (trnSalesInvoiceItemDTO.ItemInventoryId != null)
                {
                    DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                        from d in _dbContext.MstArticleItemInventories
                        where d.Id == trnSalesInvoiceItemDTO.ItemInventoryId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemInventory == null)
                    {
                        return StatusCode(404, "Inventory code not found.");
                    }
                }

                if (trnSalesInvoiceItemDTO.ItemJobTypeId != null)
                {
                    DBSets.MstJobTypeDBSet itemJobType = await (
                        from d in _dbContext.MstJobTypes
                        where d.Id == trnSalesInvoiceItemDTO.ItemJobTypeId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemJobType == null)
                    {
                        return StatusCode(404, "Job type not found.");
                    }
                }

                DBSets.MstDiscountDBSet discount = await (
                    from d in _dbContext.MstDiscounts
                    where d.Id == trnSalesInvoiceItemDTO.DiscountId
                    select d
                ).FirstOrDefaultAsync();

                if (discount == null)
                {
                    return StatusCode(404, "Discount not found.");
                }

                DBSets.MstTaxDBSet VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesInvoiceItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesInvoiceItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnSalesInvoiceItemDTO.ItemId
                    && d.UnitId == trnSalesInvoiceItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnSalesInvoiceItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnSalesInvoiceItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseNetPrice = trnSalesInvoiceItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseNetPrice = trnSalesInvoiceItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnSalesInvoiceItemDBSet newSalesInvoiceItems = new DBSets.TrnSalesInvoiceItemDBSet()
                {
                    SIId = trnSalesInvoiceItemDTO.SIId,
                    ItemId = trnSalesInvoiceItemDTO.ItemId,
                    ItemInventoryId = trnSalesInvoiceItemDTO.ItemInventoryId,
                    ItemJobTypeId = trnSalesInvoiceItemDTO.ItemJobTypeId,
                    Particulars = trnSalesInvoiceItemDTO.Particulars,
                    Quantity = trnSalesInvoiceItemDTO.Quantity,
                    UnitId = trnSalesInvoiceItemDTO.UnitId,
                    Price = trnSalesInvoiceItemDTO.Price,
                    DiscountId = trnSalesInvoiceItemDTO.DiscountId,
                    DiscountRate = trnSalesInvoiceItemDTO.DiscountRate,
                    DiscountAmount = trnSalesInvoiceItemDTO.DiscountAmount,
                    NetPrice = trnSalesInvoiceItemDTO.NetPrice,
                    Amount = trnSalesInvoiceItemDTO.Amount,
                    VATId = trnSalesInvoiceItemDTO.VATId,
                    WTAXId = trnSalesInvoiceItemDTO.WTAXId,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseNetPrice = baseNetPrice,
                    LineTimeStamp = DateTime.Now
                };

                _dbContext.TrnSalesInvoiceItems.Add(newSalesInvoiceItems);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateSalesInvoiceItem(Int32 id, [FromBody] DTO.TrnSalesInvoiceItemDTO trnSalesInvoiceItemDTO)
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

                DBSets.TrnSalesInvoiceItemDBSet salesInvoiceItem = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoiceItem == null)
                {
                    return StatusCode(404, "Sales invoice item not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnSalesInvoiceItemDTO.SIId
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update sales invoice items if the current sales invoice is locked.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnSalesInvoiceItemDTO.ItemId
                    && d.MstArticle_Article.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (trnSalesInvoiceItemDTO.ItemInventoryId != null)
                {
                    DBSets.MstArticleItemInventoryDBSet itemInventory = await (
                        from d in _dbContext.MstArticleItemInventories
                        where d.Id == trnSalesInvoiceItemDTO.ItemInventoryId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemInventory == null)
                    {
                        return StatusCode(404, "Inventory code not found.");
                    }
                }

                if (trnSalesInvoiceItemDTO.ItemJobTypeId != null)
                {
                    DBSets.MstJobTypeDBSet itemJobType = await (
                        from d in _dbContext.MstJobTypes
                        where d.Id == trnSalesInvoiceItemDTO.ItemJobTypeId
                        select d
                    ).FirstOrDefaultAsync();

                    if (itemJobType == null)
                    {
                        return StatusCode(404, "Job type not found.");
                    }
                }

                DBSets.MstDiscountDBSet discount = await (
                    from d in _dbContext.MstDiscounts
                    where d.Id == trnSalesInvoiceItemDTO.DiscountId
                    select d
                ).FirstOrDefaultAsync();

                if (discount == null)
                {
                    return StatusCode(404, "Discount not found.");
                }

                DBSets.MstTaxDBSet VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesInvoiceItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnSalesInvoiceItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnSalesInvoiceItemDTO.ItemId
                    && d.UnitId == trnSalesInvoiceItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnSalesInvoiceItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnSalesInvoiceItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseNetPrice = trnSalesInvoiceItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseNetPrice = trnSalesInvoiceItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnSalesInvoiceItemDBSet updateSalesInvoiceItems = salesInvoiceItem;
                updateSalesInvoiceItems.SIId = trnSalesInvoiceItemDTO.SIId;
                updateSalesInvoiceItems.ItemInventoryId = trnSalesInvoiceItemDTO.ItemInventoryId;
                updateSalesInvoiceItems.ItemJobTypeId = trnSalesInvoiceItemDTO.ItemJobTypeId;
                updateSalesInvoiceItems.Particulars = trnSalesInvoiceItemDTO.Particulars;
                updateSalesInvoiceItems.Quantity = trnSalesInvoiceItemDTO.Quantity;
                updateSalesInvoiceItems.UnitId = trnSalesInvoiceItemDTO.UnitId;
                updateSalesInvoiceItems.Price = trnSalesInvoiceItemDTO.Price;
                updateSalesInvoiceItems.DiscountId = trnSalesInvoiceItemDTO.DiscountId;
                updateSalesInvoiceItems.DiscountRate = trnSalesInvoiceItemDTO.DiscountRate;
                updateSalesInvoiceItems.DiscountAmount = trnSalesInvoiceItemDTO.DiscountAmount;
                updateSalesInvoiceItems.NetPrice = trnSalesInvoiceItemDTO.NetPrice;
                updateSalesInvoiceItems.Amount = trnSalesInvoiceItemDTO.Amount;
                updateSalesInvoiceItems.VATId = trnSalesInvoiceItemDTO.VATId;
                updateSalesInvoiceItems.WTAXId = trnSalesInvoiceItemDTO.WTAXId;
                updateSalesInvoiceItems.BaseQuantity = baseQuantity;
                updateSalesInvoiceItems.BaseUnitId = item.UnitId;
                updateSalesInvoiceItems.BaseNetPrice = baseNetPrice;
                updateSalesInvoiceItems.LineTimeStamp = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> GetSalesInvoiceItem(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                DBSets.TrnSalesInvoiceItemDBSet salesInvoiceItem = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoiceItem == null)
                {
                    return StatusCode(404, "Sales invoice item not found.");
                }

                if (salesInvoiceItem.TrnSalesInvoice_SalesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete sales invoice items if the current sales invoice is locked.");
                }

                _dbContext.TrnSalesInvoiceItems.Remove(salesInvoiceItem);
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
