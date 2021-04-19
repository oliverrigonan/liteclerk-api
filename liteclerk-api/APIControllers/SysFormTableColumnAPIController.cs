using System;
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
    public class SysFormTableColumnAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysFormTableColumnAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetFormTableList()
        {
            try
            {
                var formTables = await (
                    from d in _dbContext.SysFormTableColumns
                    select new DTO.SysFormTableColumnDTO
                    {
                        Id = d.Id,
                        TableId = d.TableId,
                        Table = new DTO.SysFormTableDTO
                        {
                            FormId = d.SysFormTable_TableId.FormId,
                            Form = new DTO.SysFormDTO
                            {
                                Id = d.SysFormTable_TableId.SysForm_FormId.Id,
                                Form = d.SysFormTable_TableId.SysForm_FormId.Form
                            },
                            Table = d.SysFormTable_TableId.Table
                        },
                        Column = d.Column,
                        IsDisplayed = d.IsDisplayed
                    }
                ).ToListAsync();

                return StatusCode(200, formTables);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byTable/{tableId}")]
        public async Task<ActionResult> GetFormTableListByTable(Int32 tableId)
        {
            try
            {
                var formTables = await (
                    from d in _dbContext.SysFormTableColumns
                    where d.TableId == tableId
                    select new DTO.SysFormTableColumnDTO
                    {
                        Id = d.Id,
                        TableId = d.TableId,
                        Table = new DTO.SysFormTableDTO
                        {
                            FormId = d.SysFormTable_TableId.FormId,
                            Form = new DTO.SysFormDTO
                            {
                                Id = d.SysFormTable_TableId.SysForm_FormId.Id,
                                Form = d.SysFormTable_TableId.SysForm_FormId.Form
                            },
                            Table = d.SysFormTable_TableId.Table
                        },
                        Column = d.Column,
                        IsDisplayed = d.IsDisplayed
                    }
                ).ToListAsync();

                return StatusCode(200, formTables);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byForm/{formId}/byTable/{tableName}")]
        public async Task<ActionResult> GetFormTableListByFormByTable(Int32 formId, String tableName)
        {
            try
            {
                var formTables = await (
                    from d in _dbContext.SysFormTableColumns
                    where d.SysFormTable_TableId.FormId == formId
                    && d.SysFormTable_TableId.Table == tableName
                    select new DTO.SysFormTableColumnDTO
                    {
                        Id = d.Id,
                        TableId = d.TableId,
                        Table = new DTO.SysFormTableDTO
                        {
                            FormId = d.SysFormTable_TableId.FormId,
                            Form = new DTO.SysFormDTO
                            {
                                Id = d.SysFormTable_TableId.SysForm_FormId.Id,
                                Form = d.SysFormTable_TableId.SysForm_FormId.Form
                            },
                            Table = d.SysFormTable_TableId.Table
                        },
                        Column = d.Column,
                        IsDisplayed = d.IsDisplayed
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
                    from d in _dbContext.SysFormTableColumns
                    where d.Id == id
                    select new DTO.SysFormTableColumnDTO
                    {
                        Id = d.Id,
                        TableId = d.TableId,
                        Table = new DTO.SysFormTableDTO
                        {
                            FormId = d.SysFormTable_TableId.FormId,
                            Form = new DTO.SysFormDTO
                            {
                                Id = d.SysFormTable_TableId.SysForm_FormId.Id,
                                Form = d.SysFormTable_TableId.SysForm_FormId.Form
                            },
                            Table = d.SysFormTable_TableId.Table
                        },
                        Column = d.Column,
                        IsDisplayed = d.IsDisplayed
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
        public async Task<ActionResult> AddFormTable([FromBody] DTO.SysFormTableColumnDTO sysFormTableDTO)
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
                    return StatusCode(404, "No rights to add a form table column.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a form table column.");
                }

                var formTable = await (
                    from d in _dbContext.SysFormTables
                    where d.Id == sysFormTableDTO.TableId
                    select d
                ).FirstOrDefaultAsync();

                if (formTable == null)
                {
                    return StatusCode(404, "Form table not found.");
                }

                var newFormTable = new DBSets.SysFormTableColumnDBSet()
                {
                    TableId = sysFormTableDTO.TableId,
                    Column = sysFormTableDTO.Column,
                    IsDisplayed = sysFormTableDTO.IsDisplayed,
                };

                _dbContext.SysFormTableColumns.Add(newFormTable);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateFormTable(int id, [FromBody] DTO.SysFormTableColumnDTO sysFormTableDTO)
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
                    return StatusCode(404, "No rights to edit or update a form table column.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a form table column.");
                }

                var formTableColumn = await (
                    from d in _dbContext.SysFormTableColumns
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (formTableColumn == null)
                {
                    return StatusCode(404, "Form table column not found.");
                }

                var formTable = await (
                    from d in _dbContext.SysFormTables
                    where d.Id == sysFormTableDTO.TableId
                    select d
                ).FirstOrDefaultAsync();

                if (formTable == null)
                {
                    return StatusCode(404, "Form table not found.");
                }

                var updateFormTableColumn = formTableColumn;
                updateFormTableColumn.TableId = sysFormTableDTO.TableId;
                updateFormTableColumn.Column = sysFormTableDTO.Column;
                updateFormTableColumn.IsDisplayed = sysFormTableDTO.IsDisplayed;

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
                    return StatusCode(404, "No rights to delete a form table column.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a form table column.");
                }

                var formTableColumn = await (
                    from d in _dbContext.SysFormTableColumns
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (formTableColumn == null)
                {
                    return StatusCode(404, "Form table column not found.");
                }

                _dbContext.SysFormTableColumns.Remove(formTableColumn);
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
