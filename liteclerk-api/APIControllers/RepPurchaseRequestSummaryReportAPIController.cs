﻿using System;
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
    public class RepPurchaseRequestSummaryReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepPurchaseRequestSummaryReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetPurchaseRequestSummaryReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                var purchaseRequests = await (
                    from d in _dbContext.TrnPurchaseRequests
                    where  d.PRDate >= Convert.ToDateTime(startDate)
                    && d.PRDate <= Convert.ToDateTime(endDate)
                    && d.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.BranchId == branchId
                    && d.IsLocked == true
                    && d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.Any() == true
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
                            Supplier = d.MstArticle_SupplierId.MstArticleSuppliers_ArticleId.FirstOrDefault().Supplier,
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
    }
}
