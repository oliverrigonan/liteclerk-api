using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace liteclerk_api.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIMstCompanyController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public APIMstCompanyController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<APIMstCompanyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<APIMstCompanyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<APIMstCompanyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<APIMstCompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<APIMstCompanyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
