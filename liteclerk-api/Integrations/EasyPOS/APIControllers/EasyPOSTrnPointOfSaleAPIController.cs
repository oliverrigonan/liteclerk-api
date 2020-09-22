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

namespace liteclerk_api.Integrations.EasyPOS.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EasyPOSTrnPointOfSaleAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public EasyPOSTrnPointOfSaleAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<ActionResult> AddPointOfSale(List<DTO.EasyPOSTrnPointOfSaleDTO> easyPOSTrnPointOfSaleDTOs)
        {
            try
            {
                if (easyPOSTrnPointOfSaleDTOs.Any())
                {
                    Int32 returnId = 0;

                    DBSets.MstCompanyBranchDBSet branch = await (
                        from d in _dbContext.MstCompanyBranches
                        where d.ManualCode == easyPOSTrnPointOfSaleDTOs.FirstOrDefault().BranchCode
                        && d.MstCompany_CompanyId.IsLocked == true
                        select d
                    ).FirstOrDefaultAsync();

                    if (branch == null)
                    {
                        return StatusCode(404, "Branch not found.");
                    }

                    foreach (var easyPOSTrnPointOfSaleDTO in easyPOSTrnPointOfSaleDTOs)
                    {
                        DBSets.TrnPointOfSaleDBSet newPointOfSale = new DBSets.TrnPointOfSaleDBSet()
                        {
                            BranchId = branch.Id,
                            TerminalCode = easyPOSTrnPointOfSaleDTO.TerminalCode,
                            POSDate = Convert.ToDateTime(easyPOSTrnPointOfSaleDTO.POSDate),
                            POSNumber = easyPOSTrnPointOfSaleDTO.POSNumber,
                            OrderNumber = easyPOSTrnPointOfSaleDTO.OrderNumber,
                            CustomerCode = easyPOSTrnPointOfSaleDTO.CustomerCode,
                            CustomerId = null,
                            ItemCode = easyPOSTrnPointOfSaleDTO.ItemCode,
                            ItemId = null,
                            Particulars = easyPOSTrnPointOfSaleDTO.Particulars,
                            Quantity = easyPOSTrnPointOfSaleDTO.Quantity,
                            Price = easyPOSTrnPointOfSaleDTO.Price,
                            Discount = easyPOSTrnPointOfSaleDTO.Discount,
                            NetPrice = easyPOSTrnPointOfSaleDTO.NetPrice,
                            Amount = easyPOSTrnPointOfSaleDTO.Amount,
                            TaxCode = easyPOSTrnPointOfSaleDTO.TaxCode,
                            TaxId = null,
                            TaxAmount = easyPOSTrnPointOfSaleDTO.TaxAmount,
                            CashierUserCode = easyPOSTrnPointOfSaleDTO.CashierUserCode,
                            CashierUserId = null,
                            TimeStamp = Convert.ToDateTime(easyPOSTrnPointOfSaleDTO.TimeStamp),
                            PostCode = null
                        };

                        _dbContext.TrnPointOfSales.Add(newPointOfSale);
                        await _dbContext.SaveChangesAsync();

                        if (returnId == 0)
                        {
                            returnId = newPointOfSale.Id;
                        }
                    }

                    return StatusCode(200, returnId);
                }
                else
                {
                    return StatusCode(404, "Data is empty.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
