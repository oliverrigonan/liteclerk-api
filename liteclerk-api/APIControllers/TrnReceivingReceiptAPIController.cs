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
    public class TrnReceivingReceiptAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsPayableModule _sysAccountsPayableModule;
        private readonly Modules.SysInventoryModule _sysInventory;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnReceivingReceiptAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            _sysAccountsPayableModule = new Modules.SysAccountsPayableModule(dbContext);
            _sysInventory = new Modules.SysInventoryModule(dbContext);
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
        public async Task<ActionResult> GetReceivingReceiptListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnReceivingReceiptDTO> receivingReceipts = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.BranchId == loginUser.BranchId
                    && d.RRDate >= Convert.ToDateTime(startDate)
                    && d.RRDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnReceivingReceiptDTO
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
                        RRNumber = d.RRNumber,
                        RRDate = d.RRDate.ToShortDateString(),
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
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        Remarks = d.Remarks,
                        ReceivedByUserId = d.ReceivedByUserId,
                        ReceivedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ReceivedByUserId.Username,
                            Fullname = d.MstUser_ReceivedByUserId.Fullname
                        },
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
                        PaidAmount = d.PaidAmount,
                        AdjustmentAmount = d.AdjustmentAmount,
                        BalanceAmount = d.BalanceAmount,
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

                return StatusCode(200, receivingReceipts);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetReceivingReceiptDetail(Int32 id)
        {
            try
            {
                DTO.TrnReceivingReceiptDTO receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == id
                    select new DTO.TrnReceivingReceiptDTO
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
                        RRNumber = d.RRNumber,
                        RRDate = d.RRDate.ToShortDateString(),
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
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        Remarks = d.Remarks,
                        ReceivedByUserId = d.ReceivedByUserId,
                        ReceivedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ReceivedByUserId.Username,
                            Fullname = d.MstUser_ReceivedByUserId.Fullname
                        },
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
                        PaidAmount = d.PaidAmount,
                        AdjustmentAmount = d.AdjustmentAmount,
                        BalanceAmount = d.BalanceAmount,
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

                return StatusCode(200, receivingReceipt);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddReceivingReceipt()
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a receiving receipt.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a receiving receipt.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String RRNumber = "0000000001";
                DBSets.TrnReceivingReceiptDBSet lastReceivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastReceivingReceipt != null)
                {
                    Int32 lastRRNumber = Convert.ToInt32(lastReceivingReceipt.RRNumber) + 0000000001;
                    RRNumber = PadZeroes(lastRRNumber, 10);
                }

                DBSets.TrnReceivingReceiptDBSet newReceivingReceipt = new DBSets.TrnReceivingReceiptDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    RRNumber = RRNumber,
                    RRDate = DateTime.Today,
                    ManualNumber = RRNumber,
                    DocumentReference = "",
                    SupplierId = supplier.ArticleId,
                    TermId = supplier.TermId,
                    Remarks = "",
                    ReceivedByUserId = loginUserId,
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Amount = 0,
                    PaidAmount = 0,
                    AdjustmentAmount = 0,
                    BalanceAmount = 0,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnReceivingReceipts.Add(newReceivingReceipt);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newReceivingReceipt.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveReceivingReceipt(Int32 id, [FromBody] DTO.TrnReceivingReceiptDTO trnReceivingReceiptDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a receiving receipt.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a receiving receipt.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a receiving receipt that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivingReceiptDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnReceivingReceiptDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnReceivingReceiptDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivingReceiptDTO.Status
                    && d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnReceivingReceiptDBSet saveReceivingReceipt = receivingReceipt;
                saveReceivingReceipt.CurrencyId = trnReceivingReceiptDTO.CurrencyId;
                saveReceivingReceipt.RRDate = Convert.ToDateTime(trnReceivingReceiptDTO.RRDate);
                saveReceivingReceipt.ManualNumber = trnReceivingReceiptDTO.ManualNumber;
                saveReceivingReceipt.DocumentReference = trnReceivingReceiptDTO.DocumentReference;
                saveReceivingReceipt.SupplierId = trnReceivingReceiptDTO.SupplierId;
                saveReceivingReceipt.TermId = trnReceivingReceiptDTO.TermId;
                saveReceivingReceipt.Remarks = trnReceivingReceiptDTO.Remarks;
                saveReceivingReceipt.ReceivedByUserId = trnReceivingReceiptDTO.ReceivedByUserId;
                saveReceivingReceipt.CheckedByUserId = trnReceivingReceiptDTO.CheckedByUserId;
                saveReceivingReceipt.ApprovedByUserId = trnReceivingReceiptDTO.ApprovedByUserId;
                saveReceivingReceipt.Status = trnReceivingReceiptDTO.Status;
                saveReceivingReceipt.UpdatedByUserId = loginUserId;
                saveReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockReceivingReceipt(Int32 id, [FromBody] DTO.TrnReceivingReceiptDTO trnReceivingReceiptDTO)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a receiving receipt.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a receiving receipt.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a receiving receipt that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivingReceiptDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnReceivingReceiptDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnReceivingReceiptDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivingReceiptDTO.Status
                    && d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnReceivingReceiptDBSet lockReceivingReceipt = receivingReceipt;
                lockReceivingReceipt.CurrencyId = trnReceivingReceiptDTO.CurrencyId;
                lockReceivingReceipt.RRDate = Convert.ToDateTime(trnReceivingReceiptDTO.RRDate);
                lockReceivingReceipt.ManualNumber = trnReceivingReceiptDTO.ManualNumber;
                lockReceivingReceipt.DocumentReference = trnReceivingReceiptDTO.DocumentReference;
                lockReceivingReceipt.SupplierId = trnReceivingReceiptDTO.SupplierId;
                lockReceivingReceipt.TermId = trnReceivingReceiptDTO.TermId;
                lockReceivingReceipt.Remarks = trnReceivingReceiptDTO.Remarks;
                lockReceivingReceipt.ReceivedByUserId = trnReceivingReceiptDTO.ReceivedByUserId;
                lockReceivingReceipt.CheckedByUserId = trnReceivingReceiptDTO.CheckedByUserId;
                lockReceivingReceipt.ApprovedByUserId = trnReceivingReceiptDTO.ApprovedByUserId;
                lockReceivingReceipt.Status = trnReceivingReceiptDTO.Status;
                lockReceivingReceipt.IsLocked = true;
                lockReceivingReceipt.UpdatedByUserId = loginUserId;
                lockReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsPayableModule.UpdateAccountsPayable(id);
                await _sysInventory.InsertReceivingReceiptInventory(id);
                await _sysJournalEntry.InsertReceivingReceiptJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockReceivingReceipt(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a receiving receipt.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a receiving receipt.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a receiving receipt that is unlocked.");
                }

                DBSets.TrnReceivingReceiptDBSet unlockReceivingReceipt = receivingReceipt;
                unlockReceivingReceipt.IsLocked = false;
                unlockReceivingReceipt.UpdatedByUserId = loginUserId;
                unlockReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsPayableModule.UpdateAccountsPayable(id);
                await _sysInventory.DeleteReceivingReceiptInventory(id);
                await _sysJournalEntry.DeleteReceivingReceiptJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelReceivingReceipt(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a receiving receipt.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a receiving receipt.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a receiving receipt that is unlocked.");
                }

                DBSets.TrnReceivingReceiptDBSet cancelReceivingReceipt = receivingReceipt;
                cancelReceivingReceipt.IsCancelled = true;
                cancelReceivingReceipt.UpdatedByUserId = loginUserId;
                cancelReceivingReceipt.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReceivingReceipt(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a receiving receipt.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a receiving receipt.");
                }

                DBSets.TrnReceivingReceiptDBSet receivingReceipt = await (
                     from d in _dbContext.TrnReceivingReceipts
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (receivingReceipt == null)
                {
                    return StatusCode(404, "Receiving receipt not found.");
                }

                if (receivingReceipt.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a receiving receipt that is locked.");
                }

                _dbContext.TrnReceivingReceipts.Remove(receivingReceipt);
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
