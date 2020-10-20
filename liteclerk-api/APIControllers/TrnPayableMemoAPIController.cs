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
    public class TrnPayableMemoAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsPayableModule _sysAccountsPayable;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnPayableMemoAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetPayableMemoListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var payableMemos = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.BranchId == loginUser.BranchId
                    && d.PMDate >= Convert.ToDateTime(startDate)
                    && d.PMDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnPayableMemoDTO
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
                        PMNumber = d.PMNumber,
                        PMDate = d.PMDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        SupplierId = d.SupplierId,
                        Supplier = new DTO.MstArticleSupplierDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.ManualCode
                            },
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
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

                return StatusCode(200, payableMemos);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPayableMemoDetail(Int32 id)
        {
            try
            {
                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == id
                    select new DTO.TrnPayableMemoDTO
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
                        PMNumber = d.PMNumber,
                        PMDate = d.PMDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        SupplierId = d.SupplierId,
                        Supplier = new DTO.MstArticleSupplierDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.ManualCode
                            },
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
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

                return StatusCode(200, payableMemo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPayableMemo()
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a payable memo.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a payable memo.");
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
                    where d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String PMNumber = "0000000001";
                var lastPayableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastPayableMemo != null)
                {
                    Int32 lastPMNumber = Convert.ToInt32(lastPayableMemo.PMNumber) + 0000000001;
                    PMNumber = PadZeroes(lastPMNumber, 10);
                }

                var newPayableMemo = new DBSets.TrnPayableMemoDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    PMNumber = PMNumber,
                    PMDate = DateTime.Today,
                    ManualNumber = PMNumber,
                    DocumentReference = "",
                    SupplierId = customer.ArticleId,
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

                _dbContext.TrnPayableMemos.Add(newPayableMemo);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newPayableMemo.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SavePayableMemo(Int32 id, [FromBody] DTO.TrnPayableMemoDTO trnPayableMemoDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a payable memo.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a payable memo.");
                }

                var payableMemo = await (
                    from d in _dbContext.TrnPayableMemos
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (payableMemo == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a payable memo that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPayableMemoDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPayableMemoDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPayableMemoDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPayableMemoDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPayableMemoDTO.Status
                    && d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var savePayableMemo = payableMemo;
                savePayableMemo.CurrencyId = trnPayableMemoDTO.CurrencyId;
                savePayableMemo.PMDate = Convert.ToDateTime(trnPayableMemoDTO.PMDate);
                savePayableMemo.ManualNumber = trnPayableMemoDTO.ManualNumber;
                savePayableMemo.DocumentReference = trnPayableMemoDTO.DocumentReference;
                savePayableMemo.SupplierId = trnPayableMemoDTO.SupplierId;
                savePayableMemo.Remarks = trnPayableMemoDTO.Remarks;
                savePayableMemo.CheckedByUserId = trnPayableMemoDTO.CheckedByUserId;
                savePayableMemo.ApprovedByUserId = trnPayableMemoDTO.ApprovedByUserId;
                savePayableMemo.Status = trnPayableMemoDTO.Status;
                savePayableMemo.UpdatedByUserId = loginUserId;
                savePayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockPayableMemo(Int32 id, [FromBody] DTO.TrnPayableMemoDTO trnPayableMemoDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a payable memo.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a payable memo.");
                }

                var payableMemo = await (
                     from d in _dbContext.TrnPayableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (payableMemo == null)
                {
                    return StatusCode(404, "PayableMemo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a payable memo that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPayableMemoDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPayableMemoDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPayableMemoDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPayableMemoDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPayableMemoDTO.Status
                    && d.Category == "RECEIVABLE MEMO STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockPayableMemo = payableMemo;
                lockPayableMemo.CurrencyId = trnPayableMemoDTO.CurrencyId;
                lockPayableMemo.PMDate = Convert.ToDateTime(trnPayableMemoDTO.PMDate);
                lockPayableMemo.ManualNumber = trnPayableMemoDTO.ManualNumber;
                lockPayableMemo.DocumentReference = trnPayableMemoDTO.DocumentReference;
                lockPayableMemo.SupplierId = trnPayableMemoDTO.SupplierId;
                lockPayableMemo.Remarks = trnPayableMemoDTO.Remarks;
                lockPayableMemo.CheckedByUserId = trnPayableMemoDTO.CheckedByUserId;
                lockPayableMemo.ApprovedByUserId = trnPayableMemoDTO.ApprovedByUserId;
                lockPayableMemo.Status = trnPayableMemoDTO.Status;
                lockPayableMemo.IsLocked = true;
                lockPayableMemo.UpdatedByUserId = loginUserId;
                lockPayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == id
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    foreach (var payableMemoLineByCurrentPayableMemo in payableMemoLinesByCurrentPayableMemo)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(payableMemoLineByCurrentPayableMemo.RRId));
                    }
                }

                await _sysJournalEntry.InsertPayableMemoJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockPayableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a payable memo.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a payable memo.");
                }

                var payableMemo = await (
                     from d in _dbContext.TrnPayableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (payableMemo == null)
                {
                    return StatusCode(404, "PayableMemo not found.");
                }

                if (payableMemo.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a payable memo that is unlocked.");
                }

                var unlockPayableMemo = payableMemo;
                unlockPayableMemo.IsLocked = false;
                unlockPayableMemo.UpdatedByUserId = loginUserId;
                unlockPayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == id
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    foreach (var payableMemoLineByCurrentPayableMemo in payableMemoLinesByCurrentPayableMemo)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(payableMemoLineByCurrentPayableMemo.RRId));
                    }
                }

                await _sysJournalEntry.DeletePayableMemoJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelPayableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a payable memo.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a payable memo.");
                }

                var payableMemo = await (
                     from d in _dbContext.TrnPayableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (payableMemo == null)
                {
                    return StatusCode(404, "PayableMemo not found.");
                }

                if (payableMemo.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a payable memo that is unlocked.");
                }

                var unlockPayableMemo = payableMemo;
                unlockPayableMemo.IsCancelled = true;
                unlockPayableMemo.UpdatedByUserId = loginUserId;
                unlockPayableMemo.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == id
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    foreach (var payableMemoLineByCurrentPayableMemo in payableMemoLinesByCurrentPayableMemo)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(payableMemoLineByCurrentPayableMemo.RRId));
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
        public async Task<ActionResult> DeletePayableMemo(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPayableMemoList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a payable memo.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a payable memo.");
                }

                var payableMemo = await (
                     from d in _dbContext.TrnPayableMemos
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (payableMemo == null)
                {
                    return StatusCode(404, "Payable memo not found.");
                }

                if (payableMemo.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a payable memo that is locked.");
                }

                _dbContext.TrnPayableMemos.Remove(payableMemo);
                await _dbContext.SaveChangesAsync();

                var payableMemoLinesByCurrentPayableMemo = await (
                    from d in _dbContext.TrnPayableMemoLines
                    where d.PMId == id
                    select d
                ).ToListAsync();

                if (payableMemoLinesByCurrentPayableMemo.Any())
                {
                    foreach (var payableMemoLineByCurrentPayableMemo in payableMemoLinesByCurrentPayableMemo)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(payableMemoLineByCurrentPayableMemo.RRId));
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
