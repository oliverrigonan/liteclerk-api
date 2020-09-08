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
    public class MstArticleAccountGroupAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleAccountGroupAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetArticleAccountGroupList()
        {
            try
            {
                IEnumerable<DTO.MstArticleAccountGroupDTO> articleAccountGroups = await (
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
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.Account,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.Account,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.Account,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.Account,
                            Account = d.MstAccount_ExpenseAccountId.Account
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

                return StatusCode(200, articleAccountGroups);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
