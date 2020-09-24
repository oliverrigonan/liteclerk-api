using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using liteclerk_api.Integrations.EasyPOS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.Integrations.EasyPOS.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasyPOSTrnStockInAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSTrnStockInAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byINDate/{INDate}/byBranch/{branchManualCode}")]
        public async Task<ActionResult> GetStockInListByINDateByBranch(String INDate, String branchManualCode)
        {
            try
            {
                IEnumerable<EasyPOSTrnStockInDTO> stockIns = await (
                    from d in _dbContext.TrnStockIns
                    where d.INDate == Convert.ToDateTime(INDate)
                    && d.MstCompanyBranch_BranchId.ManualCode == branchManualCode
                    orderby d.Id descending
                    select new EasyPOSTrnStockInDTO
                    {
                        Id = d.Id,
                        INNumber = d.INNumber,
                        INDate = d.INDate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        Remarks = d.Remarks,
                        StockInItems = d.TrnStockInItems_INId.Any() ? d.TrnStockInItems_INId.Where(i => i.INId == d.Id).Select(i => new EasyPOSTrnStockInItemDTO
                        {
                            Id = i.Id,
                            INId = i.INId,
                            ItemId = i.ItemId,
                            Item = new EasyPOSMstArticleItemDTO
                            {
                                Article = new EasyPOSMstArticleDTO
                                {
                                    ManualCode = i.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = i.MstArticle_ItemId.MstArticleItems_ArticleId.Any() ? i.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : ""
                            },
                            Particulars = i.Particulars,
                            Quantity = i.Quantity,
                            UnitId = i.UnitId,
                            Unit = new EasyPOSMstUnitDTO
                            {
                                ManualCode = i.MstUnit_UnitId.ManualCode,
                                Unit = i.MstUnit_UnitId.Unit
                            },
                            Cost = i.Cost,
                            Amount = i.Amount,
                        }).ToList() : new List<EasyPOSTrnStockInItemDTO>()
                    }
                ).ToListAsync();

                return StatusCode(200, stockIns);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
