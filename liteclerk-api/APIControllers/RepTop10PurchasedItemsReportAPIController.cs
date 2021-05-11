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
    [Route("api/[controller]")]
    [ApiController]
    public class RepTop10PurchasedItemsReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10PurchasedItemsReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10PurchasedItemsReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10PurchasedItemsReportDTO>> taskTop10PurchasedItemsReportList = Task.FromResult(new List<DTO.RepTop10PurchasedItemsReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var receivingReceiptItems = await (
                    from d in _dbContext.TrnReceivingReceiptItems
                    where d.TrnReceivingReceipt_RRId.RRDate >= Convert.ToDateTime(startDate)
                    && d.TrnReceivingReceipt_RRId.RRDate <= Convert.ToDateTime(endDate)
                    && d.TrnReceivingReceipt_RRId.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.TrnReceivingReceipt_RRId.BranchId == loginUser.BranchId
                    && d.TrnReceivingReceipt_RRId.IsLocked == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (receivingReceiptItems.Any())
                {
                    var groupReceivingReceiptItems = from d in receivingReceiptItems
                                                     group d by new
                                                     {
                                                         ItemDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description
                                                     } into g
                                                     select new DTO.RepTop10PurchasedItemsReportDTO
                                                     {
                                                         Item = new DTO.MstArticleItemDTO
                                                         {
                                                             Description = g.Key.ItemDescription
                                                         },
                                                         Quantity = g.Sum(d => d.BaseQuantity)
                                                     };

                    taskTop10PurchasedItemsReportList = Task.FromResult(groupReceivingReceiptItems.OrderByDescending(d => d.Quantity).Take(10).ToList());
                }

                return StatusCode(200, taskTop10PurchasedItemsReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
