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
    public class TrnDisbursementAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        private readonly Modules.SysAccountsPayableModule _sysAccountsPayable;
        private readonly Modules.SysJournalEntryModule _sysJournalEntry;

        public TrnDisbursementAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;

            _sysAccountsPayable = new Modules.SysAccountsPayableModule(dbContext);
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

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                var disbursements = await (
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
                         ExchangeCurrencyId = d.ExchangeCurrencyId,
                         ExchangeCurrency = new DTO.MstCurrencyDTO
                         {
                             ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                             Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                         },
                         ExchangeRate = d.ExchangeRate,
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
                         Amount = d.Amount,
                         BaseAmount = d.BaseAmount,
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
                DTO.TrnDisbursementDTO disbursement = await (
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
                        ExchangeCurrencyId = d.ExchangeCurrencyId,
                        ExchangeCurrency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_ExchangeCurrencyId.ManualCode,
                            Currency = d.MstCurrency_ExchangeCurrencyId.Currency
                        },
                        ExchangeRate = d.ExchangeRate,
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
                        Amount = d.Amount,
                        BaseAmount = d.BaseAmount,
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

                return StatusCode(200, disbursement);
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

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var payType = await (
                    from d in _dbContext.MstPayTypes
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                var bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "DISBURSEMENT STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                String CVNumber = "0000000001";
                var lastDisbursement = await (
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

                var newDisbursement = new DBSets.TrnDisbursementDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    CurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeCurrencyId = loginUser.MstCompany_CompanyId.CurrencyId,
                    ExchangeRate = 0,
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
                    BankId = bank.ArticleId,
                    IsClear = false,
                    Amount = 0,
                    BaseAmount = 0,
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

                var disbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a disbursement that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnDisbursementDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnDisbursementDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnDisbursementDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                var bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnDisbursementDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                var checkedByUser = await (
                     from d in _dbContext.MstUsers
                     where d.Id == trnDisbursementDTO.CheckedByUserId
                     select d
                 ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                     from d in _dbContext.MstUsers
                     where d.Id == trnDisbursementDTO.ApprovedByUserId
                     select d
                 ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
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

                var saveDisbursement = disbursement;
                saveDisbursement.ExchangeCurrencyId = trnDisbursementDTO.ExchangeCurrencyId;
                saveDisbursement.ExchangeRate = trnDisbursementDTO.ExchangeRate;
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

                var disbursement = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a disbursement that is locked.");
                }

                var exchangeCurrency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == trnDisbursementDTO.ExchangeCurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (exchangeCurrency == null)
                {
                    return StatusCode(404, "Exchange currency not found.");
                }

                var supplier = await (
                    from d in _dbContext.MstArticleSuppliers
                    where d.ArticleId == trnDisbursementDTO.SupplierId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (supplier == null)
                {
                    return StatusCode(404, "Supplier not found.");
                }

                var payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnDisbursementDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                var bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnDisbursementDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                var checkedByUser = await (
                     from d in _dbContext.MstUsers
                     where d.Id == trnDisbursementDTO.CheckedByUserId
                     select d
                 ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                     from d in _dbContext.MstUsers
                     where d.Id == trnDisbursementDTO.ApprovedByUserId
                     select d
                 ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
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

                var lockDisbursement = disbursement;
                lockDisbursement.ExchangeCurrencyId = trnDisbursementDTO.ExchangeCurrencyId;
                lockDisbursement.ExchangeRate = trnDisbursementDTO.ExchangeRate;
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
                lockDisbursement.IsLocked = true;
                lockDisbursement.UpdatedByUserId = loginUserId;
                lockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var disbursementLinesByCurrentDisbursement = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == id
                    && d.RRId != null
                    select d
                ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                    }
                }

                await _sysJournalEntry.InsertDisbursementJournalEntry(id);

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

                var disbursement = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a disbursement that is unlocked.");
                }

                var unlockDisbursement = disbursement;
                unlockDisbursement.IsLocked = false;
                unlockDisbursement.UpdatedByUserId = loginUserId;
                unlockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var disbursementLinesByCurrentDisbursement = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.CVId == id
                    && d.RRId != null
                    select d
                ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                    }
                }

                await _sysJournalEntry.DeleteDisbursementJournalEntry(id);

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

                var disbursement = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a disbursement that is unlocked.");
                }

                var unlockDisbursement = disbursement;
                unlockDisbursement.IsCancelled = true;
                unlockDisbursement.UpdatedByUserId = loginUserId;
                unlockDisbursement.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                var disbursementLinesByCurrentDisbursement = await (
                     from d in _dbContext.TrnDisbursementLines
                     where d.CVId == id
                     && d.RRId != null
                     select d
                 ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
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
        public async Task<ActionResult> DeleteDisbursement(Int32 id)
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

                var disbursement = await (
                     from d in _dbContext.TrnDisbursements
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                if (disbursement.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a disbursement that is locked.");
                }

                _dbContext.TrnDisbursements.Remove(disbursement);
                await _dbContext.SaveChangesAsync();

                var disbursementLinesByCurrentDisbursement = await (
                     from d in _dbContext.TrnDisbursementLines
                     where d.CVId == id
                     && d.RRId != null
                     select d
                 ).ToListAsync();

                if (disbursementLinesByCurrentDisbursement.Any())
                {
                    foreach (var disbursementLineByCurrentDisbursement in disbursementLinesByCurrentDisbursement)
                    {
                        await _sysAccountsPayable.UpdateAccountsPayable(Convert.ToInt32(disbursementLineByCurrentDisbursement.RRId));
                    }
                }

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintDisbursement(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityDisbursementDetail"
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

                        var disbursement = await (
                             from d in _dbContext.TrnDisbursements
                             where d.Id == id
                             && d.IsLocked == true
                             select d
                         ).FirstOrDefaultAsync(); ;

                        if (disbursement != null)
                        {
                            String reprinted = "";
                            if (disbursement.IsPrinted == true)
                            {
                                reprinted = "(REPRCVTED)";
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("CASH/CHECK VOUCHER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            String supplier = disbursement.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier;
                            String payee = disbursement.Payee;
                            String checkNumber = disbursement.CheckNumber;
                            String checkDate = disbursement.CheckDate != null ? Convert.ToDateTime(disbursement.CheckDate).ToShortDateString() : "";
                            String checkBank = disbursement.CheckBank;
                            String remarks = disbursement.Remarks;

                            String branch = disbursement.MstCompanyBranch_BranchId.Branch;
                            String CVNumber = "CV-" + disbursement.MstCompanyBranch_BranchId.ManualCode + "-" + disbursement.CVNumber;
                            String CVDate = disbursement.CVDate.ToString("MMMM dd, yyyy");
                            String manualNumber = disbursement.ManualNumber;
                            String documentReference = disbursement.DocumentReference;

                            PdfPTable tableDisbursement = new PdfPTable(4);
                            tableDisbursement.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                            tableDisbursement.WidthPercentage = 100;

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Supplier:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(supplier, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(CVNumber, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Payee:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(payee, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(branch, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Check No.", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(checkNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(CVDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Check Date", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(checkDate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Check Bank", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(checkBank, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                            tableDisbursement.AddCell(new PdfPCell(new Phrase("Remarks", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableDisbursement.AddCell(new PdfPCell(new Phrase("", fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableDisbursement);

                            var journalEntries = await (
                                from d in _dbContext.SysJournalEntries
                                where d.CVId == id
                                select d
                            ).ToListAsync();

                            PdfPTable tableJournalEntries = new PdfPTable(6);
                            tableJournalEntries.SetWidths(new float[] { 120f, 70f, 150f, 150f, 80f, 80f });
                            tableJournalEntries.WidthPercentage = 100;
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Branch", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Code", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Account", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Article", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Debit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("Credit", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            if (journalEntries.Any())
                            {
                                foreach (var journalEntry in journalEntries)
                                {
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.MstCompanyBranch_BranchId.Branch, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.MstAccount_AccountId.ManualCode, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.MstAccount_AccountId.Account, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.MstArticle_ArticleId.Article, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.DebitAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntry.CreditAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tableJournalEntries.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 4 });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntries.Sum(d => d.DebitAmount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase(journalEntries.Sum(d => d.CreditAmount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableJournalEntries.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableJournalEntries);

                            PdfPTable tableDisbursementLines = new PdfPTable(4);
                            tableDisbursementLines.SetWidths(new float[] { 50f, 50f, 200f, 80f });
                            tableDisbursementLines.WidthPercentage = 100;
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase("RR No.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase("RR Date", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase("Particulars", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase("Paid Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });

                            var disbursementLines = await (
                                from d in _dbContext.TrnDisbursementLines
                                where d.CVId == id
                                select d
                            ).ToListAsync();

                            if (disbursementLines.Any())
                            {
                                foreach (var disbursementItem in disbursementLines)
                                {
                                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementItem.TrnReceivingReceipt_RRId.RRNumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementItem.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(), fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementItem.Particulars, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementItem.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }

                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 3 });
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementLines.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableDisbursementLines.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableDisbursementLines);

                            String preparedBy = disbursement.MstUser_PreparedByUserId.Fullname;
                            String checkedBy = disbursement.MstUser_CheckedByUserId.Fullname;
                            String approvedBy = disbursement.MstUser_ApprovedByUserId.Fullname;

                            PdfPTable tableUsers = new PdfPTable(3);
                            tableUsers.SetWidths(new float[] { 105f, 100f, 100f });
                            tableUsers.WidthPercentage = 100;
                            tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableUsers);

                            PdfPTable tableMoneyWord = new PdfPTable(2);
                            tableMoneyWord.SetWidths(new float[] { 140f, 115f });
                            tableMoneyWord.WidthPercentage = 100;

                            String paidAmount = Convert.ToString(Math.Round(disbursement.Amount * 100) / 100);

                            var amountTablePhrase = new Phrase();
                            var amountString = "ZERO";

                            if (Convert.ToDecimal(paidAmount) != 0)
                            {
                                amountString = GetMoneyWord(paidAmount).ToUpper();
                            }

                            amountTablePhrase.Add(new Chunk("Representing Payment from " + companyName + " the amount of ", fontSegoeUI09));
                            amountTablePhrase.Add(new Chunk(amountString + " (P " + disbursement.Amount.ToString("#,##0.00") + ")", fontSegoeUI09Bold));

                            Paragraph paragraphAmountTable = new Paragraph();
                            paragraphAmountTable.SetLeading(0, 1.4f);
                            paragraphAmountTable.Add(amountTablePhrase);

                            PdfPCell chunkyAmountTable = new PdfPCell();
                            chunkyAmountTable.AddElement(paragraphAmountTable);
                            chunkyAmountTable.BorderWidth = PdfPCell.NO_BORDER;

                            tableMoneyWord.AddCell(new PdfPCell(chunkyAmountTable) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableMoneyWord.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableMoneyWord);

                            PdfPTable tableUserSign = new PdfPTable(4);
                            tableUserSign.SetWidths(new float[] { 60f, 5f, 40f, 115f });
                            tableUserSign.WidthPercentage = 100;
                            tableUserSign.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 4, PaddingTop = 30f });
                            tableUserSign.AddCell(new PdfPCell(new Phrase("Signature Over Printed Name", fontSegoeUI09Bold)) { Border = 1, HorizontalAlignment = 1 });
                            tableUserSign.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0 });
                            tableUserSign.AddCell(new PdfPCell(new Phrase("Date", fontSegoeUI09Bold)) { Border = 1, HorizontalAlignment = 1 });
                            tableUserSign.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, });
                            document.Add(tableUserSign);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print disbursement"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print disbursement"
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

        public static String GetMoneyWord(String input)
        {
            String decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                input = input.Remove(input.IndexOf("."));
            }

            String strWords = GetMoreThanThousandNumberWords(input);
            if (decimals.Length > 0)
            {
                if (Convert.ToDecimal(decimals) > 0)
                {
                    String getFirstRoundedDecimals = new String(decimals.Take(2).ToArray());
                    strWords += " Pesos And " + GetMoreThanThousandNumberWords(getFirstRoundedDecimals) + " Cents Only";
                }
                else
                {
                    strWords += " Pesos Only";
                }
            }
            else
            {
                strWords += " Pesos Only";
            }

            return strWords;
        }

        private static String GetMoreThanThousandNumberWords(string input)
        {
            try
            {
                String[] seperators = { "", " Thousand ", " Million ", " Billion " };

                int i = 0;

                String strWords = "";

                while (input.Length > 0)
                {
                    String _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
                    input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                    Int32 no = Int32.Parse(_3digits);
                    _3digits = GetHundredNumberWords(no);

                    _3digits += seperators[i];
                    strWords = _3digits + strWords;

                    i++;
                }

                return strWords;
            }
            catch
            {
                return "Invalid Amount";
            }
        }

        private static String GetHundredNumberWords(Int32 no)
        {
            String[] Ones =
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
            };

            String[] Tens = { "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            String word = "";

            if (no > 99 && no < 1000)
            {
                Int32 i = no / 100;
                word = word + Ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                Int32 i = no / 10;
                word = word + Tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + Ones[no - 1];
            }

            return word;
        }
    }
}
