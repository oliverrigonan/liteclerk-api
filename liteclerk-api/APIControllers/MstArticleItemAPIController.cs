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
    public class MstArticleItemAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> items = await (
                    from d in _dbContext.MstArticleItems
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleCode = d.MstArticle_Article.ArticleCode,
                        ManualCode = d.MstArticle_Article.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = d.MstUnit_Unit.Unit,
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup,
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = d.MstAccount_AssetAccount.Account,
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = d.MstAccount_SalesAccount.Account,
                        CostAccountId = d.CostAccountId,
                        CostAccount = d.MstAccount_CostAccount.Account,
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = d.MstAccount_ExpenseAccount.Account,
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUserFullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.MstArticle_Article.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.MstArticle_Article.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, items);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("locked/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetLockedItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> lockedItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_Article.IsLocked == true
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        ArticleCode = d.MstArticle_Article.ArticleCode,
                        ManualCode = d.MstArticle_Article.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = d.MstUnit_Unit.Unit,
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup,
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = d.MstAccount_AssetAccount.Account,
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = d.MstAccount_SalesAccount.Account,
                        CostAccountId = d.CostAccountId,
                        CostAccount = d.MstAccount_CostAccount.Account,
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = d.MstAccount_ExpenseAccount.Account,
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUserFullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.MstArticle_Article.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.MstArticle_Article.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, lockedItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
