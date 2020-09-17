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
    public class MstArticleAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetArticleList()
        {
            try
            {
                IEnumerable<DTO.MstArticleDTO> articles = await (
                    from d in _dbContext.MstArticles
                    select new DTO.MstArticleDTO
                    {
                        Id = d.Id,
                        ArticleCode = d.ArticleCode,
                        ManualCode = d.ManualCode,
                        Article = d.Article,
                        ArticleTypeId = d.ArticleTypeId,
                        ArticleType = new DTO.MstArticleTypeDTO
                        {
                            ArticleType = d.MstArticleType_ArticleTypeId.ArticleType
                        },
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, articles);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedArticleList()
        {
            try
            {
                IEnumerable<DTO.MstArticleDTO> lockedArticles = await (
                    from d in _dbContext.MstArticles
                    where d.IsLocked == true
                    select new DTO.MstArticleDTO
                    {
                        Id = d.Id,
                        ArticleCode = d.ArticleCode,
                        ManualCode = d.ManualCode,
                        Article = d.Article,
                        ArticleTypeId = d.ArticleTypeId,
                        ArticleType = new DTO.MstArticleTypeDTO
                        {
                            ArticleType = d.MstArticleType_ArticleTypeId.ArticleType
                        },
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, lockedArticles);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byAccountArticleType/{accountId}")]
        public async Task<ActionResult> GetArticleListByAccountArticleType(Int32 accountId)
        {
            try
            {
                List<DTO.MstArticleDTO> newArticles = new List<DTO.MstArticleDTO>();

                IEnumerable<Int32> articleTypeIds = await (
                    from d in _dbContext.MstAccountArticleTypes
                    where d.AccountId == accountId
                    group d by d.ArticleTypeId into g
                    select g.Key
                ).ToListAsync();

                if (articleTypeIds.Any())
                {
                    foreach (var articleTypeId in articleTypeIds)
                    {
                        IEnumerable<DTO.MstArticleDTO> articles = await (
                            from d in _dbContext.MstArticles
                            where d.IsLocked == true
                            && d.ArticleTypeId == articleTypeId
                            select new DTO.MstArticleDTO
                            {
                                Id = d.Id,
                                ArticleCode = d.ArticleCode,
                                ManualCode = d.ManualCode,
                                Article = d.Article,
                                ArticleTypeId = d.ArticleTypeId,
                                ArticleType = new DTO.MstArticleTypeDTO
                                {
                                    ArticleType = d.MstArticleType_ArticleTypeId.ArticleType
                                },
                                IsLocked = d.IsLocked,
                                CreatedByUser = new DTO.MstUserDTO
                                {
                                    Username = d.MstUser_CreatedByUserId.Username,
                                    Fullname = d.MstUser_CreatedByUserId.Fullname
                                },
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUser = new DTO.MstUserDTO
                                {
                                    Username = d.MstUser_UpdatedByUserId.Username,
                                    Fullname = d.MstUser_UpdatedByUserId.Fullname
                                },
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            }
                        ).ToListAsync();

                        newArticles.AddRange(articles);
                    }
                }

                IEnumerable<DTO.MstArticleDTO> articleByAccountArticleTypes = await Task.FromResult(newArticles);

                return StatusCode(200, articleByAccountArticleTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
