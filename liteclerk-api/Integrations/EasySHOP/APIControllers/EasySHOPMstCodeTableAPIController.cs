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
using liteclerk_api.Integrations.EasySHOP.DTO;

namespace liteclerk_api.Integrations.EasySHOP.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasySHOPMstCodeTableAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasySHOPMstCodeTableAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("list/{category}")]
        public async Task<ActionResult> GetCodeTableListByCategory(String category)
        {
            try
            {
                IEnumerable<EasySHOPMstCodeTableDTO> codeTables = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == category
                    select new EasySHOPMstCodeTableDTO
                    {
                        Id = d.Id,
                        Code = d.Code,
                        CodeValue = d.CodeValue,
                        Category = d.Category
                    }
                ).ToListAsync();

                return StatusCode(200, codeTables);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
