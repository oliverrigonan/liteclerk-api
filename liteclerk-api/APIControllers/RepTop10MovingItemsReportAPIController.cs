﻿using System;
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
    public class RepTop10MovingItemsReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10MovingItemsReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10MovingItemsReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10MovingItemsReportDTO>> taskTop10MovingItemsReportList = Task.FromResult(new List<DTO.RepTop10MovingItemsReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var inventories = await (
                    from d in _dbContext.SysInventories
                    where d.InventoryDate >= Convert.ToDateTime(startDate)
                    && d.InventoryDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.BranchId == loginUser.BranchId
                    && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (inventories.Any())
                {
                    var groupInventories = from d in inventories
                                           group d by new
                                           {
                                               ItemDescription = d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                                           } into g
                                           select new DTO.RepTop10MovingItemsReportDTO
                                           {
                                               Item = new DTO.MstArticleItemDTO
                                               {
                                                   Description = g.Key.ItemDescription
                                               },
                                               QuantityIn = g.Sum(d => d.QuantityIn),
                                               QuantityOut = g.Sum(d => d.QuantityOut),
                                               Count = g.Count()
                                           };

                    taskTop10MovingItemsReportList = Task.FromResult(groupInventories.OrderByDescending(d => d.Count).Take(10).ToList());
                }

                return StatusCode(200, taskTop10MovingItemsReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
