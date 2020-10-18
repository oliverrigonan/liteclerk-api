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

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnPurchaseRequestDTO> purchaseRequests = await (
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

                return StatusCode(200, purchaseRequests);
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
                DTO.TrnPurchaseRequestDTO purchaseRequest = await (
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
                    where d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String PRNumber = "0000000001";
                DBSets.TrnPurchaseRequestDBSet lastPurchaseRequest = await (
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

                DBSets.TrnPurchaseRequestDBSet newPurchaseRequest = new DBSets.TrnPurchaseRequestDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    PRNumber = PRNumber,
                    PRDate = DateTime.Today,
                    ManualNumber = PRNumber,
                    DocumentReference = "",
                    SupplierId = supplier.ArticleId,
                    TermId = supplier.TermId,
                    DateNeeded = DateTime.Today.AddDays(Convert.ToDouble(supplier.MstTerm_TermId.NumberOfDays)),
                    Remarks = "",
                    RequestedByUserId = loginUserId,
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Amount = 0,
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

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
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

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseRequestDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPurchaseRequestDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseRequestDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Requested by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseRequestDTO.Status
                    && d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnPurchaseRequestDBSet savePurchaseRequest = purchaseRequest;
                savePurchaseRequest.CurrencyId = trnPurchaseRequestDTO.CurrencyId;
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

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
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

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseRequestDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPurchaseRequestDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseRequestDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Requested by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseRequestDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseRequestDTO.Status
                    && d.Category == "PURCHASE REQUEST STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnPurchaseRequestDBSet lockPurchaseRequest = purchaseRequest;
                lockPurchaseRequest.CurrencyId = trnPurchaseRequestDTO.CurrencyId;
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

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
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

                DBSets.TrnPurchaseRequestDBSet unlockPurchaseRequest = purchaseRequest;
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

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
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

                DBSets.TrnPurchaseRequestDBSet cancelPurchaseRequest = purchaseRequest;
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

                DBSets.TrnPurchaseRequestDBSet purchaseRequest = await (
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
    }
}
