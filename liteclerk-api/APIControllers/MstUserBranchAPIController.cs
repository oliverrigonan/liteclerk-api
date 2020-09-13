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
    public class MstUserBranchAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstUserBranchAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{userId}")]
        public async Task<ActionResult> GetUserBranchList(Int32 userId)
        {
            try
            {
                IEnumerable<DTO.MstUserBranchDTO> userBranches = await (
                    from d in _dbContext.MstUserBranches
                    where d.UserId == userId
                    select new DTO.MstUserBranchDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, userBranches);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byLoginUser")]
        public async Task<ActionResult> GetUserBranchListByLoginUser()
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.MstUserBranchDTO> userBranches = await (
                    from d in _dbContext.MstUserBranches
                    where d.UserId == loginUserId
                    select new DTO.MstUserBranchDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, userBranches);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetUserBranchDetail(Int32 id)
        {
            try
            {
                DTO.MstUserBranchDTO userBranch = await (
                    from d in _dbContext.MstUserBranches
                    where d.Id == id
                    select new DTO.MstUserBranchDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        User = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UserId.Username,
                            Fullname = d.MstUser_UserId.Fullname
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        }
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, userBranch);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddUserBranch([FromBody] DTO.MstUserBranchDTO mstUserBranchDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a user branch.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a user branch.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == mstUserBranchDTO.UserId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add a user branch if the current user is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == mstUserBranchDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserBranchDBSet newUserBranch = new DBSets.MstUserBranchDBSet()
                {
                    UserId = mstUserBranchDTO.UserId,
                    BranchId = mstUserBranchDTO.BranchId
                };

                _dbContext.MstUserBranches.Add(newUserBranch);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUserBranch(int id, [FromBody] DTO.MstUserBranchDTO mstUserBranchDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a user branch.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a user branch.");
                }

                DBSets.MstUserBranchDBSet userBranch = await (
                    from d in _dbContext.MstUserBranches
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userBranch == null)
                {
                    return StatusCode(404, "User branch not found.");
                }

                if (userBranch.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update a user branch if the current user is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == mstUserBranchDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserBranchDBSet updateUserBranch = userBranch;
                updateUserBranch.BranchId = mstUserBranchDTO.BranchId;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUserBranch(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a user branch.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a user branch.");
                }

                DBSets.MstUserBranchDBSet userBranch = await (
                    from d in _dbContext.MstUserBranches
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (userBranch == null)
                {
                    return StatusCode(404, "User branch not found.");
                }

                if (userBranch.MstUser_UserId.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a user branch if the current user is locked.");
                }

                _dbContext.MstUserBranches.Remove(userBranch);
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
