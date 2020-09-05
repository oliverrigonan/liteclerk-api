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
                            AccountCode = d.MstAccount_AssetAccount.AccountCode,
                            ManualCode = d.MstAccount_AssetAccount.Account,
                            Account = d.MstAccount_AssetAccount.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccount.AccountCode,
                            ManualCode = d.MstAccount_SalesAccount.Account,
                            Account = d.MstAccount_SalesAccount.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccount.AccountCode,
                            ManualCode = d.MstAccount_CostAccount.Account,
                            Account = d.MstAccount_CostAccount.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccount.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccount.Account,
                            Account = d.MstAccount_ExpenseAccount.Account
                        },
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
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
