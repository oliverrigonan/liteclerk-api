using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using liteclerk_api.Integrations.EasyPOS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.Integrations.EasyPOS.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasyPOSTrnSalesOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSTrnSalesOrderAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/bySODate/{SODate}/byBranch/{branchManualCode}")]
        public async Task<ActionResult> GetSalesOrderListByINDateByBranch(String SODate, String branchManualCode)
        {
            try
            {
                IEnumerable<EasyPOSTrnSalesOrderDTO> salesOrders = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.SODate == Convert.ToDateTime(SODate)
                    && d.MstCompanyBranch_BranchId.ManualCode == branchManualCode
                    && d.IsLocked == true
                    select new EasyPOSTrnSalesOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new EasyPOSMstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        SONumber = d.SONumber,
                        SODate = d.SODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new EasyPOSMstArticleCustomerDTO
                        {
                            Article = new EasyPOSMstArticleDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.ManualCode
                            },
                            Customer = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                        },
                        CustomerManualCode = d.MstArticle_CustomerId.ManualCode,
                        CustomerName = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                        Remarks = d.Remarks,
                        SalesOrderItems = d.TrnSalesOrderItems_SOId.Any() ? d.TrnSalesOrderItems_SOId.Where(i => i.SOId == d.Id).Select(i => new EasyPOSTrnSalesOrderItemDTO
                        {
                            Id = i.Id,
                            SOId = i.SOId,
                            ItemId = i.ItemId,
                            Item = new EasyPOSMstArticleItemDTO
                            {
                                Article = new EasyPOSMstArticleDTO
                                {
                                    ManualCode = i.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                RRVATId = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().RRVATId : 0,
                                RRVAT = new EasyPOSMstTaxDTO
                                {
                                    ManualCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.ManualCode : "",
                                    TaxDescription = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_RRVATId.TaxDescription : ""
                                },
                                SIVATId = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SIVATId : 0,
                                SIVAT = new EasyPOSMstTaxDTO
                                {
                                    ManualCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.ManualCode : "",
                                    TaxDescription = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().MstTax_SIVATId.TaxDescription : ""
                                }
                            },
                            ItemBarCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                            Particulars = i.Particulars,
                            Quantity = i.Quantity,
                            UnitId = i.UnitId,
                            Unit = new EasyPOSMstUnitDTO
                            {
                                ManualCode = i.MstUnit_UnitId.ManualCode,
                                Unit = i.MstUnit_UnitId.Unit
                            },
                            Price = i.Price,
                            DiscountId = i.DiscountId,
                            Discount = new EasyPOSMstDiscountDTO
                            {
                                ManualCode = i.MstDiscount_DiscountId.ManualCode,
                                Discount = i.MstDiscount_DiscountId.Discount
                            },
                            DiscountRate = i.DiscountRate,
                            DiscountAmount = i.DiscountAmount,
                            NetPrice = i.NetPrice,
                            Amount = i.Amount
                        }).ToList() : new List<EasyPOSTrnSalesOrderItemDTO>()
                    }
                ).ToListAsync();

                return StatusCode(200, salesOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

    }
}
