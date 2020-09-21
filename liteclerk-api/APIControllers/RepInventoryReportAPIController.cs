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
    public class RepInventoryReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepInventoryReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetInventoryReportList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            IQueryable<DTO.RepInventoryReportDTO> beginningInventories;
            beginningInventories = from d in _dbContext.SysInventories
                                   where d.IVDate < Convert.ToDateTime(startDate)
                                   && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                   && d.MstArticleItemInventory_ArticleItemInventoryId.BranchId == branchId
                                   && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ?
                                      d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                                   select new DTO.RepInventoryReportDTO
                                   {
                                       Id = d.Id,
                                       Document = "Beginning Balance",
                                       Branch = new DTO.MstCompanyBranchDTO
                                       {
                                           Company = new DTO.MstCompanyDTO
                                           {
                                               Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company
                                           },
                                           ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                           Branch = d.MstCompanyBranch_BranchId.Branch
                                       },
                                       ItemInventory = new DTO.MstArticleItemInventoryDTO
                                       {
                                           InventoryCode = d.MstArticleItemInventory_ArticleItemInventoryId.InventoryCode,
                                           Cost = d.MstArticleItemInventory_ArticleItemInventoryId.Cost
                                       },
                                       Item = new DTO.MstArticleItemDTO
                                       {
                                           Article = new DTO.MstArticleDTO
                                           {
                                               ManualCode = d.MstArticle_ArticleId.ManualCode
                                           },
                                           SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                           BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                           Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                           Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price : 0,
                                           Unit = new DTO.MstUnitDTO
                                           {
                                               ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                               Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : ""
                                           }
                                       },
                                       BegQuantity = d.Quantity,
                                       InQuantity = d.QuantityIn,
                                       OutQuantity = d.QuantityOut,
                                       EndQuantity = d.Quantity
                                   };

            IQueryable<DTO.RepInventoryReportDTO> currentInventories;
            currentInventories = from d in _dbContext.SysInventories
                                 where d.IVDate >= Convert.ToDateTime(startDate)
                                 && d.IVDate <= Convert.ToDateTime(endDate)
                                 && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                 && d.MstArticleItemInventory_ArticleItemInventoryId.BranchId == branchId
                                 && d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ?
                                    d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory == true : false
                                 select new DTO.RepInventoryReportDTO
                                 {
                                     Id = d.Id,
                                     Document = "Current",
                                     Branch = new DTO.MstCompanyBranchDTO
                                     {
                                         Company = new DTO.MstCompanyDTO
                                         {
                                             Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company
                                         },
                                         ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                         Branch = d.MstCompanyBranch_BranchId.Branch
                                     },
                                     ItemInventory = new DTO.MstArticleItemInventoryDTO
                                     {
                                         InventoryCode = d.MstArticleItemInventory_ArticleItemInventoryId.InventoryCode,
                                         Cost = d.MstArticleItemInventory_ArticleItemInventoryId.Cost
                                     },
                                     Item = new DTO.MstArticleItemDTO
                                     {
                                         Article = new DTO.MstArticleDTO
                                         {
                                             ManualCode = d.MstArticle_ArticleId.ManualCode
                                         },
                                         SKUCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                         BarCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                         Description = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                         Price = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().Price : 0,
                                         Unit = new DTO.MstUnitDTO
                                         {
                                             ManualCode = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.ManualCode : "",
                                             Unit = d.MstArticle_ArticleId.MstArticleItems_ArticleId.Any() ? d.MstArticle_ArticleId.MstArticleItems_ArticleId.FirstOrDefault().MstUnit_UnitId.Unit : ""
                                         }
                                     },
                                     BegQuantity = d.Quantity,
                                     InQuantity = d.QuantityIn,
                                     OutQuantity = d.QuantityOut,
                                     EndQuantity = d.Quantity
                                 };

            IQueryable<DTO.RepInventoryReportDTO> unionInventories;
            unionInventories = beginningInventories.Union(currentInventories);

            if (unionInventories.Any())
            {
                IEnumerable<DTO.RepInventoryReportDTO> inventories = await (
                    from d in unionInventories
                    group d by new
                    {
                        d.Branch,
                        d.ItemInventory,
                        d.Item
                    } into g
                    select new DTO.RepInventoryReportDTO
                    {
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            Company = new DTO.MstCompanyDTO
                            {
                                Company = g.Key.Branch.Company.Company
                            },
                            ManualCode = g.Key.Branch.ManualCode,
                            Branch = g.Key.Branch.Branch
                        },
                        ItemInventory = new DTO.MstArticleItemInventoryDTO
                        {
                            InventoryCode = g.Key.ItemInventory.InventoryCode,
                            Cost = g.Key.ItemInventory.Cost
                        },
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = g.Key.Item.Article.ManualCode
                            },
                            SKUCode = g.Key.Item.SKUCode,
                            BarCode = g.Key.Item.BarCode,
                            Description = g.Key.Item.Description,
                            Price = g.Key.Item.Price,
                            Unit = new DTO.MstUnitDTO
                            {
                                ManualCode = g.Key.Item.Unit.ManualCode,
                                Unit = g.Key.Item.Unit.Unit
                            }
                        },
                        BegQuantity = g.Sum(d => d.Document.Equals("Current") ? 0 : d.BegQuantity),
                        InQuantity = g.Sum(d => d.Document.Equals("Beginning Balance") ? 0 : d.InQuantity),
                        OutQuantity = g.Sum(d => d.Document.Equals("Beginning Balance") ? 0 : d.OutQuantity),
                        EndQuantity = g.Sum(d => d.EndQuantity),
                        CostAmount = g.Sum(d => d.EndQuantity) * g.Key.ItemInventory.Cost,
                        SellingAmount = g.Sum(d => d.EndQuantity) * g.Key.Item.Price
                    }
                ).ToListAsync();

                return StatusCode(200, inventories);
            }
            else
            {
                return StatusCode(200, Enumerable.Empty<DTO.RepInventoryReportDTO>());
            }
        }
    }
}
