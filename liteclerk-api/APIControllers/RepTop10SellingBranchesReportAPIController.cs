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
    public class RepTop10SellingBranchesReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10SellingBranchesReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10SellingBranchesReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.TrnSalesInvoiceDTO>> taskTop10SellingBranchesReportList = Task.FromResult(new List<DTO.TrnSalesInvoiceDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.SIDate >= Convert.ToDateTime(startDate)
                    && d.SIDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.IsLocked == true
                    select d
                ).ToListAsync();

                if (salesInvoices.Any())
                {
                    var groupSalesInvoices = from d in salesInvoices
                                             group d by new
                                             {
                                                 Branch = d.MstCompanyBranch_BranchId.Branch
                                             } into g
                                             select new DTO.TrnSalesInvoiceDTO
                                             {
                                                 Branch = new DTO.MstCompanyBranchDTO
                                                 {
                                                     Branch = g.Key.Branch
                                                 },
                                                 Amount = g.Sum(d => d.Amount)
                                             };

                    taskTop10SellingBranchesReportList = Task.FromResult(groupSalesInvoices.OrderByDescending(d => d.Amount).Take(10).ToList());
                }

                return StatusCode(200, taskTop10SellingBranchesReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
