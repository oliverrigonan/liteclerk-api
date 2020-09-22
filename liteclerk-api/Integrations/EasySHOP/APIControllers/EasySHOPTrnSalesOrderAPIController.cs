using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.Integrations.EasySHOP.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasySHOPTrnSalesOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPTrnSalesOrderAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpPost("add")]
        public async Task<ActionResult> AddSalesOrder(DTO.EasySHOPTrnSalesOrderDTO objSalesOrder)
        {
            try
            {
                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.ManualCode == "104"
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

                Int32 customerId = 0;
                Int32 termId = 0;

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.MstArticle_ArticleId.ManualCode == objSalesOrder.CustomerCode
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
                        ManualCode = objSalesOrder.CustomerCode,
                        ArticleTypeId = 2,
                        Article = objSalesOrder.CustomerName,
                        IsLocked = false,
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
                        CreditLimit = 0
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
                    DocumentReference = "",
                    CustomerId = customerId,
                    TermId = termId,
                    DateNeeded = DateTime.Today,
                    Remarks = "",
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

                if (objSalesOrder.ListSalesOrderItems.Any())
                {
                    List<DBSets.TrnSalesOrderItemDBSet> newSalesOrderItems = new List<DBSets.TrnSalesOrderItemDBSet>();
                    Decimal totalAmount = 0;

                    foreach (var salesOrderItem in objSalesOrder.ListSalesOrderItems)
                    {
                        DBSets.MstArticleItemDBSet item = await (
                            from d in _dbContext.MstArticleItems
                            where d.BarCode == salesOrderItem.ItemCode
                            && d.MstArticle_ArticleId.IsLocked == true
                            select d
                        ).FirstOrDefaultAsync();

                        if (item == null)
                        {
                            return StatusCode(404, "Item not found.");
                        }

                        newSalesOrderItems.Add(new DBSets.TrnSalesOrderItemDBSet
                        {

                        });
                    }

                    _dbContext.TrnSalesOrderItems.AddRange(newSalesOrderItems);
                    await _dbContext.SaveChangesAsync();

                }

                return StatusCode(200, newSalesOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

    }
}
