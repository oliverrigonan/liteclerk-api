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
    public class TrnSalesInvoiceAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public TrnSalesInvoiceAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        [HttpGet("list/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<DTO.TrnSalesInvoiceDTO>>> GetSalesInvoiceListFilteredByDateRanged(String startDate, String endDate)
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
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Customer.ArticleCode,
                            ManualCode = d.MstArticle_Customer.ManualCode,
                            Article = d.MstArticle_Customer.Article
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_Term.TermCode,
                            ManualCode = d.MstTerm_Term.ManualCode,
                            Term = d.MstTerm_Term.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUser.Username,
                            Fullname = d.MstUser_SoldByUser.Fullname
                        },
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUser.Username,
                            Fullname = d.MstUser_PreparedByUser.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUser.Username,
                            Fullname = d.MstUser_CheckedByUser.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUser.Username,
                            Fullname = d.MstUser_ApprovedByUser.Fullname
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
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTO.TrnSalesInvoiceDTO>> GetSalesInvoiceDetail(Int32 id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Id == userId
                    select d
                ).FirstOrDefaultAsync();

                DTO.TrnSalesInvoiceDTO salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == id
                    select new DTO.TrnSalesInvoiceDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_Branch.BranchCode,
                            ManualCode = d.MstCompanyBranch_Branch.ManualCode,
                            Branch = d.MstCompanyBranch_Branch.Branch
                        },
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            CurrencyCode = d.MstCurrency_Currency.CurrencyCode,
                            ManualCode = d.MstCurrency_Currency.ManualCode,
                            Currency = d.MstCurrency_Currency.Currency
                        },
                        SINumber = d.SINumber,
                        SIDate = d.SIDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_Customer.ArticleCode,
                            ManualCode = d.MstArticle_Customer.ManualCode,
                            Article = d.MstArticle_Customer.Article
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            TermCode = d.MstTerm_Term.TermCode,
                            ManualCode = d.MstTerm_Term.ManualCode,
                            Term = d.MstTerm_Term.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        SoldByUserId = d.SoldByUserId,
                        SoldByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SoldByUser.Username,
                            Fullname = d.MstUser_SoldByUser.Fullname
                        },
                        PreparedByUserId = d.PreparedByUserId,
                        PreparedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_PreparedByUser.Username,
                            Fullname = d.MstUser_PreparedByUser.Fullname
                        },
                        CheckedByUserId = d.CheckedByUserId,
                        CheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CheckedByUser.Username,
                            Fullname = d.MstUser_CheckedByUser.Fullname
                        },
                        ApprovedByUserId = d.ApprovedByUserId,
                        ApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_ApprovedByUser.Username,
                            Fullname = d.MstUser_ApprovedByUser.Fullname
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
                            Username = d.MstUser_CreatedByUser.Username,
                            Fullname = d.MstUser_CreatedByUser.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUser.Username,
                            Fullname = d.MstUser_UpdatedByUser.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).FirstOrDefaultAsync();

                return StatusCode(200, salesInvoice);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<DTO.MstCompanyDTO>> AddSalesInvoice()
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

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_Article.IsLocked == true
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
                    BranchId = user.BranchId,
                    CurrencyId = user.MstCompany_Company.CurrencyId,
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
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<IActionResult> SaveSalesInvoice(Int32 id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
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
                    && d.MstArticle_Article.IsLocked == true
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
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockSalesInvoice(Int32 id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
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
                    && d.MstArticle_Article.IsLocked == true
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

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnlockSalesInvoice(Int32 id)
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

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelSalesInvoice(Int32 id)
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

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSalesInvoice(Int32 id)
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
                return StatusCode(500, e.Message);
            }
        }
    }
}
