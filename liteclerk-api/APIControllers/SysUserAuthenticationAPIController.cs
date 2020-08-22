using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using liteclerk_api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserAuthenticationAPIController : ControllerBase
    {
        private Modules.ISysUserAuthenticationModule _userAuthentication;

        public SysUserAuthenticationAPIController(Modules.ISysUserAuthenticationModule iSysUserAuthenticationModule)
        {
            _userAuthentication = iSysUserAuthenticationModule;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] SysUserAuthenticationRequestDTO sysUserAuthenticationRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _userAuthentication.Authenticate(sysUserAuthenticationRequestDTO);
                if (response == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
