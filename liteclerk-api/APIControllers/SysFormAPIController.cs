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
    public class SysFormAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysFormAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetFormList()
        {
            try
            {
                IEnumerable<DTO.SysFormDTO> forms = await (
                    from d in _dbContext.SysForms
                    select new DTO.SysFormDTO
                    {
                        Id = d.Id,
                        Form = d.Form,
                        Description = d.Description
                    }
                ).ToListAsync();

                return StatusCode(200, forms);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
