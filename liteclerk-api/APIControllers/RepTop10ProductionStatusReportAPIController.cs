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
    public class RepTop10ProductionStatusReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10ProductionStatusReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10ProductionStatusReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10ProductionStatusReportDTO>> taskTop10ProductionStatusReportList = Task.FromResult(new List<DTO.RepTop10ProductionStatusReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var jobOrderDepartments = await (
                    from d in _dbContext.TrnJobOrderDepartments
                    where d.TrnJobOrder_JOId.JODate >= Convert.ToDateTime(startDate)
                    && d.TrnJobOrder_JOId.JODate <= Convert.ToDateTime(endDate)
                    && d.TrnJobOrder_JOId.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.TrnJobOrder_JOId.BranchId == loginUser.BranchId
                    && d.TrnJobOrder_JOId.IsLocked == true
                    && d.TrnJobOrder_JOId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (jobOrderDepartments.Any())
                {
                    var groupJobOrderDepartments = from d in jobOrderDepartments
                                                   group d by new
                                                   {
                                                       Status = d.Status
                                                   } into g
                                                   select new DTO.RepTop10ProductionStatusReportDTO
                                                   {
                                                       Status = g.Key.Status,
                                                       Count = g.Count()
                                                   };

                    taskTop10ProductionStatusReportList = Task.FromResult(groupJobOrderDepartments.OrderByDescending(d => d.Count).Take(10).ToList());
                }

                return StatusCode(200, taskTop10ProductionStatusReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
