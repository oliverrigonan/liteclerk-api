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
    public class MstUnitAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstUnitAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstUnitDTO>>> GetUnitList()
        {
            try
            {
                IEnumerable<DTO.MstUnitDTO> units = await (
                    from d in _dbContext.MstUnits
                    select new DTO.MstUnitDTO
                    {
                        Id = d.Id,
                        UnitCode = d.UnitCode,
                        ManualCode = d.ManualCode,
                        Unit = d.Unit,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, units);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
