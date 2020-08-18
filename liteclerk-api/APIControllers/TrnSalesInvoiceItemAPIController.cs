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
        private DBContext.LiteclerkDBContext _dbContext;

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
        public async Task<ActionResult<IEnumerable<DTO.TrnSalesInvoiceItemDTO>>> GetSalesInvoiceItemList(Int32 SIId)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnSalesInvoiceItemDTO> salesInvoices = await (
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
                            SKUCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_Item.MstArticleItems_Article.Any() ? d.MstArticle_Item.MstArticleItems_Article.FirstOrDefault().Description : "",
                        },
                        ItemInventoryId = d.ItemInventoryId,
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = d.MstArticleItemInventory_ItemInventory.InventoryCode
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobType.JobType
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            Unit = d.MstUnit_Unit.Unit
                        },
                        Price = d.Price,
                        DiscountId = d.DiscountId,
                        Discount = new DTO.MstDiscountDTO
                        {
                            Discount = d.MstDiscount_Discount.Discount
                        },
                        DiscountRate = d.DiscountRate,
                        DiscountAmount = d.DiscountAmount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        VATId = d.VATId,
                        VAT = new DTO.MstTaxDTO
                        {
                            TaxDescription = d.MstTax_VAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            Unit = d.MstUnit_Unit.Unit
                        },
                        BaseQuantity = d.BaseQuantity,
                        BaseNetPrice = d.BaseNetPrice,
                        LineTimeStamp = d.LineTimeStamp.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
