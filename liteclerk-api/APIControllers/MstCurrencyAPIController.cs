using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MstCurrencyAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstCurrencyAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
