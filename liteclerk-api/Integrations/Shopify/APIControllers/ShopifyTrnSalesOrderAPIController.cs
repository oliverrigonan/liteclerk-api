using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using liteclerk_api.Integrations.Shopify.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.Integrations.Shopify.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyTrnSalesOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public ShopifyTrnSalesOrderAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
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

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<ActionResult> AddSalesOrder(ShopifyTrnSalesOrderDTO objSalesOrder)
        {
            try
            {
                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.ManualCode == objSalesOrder.BranchManualCode
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.TrnSalesOrderDBSet existingSalesOrder = await (
                     from d in _dbContext.TrnSalesOrders
                     where d.BranchId == branch.Id
                     && d.DocumentReference == objSalesOrder.DocumentReference
                     select d
                 ).FirstOrDefaultAsync();

                if (existingSalesOrder != null)
                {
                    return StatusCode(404, "Sales order " + objSalesOrder.DocumentReference + " already exist.");
                }

                Int32 customerId = 0;
                Int32 termId = 0;

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.MstArticle_ArticleId.ManualCode == objSalesOrder.CustomerManualCode
                    select d
                ).FirstOrDefaultAsync();

                if (customer != null)
                {
                    customerId = customer.ArticleId;
                    termId = customer.TermId;
                }
                else
                {
                    DBSets.MstAccountDBSet receivableAccount = await (
                        from d in _dbContext.MstAccounts
                        select d
                    ).FirstOrDefaultAsync();

                    if (receivableAccount == null)
                    {
                        return StatusCode(404, "Receivable account not found.");
                    }

                    DBSets.MstTermDBSet term = await (
                        from d in _dbContext.MstTerms
                        select d
                    ).FirstOrDefaultAsync();

                    if (term == null)
                    {
                        return StatusCode(404, "Term not found.");
                    }

                    String articleCode = "0000000001";
                    DBSets.MstArticleDBSet lastArticle = await (
                        from d in _dbContext.MstArticles
                        where d.ArticleTypeId == 2
                        orderby d.Id descending
                        select d
                    ).FirstOrDefaultAsync();

                    if (lastArticle != null)
                    {
                        Int32 lastArticleCode = Convert.ToInt32(lastArticle.ArticleCode) + 0000000001;
                        articleCode = PadZeroes(lastArticleCode, 10);
                    }

                    DBSets.MstArticleDBSet newArticle = new DBSets.MstArticleDBSet()
                    {
                        ArticleCode = articleCode,
                        ManualCode = objSalesOrder.CustomerManualCode,
                        ArticleTypeId = 2,
                        Article = objSalesOrder.CustomerName,
                        ImageURL = "",
                        Particulars = "Shopify",
                        IsLocked = true,
                        CreatedByUserId = user.Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedByUserId = user.Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    _dbContext.MstArticles.Add(newArticle);
                    await _dbContext.SaveChangesAsync();

                    DBSets.MstArticleCustomerDBSet newArticleCustomer = new DBSets.MstArticleCustomerDBSet()
                    {
                        ArticleId = newArticle.Id,
                        Customer = objSalesOrder.CustomerName,
                        Address = "",
                        ContactPerson = "",
                        ContactNumber = "",
                        ReceivableAccountId = receivableAccount.Id,
                        TermId = term.Id,
                        CreditLimit = 0,
                        Category = ""
                    };

                    _dbContext.MstArticleCustomers.Add(newArticleCustomer);
                    await _dbContext.SaveChangesAsync();

                    customerId = newArticleCustomer.ArticleId;
                    termId = newArticleCustomer.TermId;
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "SALES ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String SONumber = "0000000001";
                DBSets.TrnSalesOrderDBSet lastSalesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.BranchId == branch.Id
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastSalesOrder != null)
                {
                    Int32 lastSONumber = Convert.ToInt32(lastSalesOrder.SONumber) + 0000000001;
                    SONumber = PadZeroes(lastSONumber, 10);
                }

                DBSets.TrnSalesOrderDBSet newSalesOrder = new DBSets.TrnSalesOrderDBSet()
                {
                    BranchId = branch.Id,
                    CurrencyId = user.MstCompany_CompanyId.CurrencyId,
                    SONumber = SONumber,
                    SODate = DateTime.Today,
                    ManualNumber = SONumber,
                    DocumentReference = objSalesOrder.DocumentReference,
                    CustomerId = customerId,
                    TermId = termId,
                    DateNeeded = DateTime.Today,
                    Remarks = objSalesOrder.Remarks,
                    SoldByUserId = user.Id,
                    PreparedByUserId = user.Id,
                    CheckedByUserId = user.Id,
                    ApprovedByUserId = user.Id,
                    Amount = 0,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = user.Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = user.Id,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnSalesOrders.Add(newSalesOrder);
                await _dbContext.SaveChangesAsync();

                Int32 SOId = newSalesOrder.Id;

                if (objSalesOrder.SalesOrderItems.Any())
                {
                    List<DBSets.TrnSalesOrderItemDBSet> newSalesOrderItems = new List<DBSets.TrnSalesOrderItemDBSet>();

                    foreach (var salesOrderItem in objSalesOrder.SalesOrderItems)
                    {
                        DBSets.MstArticleItemDBSet item = await (
                            from d in _dbContext.MstArticleItems
                            where d.BarCode == salesOrderItem.ItemBarCode
                            && d.MstArticle_ArticleId.IsLocked == true
                            select d
                        ).FirstOrDefaultAsync();

                        if (item != null)
                        {
                            DBSets.MstDiscountDBSet discount = await (
                                from d in _dbContext.MstDiscounts
                                select d
                            ).FirstOrDefaultAsync();

                            if (discount != null)
                            {
                                DBSets.MstArticleItemUnitDBSet itemUnit = await (
                                    from d in _dbContext.MstArticleItemUnits
                                    where d.ArticleId == item.ArticleId
                                    && d.UnitId == item.UnitId
                                    select d
                                ).FirstOrDefaultAsync();

                                if (itemUnit != null)
                                {
                                    Decimal VATAmount = (salesOrderItem.Amount / ((item.MstTax_SIVATId.TaxRate / 100) + 1)) * (item.MstTax_SIVATId.TaxRate / 100);
                                    Decimal WTAXAmount = (salesOrderItem.Amount / ((item.MstTax_SIVATId.TaxRate / 100) + 1)) * (item.MstTax_SIVATId.TaxRate / 100);

                                    Decimal baseQuantity = salesOrderItem.Quantity;
                                    if (itemUnit.Multiplier > 0)
                                    {
                                        baseQuantity = salesOrderItem.Quantity * (1 / itemUnit.Multiplier);
                                    }

                                    Decimal baseNetPrice = salesOrderItem.Amount;
                                    if (baseQuantity > 0)
                                    {
                                        baseNetPrice = salesOrderItem.Amount / baseQuantity;
                                    }

                                    newSalesOrderItems.Add(new DBSets.TrnSalesOrderItemDBSet
                                    {
                                        SOId = SOId,
                                        ItemId = item.ArticleId,
                                        ItemInventoryId = null,
                                        Particulars = salesOrderItem.Particulars,
                                        Quantity = salesOrderItem.Quantity,
                                        UnitId = item.UnitId,
                                        Price = salesOrderItem.Price,
                                        DiscountId = discount.Id,
                                        DiscountRate = discount.DiscountRate,
                                        DiscountAmount = salesOrderItem.DiscountAmount,
                                        NetPrice = salesOrderItem.NetPrice,
                                        Amount = salesOrderItem.Amount,
                                        VATId = item.SIVATId,
                                        VATRate = item.MstTax_SIVATId.TaxRate,
                                        VATAmount = VATAmount,
                                        WTAXId = item.WTAXId,
                                        WTAXRate = item.MstTax_WTAXId.TaxRate,
                                        WTAXAmount = WTAXAmount,
                                        BaseQuantity = baseQuantity,
                                        BaseUnitId = item.UnitId,
                                        BaseNetPrice = baseNetPrice,
                                        LineTimeStamp = DateTime.Now
                                    });
                                }
                            }
                        }
                    }

                    _dbContext.TrnSalesOrderItems.AddRange(newSalesOrderItems);
                    await _dbContext.SaveChangesAsync();
                }

                DBSets.TrnSalesOrderDBSet salesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.Id == SOId
                    select d
                ).FirstOrDefaultAsync();

                if (salesOrder != null)
                {
                    IEnumerable<DBSets.TrnSalesOrderItemDBSet> salesOrderItemsByCurrentSalesOrder = await (
                        from d in _dbContext.TrnSalesOrderItems
                        where d.SOId == SOId
                        select d
                    ).ToListAsync();

                    Decimal totalAmount = 0;

                    if (salesOrderItemsByCurrentSalesOrder.Any())
                    {
                        totalAmount = salesOrderItemsByCurrentSalesOrder.Sum(d => d.Amount);
                    }

                    DBSets.TrnSalesOrderDBSet lockSalesOrder = salesOrder;
                    lockSalesOrder.Amount = totalAmount;
                    lockSalesOrder.IsLocked = true;
                    lockSalesOrder.UpdatedByUserId = user.Id;
                    lockSalesOrder.UpdatedDateTime = DateTime.Now;

                    await _dbContext.SaveChangesAsync();
                }

                return StatusCode(200, newSalesOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
