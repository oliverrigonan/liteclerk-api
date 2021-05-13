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
    public class RepTop10ExpensesReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTop10ExpensesReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetTop10ExpensesReportList(String startDate, String endDate)
        {
            try
            {
                Task<List<DTO.RepTop10ExpensesReportDTO>> taskTop10ExpensesReportList = Task.FromResult(new List<DTO.RepTop10ExpensesReportDTO>());

                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var journalEntries = await (
                    from d in _dbContext.SysJournalEntries
                    where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                    && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == loginUser.CompanyId
                    && d.BranchId == loginUser.BranchId
                    && d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountCategoryId == 5
                    select d
                ).ToListAsync();

                if (journalEntries.Any())
                {
                    var groupJournalEntries = from d in journalEntries
                                              group d by new
                                              {
                                                  Account = d.MstAccount_AccountId.Account
                                              } into g
                                              select new DTO.RepTop10ExpensesReportDTO
                                              {
                                                  Account = new DTO.MstAccountDTO
                                                  {
                                                      Account = g.Key.Account
                                                  },
                                                  Amount = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                              };

                    taskTop10ExpensesReportList = Task.FromResult(groupJournalEntries.OrderByDescending(d => d.Amount).Take(10).ToList());
                }

                return StatusCode(200, taskTop10ExpensesReportList.Result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
