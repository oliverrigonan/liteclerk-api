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
    public class MstArticleItemInventoryAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstArticleItemInventoryAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleItemInventoryDTO>>> GetArticleItemInventoryList()
        {
            try
            {
                IEnumerable<DTO.MstArticleItemInventoryDTO> articleItemInventories = await (
                    from d in _dbContext.MstArticleItemInventories
                    select new DTO.MstArticleItemInventoryDTO
                    {
                        Id = d.Id,
                        ArticleId = d.ArticleId,
                        Article = d.MstArticle_Article.Article,
                        BranchId = d.BranchId,
                        Branch = d.MstCompanyBranch_Branch.Branch,
                        InventoryCode = d.InventoryCode,
                        Quantity = d.Quantity,
                        Cost = d.Cost
                    }
                ).ToListAsync();

                return StatusCode(200, articleItemInventories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
