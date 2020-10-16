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
    public class TrnDisbursementAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsPayableModule _sysAccountsPayable;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnDisbursementAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            //_sysAccountsPayable = new Modules.SysAccountsPayableModule(dbContext);
            _sysJournalEntry = new Modules.SysJournalEntryModule(dbContext);
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
        public async Task<ActionResult> GetDisbursementListByDateRanged(String startDate, String endDate)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnDisbursementDTO> disbursements = await (
                    from d in _dbContext.TrnDisbursements
                    where d.BranchId == loginUser.BranchId
                    && d.CVDate >= Convert.ToDateTime(startDate)
                    && d.CVDate <= Convert.ToDateTime(endDate)
                    orderby d.Id descending
                    select new DTO.TrnDisbursementDTO
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
                        CVNumber = d.CVNumber,
                        CVDate = d.CVDate.ToShortDateString(),
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
                        Payee = d.Payee,
                        Remarks = d.Remarks,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        CheckNumber = d.CheckNumber,
                        CheckDate = d.CheckDate != null ? Convert.ToDateTime(d.CheckDate).ToShortDateString() : "",
                        CheckBank = d.CheckBank,
                        IsCrossCheck = d.IsCrossCheck,
                        BankId = d.BankId,
                        Bank = new DTO.MstArticleBankDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_BankId.ManualCode
                            },
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
                        },
                        IsClear = d.IsClear,
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

                return StatusCode(200, disbursements);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetDisbursementDetail(Int32 id)
        {
            try
            {
                DTO.TrnDisbursementDTO salesInvoice = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == id
                    select new DTO.TrnDisbursementDTO
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
                        CVNumber = d.CVNumber,
                        CVDate = d.CVDate.ToShortDateString(),
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
                        Payee = d.Payee,
                        Remarks = d.Remarks,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        CheckNumber = d.CheckNumber,
                        CheckDate = d.CheckDate != null ? Convert.ToDateTime(d.CheckDate).ToShortDateString() : "",
                        CheckBank = d.CheckBank,
                        IsCrossCheck = d.IsCrossCheck,
                        BankId = d.BankId,
                        Bank = new DTO.MstArticleBankDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_BankId.ManualCode
                            },
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
                        },
                        IsClear = d.IsClear,
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
        public async Task<ActionResult> AddDisbursement()
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a disbursement.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a disbursement.");
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

                DBSets.MstPayTypeDBSet payType = await (
                    from d in _dbContext.MstPayTypes
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                DBSets.MstArticleBankDBSet bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "DISBURSEMENT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String CVNumber = "0000000001";
                DBSets.TrnDisbursementDBSet lastDisbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastDisbursement != null)
                {
                    Int32 lastCVNumber = Convert.ToInt32(lastDisbursement.CVNumber) + 0000000001;
                    CVNumber = PadZeroes(lastCVNumber, 10);
                }

                DBSets.TrnDisbursementDBSet newDisbursement = new DBSets.TrnDisbursementDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    CVNumber = CVNumber,
                    CVDate = DateTime.Today,
                    ManualNumber = CVNumber,
                    DocumentReference = "",
                    SupplierId = supplier.ArticleId,
                    Payee = supplier.Supplier,
                    Remarks = "",
                    PayTypeId = payType.Id,
                    CheckNumber = "",
                    CheckDate = null,
                    CheckBank = "",
                    IsCrossCheck = false,
                    BankId = bank.Id,
                    IsClear = false,
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

                _dbContext.TrnDisbursements.Add(newDisbursement);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newDisbursement.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveDisbursement(Int32 id, [FromBody] DTO.TrnDisbursementDTO trnDisbursementDTO)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a disbursement.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a disbursement.");
                }

                DBSets.TrnDisbursementDBSet salesInvoice = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a disbursement that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnDisbursementDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnDisbursementDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstPayTypeDBSet payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnDisbursementDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                DBSets.MstArticleBankDBSet bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnDisbursementDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnDisbursementDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnDisbursementDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnDisbursementDTO.Status
                    && d.Category == "DISBURSEMENT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DateTime? checkDate = null;
                if (String.IsNullOrEmpty(trnDisbursementDTO.CheckDate) == false)
                {
                    checkDate = Convert.ToDateTime(trnDisbursementDTO.CheckDate);
                }

                DBSets.TrnDisbursementDBSet saveDisbursement = salesInvoice;
                saveDisbursement.CurrencyId = trnDisbursementDTO.CurrencyId;
                saveDisbursement.CVDate = Convert.ToDateTime(trnDisbursementDTO.CVDate);
                saveDisbursement.ManualNumber = trnDisbursementDTO.ManualNumber;
                saveDisbursement.DocumentReference = trnDisbursementDTO.DocumentReference;
                saveDisbursement.SupplierId = trnDisbursementDTO.SupplierId;
                saveDisbursement.Payee = trnDisbursementDTO.Payee;
                saveDisbursement.PayTypeId = trnDisbursementDTO.PayTypeId;
                saveDisbursement.CheckNumber = trnDisbursementDTO.CheckNumber;
                saveDisbursement.CheckDate = checkDate;
                saveDisbursement.CheckBank = trnDisbursementDTO.CheckBank;
                saveDisbursement.IsCrossCheck = trnDisbursementDTO.IsCrossCheck;
                saveDisbursement.BankId = trnDisbursementDTO.BankId;
                saveDisbursement.IsClear = trnDisbursementDTO.IsClear;
                saveDisbursement.Remarks = trnDisbursementDTO.Remarks;
                saveDisbursement.CheckedByUserId = trnDisbursementDTO.CheckedByUserId;
                saveDisbursement.ApprovedByUserId = trnDisbursementDTO.ApprovedByUserId;
                saveDisbursement.Status = trnDisbursementDTO.Status;
                saveDisbursement.UpdatedByUserId = loginUserId;
                saveDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockDisbursement(Int32 id, [FromBody] DTO.TrnDisbursementDTO trnDisbursementDTO)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to lock a disbursement.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a disbursement.");
                }

                DBSets.TrnDisbursementDBSet salesInvoice = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a disbursement that is locked.");
                }

                DBSets.MstCurrencyDBSet currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnDisbursementDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                DBSets.MstArticleSupplierDBSet supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnDisbursementDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                DBSets.MstPayTypeDBSet payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnDisbursementDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                DBSets.MstArticleBankDBSet bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnDisbursementDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                DBSets.MstUserDBSet checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnDisbursementDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                DBSets.MstUserDBSet approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnDisbursementDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                DBSets.MstCodeTableDBSet codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnDisbursementDTO.Status
                    && d.Category == "DISBURSEMENT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                DateTime? checkDate = null;
                if (String.IsNullOrEmpty(trnDisbursementDTO.CheckDate) == false)
                {
                    checkDate = Convert.ToDateTime(trnDisbursementDTO.CheckDate);
                }

                DBSets.TrnDisbursementDBSet lockDisbursement = salesInvoice;
                lockDisbursement.CurrencyId = trnDisbursementDTO.CurrencyId;
                lockDisbursement.CVDate = Convert.ToDateTime(trnDisbursementDTO.CVDate);
                lockDisbursement.ManualNumber = trnDisbursementDTO.ManualNumber;
                lockDisbursement.DocumentReference = trnDisbursementDTO.DocumentReference;
                lockDisbursement.SupplierId = trnDisbursementDTO.SupplierId;
                lockDisbursement.Payee = trnDisbursementDTO.Payee;
                lockDisbursement.PayTypeId = trnDisbursementDTO.PayTypeId;
                lockDisbursement.CheckNumber = trnDisbursementDTO.CheckNumber;
                lockDisbursement.CheckDate = checkDate;
                lockDisbursement.CheckBank = trnDisbursementDTO.CheckBank;
                lockDisbursement.IsCrossCheck = trnDisbursementDTO.IsCrossCheck;
                lockDisbursement.BankId = trnDisbursementDTO.BankId;
                lockDisbursement.IsClear = trnDisbursementDTO.IsClear;
                lockDisbursement.Remarks = trnDisbursementDTO.Remarks;
                lockDisbursement.CheckedByUserId = trnDisbursementDTO.CheckedByUserId;
                lockDisbursement.ApprovedByUserId = trnDisbursementDTO.ApprovedByUserId;
                lockDisbursement.Status = trnDisbursementDTO.Status;
                lockDisbursement.UpdatedByUserId = loginUserId;
                lockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                //IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                //    from d in _dbContext.TrnDisbursementLines
                //    where d.CVId == id
                //    && d.RRId != null
                //    select d
                //).ToListAsync();

                //if (disbursementLinesByCurrentDisbursement.Any())
                //{
                //    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                //    {
                //        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                //    }
                //}

                //await _sysJournalEntry.InsertDisbursementJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockDisbursement(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to unlock a disbursement.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a disbursement.");
                }

                DBSets.TrnDisbursementDBSet salesInvoice = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a disbursement that is unlocked.");
                }

                DBSets.TrnDisbursementDBSet unlockDisbursement = salesInvoice;
                unlockDisbursement.IsLocked = false;
                unlockDisbursement.UpdatedByUserId = loginUserId;
                unlockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                //IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                //    from d in _dbContext.TrnDisbursementLines
                //    where d.CVId == id
                //    && d.RRId != null
                //    select d
                //).ToListAsync();

                //if (disbursementLinesByCurrentDisbursement.Any())
                //{
                //    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                //    {
                //        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                //    }
                //}

                //await _sysJournalEntry.DeleteDisbursementJournalEntry(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelDisbursement(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to cancel a disbursement.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a disbursement.");
                }

                DBSets.TrnDisbursementDBSet salesInvoice = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (salesInvoice.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a disbursement that is unlocked.");
                }

                DBSets.TrnDisbursementDBSet unlockDisbursement = salesInvoice;
                unlockDisbursement.IsCancelled = true;
                unlockDisbursement.UpdatedByUserId = loginUserId;
                unlockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                //IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                //    from d in _dbContext.TrnDisbursementLines
                //    where d.CVId == id
                //    && d.RRId != null
                //    select d
                //).ToListAsync();

                //if (disbursementLinesByCurrentDisbursement.Any())
                //{
                //    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                //    {
                //        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                //    }
                //}

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteDisbursement(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a disbursement.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a disbursement.");
                }

                DBSets.TrnDisbursementDBSet salesInvoice = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (salesInvoice.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a disbursement that is locked.");
                }

                _dbContext.TrnDisbursements.Remove(salesInvoice);
                await _dbContext.SaveChangesAsync();

                //IEnumerable<DBSets.TrnDisbursementLineDBSet> disbursementLinesByCurrentDisbursement = await (
                //    from d in _dbContext.TrnDisbursementLines
                //    where d.CVId == id
                //    && d.RRId != null
                //    select d
                //).ToListAsync();

                //if (disbursementLinesByCurrentDisbursement.Any())
                //{
                //    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                //    {
                //        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                //    }
                //}

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
