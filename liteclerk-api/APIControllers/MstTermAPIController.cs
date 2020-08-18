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
    public class MstTermAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstTermAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstTermDTO>>> GetTermList()
        {
            try
            {
                IEnumerable<DTO.MstTermDTO> terms = await (
                    from d in _dbContext.MstTerms
                    select new DTO.MstTermDTO
                    {
                        Id = d.Id,
                        TermCode = d.Term,
                        ManualCode = d.ManualCode,
                        Term = d.Term,
                        NumberOfDays = d.NumberOfDays,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, terms);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
