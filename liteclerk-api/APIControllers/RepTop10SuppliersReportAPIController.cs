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
    public class RepTop10SuppliersReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10SuppliersReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10SuppliersReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10SuppliersReportDTO>> taskTop10SuppliersReportList = Task.FromResult(new List<DTO.RepTop10SuppliersReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var receivingReceipts = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.RRDate >= Convert.ToDateTime(startDate)
                    && d.RRDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.IsLocked == true
                    && d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (receivingReceipts.Any())
                {
                    var groupReceivingReceipts = from d in receivingReceipts
                                                 group d by new
                                                 {
                                                     Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier
                                                 } into g
                                                 select new DTO.RepTop10SuppliersReportDTO
                                                 {
                                                     Supplier = new DTO.MstArticleSupplierDTO
                                                     {
                                                         Supplier = g.Key.Supplier
                                                     },
                                                     Amount = g.Sum(d => d.BaseAmount)
                                                 };

                    taskTop10SuppliersReportList = Task.FromResult(groupReceivingReceipts.OrderByDescending(d => d.Amount).Take(10).ToList());
                }

                return StatusCode(200, taskTop10SuppliersReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
