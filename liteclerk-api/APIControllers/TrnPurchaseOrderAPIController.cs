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
    public class TrnPurchaseOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPurchaseOrderAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetPurchaseOrderListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnPurchaseOrderDTO> purchaseOrders = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.BranchId == loginUser.BranchId
                    && d.PODate >= Convert.ToDateTime(startDate)
                    && d.PODate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnPurchaseOrderDTO
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
                        PONumber = d.PONumber,
                        PODate = d.PODate.ToShortDateString(),
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
                        PRId = d.PRId,
                        PurchaseRequest = new DTO.TrnPurchaseRequestDTO
                        {
                            PRNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.PRNumber : "",
                            ManualNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.ManualNumber : "",
                        },
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

                return StatusCode(200, purchaseOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/bySupplier/{supplierId}")]
        public async Task<ActionResult> GetPurchaseOrderListBySupplier(Int32 supplierId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnPurchaseOrderDTO> purchaseOrders = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.BranchId == loginUser.BranchId
                    && d.SupplierId == supplierId
                    && d.IsLocked == true
                    orderby d.Id descending
                    select new DTO.TrnPurchaseOrderDTO
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
                        PONumber = d.PONumber,
                        PODate = d.PODate.ToShortDateString(),
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
                        PRId = d.PRId,
                        PurchaseRequest = new DTO.TrnPurchaseRequestDTO
                        {
                            PRNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.PRNumber : "",
                            ManualNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.ManualNumber : "",
                        },
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

                return StatusCode(200, purchaseOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetPurchaseOrderDetail(Int32 id)
        {
            try
            {
                DTO.TrnPurchaseOrderDTO purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == id
                    select new DTO.TrnPurchaseOrderDTO
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
                        PONumber = d.PONumber,
                        PODate = d.PODate.ToShortDateString(),
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
                        PRId = d.PRId,
                        PurchaseRequest = new DTO.TrnPurchaseRequestDTO
                        {
                            PRNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.PRNumber : "",
                            ManualNumber = d.PRId != null ? d.TrnPurchaseRequest_PRId.ManualNumber : "",
                        },
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

                return StatusCode(200, purchaseOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPurchaseOrder()
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a purchase order.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a purchase order.");
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
                    where d.Category == "PURCHASE ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String PONumber = "0000000001";
                DBSets.TrnPurchaseOrderDBSet lastPurchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastPurchaseOrder != null)
                {
                    Int32 lastPONumber = Convert.ToInt32(lastPurchaseOrder.PONumber) + 0000000001;
                    PONumber = PadZeroes(lastPONumber, 10);
                }

                DBSets.TrnPurchaseOrderDBSet newPurchaseOrder = new DBSets.TrnPurchaseOrderDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    PONumber = PONumber,
                    PODate = DateTime.Today,
                    ManualNumber = PONumber,
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

                _dbContext.TrnPurchaseOrders.Add(newPurchaseOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newPurchaseOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SavePurchaseOrder(Int32 id, [FromBody] DTO.TrnPurchaseOrderDTO trnPurchaseOrderDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a purchase order.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a purchase order.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                    from d in _dbContext.TrnPurchaseOrders
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a purchase order that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPurchaseOrderDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseOrderDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseOrderDTO.Status
                    && d.Category == "PURCHASE ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnPurchaseOrderDBSet savePurchaseOrder = purchaseOrder;
                savePurchaseOrder.CurrencyId = trnPurchaseOrderDTO.CurrencyId;
                savePurchaseOrder.PODate = Convert.ToDateTime(trnPurchaseOrderDTO.PODate);
                savePurchaseOrder.ManualNumber = trnPurchaseOrderDTO.ManualNumber;
                savePurchaseOrder.DocumentReference = trnPurchaseOrderDTO.DocumentReference;
                savePurchaseOrder.SupplierId = trnPurchaseOrderDTO.SupplierId;
                savePurchaseOrder.TermId = trnPurchaseOrderDTO.TermId;
                savePurchaseOrder.DateNeeded = Convert.ToDateTime(trnPurchaseOrderDTO.DateNeeded);
                savePurchaseOrder.Remarks = trnPurchaseOrderDTO.Remarks;
                savePurchaseOrder.RequestedByUserId = trnPurchaseOrderDTO.RequestedByUserId;
                savePurchaseOrder.CheckedByUserId = trnPurchaseOrderDTO.CheckedByUserId;
                savePurchaseOrder.ApprovedByUserId = trnPurchaseOrderDTO.ApprovedByUserId;
                savePurchaseOrder.Status = trnPurchaseOrderDTO.Status;
                savePurchaseOrder.UpdatedByUserId = loginUserId;
                savePurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockPurchaseOrder(Int32 id, [FromBody] DTO.TrnPurchaseOrderDTO trnPurchaseOrderDTO)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a purchase order.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a purchase order.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                     from d in _dbContext.TrnPurchaseOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a purchase order that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnPurchaseOrderDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnPurchaseOrderDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstTermDBSet term = await (
                    from d in _dbContext.MstTerms
                    where d.Id == trnPurchaseOrderDTO.TermId
                    select d
                ).FirstOrDefaultAsync();

                if (term == null)
                {
                    return StatusCode(404, "Term not found.");
                }

                DBSets.MstUserDBSet requestedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.RequestedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (requestedByUser == null)
                {
                    return StatusCode(404, "Ordered by user not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnPurchaseOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnPurchaseOrderDTO.Status
                    && d.Category == "PURCHASE ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DBSets.TrnPurchaseOrderDBSet lockPurchaseOrder = purchaseOrder;
                lockPurchaseOrder.CurrencyId = trnPurchaseOrderDTO.CurrencyId;
                lockPurchaseOrder.PODate = Convert.ToDateTime(trnPurchaseOrderDTO.PODate);
                lockPurchaseOrder.ManualNumber = trnPurchaseOrderDTO.ManualNumber;
                lockPurchaseOrder.DocumentReference = trnPurchaseOrderDTO.DocumentReference;
                lockPurchaseOrder.SupplierId = trnPurchaseOrderDTO.SupplierId;
                lockPurchaseOrder.TermId = trnPurchaseOrderDTO.TermId;
                lockPurchaseOrder.DateNeeded = Convert.ToDateTime(trnPurchaseOrderDTO.DateNeeded);
                lockPurchaseOrder.Remarks = trnPurchaseOrderDTO.Remarks;
                lockPurchaseOrder.RequestedByUserId = trnPurchaseOrderDTO.RequestedByUserId;
                lockPurchaseOrder.CheckedByUserId = trnPurchaseOrderDTO.CheckedByUserId;
                lockPurchaseOrder.ApprovedByUserId = trnPurchaseOrderDTO.ApprovedByUserId;
                lockPurchaseOrder.Status = trnPurchaseOrderDTO.Status;
                lockPurchaseOrder.IsLocked = true;
                lockPurchaseOrder.UpdatedByUserId = loginUserId;
                lockPurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockPurchaseOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a purchase order.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a purchase order.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                     from d in _dbContext.TrnPurchaseOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a purchase order that is unlocked.");
                }

                DBSets.TrnPurchaseOrderDBSet unlockPurchaseOrder = purchaseOrder;
                unlockPurchaseOrder.IsLocked = false;
                unlockPurchaseOrder.UpdatedByUserId = loginUserId;
                unlockPurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelPurchaseOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a purchase order.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a purchase order.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                     from d in _dbContext.TrnPurchaseOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a purchase order that is unlocked.");
                }

                DBSets.TrnPurchaseOrderDBSet cancelPurchaseOrder = purchaseOrder;
                cancelPurchaseOrder.IsCancelled = true;
                cancelPurchaseOrder.UpdatedByUserId = loginUserId;
                cancelPurchaseOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePurchaseOrder(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityPurchaseOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a purchase order.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a purchase order.");
                }

                DBSets.TrnPurchaseOrderDBSet purchaseOrder = await (
                     from d in _dbContext.TrnPurchaseOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (purchaseOrder == null)
                {
                    return StatusCode(404, "Purchase order not found.");
                }

                if (purchaseOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a purchase order that is locked.");
                }

                _dbContext.TrnPurchaseOrders.Remove(purchaseOrder);
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
