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
    public class TrnReceivingReceiptItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnReceivingReceiptItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{RRId}")]
        public async Task<ActionResult> GetReceivingReceiptItemListByReceivingReceipt(Int32 RRId)
        {
            try
            {
                IEnumerable<DTO.TrnReceivingReceiptItemDTO> receivingReceiptItems = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == RRId
                    orderby d.Id descending
                    select new DTO.TrnReceivingReceiptItemDTO
                    {
                        Id = d.Id,
                        RRId = d.RRId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        POId = d.POId,
                        PurchaseOrder = new DTO.TrnPurchaseOrderDTO
                        {
                            PONumber = d.TrnPurchaseOrder_POId.PONumber,
                            ManualNumber = d.TrnPurchaseOrder_POId.ManualNumber
                        },
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
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).ToListAsync();

                return StatusCode(200, receivingReceiptItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetReceivingReceiptItemDetail(Int32 id)
        {
            try
            {
                DTO.TrnReceivingReceiptItemDTO receivingReceiptItem = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.Id == id
                    select new DTO.TrnReceivingReceiptItemDTO
                    {
                        Id = d.Id,
                        RRId = d.RRId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        POId = d.POId,
                        PurchaseOrder = new DTO.TrnPurchaseOrderDTO
                        {
                            PONumber = d.TrnPurchaseOrder_POId.PONumber,
                            ManualNumber = d.TrnPurchaseOrder_POId.ManualNumber
                        },
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
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        BaseCost = d.BaseCost
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, receivingReceiptItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddReceivingReceiptItem([FromBody] DTO.TrnReceivingReceiptItemDTO trnReceivingReceiptItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a receiving receipt item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a receiving receipt item.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == trnReceivingReceiptItemDTO.RRId
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add receiving receipt items if the current receiving receipt is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnReceivingReceiptItemDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == trnReceivingReceiptItemDTO.POId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnReceivingReceiptItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstTaxDBSet VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnReceivingReceiptItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnReceivingReceiptItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnReceivingReceiptItemDTO.ItemId
                    && d.UnitId == trnReceivingReceiptItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnReceivingReceiptItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnReceivingReceiptItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnReceivingReceiptItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnReceivingReceiptItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnReceivingReceiptItemDBSet newReceivingReceiptItems = new DBSets.TrnReceivingReceiptItemDBSet()
                {
                    RRId = trnReceivingReceiptItemDTO.RRId,
                    BranchId = trnReceivingReceiptItemDTO.BranchId,
                    POId = trnReceivingReceiptItemDTO.POId,
                    ItemId = trnReceivingReceiptItemDTO.ItemId,
                    Particulars = trnReceivingReceiptItemDTO.Particulars,
                    Quantity = trnReceivingReceiptItemDTO.Quantity,
                    UnitId = trnReceivingReceiptItemDTO.UnitId,
                    Cost = trnReceivingReceiptItemDTO.Cost,
                    Amount = trnReceivingReceiptItemDTO.Amount,
                    VATId = trnReceivingReceiptItemDTO.VATId,
                    VATRate = trnReceivingReceiptItemDTO.VATRate,
                    VATAmount = trnReceivingReceiptItemDTO.VATAmount,
                    WTAXId = trnReceivingReceiptItemDTO.WTAXId,
                    WTAXRate = trnReceivingReceiptItemDTO.WTAXRate,
                    WTAXAmount = trnReceivingReceiptItemDTO.WTAXAmount,
                    BaseQuantity = baseQuantity,
                    BaseUnitId = item.UnitId,
                    BaseCost = baseCost,
                };

                _dbContext.TrnReceivingReceiptItems.Add(newReceivingReceiptItems);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnReceivingReceiptItemDBSet> receivingReceiptItemsByCurrentReceivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == trnReceivingReceiptItemDTO.RRId
                    select d
                ).ToListAsync();

                if (receivingReceiptItemsByCurrentReceivingReceipt.Any())
                {
                    amount = receivingReceiptItemsByCurrentReceivingReceipt.Sum(d => d.Amount) - receivingReceiptItemsByCurrentReceivingReceipt.Sum(d => d.WTAXAmount);
                }

                DBSets.TrnReceivingReceiptDBSet updateReceivingReceipt = receivingReceipt;
                updateReceivingReceipt.Amount = amount;
                updateReceivingReceipt.UpdatedByUserId = loginUserId;
                updateReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateReceivingReceiptItem(Int32 id, [FromBody] DTO.TrnReceivingReceiptItemDTO trnReceivingReceiptItemDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a receiving receipt item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a receiving receipt item.");
                }

                DBSets.TrnReceivingReceiptItemDBSet receivingReceiptItem = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceiptItem == null)
                {
                    return StatusCode(404, "Receiving receipt item not found.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == trnReceivingReceiptItemDTO.RRId
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update receiving receipt items if the current receiving receipt is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnReceivingReceiptItemDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == trnReceivingReceiptItemDTO.POId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                DBSets.MstArticleItemDBSet item = await (
                    from d in _dbContext.MstArticleItems
                    where d.ArticleId == trnReceivingReceiptItemDTO.ItemId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                DBSets.MstTaxDBSet VAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnReceivingReceiptItemDTO.VATId
                    select d
                ).FirstOrDefaultAsync();

                if (VAT == null)
                {
                    return StatusCode(404, "VAT not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnReceivingReceiptItemDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                    from d in _dbContext.MstArticleItemUnits
                    where d.ArticleId == trnReceivingReceiptItemDTO.ItemId
                    && d.UnitId == trnReceivingReceiptItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (itemUnit == null)
                {
                    return StatusCode(404, "Item unit not found.");
                }

                Decimal baseQuantity = trnReceivingReceiptItemDTO.Quantity;
                if (itemUnit.Multiplier > 0)
                {
                    baseQuantity = trnReceivingReceiptItemDTO.Quantity * (1 / itemUnit.Multiplier);
                }

                Decimal baseCost = trnReceivingReceiptItemDTO.Amount;
                if (baseQuantity > 0)
                {
                    baseCost = trnReceivingReceiptItemDTO.Amount / baseQuantity;
                }

                DBSets.TrnReceivingReceiptItemDBSet updateReceivingReceiptItems = receivingReceiptItem;
                updateReceivingReceiptItems.RRId = trnReceivingReceiptItemDTO.RRId;
                updateReceivingReceiptItems.BranchId = trnReceivingReceiptItemDTO.BranchId;
                updateReceivingReceiptItems.POId = trnReceivingReceiptItemDTO.POId;
                updateReceivingReceiptItems.Particulars = trnReceivingReceiptItemDTO.Particulars;
                updateReceivingReceiptItems.Quantity = trnReceivingReceiptItemDTO.Quantity;
                updateReceivingReceiptItems.UnitId = trnReceivingReceiptItemDTO.UnitId;
                updateReceivingReceiptItems.Cost = trnReceivingReceiptItemDTO.Cost;
                updateReceivingReceiptItems.Amount = trnReceivingReceiptItemDTO.Amount;
                updateReceivingReceiptItems.VATId = trnReceivingReceiptItemDTO.VATId;
                updateReceivingReceiptItems.VATRate = trnReceivingReceiptItemDTO.VATRate;
                updateReceivingReceiptItems.VATAmount = trnReceivingReceiptItemDTO.VATAmount;
                updateReceivingReceiptItems.WTAXId = trnReceivingReceiptItemDTO.WTAXId;
                updateReceivingReceiptItems.WTAXRate = trnReceivingReceiptItemDTO.WTAXRate;
                updateReceivingReceiptItems.WTAXAmount = trnReceivingReceiptItemDTO.WTAXAmount;
                updateReceivingReceiptItems.BaseQuantity = baseQuantity;
                updateReceivingReceiptItems.BaseUnitId = item.UnitId;
                updateReceivingReceiptItems.BaseCost = baseCost;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnReceivingReceiptItemDBSet> receivingReceiptItemsByCurrentReceivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == trnReceivingReceiptItemDTO.RRId
                    select d
                ).ToListAsync();

                if (receivingReceiptItemsByCurrentReceivingReceipt.Any())
                {
                    amount = receivingReceiptItemsByCurrentReceivingReceipt.Sum(d => d.Amount) - receivingReceiptItemsByCurrentReceivingReceipt.Sum(d => d.WTAXAmount);
                }

                DBSets.TrnReceivingReceiptDBSet updateReceivingReceipt = receivingReceipt;
                updateReceivingReceipt.Amount = amount;
                updateReceivingReceipt.UpdatedByUserId = loginUserId;
                updateReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReceivingReceiptItem(int id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a receiving receipt item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a receiving receipt item.");
                }

                Int32 RRId = 0;

                DBSets.TrnReceivingReceiptItemDBSet receivingReceiptItem = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceiptItem == null)
                {
                    return StatusCode(404, "Receiving receipt item not found.");
                }

                RRId = receivingReceiptItem.RRId;

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == RRId
                    select d
                ).FirstOrDefaultAsync();

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete receiving receipt items if the current receiving receipt is locked.");
                }

                _dbContext.TrnReceivingReceiptItems.Remove(receivingReceiptItem);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnReceivingReceiptItemDBSet> receivingReceiptItemsByCurrentReceivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.RRId == RRId
                    select d
                ).ToListAsync();

                if (receivingReceiptItemsByCurrentReceivingReceipt.Any())
                {
                    amount = receivingReceiptItemsByCurrentReceivingReceipt.Sum(d => d.Amount);
                }

                DBSets.TrnReceivingReceiptDBSet updateReceivingReceipt = receivingReceipt;
                updateReceivingReceipt.Amount = amount;
                updateReceivingReceipt.UpdatedByUserId = loginUserId;
                updateReceivingReceipt.UpdatedDateTime = DateTime.Now;

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
