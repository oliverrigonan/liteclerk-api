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

        public TrnSalesInvoiceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
            _sysAccountsReceivable = new Modules.SysAccountsReceivableModule(dbContext);
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

        [HttpGet("list/byCustomer/{customerId}")]
        public async Task<ActionResult> GetSalesInvoiceListByCustomer(Int32 customerId)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnSalesInvoiceDTO> salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == user.BranchId
                    && d.CustomerId == customerId
                    && d.IsLocked == true
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
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
                            TermCode = d.MstTerm_TermId.TermCode,
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

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetSalesInvoiceListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnSalesInvoiceDTO> salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == user.BranchId
                    && d.SIDate >= Convert.ToDateTime(startDate)
                    && d.SIDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnSalesInvoiceDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
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
                            TermCode = d.MstTerm_TermId.TermCode,
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
                DTO.TrnSalesInvoiceDTO salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == id
                    select new DTO.TrnSalesInvoiceDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_CurrencyId.CurrencyCode,
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
                            TermCode = d.MstTerm_TermId.TermCode,
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
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceList"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to add a sales invoice.");
                }

                if (userForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a sales invoice.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                String SINumber = "0000000001";
                DBSets.TrnSalesInvoiceDBSet lastSalesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.BranchId == user.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastSalesInvoice != null)
                {
                    Int32 lastSINumber = Convert.ToInt32(lastSalesInvoice.SINumber) + 0000000001;
                    SINumber = PadZeroes(lastSINumber, 10);
                }

                DBSets.TrnSalesInvoiceDBSet newSalesInvoice = new DBSets.TrnSalesInvoiceDBSet()
                {
                    BranchId = Convert.ToInt32(user.BranchId),
                    CurrencyId = user.MstCompany_CompanyId.CurrencyId,
                    SINumber = SINumber,
                    SIDate = DateTime.Today,
                    ManualNumber = SINumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    TermId = customer.TermId,
                    DateNeeded = DateTime.Today,
                    Remarks = "",
                    SoldByUserId = userId,
                    PreparedByUserId = userId,
                    CheckedByUserId = userId,
                    ApprovedByUserId = userId,
                    Amount = 0,
                    PaidAmount = 0,
                    AdjustmentAmount = 0,
                    BalanceAmount = 0,
                    Status = "",
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = userId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = userId,
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
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a sales invoice.");
                }

                if (userForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a sales invoice.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesInvoiceDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesInvoiceDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesInvoiceDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.TrnSalesInvoiceDBSet saveSalesInvoice = salesInvoice;
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
                saveSalesInvoice.UpdatedByUserId = userId;
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
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to lock a sales invoice.");
                }

                if (userForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a sales invoice.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesInvoiceDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesInvoiceDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesInvoiceDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesInvoiceDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.TrnSalesInvoiceDBSet lockSalesInvoice = salesInvoice;
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
                lockSalesInvoice.UpdatedByUserId = userId;
                lockSalesInvoice.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysAccountsReceivable.UpdateAccountsReceivable(id);

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
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to unlock a sales invoice.");
                }

                if (userForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a sales invoice.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                DBSets.TrnSalesInvoiceDBSet unlockSalesInvoice = salesInvoice;
                unlockSalesInvoice.IsLocked = false;
                unlockSalesInvoice.UpdatedByUserId = userId;
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

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelSalesInvoice(Int32 id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to cancel a sales invoice.");
                }

                if (userForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a sales invoice.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                DBSets.TrnSalesInvoiceDBSet unlockSalesInvoice = salesInvoice;
                unlockSalesInvoice.IsCancelled = true;
                unlockSalesInvoice.UpdatedByUserId = userId;
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
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceList"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm == null)
                {
                    return StatusCode(404, "No rights to delete a sales invoice.");
                }

                if (userForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a sales invoice.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

            Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            DBSets.MstUserDBSet user = await (
                from d in _dbContext.MstUsers
                where d.Id == userId
                select d
            ).FirstOrDefaultAsync();

            if (user != null)
            {
                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm != null)
                {
                    if (userForm.CanPrint == true)
                    {
                        String companyName = "";
                        String companyAddress = "";
                        String companyTaxNumber = "";

                        if (user.CompanyId != null)
                        {
                            companyName = user.MstCompany_CompanyId.Company;
                            companyAddress = user.MstCompany_CompanyId.Address;
                            companyTaxNumber = user.MstCompany_CompanyId.TIN;
                        }

                        DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                            PdfPTable tableHeader = new PdfPTable(2);
                            tableHeader.SetWidths(new float[] { 70f, 30f });
                            tableHeader.WidthPercentage = 100f;
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontSegoeUI13Bold)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase("LOGO", fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 3f, Rowspan = 4 });
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

                            IEnumerable<DBSets.TrnSalesInvoiceItemDBSet> salesInvoiceItems = await (
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
                        Paragraph paragraph = new Paragraph();
                        paragraph.Add("No rights to print sales invoice");

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Add("No rights to print sales invoice");

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

            Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            DBSets.MstUserDBSet user = await (
                from d in _dbContext.MstUsers
                where d.Id == userId
                select d
            ).FirstOrDefaultAsync();

            if (user != null)
            {
                DBSets.MstUserFormDBSet userForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == userId
                    && d.SysForm_FormId.Form == "ActivitySalesInvoiceDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (userForm != null)
                {
                    if (userForm.CanPrint == true)
                    {
                        String companyName = "";
                        String companyAddress = "";
                        String companyTaxNumber = "";

                        if (user.CompanyId != null)
                        {
                            companyName = user.MstCompany_CompanyId.Company;
                            companyAddress = user.MstCompany_CompanyId.Address;
                            companyTaxNumber = user.MstCompany_CompanyId.TIN;
                        }

                        DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
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

                            PdfPTable tableHeader = new PdfPTable(2);
                            tableHeader.SetWidths(new float[] { 70f, 30f });
                            tableHeader.WidthPercentage = 100f;
                            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontSegoeUI13Bold)) { Border = 0 });
                            tableHeader.AddCell(new PdfPCell(new Phrase("LOGO", fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 3f, Rowspan = 4 });
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

                            IEnumerable<DBSets.TrnJobOrderDBSet> jobOrders = await (
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

                                    IEnumerable<DBSets.TrnJobOrderInformationDBSet> jobOrderInformations = await (
                                        from d in _dbContext.TrnJobOrderInformations
                                        where d.JOId == jobOrder.Id
                                        && d.Value != String.Empty
                                        select d
                                    ).ToListAsync();

                                    PdfPTable tableJobOrderInformation = new PdfPTable(3);
                                    tableJobOrderInformation.SetWidths(new float[] { 10f, 50f, 70f });
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
                                                tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationGroup.InformationGroup, fontSegoeUI09Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 3 });

                                                var groupedJobOrderInformationValues = from d in jobOrderInformations
                                                                                       where d.InformationGroup == groupedJobOrderInformationGroup.InformationGroup
                                                                                       select d;

                                                if (groupedJobOrderInformationValues.Any())
                                                {
                                                    foreach (var groupedJobOrderInformationValue in groupedJobOrderInformationValues)
                                                    {
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationValue.Value, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                        tableJobOrderInformation.AddCell(new PdfPCell(new Phrase(groupedJobOrderInformationValue.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    IEnumerable<DBSets.TrnJobOrderAttachmentDBSet> jobOrderAttachments = await (
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

                                            tableJobOrderAttachment.AddCell(new PdfPCell(new Phrase(jobOrderAttachment.AttachmentType, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
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
                        Paragraph paragraph = new Paragraph();
                        paragraph.Add("No rights to print sales invoice - job orders");

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Add("No rights to print sales invoice - job orders");

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
