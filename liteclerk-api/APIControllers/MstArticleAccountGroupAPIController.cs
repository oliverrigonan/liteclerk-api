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
    public class MstArticleAccountGroupAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleAccountGroupAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetArticleAccountGroupList()
        {
            try
            {
                var articleAccountGroups = await (
                    from d in _dbContext.MstArticleAccountGroups
                    select new DTO.MstArticleAccountGroupDTO
                    {
                        Id = d.Id,
                        ArticleAccountGroupCode = d.ArticleAccountGroupCode,
                        ManualCode = d.ManualCode,
                        ArticleAccountGroup = d.ArticleAccountGroup,
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },

                        ArticleTypeId = d.ArticleTypeId,

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

                return StatusCode(200, articleAccountGroups);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleAccountGroupDetail(Int32 id)
        {
            try
            {
                var articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    where d.Id == id
                    select new DTO.MstArticleAccountGroupDTO
                    {
                        Id = d.Id,
                        ArticleAccountGroupCode = d.ArticleAccountGroupCode,
                        ManualCode = d.ManualCode,
                        ArticleAccountGroup = d.ArticleAccountGroup,
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },

                        ArticleTypeId = d.ArticleTypeId,

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

                return StatusCode(200, articleAccountGroup);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleAccountGroup([FromBody] DTO.MstArticleAccountGroupDTO mstArticleAccountGroupDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a article account group.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a article account group.");
                }

                var assetAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.AssetAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (assetAccount == null)
                {
                    return StatusCode(404, "Asset account not found.");
                }

                var salesAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.SalesAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (salesAccount == null)
                {
                    return StatusCode(404, "Sales account not found.");
                }

                var costAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.CostAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (costAccount == null)
                {
                    return StatusCode(404, "Cost account not found.");
                }

                var expenseAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.ExpenseAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (expenseAccount == null)
                {
                    return StatusCode(404, "Expense account not found.");
                }

                String articleAccountGroupCode = "0000000001";
                var lastArticleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastArticleAccountGroup != null)
                {
                    Int32 lastArticleAccountGroupCode = Convert.ToInt32(lastArticleAccountGroup.ArticleAccountGroupCode) + 0000000001;
                    articleAccountGroupCode = PadZeroes(lastArticleAccountGroupCode, 10);
                }

                var newArticleAccountGroup = new DBSets.MstArticleAccountGroupDBSet()
                {
                    ArticleAccountGroupCode = articleAccountGroupCode,
                    ManualCode = mstArticleAccountGroupDTO.ManualCode,
                    ArticleAccountGroup = mstArticleAccountGroupDTO.ArticleAccountGroup,
                    AssetAccountId = mstArticleAccountGroupDTO.AssetAccountId,
                    SalesAccountId = mstArticleAccountGroupDTO.SalesAccountId,
                    CostAccountId = mstArticleAccountGroupDTO.CostAccountId,
                    ExpenseAccountId = mstArticleAccountGroupDTO.ExpenseAccountId,
                    ArticleTypeId = mstArticleAccountGroupDTO.ArticleTypeId,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticleAccountGroups.Add(newArticleAccountGroup);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateArticleAccountGroup(int id, [FromBody] DTO.MstArticleAccountGroupDTO mstArticleAccountGroupDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a article account group.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a article account group.");
                }

                var articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (articleAccountGroup == null)
                {
                    return StatusCode(404, "Article account group not found.");
                }

                var assetAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.AssetAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (assetAccount == null)
                {
                    return StatusCode(404, "Asset account not found.");
                }

                var salesAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.SalesAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (salesAccount == null)
                {
                    return StatusCode(404, "Sales account not found.");
                }

                var costAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.CostAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (costAccount == null)
                {
                    return StatusCode(404, "Cost account not found.");
                }

                var expenseAccount = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstArticleAccountGroupDTO.ExpenseAccountId
                    select d
                ).FirstOrDefaultAsync();

                if (expenseAccount == null)
                {
                    return StatusCode(404, "Expense account not found.");
                }

                var updateArticleAccountGroup = articleAccountGroup;
                updateArticleAccountGroup.ManualCode = mstArticleAccountGroupDTO.ManualCode;
                updateArticleAccountGroup.ArticleAccountGroup = mstArticleAccountGroupDTO.ArticleAccountGroup;
                updateArticleAccountGroup.AssetAccountId = mstArticleAccountGroupDTO.AssetAccountId;
                updateArticleAccountGroup.SalesAccountId = mstArticleAccountGroupDTO.SalesAccountId;
                updateArticleAccountGroup.CostAccountId = mstArticleAccountGroupDTO.CostAccountId;
                updateArticleAccountGroup.ExpenseAccountId = mstArticleAccountGroupDTO.ExpenseAccountId;
                updateArticleAccountGroup.ArticleTypeId = mstArticleAccountGroupDTO.ArticleTypeId;
                updateArticleAccountGroup.UpdatedByUserId = loginUserId;
                updateArticleAccountGroup.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteArticleAccountGroup(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a article account group.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a article account group.");
                }

                var articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (articleAccountGroup == null)
                {
                    return StatusCode(404, "Article account group not found.");
                }

                _dbContext.MstArticleAccountGroups.Remove(articleAccountGroup);
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
