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
    public class MstDiscountAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstDiscountAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstDiscountDTO>>> GetDiscountList()
        {
            try
            {
                IEnumerable<DTO.MstDiscountDTO> discounts = await (
                    from d in _dbContext.MstDiscounts
                    select new DTO.MstDiscountDTO
                    {
                        Id = d.Id,
                        DiscountCode = d.DiscountCode,
                        ManualCode = d.ManualCode,
                        Discount = d.Discount,
                        DiscountRate = d.DiscountRate,
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                    }
                ).ToListAsync();

                return StatusCode(200, discounts);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
