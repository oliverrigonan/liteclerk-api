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
    public class MstArticleOtherAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstArticleOtherAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetArticleOtherList()
        {
            try
            {
                var articleOthers = await (
                    from d in _dbContext.MstArticleOthers
                    select new DTO.MstArticleOtherDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL,
                            Particulars = d.MstArticle_ArticleId.Particulars
                        },
                        Other = d.Other
                    }
                ).ToListAsync();

                return StatusCode(200, articleOthers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetArticleOtherDetail(Int32 id)
        {
            try
            {
                var articleOther = await (
                    from d in _dbContext.MstArticleOthers
                    where d.Id == id
                    select new DTO.MstArticleOtherDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article,
                            ImageURL = d.MstArticle_ArticleId.ImageURL,
                            Particulars = d.MstArticle_ArticleId.Particulars
                        },
                        Other = d.Other
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, articleOther);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddArticleOther([FromBody] DTO.MstArticleOtherDTO mstArticleOtherDTO)
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
                    return StatusCode(404, "No rights to add an other article.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add an other article.");
                }

                String articleCode = "0000000001";
                var lastArticle = await (
                    from d in _dbContext.MstArticles
                    where d.ArticleTypeId == 5
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastArticle != null)
                {
                    Int32 lastArticleCode = Convert.ToInt32(lastArticle.ArticleCode) + 0000000001;
                    articleCode = PadZeroes(lastArticleCode, 10);
                }

                var newArticle = new DBSets.MstArticleDBSet()
                {
                    ArticleCode = articleCode,
                    ManualCode = mstArticleOtherDTO.ArticleManualCode,
                    ArticleTypeId = 5,
                    Article = mstArticleOtherDTO.Other,
                    ImageURL = "",
                    Particulars = mstArticleOtherDTO.ArticleParticulars,
                    IsLocked = true,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstArticles.Add(newArticle);
                await _dbContext.SaveChangesAsync();

                var newArticleOther = new DBSets.MstArticleOtherDBSet()
                {
                    ArticleId = newArticle.Id,
                    Other = mstArticleOtherDTO.Other
                };

                _dbContext.MstArticleOthers.Add(newArticleOther);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateArticleOther(int id, [FromBody] DTO.MstArticleOtherDTO mstArticleOtherDTO)
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
                    return StatusCode(404, "No rights to edit or update an other article.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update an other article.");
                }

                var articleOther = await (
                    from d in _dbContext.MstArticleOthers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (articleOther == null)
                {
                    return StatusCode(404, "Other article not found.");
                }

                var updateArticleOther = articleOther;
                updateArticleOther.Other = mstArticleOtherDTO.Other;

                await _dbContext.SaveChangesAsync();

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == mstArticleOtherDTO.ArticleId
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                DBSets.MstArticleDBSet updateArticle = article;
                updateArticle.ManualCode = mstArticleOtherDTO.ArticleManualCode;
                updateArticle.Article = mstArticleOtherDTO.Other;
                updateArticle.Particulars = mstArticleOtherDTO.ArticleParticulars;
                updateArticle.UpdatedByUserId = loginUserId;
                updateArticle.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteArticleOther(int id)
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
                    return StatusCode(404, "No rights to delete a other article.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a other article.");
                }

                var articleOther = await (
                    from d in _dbContext.MstArticleOthers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (articleOther == null)
                {
                    return StatusCode(404, "Other article not found.");
                }

                _dbContext.MstArticleOthers.Remove(articleOther);
                await _dbContext.SaveChangesAsync();

                var article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == articleOther.ArticleId
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
