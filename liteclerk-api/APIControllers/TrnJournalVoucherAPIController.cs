using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
    public class TrnJournalVoucherAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsPayableModule _sysAccountsPayable;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnJournalVoucherAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            _sysAccountsPayable = new Modules.SysAccountsPayableModule(dbContext);
            _sysJournalEntry = new Modules.SysJournalEntryModule(dbContext);
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

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetJournalVoucherListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var journalVouchers = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.BranchId == loginUser.BranchId
                    && d.JVDate >= Convert.ToDateTime(startDate)
                    && d.JVDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnJournalVoucherDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        JVNumber = d.JVNumber,
                        JVDate = d.JVDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        Remarks = d.Remarks,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, journalVouchers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJournalVoucherDetail(Int32 id)
        {
            try
            {
                var journalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.Id == id
                    select new DTO.TrnJournalVoucherDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        JVNumber = d.JVNumber,
                        JVDate = d.JVDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        Remarks = d.Remarks,
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUserId.Username,
                            Fullname = d.MstUser_PreparedByUserId.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUserId.Username,
                            Fullname = d.MstUser_CheckedByUserId.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUserId.Username,
                            Fullname = d.MstUser_ApprovedByUserId.Fullname
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Status = d.Status,
                        IsCancelled = d.IsCancelled,
                        IsPrinted = d.IsPrinted,
                        IsLocked = d.IsLocked,
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

                return StatusCode(200, journalVoucher);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJournalVoucher()
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a journal voucher.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a journal voucher.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "PAYABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String JVNumber = "0000000001";
                var lastJournalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJournalVoucher != null)
                {
                    Int32 lastJVNumber = Convert.ToInt32(lastJournalVoucher.JVNumber) + 0000000001;
                    JVNumber = PadZeroes(lastJVNumber, 10);
                }

                var newJournalVoucher = new DBSets.TrnJournalVoucherDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    JVNumber = JVNumber,
                    JVDate = DateTime.Today,
                    ManualNumber = JVNumber,
                    DocumentReference = "",
                    Remarks = "",
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    DebitAmount = 0,
                    CreditAmount = 0,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnJournalVouchers.Add(newJournalVoucher);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJournalVoucher.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJournalVoucher(Int32 id, [FromBody] DTO.TrnJournalVoucherDTO trnJournalVoucherDTO)
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a journal voucher.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a journal voucher.");
                }

                var journalVoucher = await (
                    from d in _dbContext.TrnJournalVouchers
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (journalVoucher == null)
                {
                    return StatusCode(404, "Journal voucher not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a journal voucher that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJournalVoucherDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJournalVoucherDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJournalVoucherDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnJournalVoucherDTO.Status
                    && d.Category == "PAYABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveJournalVoucher = journalVoucher;
                saveJournalVoucher.CurrencyId = trnJournalVoucherDTO.CurrencyId;
                saveJournalVoucher.JVDate = Convert.ToDateTime(trnJournalVoucherDTO.JVDate);
                saveJournalVoucher.ManualNumber = trnJournalVoucherDTO.ManualNumber;
                saveJournalVoucher.DocumentReference = trnJournalVoucherDTO.DocumentReference;
                saveJournalVoucher.Remarks = trnJournalVoucherDTO.Remarks;
                saveJournalVoucher.CheckedByUserId = trnJournalVoucherDTO.CheckedByUserId;
                saveJournalVoucher.ApprovedByUserId = trnJournalVoucherDTO.ApprovedByUserId;
                saveJournalVoucher.Status = trnJournalVoucherDTO.Status;
                saveJournalVoucher.UpdatedByUserId = loginUserId;
                saveJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockJournalVoucher(Int32 id, [FromBody] DTO.TrnJournalVoucherDTO trnJournalVoucherDTO)
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a journal voucher.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a journal voucher.");
                }

                var journalVoucher = await (
                     from d in _dbContext.TrnJournalVouchers
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (journalVoucher == null)
                {
                    return StatusCode(404, "JournalVoucher not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a journal voucher that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnJournalVoucherDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJournalVoucherDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnJournalVoucherDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnJournalVoucherDTO.Status
                    && d.Category == "PAYABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockJournalVoucher = journalVoucher;
                lockJournalVoucher.CurrencyId = trnJournalVoucherDTO.CurrencyId;
                lockJournalVoucher.JVDate = Convert.ToDateTime(trnJournalVoucherDTO.JVDate);
                lockJournalVoucher.ManualNumber = trnJournalVoucherDTO.ManualNumber;
                lockJournalVoucher.DocumentReference = trnJournalVoucherDTO.DocumentReference;
                lockJournalVoucher.Remarks = trnJournalVoucherDTO.Remarks;
                lockJournalVoucher.CheckedByUserId = trnJournalVoucherDTO.CheckedByUserId;
                lockJournalVoucher.ApprovedByUserId = trnJournalVoucherDTO.ApprovedByUserId;
                lockJournalVoucher.Status = trnJournalVoucherDTO.Status;
                lockJournalVoucher.IsLocked = true;
                lockJournalVoucher.UpdatedByUserId = loginUserId;
                lockJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysJournalEntry.InsertJournalVoucherJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockJournalVoucher(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a journal voucher.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a journal voucher.");
                }

                var journalVoucher = await (
                     from d in _dbContext.TrnJournalVouchers
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (journalVoucher == null)
                {
                    return StatusCode(404, "JournalVoucher not found.");
                }

                if (journalVoucher.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a journal voucher that is unlocked.");
                }

                var unlockJournalVoucher = journalVoucher;
                unlockJournalVoucher.IsLocked = false;
                unlockJournalVoucher.UpdatedByUserId = loginUserId;
                unlockJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysJournalEntry.DeleteJournalVoucherJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelJournalVoucher(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a journal voucher.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a journal voucher.");
                }

                var journalVoucher = await (
                     from d in _dbContext.TrnJournalVouchers
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (journalVoucher == null)
                {
                    return StatusCode(404, "JournalVoucher not found.");
                }

                if (journalVoucher.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a journal voucher that is unlocked.");
                }

                var unlockJournalVoucher = journalVoucher;
                unlockJournalVoucher.IsCancelled = true;
                unlockJournalVoucher.UpdatedByUserId = loginUserId;
                unlockJournalVoucher.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJournalVoucher(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityJournalVoucherList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a journal voucher.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a journal voucher.");
                }

                var journalVoucher = await (
                     from d in _dbContext.TrnJournalVouchers
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (journalVoucher == null)
                {
                    return StatusCode(404, "Journal voucher not found.");
                }

                if (journalVoucher.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a journal voucher that is locked.");
                }

                _dbContext.TrnJournalVouchers.Remove(journalVoucher);
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
