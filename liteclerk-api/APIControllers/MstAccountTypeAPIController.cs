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
    public class MstAccountTypeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstAccountTypeAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetAccountTypeList()
        {
            try
            {
                IEnumerable<DTO.MstAccountTypeDTO> accounts = await (
                    from d in _dbContext.MstAccountTypes
                    select new DTO.MstAccountTypeDTO
                    {
                        Id = d.Id,
                        AccountTypeCode = d.AccountTypeCode,
                        ManualCode = d.ManualCode,
                        AccountType = d.AccountType,
                        AccountCategoryId = d.AccountCategoryId,
                        AccountCategory = new DTO.MstAccountCategoryDTO
                        {
                            AccountCategoryCode = d.MstAccountCategory_AccountCategoryId.AccountCategoryCode,
                            ManualCode = d.MstAccountCategory_AccountCategoryId.ManualCode,
                            AccountCategory = d.MstAccountCategory_AccountCategoryId.AccountCategory
                        },
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, accounts);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
