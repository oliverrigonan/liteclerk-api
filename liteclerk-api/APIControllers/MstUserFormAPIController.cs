﻿using System;
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

        [HttpGet("list/{userId}")]
        public async Task<ActionResult> GetUserFormList(Int32 userId)
        {
            try
            {
                var userForms = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
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
                ).ToListAsync();

                return StatusCode(200, userForms);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetUserFormDetail(Int32 id)
        {
            try
            {
                var userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == id
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

        [HttpGet("detail/byLoginUser/byForm/{form}")]
        public async Task<ActionResult> GetUserFormDetailByLoginUserForm(String form)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
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

        [HttpPost("add")]
        public async Task<ActionResult> AddUserForm([FromBody] DTO.MstUserFormDTO mstUserFormDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a user form.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a user form.");
                }

                var user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == mstUserFormDTO.UserId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a user form if the current user is locked.");
                }

                var form = await (
                    from d in _dbContext.SysForms
                    where d.Id == mstUserFormDTO.FormId
                    select d
                ).FirstOrDefaultAsync();

                if (form == null)
                {
                    return StatusCode(404, "Form not found.");
                }

                var newUserForm = new DBSets.MstUserFormDBSet()
                {
                    UserId = mstUserFormDTO.UserId,
                    FormId = mstUserFormDTO.FormId,
                    CanAdd = mstUserFormDTO.CanAdd,
                    CanEdit = mstUserFormDTO.CanEdit,
                    CanDelete = mstUserFormDTO.CanDelete,
                    CanLock = mstUserFormDTO.CanLock,
                    CanUnlock = mstUserFormDTO.CanUnlock,
                    CanCancel = mstUserFormDTO.CanCancel,
                    CanPrint = mstUserFormDTO.CanPrint
                };

                _dbContext.MstUserForms.Add(newUserForm);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUserForm(int id, [FromBody] DTO.MstUserFormDTO mstUserFormDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a user form.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a user form.");
                }

                var userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "User form not found.");
                }

                if (userForm.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a user form if the current user is locked.");
                }

                var form = await (
                    from d in _dbContext.SysForms
                    where d.Id == mstUserFormDTO.FormId
                    select d
                ).FirstOrDefaultAsync();

                if (form == null)
                {
                    return StatusCode(404, "Form not found.");
                }

                var updateUserForm = userForm;
                updateUserForm.FormId = mstUserFormDTO.FormId;
                updateUserForm.CanAdd = mstUserFormDTO.CanAdd;
                updateUserForm.CanEdit = mstUserFormDTO.CanEdit;
                updateUserForm.CanDelete = mstUserFormDTO.CanDelete;
                updateUserForm.CanLock = mstUserFormDTO.CanLock;
                updateUserForm.CanUnlock = mstUserFormDTO.CanUnlock;
                updateUserForm.CanCancel = mstUserFormDTO.CanCancel;
                updateUserForm.CanPrint = mstUserFormDTO.CanPrint;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUserForm(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a user form.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a user form.");
                }

                var userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "User form not found.");
                }

                if (userForm.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a user form if the current user is locked.");
                }

                _dbContext.MstUserForms.Remove(userForm);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("function/copyRights/{username}")]
        public async Task<ActionResult> FunctionCopyRightsUserForm(String username, [FromBody] DTO.MstUserFormDTO mstUserFormDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a user form.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a user form.");
                }

                var user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == mstUserFormDTO.UserId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a user form if the current user is locked.");
                }

                var selectedUser = await (
                    from d in _dbContext.MstUsers
                    where d.Username == username
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (selectedUser == null)
                {
                    return StatusCode(404, "Selected user " + username + " was not found.");
                }

                var userForms = await (
                    from d in _dbContext.MstUserForms
                    where d.MstUser_UserId.Username == username
                    && d.MstUser_UserId.IsLocked == true
                    select d
                ).ToListAsync();

                if (userForms.Any() == true)
                {
                    var currentUserForms = await (
                        from d in _dbContext.MstUserForms
                        where d.UserId == mstUserFormDTO.UserId
                        select d
                    ).ToListAsync();

                    if (currentUserForms.Any() == true)
                    {
                        _dbContext.MstUserForms.RemoveRange(currentUserForms);
                        await _dbContext.SaveChangesAsync();
                    }

                    List<DBSets.MstUserFormDBSet> newUserForms = new List<DBSets.MstUserFormDBSet>();

                    foreach (var userForm in userForms)
                    {
                        newUserForms.Add(new DBSets.MstUserFormDBSet()
                        {
                            UserId = mstUserFormDTO.UserId,
                            FormId = userForm.FormId,
                            CanAdd = userForm.CanAdd,
                            CanEdit = userForm.CanEdit,
                            CanDelete = userForm.CanDelete,
                            CanLock = userForm.CanLock,
                            CanUnlock = userForm.CanUnlock,
                            CanCancel = userForm.CanCancel,
                            CanPrint = userForm.CanPrint
                        });

                        _dbContext.MstUserForms.AddRange(newUserForms);
                    }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return StatusCode(404, "User " + username + " has empty forms.");
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
