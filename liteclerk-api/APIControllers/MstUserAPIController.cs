using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class MstUserAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public MstUserAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetUserList()
        {
            try
            {
                IEnumerable<DTO.MstUserDTO> users = await (
                    from d in _dbContext.MstUsers
                    select new DTO.MstUserDTO
                    {
                        Id = d.Id,
                        Username = d.Username,
                        Password = d.Password,
                        Fullname = d.Fullname,
                        CompanyId = d.CompanyId,
                        Company = new DTO.MstCompanyDTO
                        {
                            CompanyCode = d.MstCompany_CompanyId.CompanyCode,
                            ManualCode = d.MstCompany_CompanyId.ManualCode,
                            Company = d.MstCompany_CompanyId.Company
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        IsActive = d.IsActive,
                        IsLocked = d.IsLocked
                    }
                ).ToListAsync();

                return StatusCode(200, users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/active")]
        public async Task<ActionResult> GetActiveUserList()
        {
            try
            {
                IEnumerable<DTO.MstUserDTO> activeUsers = await (
                    from d in _dbContext.MstUsers
                    where d.IsActive == true
                    select new DTO.MstUserDTO
                    {
                        Id = d.Id,
                        Username = d.Username,
                        Fullname = d.Fullname,
                        CompanyId = d.CompanyId,
                        Company = new DTO.MstCompanyDTO
                        {
                            CompanyCode = d.MstCompany_CompanyId.CompanyCode,
                            ManualCode = d.MstCompany_CompanyId.ManualCode,
                            Company = d.MstCompany_CompanyId.Company,
                            ImageURL = d.MstCompany_CompanyId.ImageURL
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        IsActive = d.IsActive
                    }
                ).ToListAsync();

                return StatusCode(200, activeUsers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/active/byJobDepartment/{jobDepartmentId}/")]
        public async Task<ActionResult> GetActiveUserListByJobOrderDepartment(Int32 jobDepartmentId)
        {
            try
            {
                List<DTO.MstUserDTO> listUsers = new List<DTO.MstUserDTO>();

                var activeUsers = await (
                    from d in _dbContext.MstUsers
                    where d.IsActive == true
                    select d
                ).ToListAsync();

                if (activeUsers.Any())
                {
                    foreach (var activeUser in activeUsers)
                    {
                        Int32 numberOfJobDepartmentCount = 0;

                        var jobOrderDepartments = await (
                                from d in _dbContext.TrnJobOrderDepartments
                                where d.JobDepartmentId == jobDepartmentId
                                && d.AssignedToUserId == activeUser.Id
                                && d.Status != "DONE"
                                && d.TrnJobOrder_JOId.IsLocked == true
                                select d
                        ).ToListAsync();

                        if (jobOrderDepartments.Any())
                        {
                            numberOfJobDepartmentCount = jobOrderDepartments.Count();
                        }

                        listUsers.Add(new DTO.MstUserDTO()
                        {
                            Id = activeUser.Id,
                            Username = activeUser.Username,
                            Fullname = activeUser.Fullname,
                            CompanyId = activeUser.CompanyId,
                            Company = new DTO.MstCompanyDTO
                            {
                                CompanyCode = activeUser.MstCompany_CompanyId.CompanyCode,
                                ManualCode = activeUser.MstCompany_CompanyId.ManualCode,
                                Company = activeUser.MstCompany_CompanyId.Company,
                                ImageURL = activeUser.MstCompany_CompanyId.ImageURL
                            },
                            BranchId = activeUser.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                BranchCode = activeUser.MstCompanyBranch_BranchId.BranchCode,
                                ManualCode = activeUser.MstCompanyBranch_BranchId.ManualCode,
                                Branch = activeUser.MstCompanyBranch_BranchId.Branch
                            },
                            IsActive = activeUser.IsActive,
                            NumberOfJobDepartment = numberOfJobDepartmentCount
                        });
                    }
                }

                return StatusCode(200, listUsers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetUserDetail(Int32 id)
        {
            try
            {
                DTO.MstUserDTO user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == id
                    select new DTO.MstUserDTO
                    {
                        Id = d.Id,
                        Username = d.Username,
                        Password = d.Password,
                        Fullname = d.Fullname,
                        CompanyId = d.CompanyId,
                        Company = new DTO.MstCompanyDTO
                        {
                            CompanyCode = d.MstCompany_CompanyId.CompanyCode,
                            ManualCode = d.MstCompany_CompanyId.ManualCode,
                            Company = d.MstCompany_CompanyId.Company,
                            ImageURL = d.MstCompany_CompanyId.ImageURL
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        IsActive = d.IsActive,
                        IsLocked = d.IsLocked
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddUser()
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserList"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to add a user.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a user.");
                }

                DBSets.MstUserDBSet newUser = new DBSets.MstUserDBSet()
                {
                    Username = "",
                    Password = "",
                    Fullname = "",
                    CompanyId = null,
                    BranchId = null,
                    IsActive = false,
                    IsLocked = false
                };

                _dbContext.MstUsers.Add(newUser);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newUser.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveUser(Int32 id, [FromBody] DTO.MstUserDTO mstUserDTO)
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a user.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a user.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a user that is locked.");
                }

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == mstUserDTO.CompanyId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == mstUserDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserDBSet saveUser = user;
                saveUser.Username = mstUserDTO.Username;
                saveUser.Password = mstUserDTO.Password;
                saveUser.Fullname = mstUserDTO.Fullname;
                saveUser.CompanyId = mstUserDTO.CompanyId;
                saveUser.BranchId = mstUserDTO.BranchId;
                saveUser.IsActive = mstUserDTO.IsActive;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockUser(Int32 id, [FromBody] DTO.MstUserDTO mstUserDTO)
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to lock a user.");
                }

                if (userForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a user.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a user that is locked.");
                }

                DBSets.MstCompanyDBSet company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == mstUserDTO.CompanyId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == mstUserDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstUserDBSet lockUser = user;
                lockUser.Username = mstUserDTO.Username;
                lockUser.Password = mstUserDTO.Password;
                lockUser.Fullname = mstUserDTO.Fullname;
                lockUser.CompanyId = mstUserDTO.CompanyId;
                lockUser.BranchId = mstUserDTO.BranchId;
                lockUser.IsActive = mstUserDTO.IsActive;
                lockUser.IsLocked = true;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockUser(Int32 id)
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to unlock a user.");
                }

                if (userForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a user.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a user that is unlocked.");
                }

                DBSets.MstUserDBSet unlockUser = user;
                unlockUser.IsLocked = false;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(Int32 id)
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

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "SystemUserList"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to delete a user.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a user.");
                }

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (user == null)
                {
                    return StatusCode(404, "User not found.");
                }

                if (user.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a user that is locked.");
                }

                _dbContext.MstUsers.Remove(user);
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
