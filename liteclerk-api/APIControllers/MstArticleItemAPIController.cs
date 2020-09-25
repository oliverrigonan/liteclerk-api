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
    public class MstArticleItemAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetArticleItemList()
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
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, articleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> lockedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/inventory")]
        public async Task<ActionResult> GetInventoryArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> inventoryArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.IsInventory == true
                    && (d.Kitting == "NONE" || d.Kitting == "PRODUCED" || d.Kitting == "COMPONENT")
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, inventoryArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/inventory/exceptByArticle/{articleId}")]
        public async Task<ActionResult> GetInventoryArticleItemListExceptByArticle(Int32 articleId)
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> inventoryArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.ArticleId != articleId
                    && d.IsInventory == true
                    && (d.Kitting == "NONE" || d.Kitting == "PRODUCED")
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, inventoryArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/non-inventory")]
        public async Task<ActionResult> GetNonInventoryArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> nonInventoryArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.IsInventory == false
                    && (d.Kitting == "NONE" || d.Kitting == "PACKAGE")
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, nonInventoryArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/produced")]
        public async Task<ActionResult> GetProducedArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> producedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.IsInventory == true
                    && d.Kitting == "PRODUCED"
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, producedArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/component")]
        public async Task<ActionResult> GetComponentArticleItemList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemDTO> producedArticleItems = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    && d.IsInventory == true
                    && d.Kitting == "COMPONENT"
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, producedArticleItems);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleItemDetail(Int32 id)
        {
            try
            {
                DTO.MstArticleItemDTO producedArticleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.Id == id
                    select new DTO.MstArticleItemDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL
                        },
                        ArticleManualCode = d.MstArticle_ArticleId.ManualCode,
                        SKUCode = d.SKUCode,
                        BarCode = d.BarCode,
                        Description = d.Description,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            UnitCode = d.MstUnit_UnitId.UnitCode,
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Category = d.Category,
                        IsInventory = d.IsInventory,
                        ArticleAccountGroupId = d.ArticleAccountGroupId,
                        ArticleAccountGroup = new DTO.MstArticleAccountGroupDTO
                        {
                            ArticleAccountGroupCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroupCode,
                            ManualCode = d.MstArticleAccountGroup_ArticleAccountGroupId.ManualCode,
                            ArticleAccountGroup = d.MstArticleAccountGroup_ArticleAccountGroupId.ArticleAccountGroup
                        },
                        AssetAccountId = d.AssetAccountId,
                        AssetAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AssetAccountId.AccountCode,
                            ManualCode = d.MstAccount_AssetAccountId.ManualCode,
                            Account = d.MstAccount_AssetAccountId.Account
                        },
                        SalesAccountId = d.SalesAccountId,
                        SalesAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_SalesAccountId.AccountCode,
                            ManualCode = d.MstAccount_SalesAccountId.ManualCode,
                            Account = d.MstAccount_SalesAccountId.Account
                        },
                        CostAccountId = d.CostAccountId,
                        CostAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_CostAccountId.AccountCode,
                            ManualCode = d.MstAccount_CostAccountId.ManualCode,
                            Account = d.MstAccount_CostAccountId.Account
                        },
                        ExpenseAccountId = d.ExpenseAccountId,
                        ExpenseAccount = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_ExpenseAccountId.AccountCode,
                            ManualCode = d.MstAccount_ExpenseAccountId.ManualCode,
                            Account = d.MstAccount_ExpenseAccountId.Account
                        },
                        Price = d.Price,
                        RRVATId = d.RRVATId,
                        RRVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_RRVATId.TaxCode,
                            ManualCode = d.MstTax_RRVATId.ManualCode,
                            TaxDescription = d.MstTax_RRVATId.TaxDescription
                        },
                        SIVATId = d.SIVATId,
                        SIVAT = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_SIVATId.TaxCode,
                            ManualCode = d.MstTax_SIVATId.ManualCode,
                            TaxDescription = d.MstTax_SIVATId.TaxDescription
                        },
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        Kitting = d.Kitting,
                        IsLocked = d.MstArticle_ArticleId.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.MstArticle_ArticleId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstArticle_ArticleId.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.MstArticle_ArticleId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, producedArticleItem);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleItem()
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
                    && d.SysForm_FormId.Form == "SetupItemList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add an item.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an item.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleAccountGroupDBSet articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    select d
                ).FirstOrDefaultAsync();

                if (articleAccountGroup == null)
                {
                    return StatusCode(404, "Account group not found.");
                }

                DBSets.MstTaxDBSet tax = await (
                    from d in _dbContext.MstTaxes
                    select d
                ).FirstOrDefaultAsync();

                if (tax == null)
                {
                    return StatusCode(404, "Tax not found.");
                }

                DBSets.MstCodeTableDBSet codeTableKitting = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "ITEM KITTING"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableKitting == null)
                {
                    return StatusCode(404, "Kitting not found.");
                }

                String articleCode = "0000000001";
                DBSets.MstArticleDBSet lastArticle = await (
                    from d in _dbContext.MstArticles
                    where d.ArticleTypeId == 1
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastArticle != null)
                {
                    Int32 lastArticleCode = Convert.ToInt32(lastArticle.ArticleCode) + 0000000001;
                    articleCode = PadZeroes(lastArticleCode, 10);
                }

                DBSets.MstArticleDBSet newArticle = new DBSets.MstArticleDBSet()
                {
                    ArticleCode = articleCode,
                    ManualCode = articleCode,
                    ArticleTypeId = 1,
                    Article = "",
                    ImageURL = "",
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticles.Add(newArticle);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleItemDBSet newArticleItem = new DBSets.MstArticleItemDBSet()
                {
                    ArticleId = newArticle.Id,
                    SKUCode = articleCode,
                    BarCode = articleCode,
                    Description = "",
                    UnitId = unit.Id,
                    Category = "",
                    IsInventory = true,
                    ArticleAccountGroupId = articleAccountGroup.Id,
                    AssetAccountId = articleAccountGroup.AssetAccountId,
                    SalesAccountId = articleAccountGroup.SalesAccountId,
                    CostAccountId = articleAccountGroup.CostAccountId,
                    ExpenseAccountId = articleAccountGroup.ExpenseAccountId,
                    Price = 0,
                    RRVATId = tax.Id,
                    SIVATId = tax.Id,
                    WTAXId = tax.Id,
                    Kitting = codeTableKitting.CodeValue
                };

                _dbContext.MstArticleItems.Add(newArticleItem);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newArticleItem.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveArticleItem(Int32 id, [FromBody] DTO.MstArticleItemDTO mstArticleItemDTO)
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
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save an item.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save an item.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (articleItem.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to an item that is locked.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == mstArticleItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleAccountGroupDBSet articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    where d.Id == mstArticleItemDTO.ArticleAccountGroupId
                    select d
                ).FirstOrDefaultAsync();

                if (articleAccountGroup == null)
                {
                    return StatusCode(404, "Account group not found.");
                }

                DBSets.MstTaxDBSet RRVAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == mstArticleItemDTO.RRVATId
                    select d
                ).FirstOrDefaultAsync();

                if (RRVAT == null)
                {
                    return StatusCode(404, "RR VAT not found.");
                }

                DBSets.MstTaxDBSet SIVAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == mstArticleItemDTO.SIVATId
                    select d
                ).FirstOrDefaultAsync();

                if (SIVAT == null)
                {
                    return StatusCode(404, "SI VAT not found.");
                }

                DBSets.MstCodeTableDBSet codeTableKitting = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == mstArticleItemDTO.Kitting
                    && d.Category == "ITEM KITTING"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableKitting == null)
                {
                    return StatusCode(404, "Kitting not found.");
                }

                DBSets.MstArticleItemDBSet saveArticleItem = articleItem;
                saveArticleItem.SKUCode = mstArticleItemDTO.SKUCode;
                saveArticleItem.BarCode = mstArticleItemDTO.BarCode;
                saveArticleItem.Description = mstArticleItemDTO.Description;
                saveArticleItem.UnitId = mstArticleItemDTO.UnitId;
                saveArticleItem.Category = mstArticleItemDTO.Category;
                saveArticleItem.IsInventory = mstArticleItemDTO.IsInventory;
                saveArticleItem.ArticleAccountGroupId = mstArticleItemDTO.ArticleAccountGroupId;
                saveArticleItem.AssetAccountId = mstArticleItemDTO.AssetAccountId;
                saveArticleItem.SalesAccountId = mstArticleItemDTO.SalesAccountId;
                saveArticleItem.CostAccountId = mstArticleItemDTO.CostAccountId;
                saveArticleItem.ExpenseAccountId = mstArticleItemDTO.ExpenseAccountId;
                saveArticleItem.Price = mstArticleItemDTO.Price;
                saveArticleItem.RRVATId = mstArticleItemDTO.RRVATId;
                saveArticleItem.SIVATId = mstArticleItemDTO.SIVATId;
                saveArticleItem.WTAXId = mstArticleItemDTO.WTAXId;
                saveArticleItem.Kitting = mstArticleItemDTO.Kitting;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleItem.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet saveArticle = article;
                saveArticle.ManualCode = mstArticleItemDTO.ArticleManualCode;
                saveArticle.Article = mstArticleItemDTO.Description;
                saveArticle.UpdatedByUserId = loginUserId;
                saveArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockArticleItem(Int32 id, [FromBody] DTO.MstArticleItemDTO mstArticleItemDTO)
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
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock an item.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock an item.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (articleItem.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock an item that is locked.");
                }

                DBSets.MstUnitDBSet unit = await (
                    from d in _dbContext.MstUnits
                    where d.Id == mstArticleItemDTO.UnitId
                    select d
                ).FirstOrDefaultAsync();

                if (unit == null)
                {
                    return StatusCode(404, "Unit not found.");
                }

                DBSets.MstArticleAccountGroupDBSet articleAccountGroup = await (
                    from d in _dbContext.MstArticleAccountGroups
                    where d.Id == mstArticleItemDTO.ArticleAccountGroupId
                    select d
                ).FirstOrDefaultAsync();

                if (articleAccountGroup == null)
                {
                    return StatusCode(404, "Account group not found.");
                }

                DBSets.MstTaxDBSet RRVAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == mstArticleItemDTO.RRVATId
                    select d
                ).FirstOrDefaultAsync();

                if (RRVAT == null)
                {
                    return StatusCode(404, "RR VAT not found.");
                }

                DBSets.MstTaxDBSet SIVAT = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == mstArticleItemDTO.SIVATId
                    select d
                ).FirstOrDefaultAsync();

                if (SIVAT == null)
                {
                    return StatusCode(404, "SI VAT not found.");
                }

                DBSets.MstCodeTableDBSet codeTableKitting = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == mstArticleItemDTO.Kitting
                    && d.Category == "ITEM KITTING"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableKitting == null)
                {
                    return StatusCode(404, "Kitting not found.");
                }

                DBSets.MstArticleItemDBSet lockArticleItem = articleItem;
                lockArticleItem.SKUCode = mstArticleItemDTO.SKUCode;
                lockArticleItem.BarCode = mstArticleItemDTO.BarCode;
                lockArticleItem.Description = mstArticleItemDTO.Description;
                lockArticleItem.UnitId = mstArticleItemDTO.UnitId;
                lockArticleItem.Category = mstArticleItemDTO.Category;
                lockArticleItem.IsInventory = mstArticleItemDTO.IsInventory;
                lockArticleItem.ArticleAccountGroupId = mstArticleItemDTO.ArticleAccountGroupId;
                lockArticleItem.AssetAccountId = mstArticleItemDTO.AssetAccountId;
                lockArticleItem.SalesAccountId = mstArticleItemDTO.SalesAccountId;
                lockArticleItem.CostAccountId = mstArticleItemDTO.CostAccountId;
                lockArticleItem.ExpenseAccountId = mstArticleItemDTO.ExpenseAccountId;
                lockArticleItem.Price = mstArticleItemDTO.Price;
                lockArticleItem.RRVATId = mstArticleItemDTO.RRVATId;
                lockArticleItem.SIVATId = mstArticleItemDTO.SIVATId;
                lockArticleItem.WTAXId = mstArticleItemDTO.WTAXId;
                lockArticleItem.Kitting = mstArticleItemDTO.Kitting;

                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleItem.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet lockArticle = article;
                lockArticle.ManualCode = mstArticleItemDTO.ArticleManualCode;
                lockArticle.Article = mstArticleItemDTO.Description;
                lockArticle.IsLocked = true;
                lockArticle.UpdatedByUserId = loginUserId;
                lockArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockArticleItem(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupItemDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock an item.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock an item.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (articleItem.MstArticle_ArticleId.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock an item that is unlocked.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleItem.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet unlockArticle = article;
                unlockArticle.IsLocked = false;
                unlockArticle.UpdatedByUserId = loginUserId;
                unlockArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteArticleItem(Int32 id)
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
                    && d.SysForm_FormId.Form == "SetupItemList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete an item.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete an item.");
                }

                DBSets.MstArticleItemDBSet articleItem = await (
                    from d in _dbContext.MstArticleItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (articleItem == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                if (articleItem.MstArticle_ArticleId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete an item that is locked.");
                }

                _dbContext.MstArticleItems.Remove(articleItem);
                await _dbContext.SaveChangesAsync();

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleItem.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                _dbContext.MstArticles.Remove(article);
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
