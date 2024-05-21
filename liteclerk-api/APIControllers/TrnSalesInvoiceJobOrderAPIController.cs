using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using liteclerk_api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using liteclerk_api.Utilities;
using liteclerk_api.DTO;
namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrnSalesInvoiceJobOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly List<PaginationData> paginationData = new List<PaginationData>();

        public TrnSalesInvoiceJobOrderAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NonAction]
        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list/SalesInvoice/{SIId}")]
        public async Task<ActionResult> GetSalesInvoiceJobOrderList(Int32 SIId)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();


                var jobOrders = await (
                    from d in _dbContext.TrnSalesInvoiceMFJOItems
                    where d.SIId == SIId
                    select new DTO.TrnSalesInvoiceMFJOItemDTO
                    {
                        Id = d.Id,
                        SIId = d.SIId,
                        MFJOId = d.MFJOId,
                        MFJobOrder = new DTO.TrnMFJobOrderDTO
                        {
                            JODate = d.TrnMFJobOrder_MFJOItemId.JODate.ToShortDateString(),
                            JONumber = d.TrnMFJobOrder_MFJOItemId.JONumber,
                            Engineer = d.TrnMFJobOrder_MFJOItemId.Engineer,
                            CustomerId = d.TrnMFJobOrder_MFJOItemId.CustomerId,
                            Customer = new DTO.MstArticleDTO
                            {
                                Article = d.TrnMFJobOrder_MFJOItemId.MstArticle_CustomerId.Article
                            }
                        }
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetJobOrderDetail(Int32 id)
        {
            try
            {

                var mfJobOrder = await (
                    from d in _dbContext.TrnSalesInvoiceMFJOItems
                    where d.Id == id
                    select new DTO.TrnSalesInvoiceMFJOItemDTO
                    {
                        Id = d.Id,
                        SIId = d.SIId,
                        MFJOId = d.MFJOId,
                        MFJobOrder = new DTO.TrnMFJobOrderDTO
                        {
                            JODate = d.TrnMFJobOrder_MFJOItemId.JODate.ToShortDateString(),
                            JONumber = d.TrnMFJobOrder_MFJOItemId.JONumber,
                            Engineer = d.TrnMFJobOrder_MFJOItemId.Engineer,
                            CustomerId = d.TrnMFJobOrder_MFJOItemId.CustomerId,
                            Customer = new DTO.MstArticleDTO
                            {
                                Article = d.TrnMFJobOrder_MFJOItemId.MstArticle_CustomerId.Article
                            },
                            Complaint = d.TrnMFJobOrder_MFJOItemId.Complaint
                        }
                    }).FirstOrDefaultAsync();

                return StatusCode(200, mfJobOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrder([FromBody] DTO.TrnSalesInvoiceMFJOItemDTO trnSalesInvoiceMFJOItemDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to add a job order.");
                }

                if (loginUserForm.CanAdd == false)
                {
                    return StatusCode(400, "No rights to add a job order.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnSalesInvoiceMFJOItemDTO.SIId
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales Invoice not found.");
                }

                var mfJobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == trnSalesInvoiceMFJOItemDTO.MFJOId
                    select d
                ).FirstOrDefaultAsync();

                if (mfJobOrder == null)
                {
                    return StatusCode(404, "Job Order not found.");
                }


                var newJobOrder = new DBSets.TrnSalesInvoiceMFJOItemDBSet()
                {
                    SIId = trnSalesInvoiceMFJOItemDTO.SIId,
                    MFJOId = trnSalesInvoiceMFJOItemDTO.MFJOId
                };

                _dbContext.TrnSalesInvoiceMFJOItems.Add(newJobOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJobOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJobOrder(Int32 id, [FromBody] DTO.TrnSalesInvoiceMFJOItemDTO trnSalesInvoiceMFJOItemDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to edit or save a job order.");
                }

                if (loginUserForm.CanEdit == false)
                {
                    return StatusCode(400, "No rights to edit or save a job order.");
                }

                var salesInvoice = await (
                    from d in _dbContext.TrnSalesInvoices
                    where d.Id == trnSalesInvoiceMFJOItemDTO.SIId
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoice == null)
                {
                    return StatusCode(404, "Sales Invoice not found.");
                }

                var jobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == trnSalesInvoiceMFJOItemDTO.MFJOId
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                var salesInvoiceJO = await (
                    from d in _dbContext.TrnSalesInvoiceMFJOItems
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (salesInvoiceJO == null)
                {
                    return StatusCode(404, "Sales Invoice JO not found.");
                }


                var saveSAlesInvoiceJobOrder = salesInvoiceJO;
                saveSAlesInvoiceJobOrder.SIId = trnSalesInvoiceMFJOItemDTO.SIId;
                saveSAlesInvoiceJobOrder.MFJOId = trnSalesInvoiceMFJOItemDTO.MFJOId;
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJobOrder(Int32 id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderList"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm == null)
                {
                    return StatusCode(404, "No rights to delete a job order.");
                }

                if (loginUserForm.CanDelete == false)
                {
                    return StatusCode(400, "No rights to delete a job order.");
                }

                var salesInvoiceJobOrder = await (
                     from d in _dbContext.TrnSalesInvoiceMFJOItems
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (salesInvoiceJobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }


                _dbContext.TrnSalesInvoiceMFJOItems.Remove(salesInvoiceJobOrder);
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
