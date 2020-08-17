using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.APIControllers
{
    [Authorize]
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

        [HttpGet("customer/dropdown/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleCustomerDTO>>> CustomerDropdownList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> customers = await _dbContext.MstArticleCustomers
                    .Select(d =>
                        new DTO.MstArticleCustomerDTO
                        {
                            Id = d.Id,
                            ArticleId = d.ArticleId,
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Customer = d.Customer
                        })
                    .ToListAsync();

                return StatusCode(200, customers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("term/dropdown/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstTermDTO>>> TermDropdownList()
        {
            try
            {
                IEnumerable<DTO.MstTermDTO> terms = await _dbContext.MstTerms
                    .Select(d =>
                        new DTO.MstTermDTO
                        {
                            Id = d.Id,
                            TermCode = d.Term,
                            ManualCode = d.ManualCode,
                            Term = d.Term
                        })
                    .ToListAsync();

                return StatusCode(200, terms);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("user/dropdown/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstUserDTO>>> UserDropdownList()
        {
            try
            {
                IEnumerable<DTO.MstUserDTO> users = await _dbContext.MstUsers
                    .Select(d =>
                        new DTO.MstUserDTO
                        {
                            Id = d.Id,
                            Username = d.Username,
                            Fullname = d.Fullname
                        })
                    .ToListAsync();

                return StatusCode(200, users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("list/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<DTO.TrnSalesInvoiceDTO>>> SalesInvoiceList(String startDate, String endDate)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                IEnumerable<DTO.TrnSalesInvoiceDTO> salesInvoices = await _dbContext.TrnSalesInvoices
                    .Where(d =>
                        d.BranchId == user.BranchId &&
                        d.SIDate >= Convert.ToDateTime(startDate) &&
                        d.SIDate <= Convert.ToDateTime(endDate))
                    .Select(d =>
                        new DTO.TrnSalesInvoiceDTO
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            Branch = d.MstCompanyBranch_Branch.Branch,
                            CurrencyId = d.CurrencyId,
                            Currency = d.MstCurrency_Currency.Currency,
                            SINumber = d.SINumber,
                            SIDate = d.SIDate.ToShortDateString(),
                            ManualNumber = d.ManualNumber,
                            DocumentReference = d.DocumentReference,
                            CustomerId = d.CustomerId,
                            Customer = d.MstArticle_Customer.Article,
                            TermId = d.TermId,
                            Term = d.MstTerm_Term.Term,
                            DateNeeded = d.DateNeeded.ToShortDateString(),
                            Remarks = d.Remarks,
                            SoldByUserId = d.SoldByUserId,
                            SoldByUserFullname = d.MstUser_SoldByUser.Fullname,
                            PreparedByUserId = d.PreparedByUserId,
                            PreparedByUserFullname = d.MstUser_PreparedByUser.Fullname,
                            CheckedByUserId = d.CheckedByUserId,
                            CheckedByUserFullname = d.MstUser_CheckedByUser.Fullname,
                            ApprovedByUserId = d.ApprovedByUserId,
                            ApprovedByUserFullname = d.MstUser_ApprovedByUser.Fullname,
                            Amount = d.Amount,
                            PaidAmount = d.PaidAmount,
                            AdjustmentAmount = d.AdjustmentAmount,
                            BalanceAmount = d.BalanceAmount,
                            Status = d.Status,
                            IsCancelled = d.IsCancelled,
                            IsPrinted = d.IsPrinted,
                            IsLocked = d.IsLocked,
                            CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                            CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                            UpdatedByUserFullname = d.MstUser_PreparedByUser.Fullname,
                            UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString(),
                        })
                    .ToListAsync();

                return StatusCode(200, salesInvoices);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTO.TrnSalesInvoiceDTO>> SalesInvoiceDetail(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

                if (salesInvoice == null)
                {
                    return StatusCode(404, new DTO.TrnSalesInvoiceDTO());
                }

                DBSets.TrnSalesInvoiceDBSet d = salesInvoice;
                DTO.TrnSalesInvoiceDTO salesInvoiceDetail = new DTO.TrnSalesInvoiceDTO
                {
                    Id = d.Id,
                    BranchId = d.BranchId,
                    Branch = d.MstCompanyBranch_Branch.Branch,
                    CurrencyId = d.CurrencyId,
                    Currency = d.MstCurrency_Currency.Currency,
                    SINumber = d.SINumber,
                    SIDate = d.SIDate.ToShortDateString(),
                    ManualNumber = d.ManualNumber,
                    DocumentReference = d.DocumentReference,
                    CustomerId = d.CustomerId,
                    Customer = d.MstArticle_Customer.Article,
                    TermId = d.TermId,
                    Term = d.MstTerm_Term.Term,
                    DateNeeded = d.DateNeeded.ToShortDateString(),
                    Remarks = d.Remarks,
                    SoldByUserId = d.SoldByUserId,
                    SoldByUserFullname = d.MstUser_SoldByUser.Fullname,
                    PreparedByUserId = d.PreparedByUserId,
                    PreparedByUserFullname = d.MstUser_PreparedByUser.Fullname,
                    CheckedByUserId = d.CheckedByUserId,
                    CheckedByUserFullname = d.MstUser_CheckedByUser.Fullname,
                    ApprovedByUserId = d.ApprovedByUserId,
                    ApprovedByUserFullname = d.MstUser_ApprovedByUser.Fullname,
                    Amount = d.Amount,
                    PaidAmount = d.PaidAmount,
                    AdjustmentAmount = d.AdjustmentAmount,
                    BalanceAmount = d.BalanceAmount,
                    Status = d.Status,
                    IsCancelled = d.IsCancelled,
                    IsPrinted = d.IsPrinted,
                    IsLocked = d.IsLocked,
                    CreatedByUserFullname = d.MstUser_CreatedByUser.Fullname,
                    CreatedByDateTime = d.CreatedByDateTime.ToShortDateString(),
                    UpdatedByUserFullname = d.MstUser_PreparedByUser.Fullname,
                    UpdatedByDateTime = d.UpdatedByDateTime.ToShortDateString()
                };

                return StatusCode(200, salesInvoiceDetail);
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

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await _dbContext.MstArticleCustomers
                    .Where(d => d.MstArticle_Article.IsLocked == true)
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                String SINumber = "0000000001";
                DBSets.TrnSalesInvoiceDBSet lastSalesInvoice = await _dbContext.TrnSalesInvoices
                    .Where(d => d.BranchId == user.BranchId)
                    .OrderByDescending(d => d.Id)
                    .FirstOrDefaultAsync();

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
                    CustomerId = customer.Id,
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
                    CreatedByDateTime = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedByDateTime = DateTime.Now
                };

                _dbContext.TrnSalesInvoices.Add(newSalesInvoice);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, SalesInvoiceDetail(newSalesInvoice.Id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<IActionResult> SaveSalesInvoice(int id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a sales invoice that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await _dbContext.MstCurrencies
                    .Where(d => d.Id == trnSalesInvoiceDTO.CurrencyId)
                    .FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await _dbContext.MstArticleCustomers
                    .Where(d =>
                        d.MstArticle_Article.Id == trnSalesInvoiceDTO.CustomerId &&
                        d.MstArticle_Article.IsLocked == true)
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstTermDBSet term = await _dbContext.MstTerms
                    .Where(d => d.Id == trnSalesInvoiceDTO.TermId)
                    .FirstOrDefaultAsync();

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
                saveSalesInvoice.UpdatedByDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockSalesInvoice(int id, [FromBody] DTO.TrnSalesInvoiceDTO trnSalesInvoiceDTO)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a sales invoice that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await _dbContext.MstCurrencies
                    .Where(d => d.Id == trnSalesInvoiceDTO.CurrencyId)
                    .FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await _dbContext.MstArticleCustomers
                    .Where(d =>
                        d.MstArticle_Article.Id == trnSalesInvoiceDTO.CustomerId &&
                        d.MstArticle_Article.IsLocked == true)
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstTermDBSet term = await _dbContext.MstTerms
                    .Where(d => d.Id == trnSalesInvoiceDTO.TermId)
                    .FirstOrDefaultAsync();

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
                lockSalesInvoice.UpdatedByDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnlockSalesInvoice(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

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
                unlockSalesInvoice.UpdatedByDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelSalesInvoice(int id)
        {
            try
            {
                Int32 userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet user = await _dbContext.MstUsers
                    .Where(d => d.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode(404, "User login not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

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
                unlockSalesInvoice.UpdatedByDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSalesInvoice(int id)
        {
            try
            {

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await _dbContext.TrnSalesInvoices.FindAsync(id);

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
