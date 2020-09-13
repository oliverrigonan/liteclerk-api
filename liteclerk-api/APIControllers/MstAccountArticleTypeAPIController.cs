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
                    group d by new
                    {
                        d.ArticleTypeId,
                        d.MstArticleType_ArticleTypeId
                    } into g
                    select new DTO.MstAccountArticleTypeDTO
                    {
                        ArticleTypeId = g.Key.ArticleTypeId,
                        ArticleType = new DTO.MstArticleTypeDTO
                        {
                            ArticleType = g.Key.MstArticleType_ArticleTypeId.ArticleType
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
    }
}
