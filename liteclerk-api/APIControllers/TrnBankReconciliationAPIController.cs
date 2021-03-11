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
    public class TrnBankReconciliationAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnBankReconciliationAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("collectionLine/list/{bankId}/{startDate}/{endDate}")]
        public async Task<ActionResult> GetBankReconciliationCollectionLineList(Int32 bankId, String startDate, String endDate)
        {
            try
            {
                var previousCollectionLines = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.BankId == bankId
                    && d.TrnCollection_CIId.CIDate < Convert.ToDateTime(startDate)
                    && d.TrnCollection_CIId.IsLocked == true
                    && d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true
                    && d.Amount > 0
                    && d.IsClear == false
                    && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
                    select new DTO.TrnCollectionLineDTO
                    {
                        Id = d.Id,
                        CIId = d.CIId,
                        Collection = new DTO.TrnCollectionDTO
                        {
                            CINumber = d.TrnCollection_CIId.CINumber,
                            CIDate = d.TrnCollection_CIId.CIDate.ToShortDateString(),
                            ManualNumber = d.TrnCollection_CIId.ManualNumber,
                            DocumentReference = d.TrnCollection_CIId.DocumentReference,
                            CustomerId = d.TrnCollection_CIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnCollection_CIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer,
                            },
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {

                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        CheckNumber = d.CheckNumber,
                        CheckDate = d.CheckDate != null ? Convert.ToDateTime(d.CheckDate).ToShortDateString() : "",
                        CheckBank = d.CheckBank,
                        BankId = d.BankId,
                        Bank = new DTO.MstArticleBankDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_BankId.ManualCode
                            },
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank,
                        },
                        IsClear = d.IsClear,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount
                    }
                ).ToListAsync();

                var currentCollectionLines = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.BankId == bankId
                    && d.TrnCollection_CIId.CIDate >= Convert.ToDateTime(startDate)
                    && d.TrnCollection_CIId.CIDate <= Convert.ToDateTime(endDate)
                    && d.TrnCollection_CIId.IsLocked == true
                    && d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true
                    && d.Amount > 0
                    && d.IsClear == false
                    && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
                    select new DTO.TrnCollectionLineDTO
                    {
                        Id = d.Id,
                        CIId = d.CIId,
                        Collection = new DTO.TrnCollectionDTO
                        {
                            CINumber = d.TrnCollection_CIId.CINumber,
                            CIDate = d.TrnCollection_CIId.CIDate.ToShortDateString(),
                            ManualNumber = d.TrnCollection_CIId.ManualNumber,
                            DocumentReference = d.TrnCollection_CIId.DocumentReference,
                            CustomerId = d.TrnCollection_CIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnCollection_CIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer,
                            },
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {

                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        CheckNumber = d.CheckNumber,
                        CheckDate = d.CheckDate != null ? Convert.ToDateTime(d.CheckDate).ToShortDateString() : "",
                        CheckBank = d.CheckBank,
                        BankId = d.BankId,
                        Bank = new DTO.MstArticleBankDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_BankId.ManualCode
                            },
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank,
                        },
                        IsClear = d.IsClear,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_WTAXId.TaxCode,
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount
                    }
                ).ToListAsync();

                var collectionLines = previousCollectionLines.Union(currentCollectionLines);

                return StatusCode(200, collectionLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("disbursement/list/{bankId}/{startDate}/{endDate}")]
        public async Task<ActionResult> GetBankReconciliationDisbursementList(Int32 bankId, String startDate, String endDate)
        {
            try
            {
                var previousDisbursements = await (
                    from d in _dbContext.TrnDisbursements
                    where d.BankId == bankId
                    && d.CVDate < Convert.ToDateTime(startDate)
                    && d.IsLocked == true
                    && d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    && d.Amount > 0
                    && d.IsClear == false
                    && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
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
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier,
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
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank,
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
                        Status = d.Status
                    }
                ).ToListAsync();

                var currentDisbursements = await (
                    from d in _dbContext.TrnDisbursements
                    where d.BankId == bankId
                    && d.CVDate >= Convert.ToDateTime(startDate)
                    && d.CVDate <= Convert.ToDateTime(endDate)
                    && d.IsLocked == true
                    && d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
                    && d.Amount > 0
                    && d.IsClear == false
                    && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
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
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier,
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
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank,
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
                        Status = d.Status
                    }
                ).ToListAsync();

                var disbursements = previousDisbursements.Union(currentDisbursements);

                return StatusCode(200, disbursements);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("journalVoucherLine/list/{bankId}/{startDate}/{endDate}")]
        public async Task<ActionResult> GetBankReconciliationJournalVoucherLineList(Int32 bankId, String startDate, String endDate)
        {
            try
            {
                var previousJournalVoucherLines = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.ArticleId == bankId
                    && d.TrnJournalVoucher_JVId.JVDate < Convert.ToDateTime(startDate)
                    && d.TrnJournalVoucher_JVId.IsLocked == true
                    && d.DebitAmount - d.CreditAmount != 0
                    && d.IsClear == false
                    select new DTO.TrnJournalVoucherLineDTO
                    {
                        Id = d.Id,
                        JVId = d.JVId,
                        JournalVoucher = new DTO.TrnJournalVoucherDTO
                        {
                            JVNumber = d.TrnJournalVoucher_JVId.JVNumber,
                            JVDate = d.TrnJournalVoucher_JVId.JVDate.ToShortDateString(),
                            ManualNumber = d.TrnJournalVoucher_JVId.ManualNumber,
                            DocumentReference = d.TrnJournalVoucher_JVId.DocumentReference
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Particulars = d.Particulars,
                        IsClear = d.IsClear
                    }
                ).ToListAsync();

                var currentJournalVoucherLines = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.ArticleId == bankId
                    && d.TrnJournalVoucher_JVId.JVDate >= Convert.ToDateTime(startDate)
                    && d.TrnJournalVoucher_JVId.JVDate <= Convert.ToDateTime(endDate)
                    && d.TrnJournalVoucher_JVId.IsLocked == true
                    && d.DebitAmount - d.CreditAmount != 0
                    && d.IsClear == false
                    select new DTO.TrnJournalVoucherLineDTO
                    {
                        Id = d.Id,
                        JVId = d.JVId,
                        JournalVoucher = new DTO.TrnJournalVoucherDTO
                        {
                            JVNumber = d.TrnJournalVoucher_JVId.JVNumber,
                            JVDate = d.TrnJournalVoucher_JVId.JVDate.ToShortDateString(),
                            ManualNumber = d.TrnJournalVoucher_JVId.ManualNumber,
                            DocumentReference = d.TrnJournalVoucher_JVId.DocumentReference
                        },
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        DebitAmount = d.DebitAmount,
                        CreditAmount = d.CreditAmount,
                        Particulars = d.Particulars,
                        IsClear = d.IsClear
                    }
                ).ToListAsync();

                var journalVoucherLines = previousJournalVoucherLines.Union(currentJournalVoucherLines);

                return StatusCode(200, journalVoucherLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("collectionLine/update/clear/{id}")]
        public async Task<ActionResult> UpdateClearBankReconciliationCollectionLine(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityBankReconciliation"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a collection line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a collection line.");
                }

                var collectionLine = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (collectionLine == null)
                {
                    return StatusCode(404, "Collection line not found.");
                }

                Boolean isClear = true;
                if (collectionLine.IsClear == true)
                {
                    isClear = false;
                }

                var updateCollectionLines = collectionLine;
                updateCollectionLines.IsClear = isClear;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("disbursement/update/clear/{id}")]
        public async Task<ActionResult> UpdateClearBankReconciliationDisbursement(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityBankReconciliation"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a disbursement.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a disbursement.");
                }

                var disbursement = await (
                    from d in _dbContext.TrnDisbursements
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (disbursement == null)
                {
                    return StatusCode(404, "Disbursement not found.");
                }

                Boolean isClear = true;
                if (disbursement.IsClear == true)
                {
                    isClear = false;
                }

                var updateDisbursements = disbursement;
                updateDisbursements.IsClear = isClear;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("journalVoucherLine/update/clear/{id}")]
        public async Task<ActionResult> UpdateClearBankReconciliationJournalVoucherLine(Int32 id)
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
                    && d.SysForm_FormId.Form == "ActivityBankReconciliation"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or update a journal voucher line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a journal voucher line.");
                }

                var journalVoucherLine = await (
                    from d in _dbContext.TrnJournalVoucherLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (journalVoucherLine == null)
                {
                    return StatusCode(404, "Journal voucher line not found.");
                }

                Boolean isClear = true;
                if (journalVoucherLine.IsClear == true)
                {
                    isClear = false;
                }

                var updateJournalVoucherLines = journalVoucherLine;
                updateJournalVoucherLines.IsClear = isClear;

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
