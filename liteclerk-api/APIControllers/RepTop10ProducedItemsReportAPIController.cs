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
    public class RepTop10ProducedItemsReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10ProducedItemsReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10ProducedItemsReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10ProducedItemsReportDTO>> taskTop10ProducedItemsReportList = Task.FromResult(new List<DTO.RepTop10ProducedItemsReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var jobOrders = await (
                    from d in _dbContext.TrnJobOrders
                    where d.JODate >= Convert.ToDateTime(startDate)
                    && d.JODate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.BranchId == loginUser.BranchId
                    && d.IsLocked == true
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (jobOrders.Any())
                {
                    var groupJobOrders = from d in jobOrders
                                         group d by new
                                         {
                                             ItemDescription = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description
                                         } into g
                                         select new DTO.RepTop10ProducedItemsReportDTO
                                         {
                                             Item = new DTO.MstArticleItemDTO
                                             {
                                                 Description = g.Key.ItemDescription
                                             },
                                             Quantity = g.Sum(d => d.BaseQuantity)
                                         };

                    taskTop10ProducedItemsReportList = Task.FromResult(groupJobOrders.OrderByDescending(d => d.Quantity).Take(10).ToList());
                }

                return StatusCode(200, taskTop10ProducedItemsReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
