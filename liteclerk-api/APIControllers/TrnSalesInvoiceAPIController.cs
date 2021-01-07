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
    public class TrnSalesInvoiceAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsReceivableModule _sysAccountsReceivable;
        private readonly Modules.SysInventoryModule _sysInventory;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnSalesInvoiceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            _sysAccountsReceivable = new Modules.SysAccountsReceivableModule(dbContext);
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
        public async Task<ActionResult> GetSalesInvoiceListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == loginUser.BranchId
                    && d.SIDate >= Convert.ToDateTime(startDate)
                    && d.SIDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceDTO
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
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
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
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUserId.Username,
                            Fullname = d.MstUser_SoldByUserId.Fullname
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

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byCustomer/{customerId}")]
        public async Task<ActionResult> GetSalesInvoiceListByCustomer(Int32 customerId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == loginUser.BranchId
                    && d.CustomerId == customerId
                    && d.IsLocked == true
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceDTO
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
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
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
                            ReceivableAccountId = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId : 0,
                            ReceivableAccount = new DTO.MstAccountDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.ManualCode : "",
                                Account = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.Account : ""
                            }
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUserId.Username,
                            Fullname = d.MstUser_SoldByUserId.Fullname
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

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byCustomer/{customerId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetSalesInvoiceListByCustomerByBranch(Int32 customerId, Int32 branchId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == branchId
                    && d.CustomerId == customerId
                    && d.IsLocked == true
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceDTO
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
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
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
                            ReceivableAccountId = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId : 0,
                            ReceivableAccount = new DTO.MstAccountDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.ManualCode : "",
                                Account = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.Account : ""
                            }
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUserId.Username,
                            Fullname = d.MstUser_SoldByUserId.Fullname
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

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetSalesInvoiceDetail(Int32 id)
        {
            try
            {
                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == id
                    select new DTO.TrnSalesInvoiceDTO
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
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
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
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUserId.Username,
                            Fullname = d.MstUser_SoldByUserId.Fullname
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

                return StatusCode(200, salesInvoice);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddSalesInvoice()
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a sales invoice.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a sales invoice.");
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
                    where d.Category == "SALES INVOICE STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String SINumber = "0000000001";
                var lastSalesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastSalesInvoice != null)
                {
                    Int32 lastSINumber = Convert.ToInt32(lastSalesInvoice.SINumber) + 0000000001;
                    SINumber = PadZeroes(lastSINumber, 10);
                }

                var newSalesInvoice = new DBSets.TrnSalesInvoiceDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    SINumber = SINumber,
                    SIDate = DateTime.Today,
                    ManualNumber = SINumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    TermId = customer.TermId,
                    DateNeeded = DateTime.Today,
                    Remarks = "",
                    SoldByUserId = loginUserId,
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

                _dbContext.TrnSalesInvoices.Add(newSalesInvoice);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newSalesInvoice.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveSalesInvoice(Int32 id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a sales invoice.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a sales invoice.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a sales invoice that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesInvoiceDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesInvoiceDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesInvoiceDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnSalesInvoiceDTO.Status
                    && d.Category == "SALES INVOICE STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveSalesInvoice = salesInvoice;
                saveSalesInvoice.CurrencyId = trnSalesInvoiceDTO.CurrencyId;
                saveSalesInvoice.SIDate = Convert.ToDateTime(trnSalesInvoiceDTO.SIDate);
                saveSalesInvoice.ManualNumber = trnSalesInvoiceDTO.ManualNumber;
                saveSalesInvoice.DocumentReference = trnSalesInvoiceDTO.DocumentReference;
                saveSalesInvoice.CustomerId = trnSalesInvoiceDTO.CustomerId;
                saveSalesInvoice.TermId = trnSalesInvoiceDTO.TermId;
                saveSalesInvoice.DateNeeded = Convert.ToDateTime(trnSalesInvoiceDTO.DateNeeded);
                saveSalesInvoice.Remarks = trnSalesInvoiceDTO.Remarks;
                saveSalesInvoice.SoldByUserId = trnSalesInvoiceDTO.SoldByUserId;
                saveSalesInvoice.CheckedByUserId = trnSalesInvoiceDTO.CheckedByUserId;
                saveSalesInvoice.ApprovedByUserId = trnSalesInvoiceDTO.ApprovedByUserId;
                saveSalesInvoice.Status = trnSalesInvoiceDTO.Status;
                saveSalesInvoice.UpdatedByUserId = loginUserId;
                saveSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockSalesInvoice(Int32 id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a sales invoice.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a sales invoice.");
                }

                var salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a sales invoice that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesInvoiceDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesInvoiceDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesInvoiceDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnSalesInvoiceDTO.Status
                    && d.Category == "SALES INVOICE STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockSalesInvoice = salesInvoice;
                lockSalesInvoice.CurrencyId = trnSalesInvoiceDTO.CurrencyId;
                lockSalesInvoice.SIDate = Convert.ToDateTime(trnSalesInvoiceDTO.SIDate);
                lockSalesInvoice.ManualNumber = trnSalesInvoiceDTO.ManualNumber;
                lockSalesInvoice.DocumentReference = trnSalesInvoiceDTO.DocumentReference;
                lockSalesInvoice.CustomerId = trnSalesInvoiceDTO.CustomerId;
                lockSalesInvoice.TermId = trnSalesInvoiceDTO.TermId;
                lockSalesInvoice.DateNeeded = Convert.ToDateTime(trnSalesInvoiceDTO.DateNeeded);
                lockSalesInvoice.Remarks = trnSalesInvoiceDTO.Remarks;
                lockSalesInvoice.SoldByUserId = trnSalesInvoiceDTO.SoldByUserId;
                lockSalesInvoice.CheckedByUserId = trnSalesInvoiceDTO.CheckedByUserId;
                lockSalesInvoice.ApprovedByUserId = trnSalesInvoiceDTO.ApprovedByUserId;
                lockSalesInvoice.Status = trnSalesInvoiceDTO.Status;
                lockSalesInvoice.IsLocked = true;
                lockSalesInvoice.UpdatedByUserId = loginUserId;
                lockSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsReceivable.UpdateAccountsReceivable(id);
                await _sysInventory.InsertSalesInvoiceInventory(id);
                await _sysJournalEntry.InsertSalesInvoiceJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockSalesInvoice(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a sales invoice.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a sales invoice.");
                }

                var salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a sales invoice that is unlocked.");
                }

                var unlockSalesInvoice = salesInvoice;
                unlockSalesInvoice.IsLocked = false;
                unlockSalesInvoice.UpdatedByUserId = loginUserId;
                unlockSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsReceivable.UpdateAccountsReceivable(id);
                await _sysInventory.DeleteSalesInvoiceInventory(id);
                await _sysJournalEntry.DeleteSalesInvoiceJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelSalesInvoice(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a sales invoice.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a sales invoice.");
                }

                var salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a sales invoice that is unlocked.");
                }

                var unlockSalesInvoice = salesInvoice;
                unlockSalesInvoice.IsCancelled = true;
                unlockSalesInvoice.UpdatedByUserId = loginUserId;
                unlockSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsReceivable.UpdateAccountsReceivable(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteSalesInvoice(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a sales invoice.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a sales invoice.");
                }

                var salesInvoice = await (
                     from d in _dbContext.TrnSalesInvoices
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a sales invoice that is locked.");
                }

                _dbContext.TrnSalesInvoices.Remove(salesInvoice);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("post/fromPointOfSales/byTerminal/{terminalCode}/byDate/{date}")]
        public async Task<ActionResult> PostSalesInvoiceFromPointOfSaleByTerminalByDate(String terminalCode, String date)
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
                    && d.SysForm_FormId.Form == "ActivityPOSSales"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to post a POS Sales.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to post a POS Sales.");
                }

                var pointOfSales = await (
                    from d in _dbContext.TrnPointOfSales
                    where d.BranchId == loginUser.BranchId
                    && d.TerminalCode == terminalCode
                    && d.POSDate == Convert.ToDateTime(date)
                    && d.CustomerId != null
                    && d.ItemId != null
                    && d.TaxId != null
                    && d.CashierUserId != null
                    && (d.PostCode == null || d.PostCode == String.Empty)
                    select d
                ).ToListAsync();

                if (pointOfSales.Any())
                {
                    var pointOfSalesCustomers = from d in pointOfSales
                                                where d.BranchId == loginUser.BranchId
                                                && d.TerminalCode == terminalCode
                                                && d.POSDate == Convert.ToDateTime(date)
                                                group d by new
                                                {
                                                    d.CustomerId
                                                } into g
                                                select new DTO.TrnPointOfSaleDTO
                                                {
                                                    CustomerId = g.Key.CustomerId
                                                };

                    foreach (var pointOfSalesCustomer in pointOfSalesCustomers.ToList())
                    {
                        var customer = await (
                            from d in _dbContext.MstArticleCustomers
                            where d.ArticleId == pointOfSalesCustomer.CustomerId
                            && d.MstArticle_ArticleId.IsLocked == true
                            select d
                        ).FirstOrDefaultAsync();

                        if (customer != null)
                        {
                            String status = "";

                            var codeTableStatus = await (
                                from d in _dbContext.MstCodeTables
                                where d.Category == "SALES INVOICE STATUS"
                                select d
                            ).FirstOrDefaultAsync();

                            if (codeTableStatus != null)
                            {
                                status = codeTableStatus.CodeValue;
                            }

                            String SINumber = "0000000001";
                            var lastSalesInvoice = await (
                                from d in _dbContext.TrnSalesInvoices
                                where d.BranchId == loginUser.BranchId
                                orderby d.Id descending
                                select d
                            ).FirstOrDefaultAsync();

                            if (lastSalesInvoice != null)
                            {
                                Int32 lastSINumber = Convert.ToInt32(lastSalesInvoice.SINumber) + 0000000001;
                                SINumber = PadZeroes(lastSINumber, 10);
                            }

                            var newSalesInvoice = new DBSets.TrnSalesInvoiceDBSet()
                            {
                                BranchId = Convert.ToInt32(loginUser.BranchId),
                                CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                                SINumber = SINumber,
                                SIDate = Convert.ToDateTime(date),
                                ManualNumber = SINumber,
                                DocumentReference = "",
                                CustomerId = customer.ArticleId,
                                TermId = customer.TermId,
                                DateNeeded = DateTime.Today,
                                Remarks = "",
                                SoldByUserId = loginUserId,
                                PreparedByUserId = loginUserId,
                                CheckedByUserId = loginUserId,
                                ApprovedByUserId = loginUserId,
                                Amount = 0,
                                PaidAmount = 0,
                                AdjustmentAmount = 0,
                                BalanceAmount = 0,
                                Status = status,
                                IsCancelled = false,
                                IsPrinted = false,
                                IsLocked = false,
                                CreatedByUserId = loginUserId,
                                CreatedDateTime = DateTime.Now,
                                UpdatedByUserId = loginUserId,
                                UpdatedDateTime = DateTime.Now
                            };

                            _dbContext.TrnSalesInvoices.Add(newSalesInvoice);
                            await _dbContext.SaveChangesAsync();

                            Int32 SIId = newSalesInvoice.Id;

                            var pointOfSalesItems = from d in pointOfSales
                                                    where d.BranchId == loginUser.BranchId
                                                    && d.TerminalCode == terminalCode
                                                    && d.POSDate == Convert.ToDateTime(date)
                                                    && d.CustomerId == pointOfSalesCustomer.CustomerId
                                                    select new DTO.TrnPointOfSaleDTO
                                                    {
                                                        ItemId = Convert.ToInt32(d.ItemId),
                                                        Quantity = d.Quantity,
                                                        Price = d.Price,
                                                        Discount = d.Discount,
                                                        NetPrice = d.NetPrice,
                                                        Amount = d.Amount,
                                                        Particulars = d.Particulars,
                                                        TaxId = Convert.ToInt32(d.TaxId),
                                                    };

                            if (pointOfSalesItems.Any())
                            {
                                List<DBSets.TrnSalesInvoiceItemDBSet> newSalesInvoiceItems = new List<DBSets.TrnSalesInvoiceItemDBSet>();

                                foreach (var pointOfSalesItem in pointOfSalesItems)
                                {
                                    var item = await (
                                        from d in _dbContext.MstArticleItems
                                        where d.ArticleId == pointOfSalesItem.ItemId
                                        && d.MstArticle_ArticleId.IsLocked == true
                                        select d
                                    ).FirstOrDefaultAsync();

                                    if (item != null)
                                    {
                                        Int32? articleItemInventoryId = null;

                                        if (item.IsInventory == true)
                                        {
                                            var itemInventory = await (
                                                 from d in _dbContext.MstArticleItemInventories
                                                 where d.ArticleId == pointOfSalesItem.ItemId
                                                 && d.BranchId == loginUser.BranchId
                                                 select d
                                            ).FirstOrDefaultAsync();

                                            if (itemInventory != null)
                                            {
                                                articleItemInventoryId = itemInventory.Id;
                                            }
                                        }

                                        var discount = await (
                                            from d in _dbContext.MstDiscounts
                                            select d
                                        ).FirstOrDefaultAsync();

                                        if (discount != null)
                                        {
                                            var itemUnit = await (
                                                from d in _dbContext.MstArticleItemUnits
                                                where d.ArticleId == item.ArticleId
                                                && d.UnitId == item.UnitId
                                                select d
                                            ).FirstOrDefaultAsync();

                                            if (itemUnit != null)
                                            {
                                                Decimal VATAmount = (pointOfSalesItem.Amount / ((item.MstTax_SIVATId.TaxRate / 100) + 1)) * (item.MstTax_SIVATId.TaxRate / 100);
                                                Decimal WTAXAmount = (pointOfSalesItem.Amount / ((item.MstTax_WTAXId.TaxRate / 100) + 1)) * (item.MstTax_WTAXId.TaxRate / 100);

                                                Decimal baseQuantity = pointOfSalesItem.Quantity;
                                                if (itemUnit.Multiplier > 0)
                                                {
                                                    baseQuantity = pointOfSalesItem.Quantity * (1 / itemUnit.Multiplier);
                                                }

                                                Decimal baseNetPrice = pointOfSalesItem.Amount;
                                                if (baseQuantity > 0)
                                                {
                                                    baseNetPrice = pointOfSalesItem.Amount / baseQuantity;
                                                }

                                                newSalesInvoiceItems.Add(new DBSets.TrnSalesInvoiceItemDBSet
                                                {
                                                    SIId = SIId,
                                                    ItemId = item.ArticleId,
                                                    ItemInventoryId = articleItemInventoryId,
                                                    Particulars = pointOfSalesItem.Particulars,
                                                    Quantity = pointOfSalesItem.Quantity,
                                                    UnitId = item.UnitId,
                                                    Price = pointOfSalesItem.Price,
                                                    DiscountId = discount.Id,
                                                    DiscountRate = discount.DiscountRate,
                                                    DiscountAmount = pointOfSalesItem.Discount,
                                                    NetPrice = pointOfSalesItem.NetPrice,
                                                    Amount = pointOfSalesItem.Amount,
                                                    VATId = item.SIVATId,
                                                    VATRate = item.MstTax_SIVATId.TaxRate,
                                                    VATAmount = VATAmount,
                                                    WTAXId = item.WTAXId,
                                                    WTAXRate = item.MstTax_WTAXId.TaxRate,
                                                    WTAXAmount = WTAXAmount,
                                                    BaseQuantity = baseQuantity,
                                                    BaseUnitId = item.UnitId,
                                                    BaseNetPrice = baseNetPrice,
                                                    LineTimeStamp = DateTime.Now
                                                });
                                            }
                                        }
                                    }
                                }

                                _dbContext.TrnSalesInvoiceItems.AddRange(newSalesInvoiceItems);
                                await _dbContext.SaveChangesAsync();
                            }

                            var salesInvoice = await (
                                from d in _dbContext.TrnSalesInvoices
                                where d.Id == SIId
                                select d
                            ).FirstOrDefaultAsync();

                            if (salesInvoice != null)
                            {
                                var salesInvoiceItemsByCurrentSalesInvoice = await (
                                    from d in _dbContext.TrnSalesInvoiceItems
                                    where d.SIId == SIId
                                    select d
                                ).ToListAsync();

                                Decimal totalAmount = 0;

                                if (salesInvoiceItemsByCurrentSalesInvoice.Any())
                                {
                                    totalAmount = salesInvoiceItemsByCurrentSalesInvoice.Sum(d => d.Amount);
                                }

                                var lockSalesInvoice = salesInvoice;
                                lockSalesInvoice.Amount = totalAmount;
                                lockSalesInvoice.IsLocked = true;
                                lockSalesInvoice.UpdatedByUserId = loginUserId;
                                lockSalesInvoice.UpdatedDateTime = DateTime.Now;

                                await _dbContext.SaveChangesAsync();

                                await _sysAccountsReceivable.UpdateAccountsReceivable(SIId);
                                await _sysInventory.InsertSalesInvoiceInventory(SIId);
                                await _sysJournalEntry.InsertSalesInvoiceJournalEntry(SIId);

                                var pointOfSalesCustomerData = await (
                                    from d in _dbContext.TrnPointOfSales
                                    where d.BranchId == loginUser.BranchId
                                    && d.TerminalCode == terminalCode
                                    && d.POSDate == Convert.ToDateTime(date)
                                    && d.CustomerId == pointOfSalesCustomer.CustomerId
                                    select d
                                ).ToListAsync();

                                if (pointOfSalesCustomerData.Any())
                                {
                                    pointOfSalesCustomerData.ForEach(d => d.PostCode = salesInvoice.SINumber);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                        }
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
        public async Task<ActionResult> PrintSalesInvoice(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
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

                        var salesInvoice = await (
                             from d in _dbContext.TrnSalesInvoices
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (salesInvoice != null)
                        {
                            String reprinted = "";
                            if (salesInvoice.IsPrinted == true)
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("SALES INVOICE", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String customer = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                              salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "";
                            String customerAddress = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Address : "";
                            String customerContactNumber = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactNumber : "";
                            String customerContactPerson = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactPerson : "";
                            String term = salesInvoice.MstTerm_TermId.Term;
                            String remarks = salesInvoice.Remarks;

                            String branch = salesInvoice.MstCompanyBranch_BranchId.Branch;
                            String SINumber = "SI-" + salesInvoice.MstCompanyBranch_BranchId.ManualCode + "-" + salesInvoice.SINumber;
                            String SIDate = salesInvoice.SIDate.ToString("MMMM dd, yyyy");
                            String DateNeeded = salesInvoice.DateNeeded.ToString("MMMM dd, yyyy");
                            String manualNumber = salesInvoice.ManualNumber;
                            String documentReference = salesInvoice.DocumentReference;
                            String salesPerson = salesInvoice.MstUser_SoldByUserId.Fullname;

                            PdfPTable tableSalesInvoice = new PdfPTable(4);
                            tableSalesInvoice.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableSalesInvoice.WidthPercentage = 100;

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Customer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customer, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SINumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Address:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerAddress, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Contact No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerContactNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SIDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Contact Person:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerContactPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Date Needed:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(DateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Business Style:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Term:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(term, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Remarks:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Sales:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(salesPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableSalesInvoice);

                            var salesInvoiceItems = await (
                                from d in _dbContext.TrnSalesInvoiceItems
                                where d.SIId == id
                                select d
                            ).ToListAsync();

                            if (salesInvoiceItems.Any())
                            {
                                PdfPTable tableJobOrders = new PdfPTable(6);
                                tableJobOrders.SetWidths(new float[] { 70f, 70f, 150f, 120f, 80f, 80f });
                                tableJobOrders.WidthPercentage = 100;
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Qty.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Unit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Item", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Price", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                                foreach (var salesInvoiceItem in salesInvoiceItems)
                                {
                                    String SKUCode = salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             salesInvoiceItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";

                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.NetPrice.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }

                                tableJobOrders.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase(salesInvoiceItems.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                                document.Add(tableJobOrders);
                            }

                            String preparedBy = salesInvoice.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = salesInvoice.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = salesInvoice.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(4);
                            tableUsers.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Received by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
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
                            "No rights to print sales invoice"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales invoice"
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

        [HttpGet("print/salesInvoiceJobOrders/{SIId}")]
        public async Task<ActionResult> PrintSalesInvoiceJobOrders(Int32 SIId)
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
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
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

                        var salesInvoice = await (
                             from d in _dbContext.TrnSalesInvoices
                             where d.Id == SIId
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (salesInvoice != null)
                        {
                            String reprinted = "";
                            if (salesInvoice.IsPrinted == true)
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("SALES INVOICE - JOB ORDERS", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String customer = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                              salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "";
                            String customerAddress = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Address : "";
                            String customerContactNumber = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactNumber : "";
                            String customerContactPerson = salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesInvoice.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactPerson : "";
                            String term = salesInvoice.MstTerm_TermId.Term;
                            String remarks = salesInvoice.Remarks;

                            String branch = salesInvoice.MstCompanyBranch_BranchId.Branch;
                            String SINumber = "SI-" + salesInvoice.MstCompanyBranch_BranchId.ManualCode + "-" + salesInvoice.SINumber;
                            String SIDate = salesInvoice.SIDate.ToString("MMMM dd, yyyy");
                            String DateNeeded = salesInvoice.DateNeeded.ToString("MMMM dd, yyyy");
                            String manualNumber = salesInvoice.ManualNumber;
                            String documentReference = salesInvoice.DocumentReference;
                            String salesPerson = salesInvoice.MstUser_SoldByUserId.Fullname;

                            PdfPTable tableSalesInvoice = new PdfPTable(4);
                            tableSalesInvoice.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableSalesInvoice.WidthPercentage = 100;

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Customer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customer, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SINumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Address:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerAddress, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Contact No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerContactNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SIDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Contact Person:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customerContactPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Date Needed:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(DateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Business Style:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Term:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(term, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Remarks:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Sales:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesInvoice.AddCell(new PdfPCell(new Phrase(salesPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableSalesInvoice);

                            var jobOrders = await (
                                from d in _dbContext.TrnJobOrders
                                where d.SIId == SIId
                                select d
                            ).ToListAsync();

                            if (jobOrders.Any())
                            {
                                foreach (var jobOrder in jobOrders)
                                {
                                    PdfPTable tableJobOrders = new PdfPTable(7);
                                    tableJobOrders.SetWidths(new float[] { 70f, 50f, 150f, 120f, 130f, 70f, 65f });
                                    tableJobOrders.WidthPercentage = 100;
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("Qty.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("Unit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("Item", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("Job Type", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("JO No.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase("JO Date", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                                    String SKUCode = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             jobOrder.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";
                                    String jobType = jobOrder.MstJobType_ItemJobTypeId.JobType;

                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobOrder.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobOrder.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobOrder.Remarks, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobType, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobOrder.JONumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(jobOrder.JODate.ToString("MM/dd/yyyy"), fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                    PdfPTable tableJobOrderInformationAndAttachment = new PdfPTable(3);
                                    tableJobOrderInformationAndAttachment.SetWidths(new float[] { 50f, 3f, 50f });
                                    tableJobOrderInformationAndAttachment.WidthPercentage = 100;
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("Information", fontSegoeUI09Bold)) { Border = PdfCell.TOP_BORDER | PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f });
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("Attachment", fontSegoeUI09Bold)) { Border = PdfCell.TOP_BORDER | PdfCell.BOTTOM_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                                    var jobOrderInformations = await (
                                        from d in _dbContext.TrnJobOrderInformations
                                        where d.JOId == jobOrder.Id
                                        && d.Value != String.Empty
                                        select d
                                    ).ToListAsync();

                                    PdfPTable tableJobOrderInformation = new PdfPTable(4);
                                    tableJobOrderInformation.SetWidths(new float[] { 10f, 50f, 50f, 70f });
                                    tableJobOrderInformation.WidthPercentage = 100;

                                    if (jobOrderInformations.Any())
                                    {
                                        var groupedJobOrderInformationGroups = from d in jobOrderInformations
                                                                               group d by new
                                                                               {
                                                                                   d.InformationGroup
                                                                               }
                                                                               into g
                                                                               select new
                                                                               {
                                                                                   g.Key.InformationGroup
                                                                               };

                                        if (groupedJobOrderInformationGroups.ToList().Any())
                                        {
                                            foreach (var groupedJobOrderInformationGroup in groupedJobOrderInformationGroups)
                                            {
                                                tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationGroup.InformationGroup, fontSegoeUI09Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });

                                                var groupedJobOrderInformationValues = from d in jobOrderInformations
                                                                                       where d.InformationGroup == groupedJobOrderInformationGroup.InformationGroup
                                                                                       select d;

                                                if (groupedJobOrderInformationValues.Any())
                                                {
                                                    foreach (var groupedJobOrderInformationValue in groupedJobOrderInformationValues)
                                                    {
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationValue.InformationCode + ": ", fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationValue.Value, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationValue.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    var jobOrderAttachments = await (
                                        from d in _dbContext.TrnJobOrderAttachments
                                        where d.JOId == jobOrder.Id
                                        && d.AttachmentURL != String.Empty
                                        select d
                                    ).ToListAsync();

                                    PdfPTable tableJobOrderAttachment = new PdfPTable(1);
                                    tableJobOrderAttachment.SetWidths(new float[] { 100f });
                                    tableJobOrderAttachment.WidthPercentage = 100;

                                    if (jobOrderAttachments.Any())
                                    {
                                        foreach (var jobOrderAttachment in jobOrderAttachments)
                                        {
                                            tableJobOrderAttachment.AddCell(new PdfPCell(new Phrase(jobOrderAttachment.AttachmentCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                            if (String.IsNullOrEmpty(jobOrderAttachment.AttachmentURL) == true)
                                            {
                                                tableJobOrderAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            }
                                            else
                                            {
                                                Image attachmentPhoto = Image.GetInstance(new Uri(jobOrderAttachment.AttachmentURL));
                                                PdfPCell attachmentPhotoPdfCell = new PdfPCell(attachmentPhoto, true) { };
                                                attachmentPhotoPdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                                tableJobOrderAttachment.AddCell(new PdfPCell(attachmentPhotoPdfCell) { Border = 0, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            }
                                        }
                                    }

                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(tableJobOrderInformation) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(tableJobOrderAttachment) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderInformationAndAttachment.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { Border = 0, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 3 });

                                    tableJobOrders.AddCell(new PdfPCell(tableJobOrderInformationAndAttachment) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 7 });
                                    document.Add(tableJobOrders);
                                }
                            }

                            String preparedBy = salesInvoice.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = salesInvoice.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = salesInvoice.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(4);
                            tableUsers.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Received by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
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
                            "No rights to print sales invoice - job orders"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales invoice - job orders"
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
