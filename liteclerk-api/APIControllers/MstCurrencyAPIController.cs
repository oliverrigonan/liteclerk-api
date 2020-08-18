using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MstCurrencyAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstCurrencyAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstCurrencyDTO>>> GetCurrencyList()
        {
            try
            {
                IEnumerable<DTO.MstCurrencyDTO> currencies = await (
                    from d in _dbContext.MstCurrencies
                    select new DTO.MstCurrencyDTO
                    {
                        Id = d.Id,
                        CurrencyCode = d.CurrencyCode,
                        ManualCode = d.ManualCode,
                        Currency = d.Currency,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, currencies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
