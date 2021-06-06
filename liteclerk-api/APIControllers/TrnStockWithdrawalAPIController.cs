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
    public class TrnStockWithdrawalAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly Modules.SysInventoryModule _sysInventory;

        public TrnStockWithdrawalAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
            _sysInventory = new Modules.SysInventoryModule(dbContext);
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
        public async Task<ActionResult> GetStockWithdrawalListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnStockWithdrawalDTO> stockWithdrawals = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.BranchId == loginUser.BranchId
                    && d.SWDate >= Convert.ToDateTime(startDate)
                    && d.SWDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnStockWithdrawalDTO
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
                        SWNumber = d.SWNumber,
                        SWDate = d.SWDate.ToShortDateString(),
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
                        FromBranchId = d.FromBranchId,
                        FromBranch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
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

                return StatusCode(200, stockWithdrawals);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/paginated/{column}/{skip}/{take}")]
        public async Task<ActionResult> GetPaginatedStockWithdrawalListByDateRanged(String startDate, String endDate, String column, Int32 skip, Int32 take, String keywords)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var stockWithdrawals = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.BranchId == loginUser.BranchId
                    && d.SWDate >= Convert.ToDateTime(startDate)
                    && d.SWDate <= Convert.ToDateTime(endDate)
                    && d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true
                    && (
                        keywords == "" || String.IsNullOrEmpty(keywords) ? true :
                        column == "All" ? d.SWNumber.Contains(keywords) ||
                                          d.ManualNumber.Contains(keywords) ||
                                          d.MstCompanyBranch_FromBranchId.Branch.Contains(keywords) ||
                                          d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer.Contains(keywords) ||
                                          d.DocumentReference.Contains(keywords) ||
                                          d.Remarks.Contains(keywords) ||
                                          d.Status.Contains(keywords) :
                        column == "OT No." ? d.SWNumber.Contains(keywords) :
                        column == "Manual No." ? d.ManualNumber.Contains(keywords) :
                        column == "Branch" ? d.MstCompanyBranch_FromBranchId.Branch.Contains(keywords) :
                        column == "Customer" ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer.Contains(keywords) :
                        column == "Document Reference" ? d.DocumentReference.Contains(keywords) :
                        column == "Remarks" ? d.Remarks.Contains(keywords) :
                        column == "Status" ? d.Status.Contains(keywords) : true
                    )
                    select new DTO.TrnStockWithdrawalDTO
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
                        SWNumber = d.SWNumber,
                        SWDate = d.SWDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleCustomerDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.ManualCode
                            },
                            Customer = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer
                        },
                        FromBranchId = d.FromBranchId,
                        FromBranch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_FromBranchId.ManualCode,
                            Branch = d.MstCompanyBranch_FromBranchId.Branch
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
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

                return StatusCode(200, stockWithdrawals.OrderByDescending(d => d.Id).Skip(skip).Take(take));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetStockWithdrawalDetail(Int32 id)
        {
            try
            {
                DTO.TrnStockWithdrawalDTO stockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.Id == id
                    select new DTO.TrnStockWithdrawalDTO
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
                        SWNumber = d.SWNumber,
                        SWDate = d.SWDate.ToShortDateString(),
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
                        FromBranchId = d.FromBranchId,
                        FromBranch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Address = d.Address,
                        ContactPerson = d.ContactPerson,
                        ContactNumber = d.ContactNumber,
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

                return StatusCode(200, stockWithdrawal);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStockWithdrawal()
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a stock withdrawal.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a stock withdrawal.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == salesInvoice.CustomerId
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "STOCK WITHDRAWAL STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String SWNumber = "0000000001";
                DBSets.TrnStockWithdrawalDBSet lastStockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastStockWithdrawal != null)
                {
                    Int32 lastSWNumber = Convert.ToInt32(lastStockWithdrawal.SWNumber) + 0000000001;
                    SWNumber = PadZeroes(lastSWNumber, 10);
                }

                DBSets.TrnStockWithdrawalDBSet newStockWithdrawal = new DBSets.TrnStockWithdrawalDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    SWNumber = SWNumber,
                    SWDate = DateTime.Today,
                    ManualNumber = SWNumber,
                    DocumentReference = "",
                    CustomerId = salesInvoice.CustomerId,
                    FromBranchId = salesInvoice.BranchId,
                    SIId = salesInvoice.Id,
                    Address = customer.Address,
                    ContactPerson = customer.ContactPerson,
                    ContactNumber = customer.ContactNumber,
                    Remarks = "",
                    Amount = 0,
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

                _dbContext.TrnStockWithdrawals.Add(newStockWithdrawal);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newStockWithdrawal.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveStockWithdrawal(Int32 id, [FromBody] DTO.TrnStockWithdrawalDTO trnStockWithdrawalDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a stock withdrawal.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a stock withdrawal.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                    from d in _dbContext.TrnStockWithdrawals
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a stock withdrawal that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnStockWithdrawalDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnStockWithdrawalDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstCompanyBranchDBSet fromBranch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnStockWithdrawalDTO.FromBranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (fromBranch == null)
                {
                    return StatusCode(404, "From branch not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnStockWithdrawalDTO.SIId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                DBSets.MstUserDBSet receivedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (receivedByUser == null)
                {
                    return StatusCode(404, "Received by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnStockWithdrawalDTO.Status
                    && d.Category == "STOCK WITHDRAWAL STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnStockWithdrawalDBSet saveStockWithdrawal = stockWithdrawal;
                saveStockWithdrawal.CurrencyId = trnStockWithdrawalDTO.CurrencyId;
                saveStockWithdrawal.SWDate = Convert.ToDateTime(trnStockWithdrawalDTO.SWDate);
                saveStockWithdrawal.ManualNumber = trnStockWithdrawalDTO.ManualNumber;
                saveStockWithdrawal.DocumentReference = trnStockWithdrawalDTO.DocumentReference;
                saveStockWithdrawal.CustomerId = trnStockWithdrawalDTO.CustomerId;
                saveStockWithdrawal.FromBranchId = trnStockWithdrawalDTO.FromBranchId;
                saveStockWithdrawal.SIId = trnStockWithdrawalDTO.SIId;
                saveStockWithdrawal.Address = trnStockWithdrawalDTO.Address;
                saveStockWithdrawal.ContactPerson = trnStockWithdrawalDTO.ContactPerson;
                saveStockWithdrawal.ContactPerson = trnStockWithdrawalDTO.ContactPerson;
                saveStockWithdrawal.Remarks = trnStockWithdrawalDTO.Remarks;
                saveStockWithdrawal.ReceivedByUserId = trnStockWithdrawalDTO.ReceivedByUserId;
                saveStockWithdrawal.CheckedByUserId = trnStockWithdrawalDTO.CheckedByUserId;
                saveStockWithdrawal.ApprovedByUserId = trnStockWithdrawalDTO.ApprovedByUserId;
                saveStockWithdrawal.Status = trnStockWithdrawalDTO.Status;
                saveStockWithdrawal.UpdatedByUserId = loginUserId;
                saveStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockStockWithdrawal(Int32 id, [FromBody] DTO.TrnStockWithdrawalDTO trnStockWithdrawalDTO)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a stock withdrawal.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a stock withdrawal.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a stock withdrawal that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnStockWithdrawalDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnStockWithdrawalDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstCompanyBranchDBSet fromBranch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnStockWithdrawalDTO.FromBranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (fromBranch == null)
                {
                    return StatusCode(404, "From branch not found.");
                }

                DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnStockWithdrawalDTO.SIId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales invoice not found.");
                }

                DBSets.MstUserDBSet receivedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.ReceivedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (receivedByUser == null)
                {
                    return StatusCode(404, "Received by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnStockWithdrawalDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnStockWithdrawalDTO.Status
                    && d.Category == "STOCK WITHDRAWAL STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnStockWithdrawalDBSet lockStockWithdrawal = stockWithdrawal;
                lockStockWithdrawal.CurrencyId = trnStockWithdrawalDTO.CurrencyId;
                lockStockWithdrawal.SWDate = Convert.ToDateTime(trnStockWithdrawalDTO.SWDate);
                lockStockWithdrawal.ManualNumber = trnStockWithdrawalDTO.ManualNumber;
                lockStockWithdrawal.DocumentReference = trnStockWithdrawalDTO.DocumentReference;
                lockStockWithdrawal.CustomerId = trnStockWithdrawalDTO.CustomerId;
                lockStockWithdrawal.FromBranchId = trnStockWithdrawalDTO.FromBranchId;
                lockStockWithdrawal.SIId = trnStockWithdrawalDTO.SIId;
                lockStockWithdrawal.Address = trnStockWithdrawalDTO.Address;
                lockStockWithdrawal.ContactPerson = trnStockWithdrawalDTO.ContactPerson;
                lockStockWithdrawal.ContactPerson = trnStockWithdrawalDTO.ContactPerson;
                lockStockWithdrawal.Remarks = trnStockWithdrawalDTO.Remarks;
                lockStockWithdrawal.ReceivedByUserId = trnStockWithdrawalDTO.ReceivedByUserId;
                lockStockWithdrawal.CheckedByUserId = trnStockWithdrawalDTO.CheckedByUserId;
                lockStockWithdrawal.ApprovedByUserId = trnStockWithdrawalDTO.ApprovedByUserId;
                lockStockWithdrawal.Status = trnStockWithdrawalDTO.Status;
                lockStockWithdrawal.IsLocked = true;
                lockStockWithdrawal.UpdatedByUserId = loginUserId;
                lockStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysInventory.InsertStockWithdrawalInventory(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockStockWithdrawal(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a stock withdrawal.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a stock withdrawal.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a stock withdrawal that is unlocked.");
                }

                DBSets.TrnStockWithdrawalDBSet unlockStockWithdrawal = stockWithdrawal;
                unlockStockWithdrawal.IsLocked = false;
                unlockStockWithdrawal.UpdatedByUserId = loginUserId;
                unlockStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                await _sysInventory.DeleteStockWithdrawalInventory(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelStockWithdrawal(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a stock withdrawal.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a stock withdrawal.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a stock withdrawal that is unlocked.");
                }

                DBSets.TrnStockWithdrawalDBSet cancelStockWithdrawal = stockWithdrawal;
                cancelStockWithdrawal.IsCancelled = true;
                cancelStockWithdrawal.UpdatedByUserId = loginUserId;
                cancelStockWithdrawal.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStockWithdrawal(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a stock withdrawal.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a stock withdrawal.");
                }

                DBSets.TrnStockWithdrawalDBSet stockWithdrawal = await (
                     from d in _dbContext.TrnStockWithdrawals
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (stockWithdrawal == null)
                {
                    return StatusCode(404, "Stock out not found.");
                }

                if (stockWithdrawal.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a stock withdrawal that is locked.");
                }

                _dbContext.TrnStockWithdrawals.Remove(stockWithdrawal);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintStockWithdrawal(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityStockWithdrawalDetail"
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

                        var stockWithdrawal = await (
                             from d in _dbContext.TrnStockWithdrawals
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (stockWithdrawal != null)
                        {
                            String reprinted = "";
                            if (stockWithdrawal.IsPrinted == true)
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("STOCK WITHDRAWAL", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String SIranch = stockWithdrawal.TrnSalesInvoice_SIId.MstCompanyBranch_BranchId.Branch;
                            String SINumber = stockWithdrawal.TrnSalesInvoice_SIId.SINumber;
                            String contractPerson = stockWithdrawal.ContactPerson;
                            String contactNumber = stockWithdrawal.ContactNumber;
                            String address = stockWithdrawal.Address;
                            String remarks = stockWithdrawal.Remarks;

                            String branch = stockWithdrawal.MstCompanyBranch_BranchId.Branch;
                            String SWNumber = "SW-" + stockWithdrawal.MstCompanyBranch_BranchId.ManualCode + "-" + stockWithdrawal.SWNumber;
                            String SWDate = stockWithdrawal.SWDate.ToString("MMMM dd, yyyy");
                            String manualNumber = stockWithdrawal.ManualNumber;
                            String documentReference = stockWithdrawal.DocumentReference;

                            PdfPTable tableStockWithdrawal = new PdfPTable(4);
                            tableStockWithdrawal.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableStockWithdrawal.WidthPercentage = 100;

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("SI Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(SIranch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(SWNumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("SI No.:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(SINumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Contact Person:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(contractPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(SWDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Contact No.:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(contactNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Address:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(address, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("Remarks", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableStockWithdrawal.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableStockWithdrawal);

                            PdfPTable tableStockWithdrawalItems = new PdfPTable(6);
                            tableStockWithdrawalItems.SetWidths(new float[] { 70f, 70f, 150f, 120f, 80f, 80f });
                            tableStockWithdrawalItems.WidthPercentage = 100;
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Qty.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Unit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Item", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Cost", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            var stockWithdrawalItems = await (
                                from d in _dbContext.TrnStockWithdrawalItems
                                where d.SWId == id
                                select d
                            ).ToListAsync();

                            if (stockWithdrawalItems.Any())
                            {
                                foreach (var stockWithdrawalItem in stockWithdrawalItems)
                                {
                                    String SKUCode = stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             stockWithdrawalItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";

                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItem.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItem.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItem.Cost.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(stockWithdrawalItems.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableStockWithdrawalItems.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableStockWithdrawalItems);

                            String preparedBy = stockWithdrawal.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = stockWithdrawal.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = stockWithdrawal.MstUser_ApprovedByUserId.Fullname;

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
                            "No rights to print stock withdrawal"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print stock withdrawal"
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
