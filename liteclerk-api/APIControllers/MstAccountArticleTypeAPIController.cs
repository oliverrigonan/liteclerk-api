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
    public class MstAccountArticleTypeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstAccountArticleTypeAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{accountId}")]
        public async Task<ActionResult> GetAccountArticleTypeList(Int32 accountId)
        {
            try
            {
                IEnumerable<DTO.MstAccountArticleTypeDTO> accountArticleTypes = await (
                    from d in _dbContext.MstAccountArticleTypes
                    where d.AccountId == accountId
                    select new DTO.MstAccountArticleTypeDTO
                    {
                        Id = d.Id,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleTypeId = d.ArticleTypeId,
                        ArticleType = new DTO.MstArticleTypeDTO
                        {
                            ArticleType = d.MstArticleType_ArticleTypeId.ArticleType
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, accountArticleTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetAccountArticleTypeDetail(Int32 id)
        {
            try
            {
                DTO.MstAccountArticleTypeDTO accountArticleType = await (
                    from d in _dbContext.MstAccountArticleTypes
                    where d.Id == id
                    select new DTO.MstAccountArticleTypeDTO
                    {
                        Id = d.Id,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleTypeId = d.ArticleTypeId,
                        ArticleType = new DTO.MstArticleTypeDTO
                        {
                            ArticleType = d.MstArticleType_ArticleTypeId.ArticleType
                        }
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, accountArticleType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddAccountArticleType([FromBody] DTO.MstAccountArticleTypeDTO mstAccountArticleTypeDTO)
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
                    return StatusCode(404, "No rights to add an account article type.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an account article type.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstAccountArticleTypeDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleTypeDBSet articleType = await (
                    from d in _dbContext.MstArticleTypes
                    where d.Id == mstAccountArticleTypeDTO.ArticleTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (articleType == null)
                {
                    return StatusCode(404, "Article type not found.");
                }

                DBSets.MstAccountArticleTypeDBSet newAccountArticleType = new DBSets.MstAccountArticleTypeDBSet()
                {
                    Id = mstAccountArticleTypeDTO.Id,
                    AccountId = mstAccountArticleTypeDTO.AccountId,
                    ArticleTypeId = mstAccountArticleTypeDTO.ArticleTypeId
                };

                _dbContext.MstAccountArticleTypes.Add(newAccountArticleType);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateAccountArticleType(int id, [FromBody] DTO.MstAccountArticleTypeDTO mstAccountArticleTypeDTO)
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
                    return StatusCode(404, "No rights to edit or update an account article type.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an account article type.");
                }

                DBSets.MstAccountArticleTypeDBSet accountArticleType = await (
                    from d in _dbContext.MstAccountArticleTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (accountArticleType == null)
                {
                    return StatusCode(404, "Account article type not found.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstAccountArticleTypeDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleTypeDBSet articleType = await (
                    from d in _dbContext.MstArticleTypes
                    where d.Id == mstAccountArticleTypeDTO.ArticleTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (articleType == null)
                {
                    return StatusCode(404, "Article type not found.");
                }

                DBSets.MstAccountArticleTypeDBSet updateAccountArticleType = accountArticleType;
                updateAccountArticleType.ArticleTypeId = mstAccountArticleTypeDTO.ArticleTypeId;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteAccountArticleType(int id)
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
                    return StatusCode(404, "No rights to delete an account article type.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an account article type.");
                }

                DBSets.MstAccountArticleTypeDBSet accountArticleType = await (
                    from d in _dbContext.MstAccountArticleTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (accountArticleType == null)
                {
                    return StatusCode(404, "Account article type not found.");
                }

                _dbContext.MstAccountArticleTypes.Remove(accountArticleType);
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
