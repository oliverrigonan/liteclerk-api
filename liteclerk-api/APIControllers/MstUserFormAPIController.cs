using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class MstUserFormAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstUserFormAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("detail/byCurrentUser/byForm/{form}")]
        public async Task<ActionResult> GetUserFormDetailByCurrentUserForm(String form)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                DTO.MstUserFormDTO userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == form
                    select new DTO.MstUserFormDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        FormId = d.FormId,
                        Form = new DTO.SysFormDTO
                        {
                            Form = d.SysForm_FormId.Form,
                            Description = d.SysForm_FormId.Description
                        },
                        CanAdd = d.CanAdd,
                        CanEdit = d.CanEdit,
                        CanDelete = d.CanDelete,
                        CanLock = d.CanLock,
                        CanUnlock = d.CanUnlock,
                        CanCancel = d.CanCancel,
                        CanPrint = d.CanPrint
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, userForm);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
