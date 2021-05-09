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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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
                    ExchangeCurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeRate = 0,
                    RMNumber = RMNumber,
                    RMDate = DateTime.Today,
                    ManualNumber = RMNumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    Remarks = "",
                    Amount = 0,
                    BaseAmount = 0,
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
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

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivableMemoDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
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
                saveReceivableMemo.ExchangeCurrencyId = trnReceivableMemoDTO.ExchangeCurrencyId;
                saveReceivableMemo.ExchangeRate = trnReceivableMemoDTO.ExchangeRate;
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

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivableMemoDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
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
                lockReceivableMemo.ExchangeCurrencyId = trnReceivableMemoDTO.ExchangeCurrencyId;
                lockReceivableMemo.ExchangeRate = trnReceivableMemoDTO.ExchangeRate;
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

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintReceivableMemo(Int32 id)
        {
            FontFactory.RegisterDirectories();

            Font fontSegoeUI09 = FontFactory.GetFont("Segoe UI light", 9);
            Font fontSegoeUI09Bold = FontFactory.GetFont("Segoe UI light", 9, Font.BOLD);
            Font fontSegoeUI10 = FontFactory.GetFont("Segoe UI light", 10);
            Font fontSegoeUI10Bold = FontFactory.GetFont("Segoe UI light", 10, Font.BOLD);
            Font fontSegoeUI11 = FontFactory.GetFont("Segoe UI light", 11);
            Font fontSegoeUI11Bold = FontFactory.GetFont("Segoe UI light", 11, Font.BOLD);
            Font fontSegoeUI12 = FontFactory.GetFont("Segoe UI light", 12);
            Font fontSegoeUI12Bold = FontFactory.GetFont("Segoe UI light", 12, Font.BOLD);
            Font fontSegoeUI13 = FontFactory.GetFont("Segoe UI light", 13);
            Font fontSegoeUI13Bold = FontFactory.GetFont("Segoe UI light", 13, Font.BOLD);
            Font fontSegoeUI14 = FontFactory.GetFont("Segoe UI light", 14);
            Font fontSegoeUI14Bold = FontFactory.GetFont("Segoe UI light", 14, Font.BOLD);
            Font fontSegoeUI15 = FontFactory.GetFont("Segoe UI light", 15);
            Font fontSegoeUI15Bold = FontFactory.GetFont("Segoe UI light", 15, Font.BOLD);
            Font fontSegoeUI16 = FontFactory.GetFont("Segoe UI light", 16);
            Font fontSegoeUI16Bold = FontFactory.GetFont("Segoe UI light", 16, Font.BOLD);

            Document document = new Document(PageSize.Letter, 30f, 30f, 30f, 30f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.Black, Element.ALIGN_MIDDLE, 5F)));

            Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            var loginUser = await (
                from d in _dbContext.MstUsers
                where d.Id == loginUserId
                select d
            ).FirstOrDefaultAsync();

            if (loginUser != null)
            {
                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityReceivableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm != null)
                {
                    if (loginUserForm.CanPrint == true)
                    {
                        String companyName = "";
                        String companyAddress = "";
                        String companyTaxNumber = "";
                        String companyImageURL = "";

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyImageURL = loginUser.MstCompany_CompanyId.ImageURL;
                        }

                        var receivableMemo = await (
                             from d in _dbContext.TrnReceivableMemos
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (receivableMemo != null)
                        {
                            String reprinted = "";
                            if (receivableMemo.IsPrinted == true)
                            {
                                reprinted = "(REPRINTED)";
                            }

                            //String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\colorideas_logo.png";
                            String logoPath = companyImageURL;

                            Image logoPhoto = Image.GetInstance(logoPath);
                            logoPhoto.Alignment = Image.ALIGN_JUSTIFIED;

                            PdfPCell logoPhotoPdfCell = new PdfPCell(logoPhoto, true) { FixedHeight = 40f };
                            logoPhotoPdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                            PdfPTable tableHeader = new PdfPTable(2);
                            tableHeader.SetWidths(new float[] { 80f, 20f });
                            tableHeader.WidthPercentage = 100f;
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontSegoeUI13Bold)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(logoPhotoPdfCell) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 3f, Rowspan = 4 });
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyAddress, fontSegoeUI09)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyTaxNumber, fontSegoeUI09)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt") + " " + reprinted, fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                            tableHeader.AddCell(new PdfPCell(new Phrase("RECEIVABLE MEMO", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String remarks = receivableMemo.Remarks;
                            String supplier = receivableMemo.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer;

                            String branch = receivableMemo.MstCompanyBranch_BranchId.Branch;
                            String RMNumber = "RM-" + receivableMemo.MstCompanyBranch_BranchId.ManualCode + "-" + receivableMemo.RMNumber;
                            String RMDate = receivableMemo.RMDate.ToString("MMMM dd, yyyy");
                            String manualNumber = receivableMemo.ManualNumber;
                            String documentReference = receivableMemo.DocumentReference;

                            PdfPTable tableReceivableMemo = new PdfPTable(4);
                            tableReceivableMemo.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableReceivableMemo.WidthPercentage = 100;

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Customer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(supplier, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(RMNumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Remarks", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 4 });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 4 });

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(RMDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingBottom = 5f, PaddingRight = 5f });
                            tableReceivableMemo.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingBottom = 5f, PaddingRight = 5f });

                            document.Add(tableReceivableMemo);

                            var receivableMemoLines = await (
                                from d in _dbContext.TrnReceivableMemoLines
                                where d.RMId == id
                                select d
                            ).ToListAsync();

                            PdfPTable tableReceivableMemoLines = new PdfPTable(6);
                            tableReceivableMemoLines.SetWidths(new float[] { 120f, 80f, 70f, 150f, 150f, 80f });
                            tableReceivableMemoLines.WidthPercentage = 100;
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("Branch", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("SI Number", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("Code", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("Account", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("Article", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            if (receivableMemoLines.Any())
                            {
                                foreach (var receivableMemoLine in receivableMemoLines)
                                {
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.MstCompanyBranch_BranchId.Branch, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.TrnSalesInvoice_SIId.SINumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.MstAccount_AccountId.ManualCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.MstAccount_AccountId.Account, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.MstArticle_ArticleId.Article, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLine.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(receivableMemoLines.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivableMemoLines.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableReceivableMemoLines);

                            String preparedBy = receivableMemo.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = receivableMemo.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = receivableMemo.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(3);
                            tableUsers.SetWidths(new float[] { 105f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableUsers);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print journal voucher"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print journal voucher"
                    };

                    document.Add(paragraph);
                }
            }
            else
            {
                document.Add(line);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}
