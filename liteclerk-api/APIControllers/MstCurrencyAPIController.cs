﻿using System;
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
                        CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                        CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                        UpdatedByUserFullname = d.MstUser_UpdatedByUser.Fullname,
                        UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
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
