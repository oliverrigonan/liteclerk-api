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
    public class MstCodeTableAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstCodeTableAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/category")]
        public async Task<ActionResult> GetCodeTableListCategory()
        {
            try
            {
                var categories = new List<String>()
                {
                    "ITEM KITTING",
                    "PRODUCTION ITEM INFORMATION",
                    "PRODUCTION ITEM ATTACHMENT",
                    "PRODUCTION DEPARTMENT STATUS",
                    "JOB ORDER STATUS",
                    "PURCHASE REQUEST STATUS",
                    "PURCHASE ORDER STATUS",
                    "RECEIVING RECEIPT STATUS",
                    "DISBURSEMENT STATUS",
                    "PAYABLE MEMO STATUS",
                    "SALES ORDER STATUS",
                    "SALES INVOICE STATUS",
                    "COLLECTION INVOICE STATUS",
                    "RECEIVABLE MEMO STATUS",
                    "STOCK IN STATUS",
                    "STOCK OUT STATUS",
                    "STOCK TRANSFER STATUS",
                    "STOCK WITHDRAWAL STATUS",
                    "STOCK COUNT STATUS",
                    "INVENTORY STATUS",
                    "JOURNAL VOUCHER STATUS",
                    "POS TERMINAL"
                };

                var listCategories = await Task.FromResult(categories);

                return StatusCode(200, categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/{category}")]
        public async Task<ActionResult> GetCodeTableListByCategory(String category)
        {
            try
            {
                var units = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == category
                    select new DTO.MstCodeTableDTO
                    {
                        Id = d.Id,
                        Code = d.Code,
                        CodeValue = d.CodeValue,
                        Category = d.Category
                    }
                ).ToListAsync();

                return StatusCode(200, units);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetCodeTableDetail(Int32 id)
        {
            try
            {
                var producedCodeTable = await (
                    from d in _dbContext.MstCodeTables
                    where d.Id == id
                    select new DTO.MstCodeTableDTO
                    {
                        Id = d.Id,
                        Code = d.Code,
                        CodeValue = d.CodeValue,
                        Category = d.Category
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, producedCodeTable);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCodeTable([FromBody] DTO.MstCodeTableDTO mstCodeTableDTO)
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
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a code table.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a code table.");
                }

                var newCodeTable = new DBSets.MstCodeTableDBSet()
                {
                    Code = mstCodeTableDTO.Code,
                    CodeValue = mstCodeTableDTO.CodeValue,
                    Category = mstCodeTableDTO.Category
                };

                _dbContext.MstCodeTables.Add(newCodeTable);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newCodeTable.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCodeTable(Int32 id, [FromBody] DTO.MstCodeTableDTO mstCodeTableDTO)
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
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a code table.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a code table.");
                }

                var codeTable = await (
                    from d in _dbContext.MstCodeTables
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (codeTable == null)
                {
                    return StatusCode(404, "Code table not found.");
                }

                var updateCodeTable = codeTable;
                updateCodeTable.Code = mstCodeTableDTO.Code;
                updateCodeTable.CodeValue = mstCodeTableDTO.CodeValue;
                updateCodeTable.Category = mstCodeTableDTO.Category;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCodeTable(Int32 id)
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
                    && d.SysForm_FormId.Form == "SystemSytemTables"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a code table.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a code table.");
                }

                var codeTable = await (
                    from d in _dbContext.MstCodeTables
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (codeTable == null)
                {
                    return StatusCode(404, "Code table not found.");
                }

                _dbContext.MstCodeTables.Remove(codeTable);
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
