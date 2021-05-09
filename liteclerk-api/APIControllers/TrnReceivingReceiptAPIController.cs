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

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var receivingReceipts = await (
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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                            PayableAccountId = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId : 0,
                            PayableAccount = new DTO.MstAccountDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.ManualCode : "",
                                Account = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.Account : ""
                            }
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        PaidAmount = d.PaidAmount,
                        BasePaidAmount = d.BasePaidAmount,
                        AdjustmentAmount = d.AdjustmentAmount,
                        BaseAdjustmentAmount = d.BaseAdjustmentAmount,
                        BalanceAmount = d.BalanceAmount,
                        BaseBalanceAmount = d.BaseBalanceAmount,
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

        [HttpGet("list/bySupplier/{supplierId}")]
        public async Task<ActionResult> GetReceivingReceiptListBySupplier(Int32 supplierId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var receivingReceipts = await (
                    from d in _dbContext.TrnReceivingReceipts
                    where d.BranchId == loginUser.BranchId
                    && d.SupplierId == supplierId
                    && d.IsLocked == true
                    && d.BalanceAmount > 0
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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                            PayableAccountId = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId : 0,
                            PayableAccount = new DTO.MstAccountDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.ManualCode : "",
                                Account = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.Account : ""
                            }
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        PaidAmount = d.PaidAmount,
                        BasePaidAmount = d.BasePaidAmount,
                        AdjustmentAmount = d.AdjustmentAmount,
                        BaseAdjustmentAmount = d.BaseAdjustmentAmount,
                        BalanceAmount = d.BalanceAmount,
                        BaseBalanceAmount = d.BaseBalanceAmount,
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
                var receivingReceipt = await (
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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                            PayableAccountId = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().PayableAccountId : 0,
                            PayableAccount = new DTO.MstAccountDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.ManualCode : "",
                                Account = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().MstAccount_PayableAccountId.Account : ""
                            }
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        PaidAmount = d.PaidAmount,
                        BasePaidAmount = d.BasePaidAmount,
                        AdjustmentAmount = d.AdjustmentAmount,
                        BaseAdjustmentAmount = d.BaseAdjustmentAmount,
                        BalanceAmount = d.BalanceAmount,
                        BaseBalanceAmount = d.BaseBalanceAmount,
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

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String RRNumber = "0000000001";
                var lastReceivingReceipt = await (
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

                var newReceivingReceipt = new DBSets.TrnReceivingReceiptDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeCurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeRate = 0,
                    RRNumber = RRNumber,
                    RRDate = DateTime.Today,
                    ManualNumber = RRNumber,
                    DocumentReference = "",
                    SupplierId = supplier.ArticleId,
                    TermId = supplier.TermId,
                    Remarks = "",
                    Amount = 0,
                    BaseAmount = 0,
                    PaidAmount = 0,
                    BasePaidAmount = 0,
                    AdjustmentAmount = 0,
                    BaseAdjustmentAmount = 0,
                    BalanceAmount = 0,
                    BaseBalanceAmount = 0,
                    ReceivedByUserId = loginUserId,
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

                var receivingReceipt = await (
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

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivingReceiptDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnReceivingReceiptDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnReceivingReceiptDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivingReceiptDTO.Status
                    && d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveReceivingReceipt = receivingReceipt;
                saveReceivingReceipt.ExchangeCurrencyId = trnReceivingReceiptDTO.ExchangeCurrencyId;
                saveReceivingReceipt.ExchangeRate = trnReceivingReceiptDTO.ExchangeRate;
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

                var receivingReceipt = await (
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

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnReceivingReceiptDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnReceivingReceiptDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnReceivingReceiptDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnReceivingReceiptDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnReceivingReceiptDTO.Status
                    && d.Category == "RECEIVING RECEIPT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockReceivingReceipt = receivingReceipt;
                lockReceivingReceipt.ExchangeCurrencyId = trnReceivingReceiptDTO.ExchangeCurrencyId;
                lockReceivingReceipt.ExchangeRate = trnReceivingReceiptDTO.ExchangeRate;
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

                var receivingReceipt = await (
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

                var unlockReceivingReceipt = receivingReceipt;
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

                var receivingReceipt = await (
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

                var cancelReceivingReceipt = receivingReceipt;
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

                var receivingReceipt = await (
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

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintReceivingReceipt(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityReceivingReceiptDetail"
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

                        var receivingReceipt = await (
                             from d in _dbContext.TrnReceivingReceipts
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (receivingReceipt != null)
                        {
                            String reprinted = "";
                            if (receivingReceipt.IsPrinted == true)
                            {
                                reprinted = "(REPRRRTED)";
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("RECEIVING RECEIPT", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String supplier = receivingReceipt.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier;
                            String term = receivingReceipt.MstTerm_TermId.Term;
                            String dueDate = receivingReceipt.RRDate.AddDays(Convert.ToDouble(receivingReceipt.MstTerm_TermId.NumberOfDays)).ToString("MMMM dd, yyyy");
                            String remarks = receivingReceipt.Remarks;

                            String branch = receivingReceipt.MstCompanyBranch_BranchId.Branch;
                            String RRNumber = "RR-" + receivingReceipt.MstCompanyBranch_BranchId.ManualCode + "-" + receivingReceipt.RRNumber;
                            String RRDate = receivingReceipt.RRDate.ToString("MMMM dd, yyyy");
                            String manualNumber = receivingReceipt.ManualNumber;
                            String documentReference = receivingReceipt.DocumentReference;

                            PdfPTable tableReceivingReceipt = new PdfPTable(4);
                            tableReceivingReceipt.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableReceivingReceipt.WidthPercentage = 100;

                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Supplier:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(supplier, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(RRNumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Term:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(term, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Due Date", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(dueDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(RRDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Remarks", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 2 });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 2 });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableReceivingReceipt);

                            PdfPTable tableReceivingReceiptItems = new PdfPTable(6);
                            tableReceivingReceiptItems.SetWidths(new float[] { 70f, 70f, 150f, 120f, 80f, 80f });
                            tableReceivingReceiptItems.WidthPercentage = 100;
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Qty.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Unit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Item", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Cost", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            var receivingReceiptItems = await (
                                from d in _dbContext.TrnReceivingReceiptItems
                                where d.RRId == id
                                select d
                            ).ToListAsync();

                            if (receivingReceiptItems.Any())
                            {
                                foreach (var receivingReceiptItem in receivingReceiptItems)
                                {
                                    String SKUCode = receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             receivingReceiptItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";

                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Cost.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItems.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableReceivingReceiptItems);

                            String preparedBy = receivingReceipt.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = receivingReceipt.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = receivingReceipt.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(4);
                            tableUsers.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Requested by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Date Received:", fontSegoeUI09Bold)) { HorizontalAlignment = 0, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableUsers);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print purchase order"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print purchase order"
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
