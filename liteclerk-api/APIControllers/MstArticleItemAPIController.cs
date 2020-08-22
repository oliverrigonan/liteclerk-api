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
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> articleItems = await (
                    from d in _dbContext.MstArticleItems
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroup.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup
                        },
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
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVAT.TaxCode,
                            ManualCode = d.MstTax_RRVAT.ManualCode,
                            TaxDescription = d.MstTax_RRVAT.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVAT.TaxCode,
                            ManualCode = d.MstTax_SIVAT.ManualCode,
                            TaxDescription = d.MstTax_SIVAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, articleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("locked/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetLockedArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> lockedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_Article.IsLocked == true
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroup.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup
                        },
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
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVAT.TaxCode,
                            ManualCode = d.MstTax_RRVAT.ManualCode,
                            TaxDescription = d.MstTax_RRVAT.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVAT.TaxCode,
                            ManualCode = d.MstTax_SIVAT.ManualCode,
                            TaxDescription = d.MstTax_SIVAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("inventory/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetInventoryArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> inventoryArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_Article.IsLocked == true
                    && d.IsInventory == true
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroup.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup
                        },
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
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVAT.TaxCode,
                            ManualCode = d.MstTax_RRVAT.ManualCode,
                            TaxDescription = d.MstTax_RRVAT.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVAT.TaxCode,
                            ManualCode = d.MstTax_SIVAT.ManualCode,
                            TaxDescription = d.MstTax_SIVAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, inventoryArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("non-inventory/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemDTO>>> GetNonInventoryArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> nonInventoryArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_Article.IsLocked == true
                    && d.IsInventory == false
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Article = d.MstArticle_Article.Article
                        },
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_Unit.UnitCode,
                            ManualCode = d.MstUnit_Unit.ManualCode,
                            Unit = d.MstUnit_Unit.Unit
                        },
                        IsJob = d.IsJob,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroup.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroup.ArticleAccountGroup
                        },
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
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVAT.TaxCode,
                            ManualCode = d.MstTax_RRVAT.ManualCode,
                            TaxDescription = d.MstTax_RRVAT.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVAT.TaxCode,
                            ManualCode = d.MstTax_SIVAT.ManualCode,
                            TaxDescription = d.MstTax_SIVAT.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAX.TaxCode,
                            ManualCode = d.MstTax_WTAX.ManualCode,
                            TaxDescription = d.MstTax_WTAX.TaxDescription
                        },
                        IsLocked = d.MstArticle_Article.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_CreatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.MstArticle_Article.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_Article.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_Article.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, nonInventoryArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
