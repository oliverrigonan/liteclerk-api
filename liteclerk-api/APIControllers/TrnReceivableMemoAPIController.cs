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
    public class TrnReceivableMemoAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsReceivableModule _sysAccountsReceivable;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnReceivableMemoAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            _sysAccountsReceivable = new Modules.SysAccountsReceivableModule(dbContext);
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
        public async Task<ActionResult> GetReceivableMemoListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var receivableMemos = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.BranchId == loginUser.BranchId
                    && d.RMDate >= Convert.ToDateTime(startDate)
                    && d.RMDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnReceivableMemoDTO
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
                        RMNumber = d.RMNumber,
                        RMDate = d.RMDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleCustomerDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.ManualCode
                            },
                            Customer = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                        },
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
                        Amount = d.Amount,
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

                return StatusCode(200, receivableMemos);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetReceivableMemoDetail(Int32 id)
        {
            try
            {
                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == id
                    select new DTO.TrnReceivableMemoDTO
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
                        RMNumber = d.RMNumber,
                        RMDate = d.RMDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleCustomerDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.ManualCode
                            },
                            Customer = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                        },
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
                        Amount = d.Amount,
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

                return StatusCode(200, receivableMemo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddReceivableMemo()
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a receivable memo.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a receivable memo.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String RMNumber = "0000000001";
                var lastReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastReceivableMemo != null)
                {
                    Int32 lastRMNumber = Convert.ToInt32(lastReceivableMemo.RMNumber) + 0000000001;
                    RMNumber = PadZeroes(lastRMNumber, 10);
                }

                var newReceivableMemo = new DBSets.TrnReceivableMemoDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    RMNumber = RMNumber,
                    RMDate = DateTime.Today,
                    ManualNumber = RMNumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    Remarks = "",
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Amount = 0,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnReceivableMemos.Add(newReceivableMemo);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newReceivableMemo.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveReceivableMemo(Int32 id, [FromBody] DTO.TrnReceivableMemoDTO trnReceivableMemoDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a receivable memo.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a receivable memo.");
                }

                var receivableMemo = await (
                    from d in _dbContext.TrnReceivableMemos
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (receivableMemo == null)
                {
                    return StatusCode(404, "Receivable memo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a receivable memo that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivableMemoDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnReceivableMemoDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivableMemoDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivableMemoDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivableMemoDTO.Status
                    && d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveReceivableMemo = receivableMemo;
                saveReceivableMemo.CurrencyId = trnReceivableMemoDTO.CurrencyId;
                saveReceivableMemo.RMDate = Convert.ToDateTime(trnReceivableMemoDTO.RMDate);
                saveReceivableMemo.ManualNumber = trnReceivableMemoDTO.ManualNumber;
                saveReceivableMemo.DocumentReference = trnReceivableMemoDTO.DocumentReference;
                saveReceivableMemo.CustomerId = trnReceivableMemoDTO.CustomerId;
                saveReceivableMemo.Remarks = trnReceivableMemoDTO.Remarks;
                saveReceivableMemo.CheckedByUserId = trnReceivableMemoDTO.CheckedByUserId;
                saveReceivableMemo.ApprovedByUserId = trnReceivableMemoDTO.ApprovedByUserId;
                saveReceivableMemo.Status = trnReceivableMemoDTO.Status;
                saveReceivableMemo.UpdatedByUserId = loginUserId;
                saveReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockReceivableMemo(Int32 id, [FromBody] DTO.TrnReceivableMemoDTO trnReceivableMemoDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a receivable memo.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a receivable memo.");
                }

                var receivableMemo = await (
                     from d in _dbContext.TrnReceivableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivableMemo == null)
                {
                    return StatusCode(404, "ReceivableMemo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a receivable memo that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivableMemoDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnReceivableMemoDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivableMemoDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivableMemoDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivableMemoDTO.Status
                    && d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockReceivableMemo = receivableMemo;
                lockReceivableMemo.CurrencyId = trnReceivableMemoDTO.CurrencyId;
                lockReceivableMemo.RMDate = Convert.ToDateTime(trnReceivableMemoDTO.RMDate);
                lockReceivableMemo.ManualNumber = trnReceivableMemoDTO.ManualNumber;
                lockReceivableMemo.DocumentReference = trnReceivableMemoDTO.DocumentReference;
                lockReceivableMemo.CustomerId = trnReceivableMemoDTO.CustomerId;
                lockReceivableMemo.Remarks = trnReceivableMemoDTO.Remarks;
                lockReceivableMemo.CheckedByUserId = trnReceivableMemoDTO.CheckedByUserId;
                lockReceivableMemo.ApprovedByUserId = trnReceivableMemoDTO.ApprovedByUserId;
                lockReceivableMemo.Status = trnReceivableMemoDTO.Status;
                lockReceivableMemo.IsLocked = true;
                lockReceivableMemo.UpdatedByUserId = loginUserId;
                lockReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == id
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    foreach (var receivableMemoLineByCurrentReceivableMemo in receivableMemoLinesByCurrentReceivableMemo)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(receivableMemoLineByCurrentReceivableMemo.SIId));
                    }
                }

                await _sysJournalEntry.InsertReceivableMemoJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockReceivableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a receivable memo.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a receivable memo.");
                }

                var receivableMemo = await (
                     from d in _dbContext.TrnReceivableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivableMemo == null)
                {
                    return StatusCode(404, "ReceivableMemo not found.");
                }

                if (receivableMemo.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a receivable memo that is unlocked.");
                }

                var unlockReceivableMemo = receivableMemo;
                unlockReceivableMemo.IsLocked = false;
                unlockReceivableMemo.UpdatedByUserId = loginUserId;
                unlockReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == id
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    foreach (var receivableMemoLineByCurrentReceivableMemo in receivableMemoLinesByCurrentReceivableMemo)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(receivableMemoLineByCurrentReceivableMemo.SIId));
                    }
                }

                await _sysJournalEntry.DeleteReceivableMemoJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelReceivableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a receivable memo.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a receivable memo.");
                }

                var receivableMemo = await (
                     from d in _dbContext.TrnReceivableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivableMemo == null)
                {
                    return StatusCode(404, "ReceivableMemo not found.");
                }

                if (receivableMemo.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a receivable memo that is unlocked.");
                }

                var unlockReceivableMemo = receivableMemo;
                unlockReceivableMemo.IsCancelled = true;
                unlockReceivableMemo.UpdatedByUserId = loginUserId;
                unlockReceivableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == id
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    foreach (var receivableMemoLineByCurrentReceivableMemo in receivableMemoLinesByCurrentReceivableMemo)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(receivableMemoLineByCurrentReceivableMemo.SIId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReceivableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a receivable memo.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a receivable memo.");
                }

                var receivableMemo = await (
                     from d in _dbContext.TrnReceivableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivableMemo == null)
                {
                    return StatusCode(404, "Receivable memo not found.");
                }

                if (receivableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a receivable memo that is locked.");
                }

                _dbContext.TrnReceivableMemos.Remove(receivableMemo);
                await _dbContext.SaveChangesAsync();

                var receivableMemoLinesByCurrentReceivableMemo = await (
                    from d in _dbContext.TrnReceivableMemoLines
                    where d.RMId == id
                    select d
                ).ToListAsync();

                if (receivableMemoLinesByCurrentReceivableMemo.Any())
                {
                    foreach (var receivableMemoLineByCurrentReceivableMemo in receivableMemoLinesByCurrentReceivableMemo)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(receivableMemoLineByCurrentReceivableMemo.SIId));
                    }
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
