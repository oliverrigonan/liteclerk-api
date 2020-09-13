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
    public class TrnCollectionLineAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnCollectionLineAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{CIId}")]
        public async Task<ActionResult> GetCollectionLineListByCollection(Int32 CIId)
        {
            try
            {
                IEnumerable<DTO.TrnCollectionLineDTO> collectionLines = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == CIId
                    orderby d.Id descending
                    select new DTO.TrnCollectionLineDTO
                    {
                        Id = d.Id,
                        CIId = d.CIId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Amount = d.Amount,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            PayTypeCode = d.MstPayType_PayTypeId.PayTypeCode,
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        Particulars = d.Particulars,
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
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
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

                return StatusCode(200, collectionLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetCollectionLineDetail(Int32 id)
        {
            try
            {
                DTO.TrnCollectionLineDTO collectionLine = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.Id == id
                    select new DTO.TrnCollectionLineDTO
                    {
                        Id = d.Id,
                        CIId = d.CIId,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        AccountId = d.AccountId,
                        Account = new DTO.MstAccountDTO
                        {
                            AccountCode = d.MstAccount_AccountId.AccountCode,
                            ManualCode = d.MstAccount_AccountId.ManualCode,
                            Account = d.MstAccount_AccountId.Account
                        },
                        ArticleId = d.ArticleId,
                        Article = new DTO.MstArticleDTO
                        {
                            ArticleCode = d.MstArticle_ArticleId.ArticleCode,
                            ManualCode = d.MstArticle_ArticleId.ManualCode,
                            Article = d.MstArticle_ArticleId.Article
                        },
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                        },
                        Amount = d.Amount,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            PayTypeCode = d.MstPayType_PayTypeId.PayTypeCode,
                            ManualCode = d.MstPayType_PayTypeId.ManualCode,
                            PayType = d.MstPayType_PayTypeId.PayType
                        },
                        Particulars = d.Particulars,
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
                            Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
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
                ).FirstOrDefaultAsync();

                return StatusCode(200, collectionLine);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCollectionLine([FromBody] DTO.TrnCollectionLineDTO trnCollectionLineDTO)
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
                    return StatusCode(404, "No rights to add a collection line.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a collection line.");
                }

                DBSets.TrnCollectionDBSet collection = await (
                    from d in _dbContext.TrnCollections
                    where d.Id == trnCollectionLineDTO.CIId
                    select d
                ).FirstOrDefaultAsync();

                if (collection == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (collection.IsLocked == true)
                {
                    return StatusCode(400, "Cannot add collection line if the current collection is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnCollectionLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnCollectionLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnCollectionLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (trnCollectionLineDTO.SIId != null)
                {
                    DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                        from d in _dbContext.TrnSalesInvoices
                        where d.Id == trnCollectionLineDTO.SIId
                        && d.IsLocked == true
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                    {
                        return StatusCode(404, "Sales invoice not found.");
                    }
                }

                DBSets.MstPayTypeDBSet payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnCollectionLineDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                DBSets.MstArticleBankDBSet bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnCollectionLineDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnCollectionLineDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DateTime? checkDate = null;
                if (String.IsNullOrEmpty(trnCollectionLineDTO.CheckDate) == false)
                {
                    checkDate = Convert.ToDateTime(trnCollectionLineDTO.CheckDate);
                }

                DBSets.TrnCollectionLineDBSet newCollectionLines = new DBSets.TrnCollectionLineDBSet()
                {
                    CIId = trnCollectionLineDTO.CIId,
                    BranchId = trnCollectionLineDTO.BranchId,
                    AccountId = trnCollectionLineDTO.AccountId,
                    ArticleId = trnCollectionLineDTO.ArticleId,
                    SIId = trnCollectionLineDTO.SIId,
                    Amount = trnCollectionLineDTO.Amount,
                    PayTypeId = trnCollectionLineDTO.PayTypeId,
                    Particulars = trnCollectionLineDTO.Particulars,
                    CheckNumber = trnCollectionLineDTO.CheckNumber,
                    CheckDate = checkDate,
                    CheckBank = trnCollectionLineDTO.CheckBank,
                    BankId = trnCollectionLineDTO.BankId,
                    IsClear = trnCollectionLineDTO.IsClear,
                    WTAXId = trnCollectionLineDTO.WTAXId,
                    WTAXRate = trnCollectionLineDTO.WTAXRate,
                    WTAXAmount = trnCollectionLineDTO.WTAXAmount
                };

                _dbContext.TrnCollectionLines.Add(newCollectionLines);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == trnCollectionLineDTO.CIId
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    amount = collectionLinesByCurrentCollection.Sum(d => d.Amount);
                }

                DBSets.TrnCollectionDBSet updateCollection = collection;
                updateCollection.Amount = amount;
                updateCollection.UpdatedByUserId = loginUserId;
                updateCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCollectionLine(Int32 id, [FromBody] DTO.TrnCollectionLineDTO trnCollectionLineDTO)
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
                    return StatusCode(404, "No rights to edit or update a collection line.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or update a collection line.");
                }

                DBSets.TrnCollectionLineDBSet collectionLine = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (collectionLine == null)
                {
                    return StatusCode(404, "Collection line not found.");
                }

                DBSets.TrnCollectionDBSet collection = await (
                    from d in _dbContext.TrnCollections
                    where d.Id == trnCollectionLineDTO.CIId
                    select d
                ).FirstOrDefaultAsync();

                if (collection == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (collection.IsLocked == true)
                {
                    return StatusCode(400, "Cannot update collection line if the current collection is locked.");
                }

                DBSets.MstCompanyBranchDBSet branch = await (
                    from d in _dbContext.MstCompanyBranches
                    where d.Id == trnCollectionLineDTO.BranchId
                    && d.MstCompany_CompanyId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (branch == null)
                {
                    return StatusCode(404, "Branch not found.");
                }

                DBSets.MstAccountDBSet account = await (
                    from d in _dbContext.MstAccounts
                    where d.Id == trnCollectionLineDTO.AccountId
                    select d
                ).FirstOrDefaultAsync();

                if (account == null)
                {
                    return StatusCode(404, "Account not found.");
                }

                DBSets.MstArticleDBSet article = await (
                    from d in _dbContext.MstArticles
                    where d.Id == trnCollectionLineDTO.ArticleId
                    && d.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (article == null)
                {
                    return StatusCode(404, "Article not found.");
                }

                if (trnCollectionLineDTO.SIId != null)
                {
                    DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
                        from d in _dbContext.TrnSalesInvoices
                        where d.Id == trnCollectionLineDTO.SIId
                        && d.IsLocked == true
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                    {
                        return StatusCode(404, "Sales invoice not found.");
                    }
                }

                DBSets.MstPayTypeDBSet payType = await (
                    from d in _dbContext.MstPayTypes
                    where d.Id == trnCollectionLineDTO.PayTypeId
                    select d
                ).FirstOrDefaultAsync();

                if (payType == null)
                {
                    return StatusCode(404, "Pay type not found.");
                }

                DBSets.MstArticleBankDBSet bank = await (
                    from d in _dbContext.MstArticleBanks
                    where d.ArticleId == trnCollectionLineDTO.BankId
                    && d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (bank == null)
                {
                    return StatusCode(404, "Bank not found.");
                }

                DBSets.MstTaxDBSet WTAX = await (
                    from d in _dbContext.MstTaxes
                    where d.Id == trnCollectionLineDTO.WTAXId
                    select d
                ).FirstOrDefaultAsync();

                if (WTAX == null)
                {
                    return StatusCode(404, "Withholding tax not found.");
                }

                DateTime? checkDate = null;
                if (String.IsNullOrEmpty(trnCollectionLineDTO.CheckDate) == false)
                {
                    checkDate = Convert.ToDateTime(trnCollectionLineDTO.CheckDate);
                }

                DBSets.TrnCollectionLineDBSet updateCollectionLines = collectionLine;
                updateCollectionLines.CIId = trnCollectionLineDTO.CIId;
                updateCollectionLines.BranchId = trnCollectionLineDTO.BranchId;
                updateCollectionLines.AccountId = trnCollectionLineDTO.AccountId;
                updateCollectionLines.ArticleId = trnCollectionLineDTO.ArticleId;
                updateCollectionLines.SIId = trnCollectionLineDTO.SIId;
                updateCollectionLines.Amount = trnCollectionLineDTO.Amount;
                updateCollectionLines.PayTypeId = trnCollectionLineDTO.PayTypeId;
                updateCollectionLines.Particulars = trnCollectionLineDTO.Particulars;
                updateCollectionLines.CheckNumber = trnCollectionLineDTO.CheckNumber;
                updateCollectionLines.CheckDate = checkDate;
                updateCollectionLines.CheckBank = trnCollectionLineDTO.CheckBank;
                updateCollectionLines.BankId = trnCollectionLineDTO.BankId;
                updateCollectionLines.IsClear = trnCollectionLineDTO.IsClear;
                updateCollectionLines.WTAXId = trnCollectionLineDTO.WTAXId;
                updateCollectionLines.WTAXRate = trnCollectionLineDTO.WTAXRate;
                updateCollectionLines.WTAXAmount = trnCollectionLineDTO.WTAXAmount;

                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == trnCollectionLineDTO.CIId
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    amount = collectionLinesByCurrentCollection.Sum(d => d.Amount);
                }

                DBSets.TrnCollectionDBSet updateCollection = collection;
                updateCollection.Amount = amount;
                updateCollection.UpdatedByUserId = loginUserId;
                updateCollection.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCollectionLine(int id)
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
                    return StatusCode(404, "No rights to delete a collection line.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a collection line.");
                }

                Int32 CIId = 0;

                DBSets.TrnCollectionLineDBSet collectionLine = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (collectionLine == null)
                {
                    return StatusCode(404, "Collection line not found.");
                }

                CIId = collectionLine.CIId;

                DBSets.TrnCollectionDBSet collection = await (
                    from d in _dbContext.TrnCollections
                    where d.Id == CIId
                    select d
                ).FirstOrDefaultAsync();

                if (collection == null)
                {
                    return StatusCode(404, "Collection not found.");
                }

                if (collection.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete collection line if the current collection is locked.");
                }

                _dbContext.TrnCollectionLines.Remove(collectionLine);
                await _dbContext.SaveChangesAsync();

                Decimal amount = 0;

                IEnumerable<DBSets.TrnCollectionLineDBSet> collectionLinesByCurrentCollection = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.CIId == CIId
                    select d
                ).ToListAsync();

                if (collectionLinesByCurrentCollection.Any())
                {
                    amount = collectionLinesByCurrentCollection.Sum(d => d.Amount);
                }

                DBSets.TrnCollectionDBSet updateCollection = collection;
                updateCollection.Amount = amount;
                updateCollection.UpdatedByUserId = loginUserId;
                updateCollection.UpdatedDateTime = DateTime.Now;

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
