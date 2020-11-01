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
    }
}
