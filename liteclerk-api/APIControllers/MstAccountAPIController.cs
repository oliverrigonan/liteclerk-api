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
    public class MstAccountAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstAccountAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetAccountDetail(Int32 id)
        {
            try
            {
                DTO.MstAccountDTO account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == id
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
                ).FirstOrDefaultAsync();

                return StatusCode(200, account);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddAccount([FromBody] DTO.MstAccountDTO mstAccountDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add an account.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an account.");
                }

                DBSets.MstAccountTypeDBSet accountType = await (
                    from d in _dbContext.MstAccountTypes
                    where d.Id == mstAccountDTO.AccountTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (accountType == null)
                {
                    return StatusCode(404, "Account type not found.");
                }

                DBSets.MstAccountCashFlowDBSet accountCashFlow = await (
                    from d in _dbContext.MstAccountCashFlows
                    where d.Id == mstAccountDTO.AccountCashFlowId
                    select d
                ).FirstOrDefaultAsync();

                if (accountCashFlow == null)
                {
                    return StatusCode(404, "Account cash flow not found.");
                }

                String accountCode = "0000000001";
                DBSets.MstArticleDBSet lastArticle = await (
                    from d in _dbContext.MstArticles
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastArticle != null)
                {
                    Int32 lastArticleCode = Convert.ToInt32(lastArticle.ArticleCode) + 0000000001;
                    accountCode = PadZeroes(lastArticleCode, 10);
                }

                DBSets.MstAccountDBSet newAccount = new DBSets.MstAccountDBSet()
                {
                    Id = mstAccountDTO.Id,
                    AccountCode = accountCode,
                    ManualCode = mstAccountDTO.ManualCode,
                    Account = mstAccountDTO.Account,
                    AccountTypeId = mstAccountDTO.AccountTypeId,
                    AccountCashFlowId = mstAccountDTO.AccountCashFlowId,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstAccounts.Add(newAccount);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateAccount(int id, [FromBody] DTO.MstAccountDTO mstAccountDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update an account.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an account.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstAccountTypeDBSet accountType = await (
                    from d in _dbContext.MstAccountTypes
                    where d.Id == mstAccountDTO.AccountTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (accountType == null)
                {
                    return StatusCode(404, "Account type not found.");
                }

                DBSets.MstAccountCashFlowDBSet accountCashFlow = await (
                    from d in _dbContext.MstAccountCashFlows
                    where d.Id == mstAccountDTO.AccountCashFlowId
                    select d
                ).FirstOrDefaultAsync();

                if (accountCashFlow == null)
                {
                    return StatusCode(404, "Account cash flow not found.");
                }

                DBSets.MstAccountDBSet updateAccount = account;
                updateAccount.ManualCode = mstAccountDTO.ManualCode;
                updateAccount.Account = mstAccountDTO.Account;
                updateAccount.AccountTypeId = mstAccountDTO.AccountTypeId;
                updateAccount.AccountCashFlowId = mstAccountDTO.AccountCashFlowId;
                updateAccount.UpdatedByUserId = loginUserId;
                updateAccount.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SetupChartOfAccount"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete an account.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an account.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                _dbContext.MstAccounts.Remove(account);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
