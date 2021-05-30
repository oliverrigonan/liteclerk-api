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
    public class TrnPurchaseRequestAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPurchaseRequestAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
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
        public async Task<ActionResult> GetPurchaseRequestListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var purchaseRequests = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.BranchId == loginUser.BranchId
                    && d.PRDate >= Convert.ToDateTime(startDate)
                    && d.PRDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnPurchaseRequestDTO
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
                        PRNumber = d.PRNumber,
                        PRDate = d.PRDate.ToShortDateString(),
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
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        RequestedByUserId = d.RequestedByUserId,
                        RequestedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_RequestedByUserId.Username,
                            Fullname = d.MstUser_RequestedByUserId.Fullname
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

                return StatusCode(200, purchaseRequests);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/paginated/{column}/{skip}/{take}")]
        public async Task<ActionResult> GetPaginatedPurchaseRequestListByDateRanged(String startDate, String endDate, String column, Int32 skip, Int32 take, String keywords)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var purchaseRequests = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.BranchId == loginUser.BranchId
                    && d.PRDate >= Convert.ToDateTime(startDate)
                    && d.PRDate <= Convert.ToDateTime(endDate)
                    && d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    && (
                        keywords == "" || String.IsNullOrEmpty(keywords) ? true :
                        column == "All" ? d.PRNumber.Contains(keywords) ||
                                          d.ManualNumber.Contains(keywords) ||
                                          d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier.Contains(keywords) ||
                                          d.DocumentReference.Contains(keywords) ||
                                          d.Remarks.Contains(keywords) ||
                                          d.Status.Contains(keywords) :
                        column == "PR No." ? d.PRNumber.Contains(keywords) :
                        column == "Manual No." ? d.ManualNumber.Contains(keywords) :
                        column == "Supplier" ? d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier.Contains(keywords) :
                        column == "Document Reference" ? d.DocumentReference.Contains(keywords) :
                        column == "Remarks" ? d.Remarks.Contains(keywords) :
                        column == "Status" ? d.Status.Contains(keywords) : true
                    )
                    select new DTO.TrnPurchaseRequestDTO
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
                        PRNumber = d.PRNumber,
                        PRDate = d.PRDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        SupplierId = d.SupplierId,
                        Supplier = new DTO.MstArticleSupplierDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_SupplierId.ManualCode
                            },
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier,
                        },
                        TermId = d.TermId,
                        Term = new DTO.MstTermDTO
                        {
                            ManualCode = d.MstTerm_TermId.ManualCode,
                            Term = d.MstTerm_TermId.Term
                        },
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        RequestedByUserId = d.RequestedByUserId,
                        RequestedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_RequestedByUserId.Username,
                            Fullname = d.MstUser_RequestedByUserId.Fullname
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

                return StatusCode(200, purchaseRequests.OrderByDescending(d => d.Id).Skip(skip).Take(take));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPurchaseRequestDetail(Int32 id)
        {
            try
            {
                var purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == id
                    select new DTO.TrnPurchaseRequestDTO
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
                        PRNumber = d.PRNumber,
                        PRDate = d.PRDate.ToShortDateString(),
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
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        Remarks = d.Remarks,
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
                        RequestedByUserId = d.RequestedByUserId,
                        RequestedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_RequestedByUserId.Username,
                            Fullname = d.MstUser_RequestedByUserId.Fullname
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

                return StatusCode(200, purchaseRequest);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPurchaseRequest()
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a purchase request.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a purchase request.");
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
                    where d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String PRNumber = "0000000001";
                var lastPurchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastPurchaseRequest != null)
                {
                    Int32 lastPRNumber = Convert.ToInt32(lastPurchaseRequest.PRNumber) + 0000000001;
                    PRNumber = PadZeroes(lastPRNumber, 10);
                }

                var newPurchaseRequest = new DBSets.TrnPurchaseRequestDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeCurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeRate = 0,
                    PRNumber = PRNumber,
                    PRDate = DateTime.Today,
                    ManualNumber = PRNumber,
                    DocumentReference = "",
                    SupplierId = supplier.ArticleId,
                    TermId = supplier.TermId,
                    DateNeeded = DateTime.Today.AddDays(Convert.ToDouble(supplier.MstTerm_TermId.NumberOfDays)),
                    Remarks = "",
                    Amount = 0,
                    BaseAmount = 0,
                    RequestedByUserId = loginUserId,
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

                _dbContext.TrnPurchaseRequests.Add(newPurchaseRequest);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newPurchaseRequest.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SavePurchaseRequest(Int32 id, [FromBody] DTO.TrnPurchaseRequestDTO trnPurchaseRequestDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a purchase request.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a purchase request.");
                }

                var purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a purchase request that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseRequestDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                     from d in _dbContext.MstArticleSuppliers
                     where d.ArticleId == trnPurchaseRequestDTO.SupplierId
                     && d.MstArticle_ArticleId.IsLocked == true
                     select d
                 ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseRequestDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Requested by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseRequestDTO.Status
                    && d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var savePurchaseRequest = purchaseRequest;
                savePurchaseRequest.ExchangeCurrencyId = trnPurchaseRequestDTO.ExchangeCurrencyId;
                savePurchaseRequest.ExchangeRate = trnPurchaseRequestDTO.ExchangeRate;
                savePurchaseRequest.PRDate = Convert.ToDateTime(trnPurchaseRequestDTO.PRDate);
                savePurchaseRequest.ManualNumber = trnPurchaseRequestDTO.ManualNumber;
                savePurchaseRequest.DocumentReference = trnPurchaseRequestDTO.DocumentReference;
                savePurchaseRequest.SupplierId = trnPurchaseRequestDTO.SupplierId;
                savePurchaseRequest.TermId = trnPurchaseRequestDTO.TermId;
                savePurchaseRequest.DateNeeded = Convert.ToDateTime(trnPurchaseRequestDTO.DateNeeded);
                savePurchaseRequest.Remarks = trnPurchaseRequestDTO.Remarks;
                savePurchaseRequest.RequestedByUserId = trnPurchaseRequestDTO.RequestedByUserId;
                savePurchaseRequest.CheckedByUserId = trnPurchaseRequestDTO.CheckedByUserId;
                savePurchaseRequest.ApprovedByUserId = trnPurchaseRequestDTO.ApprovedByUserId;
                savePurchaseRequest.Status = trnPurchaseRequestDTO.Status;
                savePurchaseRequest.UpdatedByUserId = loginUserId;
                savePurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockPurchaseRequest(Int32 id, [FromBody] DTO.TrnPurchaseRequestDTO trnPurchaseRequestDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a purchase request.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a purchase request.");
                }

                var purchaseRequest = await (
                     from d in _dbContext.TrnPurchaseRequests
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a purchase request that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseRequestDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                     from d in _dbContext.MstArticleSuppliers
                     where d.ArticleId == trnPurchaseRequestDTO.SupplierId
                     && d.MstArticle_ArticleId.IsLocked == true
                     select d
                 ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseRequestDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                var requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Requested by user not found.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseRequestDTO.Status
                    && d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockPurchaseRequest = purchaseRequest;
                lockPurchaseRequest.ExchangeCurrencyId = trnPurchaseRequestDTO.ExchangeCurrencyId;
                lockPurchaseRequest.ExchangeRate = trnPurchaseRequestDTO.ExchangeRate;
                lockPurchaseRequest.PRDate = Convert.ToDateTime(trnPurchaseRequestDTO.PRDate);
                lockPurchaseRequest.ManualNumber = trnPurchaseRequestDTO.ManualNumber;
                lockPurchaseRequest.DocumentReference = trnPurchaseRequestDTO.DocumentReference;
                lockPurchaseRequest.SupplierId = trnPurchaseRequestDTO.SupplierId;
                lockPurchaseRequest.TermId = trnPurchaseRequestDTO.TermId;
                lockPurchaseRequest.DateNeeded = Convert.ToDateTime(trnPurchaseRequestDTO.DateNeeded);
                lockPurchaseRequest.Remarks = trnPurchaseRequestDTO.Remarks;
                lockPurchaseRequest.RequestedByUserId = trnPurchaseRequestDTO.RequestedByUserId;
                lockPurchaseRequest.CheckedByUserId = trnPurchaseRequestDTO.CheckedByUserId;
                lockPurchaseRequest.ApprovedByUserId = trnPurchaseRequestDTO.ApprovedByUserId;
                lockPurchaseRequest.Status = trnPurchaseRequestDTO.Status;
                lockPurchaseRequest.IsLocked = true;
                lockPurchaseRequest.UpdatedByUserId = loginUserId;
                lockPurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockPurchaseRequest(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a purchase request.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a purchase request.");
                }

                var purchaseRequest = await (
                     from d in _dbContext.TrnPurchaseRequests
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a purchase request that is unlocked.");
                }

                var unlockPurchaseRequest = purchaseRequest;
                unlockPurchaseRequest.IsLocked = false;
                unlockPurchaseRequest.UpdatedByUserId = loginUserId;
                unlockPurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelPurchaseRequest(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a purchase request.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a purchase request.");
                }

                var purchaseRequest = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where d.Id == id
                    select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a purchase request that is unlocked.");
                }

                var cancelPurchaseRequest = purchaseRequest;
                cancelPurchaseRequest.IsCancelled = true;
                cancelPurchaseRequest.UpdatedByUserId = loginUserId;
                cancelPurchaseRequest.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePurchaseRequest(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a purchase request.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a purchase request.");
                }

                var purchaseRequest = await (
                     from d in _dbContext.TrnPurchaseRequests
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseRequest == null)
                {
                    return StatusCode(404, "Purchase request not found.");
                }

                if (purchaseRequest.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a purchase request that is locked.");
                }

                _dbContext.TrnPurchaseRequests.Remove(purchaseRequest);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintPurchaseRequest(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseRequestDetail"
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

                        var purchaseRequest = await (
                             from d in _dbContext.TrnPurchaseRequests
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (purchaseRequest != null)
                        {
                            String reprinted = "";
                            if (purchaseRequest.IsPrinted == true)
                            {
                                reprinted = "(REPRPRTED)";
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("PURCHASE REQUEST", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String supplier = purchaseRequest.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier;
                            String term = purchaseRequest.MstTerm_TermId.Term;
                            String dateNeeded = purchaseRequest.DateNeeded.ToShortDateString();
                            String remarks = purchaseRequest.Remarks;

                            String branch = purchaseRequest.MstCompanyBranch_BranchId.Branch;
                            String PRNumber = "PR-" + purchaseRequest.MstCompanyBranch_BranchId.ManualCode + "-" + purchaseRequest.PRNumber;
                            String PRDate = purchaseRequest.PRDate.ToString("MMMM dd, yyyy");
                            String manualNumber = purchaseRequest.ManualNumber;
                            String documentReference = purchaseRequest.DocumentReference;

                            PdfPTable tablePurchaseRequest = new PdfPTable(4);
                            tablePurchaseRequest.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tablePurchaseRequest.WidthPercentage = 100;

                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Supplier:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(supplier, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(PRNumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Term:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(term, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Date Needed", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(dateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(PRDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Remarks", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 2 });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f, Rowspan = 2 });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tablePurchaseRequest.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tablePurchaseRequest);

                            PdfPTable tablePurchaseRequestItems = new PdfPTable(6);
                            tablePurchaseRequestItems.SetWidths(new float[] { 70f, 70f, 150f, 120f, 80f, 80f });
                            tablePurchaseRequestItems.WidthPercentage = 100;
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Qty.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Unit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Item", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Cost", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            var purchaseRequestItems = await (
                                from d in _dbContext.TrnPurchaseRequestItems
                                where d.PRId == id
                                select d
                            ).ToListAsync();

                            if (purchaseRequestItems.Any())
                            {
                                foreach (var purchaseRequestItem in purchaseRequestItems)
                                {
                                    String SKUCode = purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "";
                                    String barCode = purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                     purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "";
                                    String itemDescription = purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ?
                                                             purchaseRequestItem.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "";

                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItem.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItem.MstUnit_UnitId.Unit, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(itemDescription + "\n" + SKUCode + "\n" + barCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItem.Cost.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 5 });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(purchaseRequestItems.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tablePurchaseRequestItems.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tablePurchaseRequestItems);

                            String preparedBy = purchaseRequest.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = purchaseRequest.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = purchaseRequest.MstUser_ApprovedByUserId.Fullname;
                            String requestedBy = purchaseRequest.MstUser_RequestedByUserId.Fullname;

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
                            tableUsers.AddCell(new PdfPCell(new Phrase(requestedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableUsers);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print purchase request"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print purchase request"
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
