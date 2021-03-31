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
    public class RepAccountsReceivableReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepAccountsReceivableReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
        public static Decimal ComputeAge(Int32 age, Int32 elapsed, Decimal amount)
        {
            Decimal returnValue = 0;

            if (age == 0)
            {
                if (elapsed < 30)
                {
                    returnValue = amount;
                }
            }
            else if (age == 1)
            {
                if (elapsed >= 30 && elapsed < 60)
                {
                    returnValue = amount;
                }
            }
            else if (age == 2)
            {
                if (elapsed >= 60 && elapsed < 90)
                {
                    returnValue = amount;
                }
            }
            else if (age == 3)
            {
                if (elapsed >= 90 && elapsed < 120)
                {
                    returnValue = amount;
                }
            }
            else if (age == 4)
            {
                if (elapsed >= 120)
                {
                    returnValue = amount;
                }
            }
            else
            {
                returnValue = 0;
            }

            return returnValue;
        }

        [HttpGet("list/byDateAsOf/{dateAsOf}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetAccountsReceivableReportList(String dateAsOf, Int32 companyId, Int32 branchId)
        {
            if (branchId == 0)
            {
                IEnumerable<DTO.RepAccountsReceivableReportDTO> salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.SIDate <= Convert.ToDateTime(dateAsOf)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.BalanceAmount > 0
                    && d.IsLocked == true
                    select new DTO.RepAccountsReceivableReportDTO
                    {
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            Company = new DTO.MstCompanyDTO
                            {
                                Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company
                            },
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            Id = d.Id,
                            SINumber = d.SINumber,
                            SIDate = d.SIDate.ToShortDateString(),
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
                                ReceivableAccountId = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId : 0,
                                ReceivableAccount = new DTO.MstAccountDTO
                                {
                                    ManualCode = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.ManualCode : "",
                                    Account = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.Account : ""
                                }
                            },
                            TermId = d.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.MstTerm_TermId.ManualCode,
                                Term = d.MstTerm_TermId.Term
                            },
                            SoldByUserId = d.SoldByUserId,
                            SoldByUser = new DTO.MstUserDTO
                            {
                                Username = d.MstUser_SoldByUserId.Username,
                                Fullname = d.MstUser_SoldByUserId.Fullname
                            }
                        },
                        DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays)).ToShortDateString(),
                        BalanceAmount = d.BalanceAmount,
                        CurrentAmount = ComputeAge(0, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age30Amount = ComputeAge(1, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age60Amount = ComputeAge(2, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age90Amount = ComputeAge(3, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age120Amount = ComputeAge(4, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount)
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoices);
            }
            else
            {
                IEnumerable<DTO.RepAccountsReceivableReportDTO> salesInvoices = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.SIDate <= Convert.ToDateTime(dateAsOf)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.BranchId == branchId
                    && d.BalanceAmount > 0
                    && d.IsLocked == true
                    select new DTO.RepAccountsReceivableReportDTO
                    {
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            Company = new DTO.MstCompanyDTO
                            {
                                Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company
                            },
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            Id = d.Id,
                            SINumber = d.SINumber,
                            SIDate = d.SIDate.ToShortDateString(),
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
                                ReceivableAccountId = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ReceivableAccountId : 0,
                                ReceivableAccount = new DTO.MstAccountDTO
                                {
                                    ManualCode = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.ManualCode : "",
                                    Account = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().MstAccount_ReceivableAccountId.Account : ""
                                }
                            },
                            TermId = d.TermId,
                            Term = new DTO.MstTermDTO
                            {
                                ManualCode = d.MstTerm_TermId.ManualCode,
                                Term = d.MstTerm_TermId.Term
                            },
                            SoldByUserId = d.SoldByUserId,
                            SoldByUser = new DTO.MstUserDTO
                            {
                                Username = d.MstUser_SoldByUserId.Username,
                                Fullname = d.MstUser_SoldByUserId.Fullname
                            }
                        },
                        DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays)).ToShortDateString(),
                        BalanceAmount = d.BalanceAmount,
                        CurrentAmount = ComputeAge(0, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age30Amount = ComputeAge(1, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age60Amount = ComputeAge(2, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age90Amount = ComputeAge(3, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount),
                        Age120Amount = ComputeAge(4, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm_TermId.NumberOfDays))).Days, d.BalanceAmount)
                    }
                ).ToListAsync();

                return StatusCode(200, salesInvoices);
            }
        }
    }
}
