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
    public class MstPayTypeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstPayTypeAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetPayTypeList()
        {
            try
            {
                var payTypes = await (
                    from d in _dbContext.MstPayTypes
                    select new DTO.MstPayTypeDTO
                    {
                        Id = d.Id,
                        PayTypeCode = d.PayTypeCode,
                        ManualCode = d.ManualCode,
                        PayType = d.PayType,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
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

                return StatusCode(200, payTypes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPayTypeDetail(Int32 id)
        {
            try
            {
                var payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == id
                    select new DTO.MstPayTypeDTO
                    {
                        Id = d.Id,
                        PayTypeCode = d.PayTypeCode,
                        ManualCode = d.ManualCode,
                        PayType = d.PayType,
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
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
                ).FirstOrDefaultAsync();

                return StatusCode(200, payType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPayType([FromBody] DTO.MstPayTypeDTO mstPayTypeDTO)
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
                    return StatusCode(404, "No rights to add a pay type.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a pay type.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstPayTypeDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                String payTypeCode = "0000000001";
                var lastPayType = await (
                    from d in _dbContext.MstPayTypes
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastPayType != null)
                {
                    Int32 lastPayTypeCode = Convert.ToInt32(lastPayType.PayTypeCode) + 0000000001;
                    payTypeCode = PadZeroes(lastPayTypeCode, 10);
                }

                var newPayType = new DBSets.MstPayTypeDBSet()
                {
                    PayTypeCode = payTypeCode,
                    ManualCode = mstPayTypeDTO.ManualCode,
                    PayType = mstPayTypeDTO.PayType,
                    AccountId = mstPayTypeDTO.AccountId,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstPayTypes.Add(newPayType);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdatePayType(int id, [FromBody] DTO.MstPayTypeDTO mstPayTypeDTO)
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
                    return StatusCode(404, "No rights to edit or update a pay type.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a pay type.");
                }

                var payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                var account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == mstPayTypeDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                var updatePayType = payType;
                updatePayType.ManualCode = mstPayTypeDTO.ManualCode;
                updatePayType.PayType = mstPayTypeDTO.PayType;
                updatePayType.AccountId = mstPayTypeDTO.AccountId;
                updatePayType.UpdatedByUserId = loginUserId;
                updatePayType.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePayType(int id)
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
                    return StatusCode(404, "No rights to delete a pay type.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a pay type.");
                }

                var payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                _dbContext.MstPayTypes.Remove(payType);
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
