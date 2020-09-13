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
    public class TrnCollectionAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly Modules.SysAccountsReceivableModule _sysAccountsReceivable;

        public TrnCollectionAPIController(DBContext.LiteclerkDBContext dbContext)
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
        public async Task<ActionResult> GetCollectionListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnCollectionDTO> collections = await (
                    from d in _dbContext.TrnCollections
                    where d.BranchId == loginUser.BranchId
                    && d.CIDate >= Convert.ToDateTime(startDate)
                    && d.CIDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnCollectionDTO
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
                        CINumber = d.CINumber,
                        CIDate = d.CIDate.ToShortDateString(),
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
                        Remarks = d.Remarks,
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

                return StatusCode(200, collections);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetCollectionDetail(Int32 id)
        {
            try
            {
                DTO.TrnCollectionDTO salesInvoice = await (
                    from d in _dbContext.TrnCollections
                    where d.Id == id
                    select new DTO.TrnCollectionDTO
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
                        CINumber = d.CINumber,
                        CIDate = d.CIDate.ToShortDateString(),
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
                        Remarks = d.Remarks,
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

                return StatusCode(200, salesInvoice);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCollection()
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
                    && d.SysForm_FormId.Form == "ActivityCollectionList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a collection.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a collection.");
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

                String CINumber = "0000000001";
                DBSets.TrnCollectionDBSet lastCollection = await (
                    from d in _dbContext.TrnCollections
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastCollection != null)
                {
                    Int32 lastCINumber = Convert.ToInt32(lastCollection.CINumber) + 0000000001;
                    CINumber = PadZeroes(lastCINumber, 10);
                }

                DBSets.TrnCollectionDBSet newCollection = new DBSets.TrnCollectionDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    CINumber = CINumber,
                    CIDate = DateTime.Today,
                    ManualNumber = CINumber,
                    DocumentReference = "",
                    CustomerId = customer.ArticleId,
                    Remarks = "",
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Amount = 0,
                    Status = "",
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnCollections.Add(newCollection);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newCollection.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveCollection(Int32 id, [FromBody] DTO.TrnCollectionDTO trnCollectionDTO)
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
                    && d.SysForm_FormId.Form == "ActivityCollectionDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a collection.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a collection.");
                }

                DBSets.TrnCollectionDBSet salesInvoice = await (
                    from d in _dbContext.TrnCollections
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a collection that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnCollectionDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnCollectionDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnCollectionDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by loginUser not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnCollectionDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by loginUser not found.");
                }

                DBSets.TrnCollectionDBSet saveCollection = salesInvoice;
                saveCollection.CurrencyId = trnCollectionDTO.CurrencyId;
                saveCollection.CIDate = Convert.ToDateTime(trnCollectionDTO.CIDate);
                saveCollection.ManualNumber = trnCollectionDTO.ManualNumber;
                saveCollection.DocumentReference = trnCollectionDTO.DocumentReference;
                saveCollection.CustomerId = trnCollectionDTO.CustomerId;
                saveCollection.Remarks = trnCollectionDTO.Remarks;
                saveCollection.CheckedByUserId = trnCollectionDTO.CheckedByUserId;
                saveCollection.ApprovedByUserId = trnCollectionDTO.ApprovedByUserId;
                saveCollection.Status = trnCollectionDTO.Status;
                saveCollection.UpdatedByUserId = loginUserId;
                saveCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockCollection(Int32 id, [FromBody] DTO.TrnCollectionDTO trnCollectionDTO)
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
                    && d.SysForm_FormId.Form == "ActivityCollectionDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a collection.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a collection.");
                }

                DBSets.TrnCollectionDBSet salesInvoice = await (
                     from d in _dbContext.TrnCollections
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a collection that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnCollectionDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleCustomerDBSet customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.ArticleId == trnCollectionDTO.CustomerId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnCollectionDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by loginUser not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnCollectionDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by loginUser not found.");
                }

                DBSets.TrnCollectionDBSet lockCollection = salesInvoice;
                lockCollection.CurrencyId = trnCollectionDTO.CurrencyId;
                lockCollection.CIDate = Convert.ToDateTime(trnCollectionDTO.CIDate);
                lockCollection.ManualNumber = trnCollectionDTO.ManualNumber;
                lockCollection.DocumentReference = trnCollectionDTO.DocumentReference;
                lockCollection.CustomerId = trnCollectionDTO.CustomerId;
                lockCollection.Remarks = trnCollectionDTO.Remarks;
                lockCollection.CheckedByUserId = trnCollectionDTO.CheckedByUserId;
                lockCollection.ApprovedByUserId = trnCollectionDTO.ApprovedByUserId;
                lockCollection.Status = trnCollectionDTO.Status;
                lockCollection.IsLocked = true;
                lockCollection.UpdatedByUserId = loginUserId;
                lockCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == id
                    && d.SIId != null
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    foreach (var collectionLineByCurrentCollection in collectionLinesByCurrentCollection)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(collectionLineByCurrentCollection.SIId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockCollection(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityCollectionDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a collection.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a collection.");
                }

                DBSets.TrnCollectionDBSet salesInvoice = await (
                     from d in _dbContext.TrnCollections
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a collection that is unlocked.");
                }

                DBSets.TrnCollectionDBSet unlockCollection = salesInvoice;
                unlockCollection.IsLocked = false;
                unlockCollection.UpdatedByUserId = loginUserId;
                unlockCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == id
                    && d.SIId != null
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    foreach (var collectionLineByCurrentCollection in collectionLinesByCurrentCollection)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(collectionLineByCurrentCollection.SIId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelCollection(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityCollectionDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a collection.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a collection.");
                }

                DBSets.TrnCollectionDBSet salesInvoice = await (
                     from d in _dbContext.TrnCollections
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a collection that is unlocked.");
                }

                DBSets.TrnCollectionDBSet unlockCollection = salesInvoice;
                unlockCollection.IsCancelled = true;
                unlockCollection.UpdatedByUserId = loginUserId;
                unlockCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == id
                    && d.SIId != null
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    foreach (var collectionLineByCurrentCollection in collectionLinesByCurrentCollection)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(collectionLineByCurrentCollection.SIId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCollection(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityCollectionList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a collection.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a collection.");
                }

                DBSets.TrnCollectionDBSet salesInvoice = await (
                     from d in _dbContext.TrnCollections
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a collection that is locked.");
                }

                _dbContext.TrnCollections.Remove(salesInvoice);
                await _dbContext.SaveChangesAsync();

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == id
                    && d.SIId != null
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    foreach (var collectionLineByCurrentCollection in collectionLinesByCurrentCollection)
                    {
                        await _sysAccountsReceivable.UpdateAccountsReceivable(Convert.ToInt32(collectionLineByCurrentCollection.SIId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
