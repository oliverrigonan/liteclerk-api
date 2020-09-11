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
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_ItemJobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_ItemJobTypeId.ManualCode,
                            JobType = d.MstJobType_ItemJobTypeId.JobType
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
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobTypeCode = d.MstJobType_ItemJobTypeId.JobTypeCode,
                            ManualCode = d.MstJobType_ItemJobTypeId.ManualCode,
                            JobType = d.MstJobType_ItemJobTypeId.JobType
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to add a sales invoice item.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a sales invoice item.");
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
                    && d.MstArticle_ArticleId.IsLocked == true
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
                    VATRate = trnSalesInvoiceItemDTO.VATRate,
                    VATAmount = trnSalesInvoiceItemDTO.VATAmount,
                    WTAXId = trnSalesInvoiceItemDTO.WTAXId,
                    WTAXRate = trnSalesInvoiceItemDTO.WTAXRate,
                    WTAXAmount = trnSalesInvoiceItemDTO.WTAXAmount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseNetPrice = baseNetPrice,
                    LineTimeStamp = DateTime.Now
                };

                _dbContext.TrnSalesInvoiceItems.Add(newSalesInvoiceItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItemsByCurrentSalesInvoice = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == trnSalesInvoiceItemDTO.SIId
                    select d
                ).ToListAsync();

                if (salesInvoiceItemsByCurrentSalesInvoice.Any())
                {
                    amount = salesInvoiceItemsByCurrentSalesInvoice.Sum(d => d.Amount);
                }

                DBSets.TrnSalesInvoiceDBSet updateSalesInvoice = salesInvoice;
                updateSalesInvoice.Amount = amount;
                updateSalesInvoice.UpdatedByUserId = userId;
                updateSalesInvoice.UpdatedDateTime = DateTime.Now;

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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a sales invoice item.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a sales invoice item.");
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
                    && d.MstArticle_ArticleId.IsLocked == true
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
                updateSalesInvoiceItems.VATRate = trnSalesInvoiceItemDTO.VATRate;
                updateSalesInvoiceItems.VATAmount = trnSalesInvoiceItemDTO.VATAmount;
                updateSalesInvoiceItems.WTAXId = trnSalesInvoiceItemDTO.WTAXId;
                updateSalesInvoiceItems.WTAXRate = trnSalesInvoiceItemDTO.WTAXRate;
                updateSalesInvoiceItems.WTAXAmount = trnSalesInvoiceItemDTO.WTAXAmount;
                updateSalesInvoiceItems.BaseQuantity = baseQuantity;
                updateSalesInvoiceItems.BaseUnitId = item.UnitId;
                updateSalesInvoiceItems.BaseNetPrice = baseNetPrice;
                updateSalesInvoiceItems.LineTimeStamp = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItemsByCurrentSalesInvoice = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == trnSalesInvoiceItemDTO.SIId
                    select d
                ).ToListAsync();

                if (salesInvoiceItemsByCurrentSalesInvoice.Any())
                {
                    amount = salesInvoiceItemsByCurrentSalesInvoice.Sum(d => d.Amount);
                }

                DBSets.TrnSalesInvoiceDBSet updateSalesInvoice = salesInvoice;
                updateSalesInvoice.Amount = amount;
                updateSalesInvoice.UpdatedByUserId = userId;
                updateSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteSalesInvoiceItem(int id)
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to delete a sales invoice item.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a sales invoice item.");
                }

                Int32 SIId = 0;

                DBSets.TrnSalesInvoiceItemDBSet salesInvoiceItem = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoiceItem == null)
                {
                    return StatusCode(404, "Sales invoice item not found.");
                }

                SIId = salesInvoiceItem.SIId;

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == SIId
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete sales invoice items if the current sales invoice is locked.");
                }

                _dbContext.TrnSalesInvoiceItems.Remove(salesInvoiceItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItemsByCurrentSalesInvoice = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.SIId == SIId
                    select d
                ).ToListAsync();

                if (salesInvoiceItemsByCurrentSalesInvoice.Any())
                {
                    amount = salesInvoiceItemsByCurrentSalesInvoice.Sum(d => d.Amount);
                }

                DBSets.TrnSalesInvoiceDBSet updateSalesInvoice = salesInvoice;
                updateSalesInvoice.Amount = amount;
                updateSalesInvoice.UpdatedByUserId = userId;
                updateSalesInvoice.UpdatedDateTime = DateTime.Now;

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
