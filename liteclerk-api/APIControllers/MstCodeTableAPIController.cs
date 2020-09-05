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
    public class MstCodeTableAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstCodeTableAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("listByCategory/{category}")]
        public async Task<ActionResult> GetCodeTableListByCategory(String category)
        {
            try
            {
                IEnumerable<DTO.MstCodeTableDTO> units = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == category
                    select new DTO.MstCodeTableDTO
                    {
                        Id = d.Id,
                        Code = d.Code,
                        CodeValue = d.CodeValue,
                        Category = d.Category
                    }
                ).ToListAsync();

                return StatusCode(200, units);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
