using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class TrnPointOfSaleAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public TrnPointOfSaleAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byTerminal/{terminalCode}/byDate/{date}")]
        public async Task<ActionResult> GetPointOfSaleListByTerminalByDate(String terminalCode, String date)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                DBSets.MstUserDBSet loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                IEnumerable<DTO.TrnPointOfSaleDTO> pointOfSales = await (
                    from d in _dbContext.TrnPointOfSales
                    where d.BranchId == loginUser.BranchId
                    && d.TerminalCode == terminalCode
                    && d.POSDate == Convert.ToDateTime(date)
                    select new DTO.TrnPointOfSaleDTO
                    {
                        Id = d.Id,
                        BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            BranchCode = d.MstCompanyBranch_BranchId.BranchCode,
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        TerminalCode = d.TerminalCode,
                        POSDate = d.POSDate.ToShortDateString(),
                        POSNumber = d.POSNumber,
                        OrderNumber = d.OrderNumber,
                        CustomerCode = d.CustomerCode,
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleCustomerDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_CustomerId.ManualCode
                            },
                            Customer = d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() ? d.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                        },
                        ItemCode = d.ItemCode,
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                        },
                        Particulars = d.Particulars,
                        Quantity = d.Quantity,
                        Price = d.Price,
                        Discount = d.Discount,
                        NetPrice = d.NetPrice,
                        Amount = d.Amount,
                        TaxCode = d.TaxCode,
                        TaxId = d.TaxId,
                        Tax = new DTO.MstTaxDTO
                        {
                            TaxCode = d.MstTax_TaxId.TaxCode,
                            ManualCode = d.MstTax_TaxId.ManualCode,
                            TaxDescription = d.MstTax_TaxId.TaxDescription
                        },
                        TaxAmount = d.TaxAmount,
                        CashierUserCode = d.CashierUserCode,
                        CashierUserId = d.CashierUserId,
                        CashierUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CashierUserId.Username,
                            Fullname = d.MstUser_CashierUserId.Fullname
                        },
                        TimeStamp = d.TimeStamp.ToString("MMMM dd, yyyy hh:mm tt"),
                        PostCode = d.PostCode
                    }
                ).ToListAsync();

                return StatusCode(200, pointOfSales);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
