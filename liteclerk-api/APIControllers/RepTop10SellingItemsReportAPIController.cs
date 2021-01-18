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
    public class RepTop10SellingItemsReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10SellingItemsReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10SellingItemsReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.TrnSalesInvoiceItemDTO>> taskTop10SellingItemsReportList = Task.FromResult(new List<DTO.TrnSalesInvoiceItemDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesInvoiceItems = await (
                    from d in _dbContext.TrnSalesInvoiceItems
                    where d.TrnSalesInvoice_SIId.SIDate >= Convert.ToDateTime(startDate)
                    && d.TrnSalesInvoice_SIId.SIDate <= Convert.ToDateTime(endDate)
                    && d.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.TrnSalesInvoice_SIId.BranchId == loginUser.BranchId
                    && d.TrnSalesInvoice_SIId.IsLocked == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (salesInvoiceItems.Any())
                {
                    var groupSalesInvoiceItems = from d in salesInvoiceItems
                                                 group d by new
                                                 {
                                                     ItemDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description
                                                 } into g
                                                 select new DTO.TrnSalesInvoiceItemDTO
                                                 {
                                                     Item = new DTO.MstArticleItemDTO
                                                     {
                                                         Description = g.Key.ItemDescription
                                                     },
                                                     Quantity = g.Sum(d => d.Quantity)
                                                 };

                    taskTop10SellingItemsReportList = Task.FromResult(groupSalesInvoiceItems.OrderByDescending(d => d.Quantity).Take(10).ToList());
                }

                return StatusCode(200, taskTop10SellingItemsReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
