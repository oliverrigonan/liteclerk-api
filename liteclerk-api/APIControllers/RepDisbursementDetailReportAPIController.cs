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
    public class RepDisbursementDetailReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepDisbursementDetailReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetDisbursementDetailReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var disbursementLines = await (
                    from d in _dbContext.TrnDisbursementLines
                    where d.TrnDisbursement_CVId.CVDate >= Convert.ToDateTime(startDate)
                    && d.TrnDisbursement_CVId.CVDate <= Convert.ToDateTime(endDate)
                    && d.TrnDisbursement_CVId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnDisbursement_CVId.BranchId == branchId
                    && d.TrnDisbursement_CVId.IsLocked == true
                    select new DTO.TrnDisbursementLineDTO
                    {
                        Id = d.Id,
                        CVId = d.CVId,
                        Disbursement = new DTO.TrnDisbursementDTO
                        {
                            Id = d.TrnDisbursement_CVId.Id,
                            BranchId = d.TrnDisbursement_CVId.BranchId,
                            Branch = new DTO.MstCompanyBranchDTO
                            {
                                ManualCode = d.TrnDisbursement_CVId.MstCompanyBranch_BranchId.ManualCode,
                                Branch = d.TrnDisbursement_CVId.MstCompanyBranch_BranchId.Branch
                            },
                            CurrencyId = d.TrnDisbursement_CVId.CurrencyId,
                            Currency = new DTO.MstCurrencyDTO
                            {
                                ManualCode = d.TrnDisbursement_CVId.MstCurrency_CurrencyId.ManualCode,
                                Currency = d.TrnDisbursement_CVId.MstCurrency_CurrencyId.Currency
                            },
                            CVNumber = d.TrnDisbursement_CVId.CVNumber,
                            CVDate = d.TrnDisbursement_CVId.CVDate.ToShortDateString(),
                            ManualNumber = d.TrnDisbursement_CVId.ManualNumber,
                            DocumentReference = d.TrnDisbursement_CVId.DocumentReference,
                            SupplierId = d.TrnDisbursement_CVId.SupplierId,
                            Supplier = new DTO.MstArticleSupplierDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnDisbursement_CVId.MstArticle_SupplierId.ManualCode
                                },
                                Supplier = d.TrnDisbursement_CVId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() ? d.TrnDisbursement_CVId.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier : "",
                            },
                            Payee = d.TrnDisbursement_CVId.Payee,
                            Remarks = d.TrnDisbursement_CVId.Remarks,
                            PayTypeId = d.TrnDisbursement_CVId.PayTypeId,
                            PayType = new DTO.MstPayTypeDTO
                            {
                                ManualCode = d.TrnDisbursement_CVId.MstPayType_PayTypeId.ManualCode,
                                PayType = d.TrnDisbursement_CVId.MstPayType_PayTypeId.PayType
                            },
                            CheckNumber = d.TrnDisbursement_CVId.CheckNumber,
                            CheckDate = d.TrnDisbursement_CVId.CheckDate != null ? Convert.ToDateTime(d.TrnDisbursement_CVId.CheckDate).ToShortDateString() : "",
                            CheckBank = d.TrnDisbursement_CVId.CheckBank,
                            IsCrossCheck = d.TrnDisbursement_CVId.IsCrossCheck,
                            BankId = d.TrnDisbursement_CVId.BankId,
                            Bank = new DTO.MstArticleBankDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnDisbursement_CVId.MstArticle_BankId.ManualCode
                                },
                                Bank = d.TrnDisbursement_CVId.MstArticle_BankId.MstArticleBanks_ArticleId.Any() ? d.TrnDisbursement_CVId.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank : "",
                            },
                            IsClear = d.TrnDisbursement_CVId.IsClear,
                            PreparedByUserId = d.TrnDisbursement_CVId.PreparedByUserId,
                            PreparedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnDisbursement_CVId.MstUser_PreparedByUserId.Username,
                                Fullname = d.TrnDisbursement_CVId.MstUser_PreparedByUserId.Fullname
                            },
                            CheckedByUserId = d.TrnDisbursement_CVId.CheckedByUserId,
                            CheckedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnDisbursement_CVId.MstUser_CheckedByUserId.Username,
                                Fullname = d.TrnDisbursement_CVId.MstUser_CheckedByUserId.Fullname
                            },
                            ApprovedByUserId = d.TrnDisbursement_CVId.ApprovedByUserId,
                            ApprovedByUser = new DTO.MstUserDTO
                            {
                                Username = d.TrnDisbursement_CVId.MstUser_ApprovedByUserId.Username,
                                Fullname = d.TrnDisbursement_CVId.MstUser_ApprovedByUserId.Fullname
                            },
                            Amount = d.TrnDisbursement_CVId.Amount,
                            Status = d.TrnDisbursement_CVId.Status
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
                        RRId = d.RRId,
                        ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                        {
                            RRNumber = d.TrnReceivingReceipt_RRId.RRNumber,
                            RRDate = d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString(),
                            ManualNumber = d.TrnReceivingReceipt_RRId.ManualNumber,
                            DocumentReference = d.TrnReceivingReceipt_RRId.DocumentReference
                        },
                        Amount = d.Amount,
                        Particulars = d.Particulars,
                        WTAXId = d.WTAXId,
                        WTAX = new DTO.MstTaxDTO
                        {
                            ManualCode = d.MstTax_WTAXId.ManualCode,
                            TaxDescription = d.MstTax_WTAXId.TaxDescription
                        },
                        WTAXRate = d.WTAXRate,
                        WTAXAmount = d.WTAXAmount
                    }
                ).ToListAsync();

                return StatusCode(200, disbursementLines);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
