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
    public class RepCollectionDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepCollectionDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public async Task<ActionResult> GetCollectionDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var collectionLines = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.TrnCollection_CIId.CIDate >= Convert.ToDateTime(startDate)
                    && d.TrnCollection_CIId.CIDate <= Convert.ToDateTime(endDate)
                    && d.TrnCollection_CIId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnCollection_CIId.BranchId == branchId
                    && d.TrnCollection_CIId.IsLocked == true
                    select new DTO.TrnCollectionLineDTO
                    {
                        Id = d.Id,
                        CIId = d.CIId,
                        Collection = new DTO.TrnCollectionDTO
                        {
                            Id = d.TrnCollection_CIId.Id,
                            BranchId = d.TrnCollection_CIId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                BranchCode = d.TrnCollection_CIId.MstCompanyBranch_BranchId.BranchCode,
                                ManualCode = d.TrnCollection_CIId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnCollection_CIId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnCollection_CIId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                CurrencyCode = d.TrnCollection_CIId.MstCurrency_CurrencyId.CurrencyCode,
                                ManualCode = d.TrnCollection_CIId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnCollection_CIId.MstCurrency_CurrencyId.Currency
                            },
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
                                Customer = d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            },
                            Remarks = d.TrnCollection_CIId.Remarks,
                            PreparedByUserId = d.TrnCollection_CIId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnCollection_CIId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnCollection_CIId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnCollection_CIId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnCollection_CIId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnCollection_CIId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnCollection_CIId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnCollection_CIId.Amount,
                            Status = d.TrnCollection_CIId.Status
                        },
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
                        Particulars = d.Particulars,
                        PayTypeId = d.PayTypeId,
                        PayType = new DTO.MstPayTypeDTO
                        {
                            PayTypeCode = d.MstPayType_PayTypeId.PayTypeCode,
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
    }
}
