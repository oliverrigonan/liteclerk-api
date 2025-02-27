﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class SysFormTableAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysFormTableAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetFormTableList()
        {
            try
            {
                var formTables = await (
                    from d in _dbContext.SysFormTables
                    select new DTO.SysFormTableDTO
                    {
                        Id = d.Id,
                        FormId = d.FormId,
                        Form = new DTO.SysFormDTO
                        {
                            Form = d.SysForm_FormId.Form,
                            Description = d.SysForm_FormId.Description
                        },
                        Table = d.Table
                    }
                ).ToListAsync();

                return StatusCode(200, formTables);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byForm/{formId}")]
        public async Task<ActionResult> GetFormTableListByForm(Int32 formId)
        {
            try
            {
                var formTables = await (
                    from d in _dbContext.SysFormTables
                    where d.FormId == formId
                    select new DTO.SysFormTableDTO
                    {
                        Id = d.Id,
                        FormId = d.FormId,
                        Form = new DTO.SysFormDTO
                        {
                            Form = d.SysForm_FormId.Form,
                            Description = d.SysForm_FormId.Description
                        },
                        Table = d.Table
                    }
                ).ToListAsync();

                return StatusCode(200, formTables);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetFormTableDetail(Int32 id)
        {
            try
            {
                var formTable = await (
                    from d in _dbContext.SysFormTables
                    where d.Id == id
                    select new DTO.SysFormTableDTO
                    {
                        Id = d.Id,
                        FormId = d.FormId,
                        Form = new DTO.SysFormDTO
                        {
                            Form = d.SysForm_FormId.Form,
                            Description = d.SysForm_FormId.Description
                        },
                        Table = d.Table
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, formTable);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddFormTable([FromBody] DTO.SysFormTableDTO sysFormTableDTO)
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
                    && d.SysForm_FormId.Form == "SystemSettings"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a form table.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a form table.");
                }

                var form = await (
                    from d in _dbContext.SysForms
                    where d.Id == sysFormTableDTO.FormId
                    select d
                ).FirstOrDefaultAsync();

                if (form == null)
                {
                    return StatusCode(404, "Form not found.");
                }

                var newFormTable = new DBSets.SysFormTableDBSet()
                {
                    FormId = sysFormTableDTO.FormId,
                    Table = sysFormTableDTO.Table
                };

                _dbContext.SysFormTables.Add(newFormTable);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateFormTable(int id, [FromBody] DTO.SysFormTableDTO sysFormTableDTO)
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
                    && d.SysForm_FormId.Form == "SystemSettings"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a form table.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a form table.");
                }

                var formTable = await (
                    from d in _dbContext.SysFormTables
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (formTable == null)
                {
                    return StatusCode(404, "Form table not found.");
                }

                var form = await (
                    from d in _dbContext.SysForms
                    where d.Id == sysFormTableDTO.FormId
                    select d
                ).FirstOrDefaultAsync();

                if (form == null)
                {
                    return StatusCode(404, "Form not found.");
                }

                var updateFormTable = formTable;
                updateFormTable.FormId = sysFormTableDTO.FormId;
                updateFormTable.Table = sysFormTableDTO.Table;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteFormTable(int id)
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
                    && d.SysForm_FormId.Form == "SystemSettings"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a form table.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a form table.");
                }

                var formTable = await (
                    from d in _dbContext.SysFormTables
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (formTable == null)
                {
                    return StatusCode(404, "Form table not found.");
                }

                _dbContext.SysFormTables.Remove(formTable);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
