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
    public class MstTaxAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstTaxAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstTaxDTO>>> GetTaxList()
        {
            try
            {
                IEnumerable<DTO.MstTaxDTO> taxes = await (
                    from d in _dbContext.MstTaxes
                    select new DTO.MstTaxDTO
                    {
                        Id = d.Id,
                        TaxCode = d.TaxCode,
                        ManualCode = d.ManualCode,
                        TaxDescription = d.TaxDescription,
                        TaxRate = d.TaxRate,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, taxes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
