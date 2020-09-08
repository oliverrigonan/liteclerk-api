using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MstAccountAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstAccountAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetAccountList()
        {
            try
            {
                IEnumerable<DTO.MstAccountDTO> accounts = await (
                    from d in _dbContext.MstAccounts
                    select new DTO.MstAccountDTO
                    {
                        Id = d.Id,
                        AccountCode = d.AccountCode,
                        ManualCode = d.ManualCode,
                        Account = d.Account,
                        AccountTypeId = d.AccountTypeId,
                        AccountType = new DTO.MstAccountTypeDTO
                        {
                            AccountTypeCode = d.MstAccountType_AccountTypeId.AccountTypeCode,
                            ManualCode = d.MstAccountType_AccountTypeId.ManualCode,
                            AccountType = d.MstAccountType_AccountTypeId.AccountType
                        },
                        AccountCashFlowId = d.AccountCashFlowId,
                        AccountCashFlow = new DTO.MstAccountCashFlowDTO
                        {
                            AccountCashFlowCode = d.MstAccountCashFlow_AccountCashFlowId.AccountCashFlowCode,
                            ManualCode = d.MstAccountCashFlow_AccountCashFlowId.ManualCode,
                            AccountCashFlow = d.MstAccountCashFlow_AccountCashFlowId.AccountCashFlow
                        },
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, accounts);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
