﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MstArticleCustomerAPIController : ControllerBase
    {
        private DBContext.LiteclerkDBContext _dbContext;

        public MstArticleCustomerAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleCustomerDTO>>> GetCustomerList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> customers = await _dbContext.MstArticleCustomers
                    .Select(d =>
                        new DTO.MstArticleCustomerDTO
                        {
                            Id = d.Id,
                            ArticleId = d.ArticleId,
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Customer = d.Customer,
                            Address = d.Address,
                            ContactPerson = d.ContactPerson,
                            ContactNumber = d.ContactNumber,
                            ReceivableAccountId = d.ReceivableAccountId,
                            ReceivableAccount = d.MstAccount_ReceivableAccount.Account,
                            TermId = d.TermId,
                            Term = d.MstTerm_Term.Term,
                            CreditLimit = d.CreditLimit,
                            IsLocked = d.MstArticle_Article.IsLocked,
                            CreatedByUserFullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname,
                            CreatedByDateTime = d.MstArticle_Article.CreatedByDateTime.ToShortDateString(),
                            UpdatedByUserFullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname,
                            UpdatedByDateTime = d.MstArticle_Article.UpdatedByDateTime.ToShortDateString()
                        })
                    .ToListAsync();

                return StatusCode(200, customers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("locked/list")]
        public async Task<ActionResult<IEnumerable<DTO.MstArticleCustomerDTO>>> GetLockedCustomerList()
        {
            try
            {
                IEnumerable<DTO.MstArticleCustomerDTO> lockedCustomers = await _dbContext.MstArticleCustomers
                    .Where(d => d.MstArticle_Article.IsLocked == true)
                    .Select(d =>
                        new DTO.MstArticleCustomerDTO
                        {
                            Id = d.Id,
                            ArticleId = d.ArticleId,
                            ArticleCode = d.MstArticle_Article.ArticleCode,
                            ManualCode = d.MstArticle_Article.ManualCode,
                            Customer = d.Customer,
                            Address = d.Address,
                            ContactPerson = d.ContactPerson,
                            ContactNumber = d.ContactNumber,
                            ReceivableAccountId = d.ReceivableAccountId,
                            ReceivableAccount = d.MstAccount_ReceivableAccount.Account,
                            TermId = d.TermId,
                            Term = d.MstTerm_Term.Term,
                            CreditLimit = d.CreditLimit,
                            IsLocked = d.MstArticle_Article.IsLocked,
                            CreatedByUserFullname = d.MstArticle_Article.MstUser_CreatedByUser.Fullname,
                            CreatedByDateTime = d.MstArticle_Article.CreatedByDateTime.ToShortDateString(),
                            UpdatedByUserFullname = d.MstArticle_Article.MstUser_UpdatedByUser.Fullname,
                            UpdatedByDateTime = d.MstArticle_Article.UpdatedByDateTime.ToShortDateString()
                        })
                    .ToListAsync();

                return StatusCode(200, lockedCustomers);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
