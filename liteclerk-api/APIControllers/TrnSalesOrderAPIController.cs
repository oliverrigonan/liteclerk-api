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
    public class TrnSalesOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly Modules.SysAccountsReceivableModule _sysAccountsReceivable;

        public TrnSalesOrderAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpGet("list/byDateRange/{startDate}/{endDate}")]
        public async Task<ActionResult> GetSalesOrderListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesOrders = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.BranchId == loginUser.BranchId
                    && d.SODate >= Convert.ToDateTime(startDate)
                    && d.SODate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnSalesOrderDTO
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
                        SONumber = d.SONumber,
                        SODate = d.SODate.ToShortDateString(),
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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

                return StatusCode(200, salesOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byCustomer/{customerId}")]
        public async Task<ActionResult> GetSalesOrderListByCustomer(Int32 customerId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var salesOrders = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.BranchId == loginUser.BranchId
                    && d.CustomerId == customerId
                    && d.IsLocked == true
                    orderby d.Id descending
                    select new DTO.TrnSalesOrderDTO
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
                        SONumber = d.SONumber,
                        SODate = d.SODate.ToShortDateString(),
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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

                return StatusCode(200, salesOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetSalesOrderDetail(Int32 id)
        {
            try
            {
                var salesOrder = await (
                    from d in _dbContext.TrnSalesOrders
                    where d.Id == id
                    select new DTO.TrnSalesOrderDTO
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
                        SONumber = d.SONumber,
                        SODate = d.SODate.ToShortDateString(),
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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

                return StatusCode(200, salesOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddSalesOrder()
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a sales order.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a sales order.");
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
                    where d.Category == "SALES ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String SONumber = "0000000001";
                var lastSalesOrder = await (
                     from d in _dbContext.TrnSalesOrders
                     where d.BranchId == loginUser.BranchId
                     orderby d.Id descending
                     select d
                 ).FirstOrDefaultAsync();

                if (lastSalesOrder != null)
                {
                    Int32 lastSONumber = Convert.ToInt32(lastSalesOrder.SONumber) + 0000000001;
                    SONumber = PadZeroes(lastSONumber, 10);
                }

                var newSalesOrder = new DBSets.TrnSalesOrderDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeCurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeRate = 0,
                    SONumber = SONumber,
                    SODate = DateTime.Today,
                    ManualNumber = SONumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    TermId = customer.TermId,
                    DateNeeded = DateTime.Today,
                    Remarks = "",
                    Amount = 0,
                    BaseAmount = 0,
                    SoldByUserId = loginUserId,
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

                _dbContext.TrnSalesOrders.Add(newSalesOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newSalesOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveSalesOrder(Int32 id, [FromBody] DTO.TrnSalesOrderDTO trnSalesOrderDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a sales order.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a sales order.");
                }

                var salesOrder = await (
                     from d in _dbContext.TrnSalesOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a sales order that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesOrderDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesOrderDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesOrderDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnSalesOrderDTO.Status
                    && d.Category == "SALES ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveSalesOrder = salesOrder;
                saveSalesOrder.ExchangeCurrencyId = trnSalesOrderDTO.ExchangeCurrencyId;
                saveSalesOrder.ExchangeRate = trnSalesOrderDTO.ExchangeRate;
                saveSalesOrder.SODate = Convert.ToDateTime(trnSalesOrderDTO.SODate);
                saveSalesOrder.ManualNumber = trnSalesOrderDTO.ManualNumber;
                saveSalesOrder.DocumentReference = trnSalesOrderDTO.DocumentReference;
                saveSalesOrder.CustomerId = trnSalesOrderDTO.CustomerId;
                saveSalesOrder.TermId = trnSalesOrderDTO.TermId;
                saveSalesOrder.DateNeeded = Convert.ToDateTime(trnSalesOrderDTO.DateNeeded);
                saveSalesOrder.Remarks = trnSalesOrderDTO.Remarks;
                saveSalesOrder.SoldByUserId = trnSalesOrderDTO.SoldByUserId;
                saveSalesOrder.CheckedByUserId = trnSalesOrderDTO.CheckedByUserId;
                saveSalesOrder.ApprovedByUserId = trnSalesOrderDTO.ApprovedByUserId;
                saveSalesOrder.Status = trnSalesOrderDTO.Status;
                saveSalesOrder.UpdatedByUserId = loginUserId;
                saveSalesOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockSalesOrder(Int32 id, [FromBody] DTO.TrnSalesOrderDTO trnSalesOrderDTO)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a sales order.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a sales order.");
                }

                var salesOrder = await (
                      from d in _dbContext.TrnSalesOrders
                      where d.Id == id
                      select d
                  ).FirstOrDefaultAsync(); ;

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a sales order that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnSalesOrderDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnSalesOrderDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnSalesOrderDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var soldByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.SoldByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (soldByUser == null)
                {
                    return StatusCode(404, "Sold by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnSalesOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnSalesOrderDTO.Status
                    && d.Category == "SALES ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockSalesOrder = salesOrder;
                lockSalesOrder.ExchangeCurrencyId = trnSalesOrderDTO.ExchangeCurrencyId;
                lockSalesOrder.ExchangeRate = trnSalesOrderDTO.ExchangeRate;
                lockSalesOrder.CurrencyId = trnSalesOrderDTO.CurrencyId;
                lockSalesOrder.SODate = Convert.ToDateTime(trnSalesOrderDTO.SODate);
                lockSalesOrder.ManualNumber = trnSalesOrderDTO.ManualNumber;
                lockSalesOrder.DocumentReference = trnSalesOrderDTO.DocumentReference;
                lockSalesOrder.CustomerId = trnSalesOrderDTO.CustomerId;
                lockSalesOrder.TermId = trnSalesOrderDTO.TermId;
                lockSalesOrder.DateNeeded = Convert.ToDateTime(trnSalesOrderDTO.DateNeeded);
                lockSalesOrder.Remarks = trnSalesOrderDTO.Remarks;
                lockSalesOrder.SoldByUserId = trnSalesOrderDTO.SoldByUserId;
                lockSalesOrder.CheckedByUserId = trnSalesOrderDTO.CheckedByUserId;
                lockSalesOrder.ApprovedByUserId = trnSalesOrderDTO.ApprovedByUserId;
                lockSalesOrder.Status = trnSalesOrderDTO.Status;
                lockSalesOrder.IsLocked = true;
                lockSalesOrder.UpdatedByUserId = loginUserId;
                lockSalesOrder.UpdatedDateTime = DateTime.Now;

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
        public async Task<ActionResult> UnlockSalesOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a sales order.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a sales order.");
                }

                var salesOrder = await (
                      from d in _dbContext.TrnSalesOrders
                      where d.Id == id
                      select d
                  ).FirstOrDefaultAsync(); ;

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a sales order that is unlocked.");
                }

                var unlockSalesOrder = salesOrder;
                unlockSalesOrder.IsLocked = false;
                unlockSalesOrder.UpdatedByUserId = loginUserId;
                unlockSalesOrder.UpdatedDateTime = DateTime.Now;

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
        public async Task<ActionResult> CancelSalesOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a sales order.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a sales order.");
                }

                var salesOrder = await (
                      from d in _dbContext.TrnSalesOrders
                      where d.Id == id
                      select d
                  ).FirstOrDefaultAsync(); ;

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a sales order that is unlocked.");
                }

                var unlockSalesOrder = salesOrder;
                unlockSalesOrder.IsCancelled = true;
                unlockSalesOrder.UpdatedByUserId = loginUserId;
                unlockSalesOrder.UpdatedDateTime = DateTime.Now;

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
        public async Task<ActionResult> DeleteSalesOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a sales order.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a sales order.");
                }

                var salesOrder = await (
                      from d in _dbContext.TrnSalesOrders
                      where d.Id == id
                      select d
                  ).FirstOrDefaultAsync(); ;

                if (salesOrder == null)
                {
                    return StatusCode(404, "Sales order not found.");
                }

                if (salesOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a sales order that is locked.");
                }

                _dbContext.TrnSalesOrders.Remove(salesOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintSalesOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivitySalesOrderDetail"
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

                        var salesOrder = await (
                             from d in _dbContext.TrnSalesOrders
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (salesOrder != null)
                        {
                            String reprinted = "";
                            if (salesOrder.IsPrinted == true)
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("SALES ORDER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String customer = salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                              salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "";
                            String customerAddress = salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Address : "";
                            String customerContactNumber = salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactNumber : "";
                            String customerContactPerson = salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ?
                                                           salesOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactPerson : "";
                            String term = salesOrder.MstTerm_TermId.Term;
                            String remarks = salesOrder.Remarks;

                            String branch = salesOrder.MstCompanyBranch_BranchId.Branch;
                            String SONumber = "SO-" + salesOrder.MstCompanyBranch_BranchId.ManualCode + "-" + salesOrder.SONumber;
                            String SODate = salesOrder.SODate.ToString("MMMM dd, yyyy");
                            String DateNeeded = salesOrder.DateNeeded.ToString("MMMM dd, yyyy");
                            String manualNumber = salesOrder.ManualNumber;
                            String documentReference = salesOrder.DocumentReference;
                            String salesPerson = salesOrder.MstUser_SoldByUserId.Fullname;

                            PdfPTable tableSalesOrder = new PdfPTable(4);
                            tableSalesOrder.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableSalesOrder.WidthPercentage = 100;

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Customer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(customer, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(SONumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Address:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(customerAddress, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Contact No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(customerContactNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(SODate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Contact Person:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(customerContactPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Date Needed:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(DateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Business Style:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Term:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(term, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Remarks:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase("Sales:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableSalesOrder.AddCell(new PdfPCell(new Phrase(salesPerson, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableSalesOrder);

                            var salesOrderItems = await (
                                from d in _dbContext.TrnSalesOrderItems
                                where d.SOId == id
                                select d
                            ).ToListAsync();

                            if (salesOrderItems.Any())
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

                                foreach (var salesOrderItem in salesOrderItems)
                                {
                                    String SKUCode = salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             salesOrderItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";

                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItem.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItem.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItem.NetPrice.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }

                                tableJobOrders.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase(salesOrderItems.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                                tableJobOrders.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                                document.Add(tableJobOrders);
                            }

                            String preparedBy = salesOrder.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = salesOrder.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = salesOrder.MstUser_ApprovedByUserId.Fullname;

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
                            "No rights to print sales order"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales order"
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
