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
namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrnMFJobOrderAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly List<PaginationData> paginationData = new List<PaginationData>();

        public TrnMFJobOrderAPIController(DBContext.LiteclerkDBContext dbContext)
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

        [HttpGet("list/byStatus/{status}")]
        public async Task<ActionResult> GetJobOrderListByStatus(String status)
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
                    from d in _dbContext.TrnJobOrders
                    where d.BranchId == loginUser.BranchId
                    && d.Status == status
                    && d.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true
                    orderby d.Id descending
                    select new DTO.TrnJobOrderDTO
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
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        SIId = d.SIId,
                        SalesInvoice = new DTO.TrnSalesInvoiceDTO
                        {
                            SINumber = d.TrnSalesInvoice_SIId.SINumber,
                            SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                            ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                            DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference,
                            CustomerId = d.TrnSalesInvoice_SIId.CustomerId,
                            Customer = new DTO.MstArticleCustomerDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.ManualCode
                                },
                                Customer = d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true ?
                                           d.TrnSalesInvoice_SIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer : "",
                            }
                        },
                        SIItemId = d.SIItemId,
                        SalesInvoiceItem = new DTO.TrnSalesInvoiceItemDTO
                        {
                            Item = new DTO.MstArticleItemDTO
                            {
                                Article = new DTO.MstArticleDTO
                                {
                                    ManualCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.ManualCode
                                },
                                SKUCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode : "",
                                BarCode = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                          d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode : "",
                                Description = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description : "",
                                IsInventory = d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.Any() == true ?
                                              d.TrnSalesInvoiceItem_SIItemId.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory : false
                            }
                        },
                        ItemId = d.ItemId,
                        Item = new DTO.MstArticleItemDTO
                        {
                            Article = new DTO.MstArticleDTO
                            {
                                ManualCode = d.MstArticle_ItemId.ManualCode
                            },
                            SKUCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().SKUCode,
                            BarCode = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().BarCode,
                            Description = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().Description,
                            IsInventory = d.MstArticle_ItemId.MstArticleItems_ArticleId.FirstOrDefault().IsInventory,
                        },
                        ItemJobTypeId = d.ItemJobTypeId,
                        ItemJobType = new DTO.MstJobTypeDTO
                        {
                            JobType = d.MstJobType_ItemJobTypeId.JobType
                        },
                        Quantity = d.Quantity,
                        UnitId = d.UnitId,
                        Unit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_UnitId.ManualCode,
                            Unit = d.MstUnit_UnitId.Unit
                        },
                        Remarks = d.Remarks,
                        BaseQuantity = d.BaseQuantity,
                        BaseUnitId = d.BaseUnitId,
                        BaseUnit = new DTO.MstUnitDTO
                        {
                            ManualCode = d.MstUnit_BaseUnitId.ManualCode,
                            Unit = d.MstUnit_BaseUnitId.Unit
                        },
                        CurrentDepartment = d.CurrentDepartment,
                        CurrentDepartmentStatus = d.CurrentDepartmentStatus,
                        CurrentDepartmentUserFullName = d.CurrentDepartmentUserFullName,
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

                return StatusCode(200, jobOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byStatus/{status}/paginated/{startDate}/{endDate}/{column}/meta")]
        public async Task<ActionResult> GetPaginatedMFJobOrderListByStatus(String status,String startDate, String endDate, String column, String? keywords, [FromQuery] PaginationHelper @params)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                List<DTO.TrnMFJobOrderDTO> mfJobOrderList = new List<DTO.TrnMFJobOrderDTO>();
                var mfJobOrdersCount = await (
                   from d in _dbContext.TrnMFJobOrders
                   where d.BranchId == loginUser.BranchId
                   && (status.ToUpper() != "ALL" ? d.Status == status : true)
                   && (
                       keywords == "" || String.IsNullOrEmpty(keywords) ? true :
                       column == "All" ? d.JONumber.Contains(keywords) ||
                                         d.ManualNumber.Contains(keywords) ||
                                         d.MstArticle_CustomerId.Article.Contains(keywords) ||
                                         d.DocumentReference.Contains(keywords) ||
                                         d.Remarks.Contains(keywords) ||
                                         d.Engineer.Contains(keywords) ||
                                         d.Complaint.Contains(keywords) ||
                                         d.Status.Contains(keywords) :
                       column == "JO No." ? d.JONumber.Contains(keywords) :
                       column == "Manual No." ? d.ManualNumber.Contains(keywords) :
                       column == "Customer" ? d.MstArticle_CustomerId.Article.Contains(keywords) :
                       column == "Document Reference" ? d.DocumentReference.Contains(keywords) :
                       column == "Remarks" ? d.Remarks.Contains(keywords) :
                       column == "Engineer" ? d.Engineer.Contains(keywords) :
                       column == "Complaint" ? d.Complaint.Contains(keywords) : true
                       )
                       select new {d.Id}
                       ).CountAsync();

                IEnumerable<DTO.TrnMFJobOrderDTO> mfJobOrders = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.BranchId == loginUser.BranchId
                    && (status.ToUpper() != "ALL" ? d.Status == status : true)
                    && (
                        keywords == "" || String.IsNullOrEmpty(keywords) ? true :
                        column == "All" ? d.JONumber.Contains(keywords) ||
                                          d.ManualNumber.Contains(keywords) ||
                                          d.MstArticle_CustomerId.Article.Contains(keywords) ||
                                          d.DocumentReference.Contains(keywords) ||
                                          d.Remarks.Contains(keywords) ||
                                          d.Engineer.Contains(keywords) ||
                                          d.Complaint.Contains(keywords) ||
                                          d.Status.Contains(keywords) :
                        column == "JO No." ? d.JONumber.Contains(keywords) :
                        column == "Manual No." ? d.ManualNumber.Contains(keywords) :
                        column == "Customer" ? d.MstArticle_CustomerId.Article.Contains(keywords) :
                        column == "Document Reference" ? d.DocumentReference.Contains(keywords) :
                        column == "Remarks" ? d.Remarks.Contains(keywords) :
                        column == "Engineer" ? d.Engineer.Contains(keywords) :
                        column == "Complaint" ? d.Complaint.Contains(keywords) :
                        column == "Status" ? d.Status.Contains(keywords) : true
                        )
                    select new DTO.TrnMFJobOrderDTO
                    {

                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_CustomerId.ManualCode,
                            Article = d.MstArticle_CustomerId.Article
                        },
                        Accessories = d.Accessories,
                        Engineer = d.Engineer,
                        Complaint = d.Complaint,
                        Remarks = d.Remarks,
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
                    }).OrderByDescending(p => p.Id).Skip((@params.PageNumber - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToListAsync();

                mfJobOrderList.AddRange(mfJobOrders);
                var paginationMetaData = new PaginationMetaData(mfJobOrdersCount, @params.PageNumber, @params.ItemsPerPage);

                paginationData.Add(new PaginationData
                {
                    Data = mfJobOrderList,
                    MetaData = paginationMetaData
                });


                return StatusCode(200, paginationData);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/byCustomer/{CustomerId}")]
        public async Task<ActionResult> GetJobOrderListBySalesInvoice(Int32 CustomerId)
        {
            try
            {
                var jobOrders = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.CustomerId == CustomerId
                    && d.IsLocked == true
                    && d.IsCancelled == false
                    && d.Status.ToUpper() == "NEW"
                    select new DTO.TrnMFJobOrderDTO
                    {

                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_CustomerId.ManualCode,
                            Article = d.MstArticle_CustomerId.Article
                        },
                        Accessories = d.Accessories,
                        Engineer = d.Engineer,
                        Complaint = d.Complaint,
                        Remarks = d.Remarks,
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
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == id
                    select new DTO.TrnMFJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.MstCompanyBranch_BranchId.Branch
                        },
                        JONumber = d.JONumber,
                        JODate = d.JODate.ToShortDateString(),
                        ManualNumber = d.ManualNumber,
                        DocumentReference = d.DocumentReference,
                        DateScheduled = d.DateScheduled.ToShortDateString(),
                        DateNeeded = d.DateNeeded.ToShortDateString(),
                        CustomerId = d.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            ManualCode = d.MstArticle_CustomerId.ManualCode,
                            Article = d.MstArticle_CustomerId.Article
                        },
                        Engineer = d.Engineer,
                        Complaint = d.Complaint,
                        Remarks = d.Remarks,
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
                    }).FirstOrDefaultAsync();

                return StatusCode(200, mfJobOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddJobOrder()
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

                var item = await (
                    from d in _dbContext.MstArticleItems
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404, "Item not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var customer = await (
                    from d in _dbContext.MstArticleCustomers
                    where d.MstArticle_ArticleId.IsLocked == true
                    select d
                ).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return StatusCode(404, "Customer not found.");
                }

                String JONumber = "0000000001";
                DBSets.TrnMFJobOrderDBSet lastJobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.BranchId == loginUser.BranchId
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastJobOrder != null)
                {
                    Int32 lastSINumber = Convert.ToInt32(lastJobOrder.JONumber) + 0000000001;
                    JONumber = PadZeroes(lastSINumber, 10);
                }

                var newJobOrder = new DBSets.TrnMFJobOrderDBSet()
                {
                    BranchId = Convert.ToInt32(loginUser.BranchId),
                    JONumber = JONumber,
                    JODate = DateTime.Today,
                    ManualNumber = JONumber,
                    DocumentReference = JONumber,
                    DateScheduled = DateTime.Today,
                    DateNeeded = DateTime.Today,
                    CustomerId = customer.ArticleId,
                    Engineer = "NA",
                    Accessories = "NA",
                    Complaint = "NA",
                    Remarks = "NA",
                    PreparedByUserId = loginUserId,
                    CheckedByUserId = loginUserId,
                    ApprovedByUserId = loginUserId,
                    Status = codeTableStatus.CodeValue,
                    IsCancelled = false,
                    IsPrinted = false,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.TrnMFJobOrders.Add(newJobOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newJobOrder.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveJobOrder(Int32 id, [FromBody] DTO.TrnMFJobOrderDTO trnMFJobOrderDTO)
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

                var jobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a job order that is locked.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnMFJobOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnMFJobOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnMFJobOrderDTO.Status
                    && d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var saveJobOrder = jobOrder;
                saveJobOrder.JODate = Convert.ToDateTime(trnMFJobOrderDTO.JODate);
                saveJobOrder.ManualNumber = trnMFJobOrderDTO.ManualNumber;
                saveJobOrder.DocumentReference = trnMFJobOrderDTO.DocumentReference;
                saveJobOrder.DateScheduled = Convert.ToDateTime(trnMFJobOrderDTO.DateScheduled);
                saveJobOrder.DateNeeded = Convert.ToDateTime(trnMFJobOrderDTO.DateNeeded);
                saveJobOrder.CustomerId = trnMFJobOrderDTO.CustomerId;
                saveJobOrder.Engineer = trnMFJobOrderDTO.Engineer;
                saveJobOrder.Complaint = trnMFJobOrderDTO.Complaint;
                saveJobOrder.Remarks = trnMFJobOrderDTO.Remarks;
                saveJobOrder.CheckedByUserId = trnMFJobOrderDTO.CheckedByUserId;
                saveJobOrder.ApprovedByUserId = trnMFJobOrderDTO.ApprovedByUserId;
                saveJobOrder.Status = trnMFJobOrderDTO.Status;
                saveJobOrder.UpdatedByUserId = loginUserId;
                saveJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockJobOrder(Int32 id, [FromBody] DTO.TrnMFJobOrderDTO trnMFJobOrderDTO)
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
                    return StatusCode(404, "No rights to lock a job order.");
                }

                if (loginUserForm.CanLock == false)
                {
                    return StatusCode(400, "No rights to lock a job order.");
                }

                var jobOrder = await (
                    from d in _dbContext.TrnMFJobOrders
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a job order that is locked.");
                }

                var checkedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnMFJobOrderDTO.CheckedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (checkedByUser == null)
                {
                    return StatusCode(404, "Checked by user not found.");
                }

                var approvedByUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == trnMFJobOrderDTO.ApprovedByUserId
                    select d
                ).FirstOrDefaultAsync();

                if (approvedByUser == null)
                {
                    return StatusCode(404, "Approved by user not found.");
                }

                var codeTableStatus = await (
                    from d in _dbContext.MstCodeTables
                    where d.CodeValue == trnMFJobOrderDTO.Status
                    && d.Category == "JOB ORDER STATUS"
                    select d
                ).FirstOrDefaultAsync();

                if (codeTableStatus == null)
                {
                    return StatusCode(404, "Status not found.");
                }

                var lockJobOrder = jobOrder;
                lockJobOrder.JODate = Convert.ToDateTime(trnMFJobOrderDTO.JODate);
                lockJobOrder.ManualNumber = trnMFJobOrderDTO.ManualNumber;
                lockJobOrder.DocumentReference = trnMFJobOrderDTO.DocumentReference;
                lockJobOrder.DateScheduled = Convert.ToDateTime(trnMFJobOrderDTO.DateScheduled);
                lockJobOrder.DateNeeded = Convert.ToDateTime(trnMFJobOrderDTO.DateNeeded);
                lockJobOrder.CustomerId = trnMFJobOrderDTO.CustomerId;
                lockJobOrder.Engineer = trnMFJobOrderDTO.Engineer;
                lockJobOrder.Complaint = trnMFJobOrderDTO.Complaint;
                lockJobOrder.Remarks = trnMFJobOrderDTO.Remarks;
                lockJobOrder.CheckedByUserId = trnMFJobOrderDTO.CheckedByUserId;
                lockJobOrder.ApprovedByUserId = trnMFJobOrderDTO.ApprovedByUserId;
                lockJobOrder.Status = trnMFJobOrderDTO.Status;
                lockJobOrder.IsLocked = true;
                lockJobOrder.UpdatedByUserId = loginUserId;
                lockJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockJobOrder(Int32 id)
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
                    return StatusCode(404, "No rights to unlock a job order.");
                }

                if (loginUserForm.CanUnlock == false)
                {
                    return StatusCode(400, "No rights to unlock a job order.");
                }

                var jobOrder = await (
                     from d in _dbContext.TrnMFJobOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a job order that is unlocked.");
                }

                var unlockJobOrder = jobOrder;
                unlockJobOrder.IsLocked = false;
                unlockJobOrder.UpdatedByUserId = loginUserId;
                unlockJobOrder.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> CancelJobOrder(Int32 id)
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
                    return StatusCode(404, "No rights to cancel a job order.");
                }

                if (loginUserForm.CanCancel == false)
                {
                    return StatusCode(400, "No rights to cancel a job order.");
                }

                var jobOrder = await (
                     from d in _dbContext.TrnMFJobOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == false)
                {
                    return StatusCode(400, "Cannot cancel a job order that is unlocked.");
                }

                var unlockJobOrder = jobOrder;
                unlockJobOrder.IsCancelled = true;
                unlockJobOrder.UpdatedByUserId = loginUserId;
                unlockJobOrder.UpdatedDateTime = DateTime.Now;

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

                var jobOrder = await (
                     from d in _dbContext.TrnMFJobOrders
                     where d.Id == id
                     select d
                 ).FirstOrDefaultAsync(); ;

                if (jobOrder == null)
                {
                    return StatusCode(404, "Job order not found.");
                }

                if (jobOrder.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a job order that is locked.");
                }

                _dbContext.TrnMFJobOrders.Remove(jobOrder);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult> PrintJobOrder(Int32 id)
        {
            FontFactory.RegisterDirectories();

            Font fontSegoeUI09 = FontFactory.GetFont("Segoe UI light", 9);
            Font fontSegoeUI09Bold = FontFactory.GetFont("Segoe UI light", 9, Font.BOLD);
            Font fontSegoeUI10 = FontFactory.GetFont("Segoe UI light", 10);
            Font fontSegoeUI10Bold = FontFactory.GetFont("Segoe UI light", 10, Font.BOLD);
            Font fontSegoeUI11 = FontFactory.GetFont("Segoe UI light", 11);
            Font fontSegoeUI11Bold = FontFactory.GetFont("Segoe UI light", 11, Font.BOLD);
            Font fontSegoeUI12 = FontFactory.GetFont("Segoe UI light", 12);
            Font fontSegoeUI12Bold = FontFactory.GetFont("Segoe UI light", 12, Font.BOLD);
            Font fontSegoeUI13 = FontFactory.GetFont("Segoe UI light", 13);
            Font fontSegoeUI13Bold = FontFactory.GetFont("Segoe UI light", 13, Font.BOLD);
            Font fontSegoeUI14 = FontFactory.GetFont("Segoe UI light", 14);
            Font fontSegoeUI14Bold = FontFactory.GetFont("Segoe UI light", 14, Font.BOLD);
            Font fontSegoeUI15 = FontFactory.GetFont("Segoe UI light", 15);
            Font fontSegoeUI15Bold = FontFactory.GetFont("Segoe UI light", 15, Font.BOLD);
            Font fontSegoeUI16 = FontFactory.GetFont("Segoe UI light", 16);
            Font fontSegoeUI16Bold = FontFactory.GetFont("Segoe UI light", 16, Font.BOLD);

            Document document = new Document(PageSize.Letter, 30f, 30f, 30f, 30f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.Black, Element.ALIGN_MIDDLE, 5F)));

            Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            var loginUser = await (
                from d in _dbContext.MstUsers
                where d.Id == loginUserId
                select d
            ).FirstOrDefaultAsync();

            if (loginUser != null)
            {
                var loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ActivityJobOrderDetail"
                    select d
                ).FirstOrDefaultAsync();

                if (loginUserForm != null)
                {
                    if (loginUserForm.CanPrint == true)
                    {
                        String companyName = "";
                        String companyAddress = "";
                        String companyTaxNumber = "";
                        String companyImageURL = "";

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyImageURL = loginUser.MstCompany_CompanyId.ImageURL;
                        }

                        DBSets.TrnMFJobOrderDBSet jobOrder = await (
                            from d in _dbContext.TrnMFJobOrders
                            where d.Id == id
                            && d.IsLocked == true
                            select d
                        ).FirstOrDefaultAsync();
                        for (id = 1; id <= 2; id++)
                        {
                            if (jobOrder != null)
                            {
                                String reprinted = "";
                                if (jobOrder.IsPrinted == true)
                                {
                                    reprinted = "(REPRINTED)";
                                }

                                //String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\colorideas_logo.png";
                                String logoPath = companyImageURL;

                                Image logoPhoto = Image.GetInstance(logoPath);
                                logoPhoto.Alignment = Image.ALIGN_JUSTIFIED;

                                PdfPCell logoPhotoPdfCell = new PdfPCell(logoPhoto, true) { FixedHeight = 40f };
                                logoPhotoPdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                PdfPTable tableHeader = new PdfPTable(2);
                                tableHeader.SetWidths(new float[] { 80f, 20f });
                                tableHeader.WidthPercentage = 100f;
                                tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontSegoeUI13Bold)) { Border = 0 });
                                tableHeader.AddCell(new PdfPCell(logoPhotoPdfCell) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 3f, Rowspan = 4 });
                                tableHeader.AddCell(new PdfPCell(new Phrase(companyAddress, fontSegoeUI09)) { Border = 0 });
                                tableHeader.AddCell(new PdfPCell(new Phrase(companyTaxNumber, fontSegoeUI09)) { Border = 0 });
                                tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt") + " " + reprinted, fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                                tableHeader.AddCell(new PdfPCell(new Phrase("JOB ORDER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                                document.Add(tableHeader);

                                String Customer = jobOrder.CustomerId != null ?
                                                  jobOrder.MstArticle_CustomerId.Article : "";
                                String ContactNumber = jobOrder.CustomerId != null ?
                                                  jobOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactNumber : "";
                                String ContactPerson = jobOrder.CustomerId != null ?
                                                  jobOrder.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().ContactPerson : "";
                                String remarks = jobOrder.Remarks;

                                String branch = jobOrder.MstCompanyBranch_BranchId.Branch;
                                String JONumber = "JO-" + jobOrder.MstCompanyBranch_BranchId.ManualCode + "-" + jobOrder.JONumber;
                                String JODate = jobOrder.JODate.ToString("MMMM dd, yyyy");
                                String dateScheduled = jobOrder.DateScheduled.ToString("MMMM dd, yyyy");
                                String dateNeeded = jobOrder.DateNeeded.ToString("MMMM dd, yyyy");
                                String manualNumber = jobOrder.ManualNumber;
                                String documentReference = jobOrder.DocumentReference;

                                PdfPTable tableJobOrder = new PdfPTable(4);
                                tableJobOrder.SetWidths(new float[] { 55f, 130f, 50f, 100f });
                                tableJobOrder.WidthPercentage = 100;
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Customer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(Customer, fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(JODate, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Contact Info:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(ContactPerson + " \n" + ContactNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Date Out:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(dateScheduled, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                                //tableJobOrder.AddCell(new PdfPCell(new Phrase("Date Needed:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableJobOrder.AddCell(new PdfPCell(new Phrase(dateNeeded, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });

                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Engineer:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(jobOrder.Engineer, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Manual No:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(manualNumber, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });


                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Accessories:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(jobOrder.Accessories, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Document Ref:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Complaint:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(jobOrder.Complaint, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase("Remarks:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrder.AddCell(new PdfPCell(new Phrase(remarks, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                                document.Add(tableJobOrder);

                                PdfPTable tableSpace = new PdfPTable(1);
                                tableSpace.WidthPercentage = 100;
                                tableSpace.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { HorizontalAlignment = 1, Border = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                document.Add(tableSpace);

                                IEnumerable<DBSets.TrnMFJobOrderLineDBSet> jobOrderLines = await (
                                    from d in _dbContext.TrnMFJobOrderLines
                                    where d.MFJOId == jobOrder.Id
                                    select d
                                ).ToListAsync();

                                if (jobOrderLines.Any())
                                {
                                    PdfPTable tableJobOrderLine = new PdfPTable(4);
                                    tableJobOrderLine.SetWidths(new float[] { 100f, 100f, 150f, 50f });
                                    tableJobOrderLine.WidthPercentage = 100;

                                    tableJobOrderLine.AddCell(new PdfPCell(new Phrase("Brand", fontSegoeUI09Bold)) { HorizontalAlignment = 1, Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderLine.AddCell(new PdfPCell(new Phrase("Serial", fontSegoeUI09Bold)) { HorizontalAlignment = 1, Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderLine.AddCell(new PdfPCell(new Phrase("Description", fontSegoeUI09Bold)) { HorizontalAlignment = 1, Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableJobOrderLine.AddCell(new PdfPCell(new Phrase("Quantity", fontSegoeUI09Bold)) { HorizontalAlignment = 1, Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                    foreach (var jobOrderLine in jobOrderLines)
                                    {
                                        tableJobOrderLine.AddCell(new PdfPCell(new Phrase(jobOrderLine.Brand, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                        tableJobOrderLine.AddCell(new PdfPCell(new Phrase(jobOrderLine.Serial, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                        tableJobOrderLine.AddCell(new PdfPCell(new Phrase(jobOrderLine.Description, fontSegoeUI09)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                        tableJobOrderLine.AddCell(new PdfPCell(new Phrase(jobOrderLine.Quantity.ToString("#,##0.00"), fontSegoeUI09)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    }
                                    document.Add(tableJobOrderLine);
                                    document.Add(tableSpace);


                                }

                                document.Add(tableSpace);

                                String preparedBy = jobOrder.MstUser_PreparedByUserId.Fullname;
                                String checkedBy = jobOrder.MstUser_CheckedByUserId.Fullname;
                                String approvedBy = jobOrder.MstUser_ApprovedByUserId.Fullname;

                                PdfPTable tableJobOrderFooter = new PdfPTable(4);
                                tableJobOrderFooter.SetWidths(new float[] { 100f, 100f, 50f, 100f });
                                tableJobOrderFooter.WidthPercentage = 100;
                                //tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("Units not claimed within 60 days after the repair completion notice date will be charge a storage fee of P 25.0 per day.", fontSegoeUI09)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("Engineer: ", fontSegoeUI09)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("Units not claimed within 30 days after the repair completion notice date will be charge a storage fee of P 25.00 per day.", fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("Engineer: ", fontSegoeUI09Bold)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase(jobOrder.Engineer, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("Received by: ", fontSegoeUI09Bold)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = PdfCell.BOTTOM_BORDER });
                                
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = 0, Colspan=4 });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = PdfCell.BOTTOM_BORDER, Colspan=4, BorderWidthBottom = 0.5f, BorderWidth = 0 });
                                tableJobOrderFooter.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = 0, Colspan = 4 });

                                document.Add(tableJobOrderFooter);

                                //PdfPTable tableUsers = new PdfPTable(4);
                                //tableUsers.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                                //tableUsers.WidthPercentage = 100;
                                //tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase("Received by", fontSegoeUI09Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 30f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontSegoeUI09)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //tableUsers.AddCell(new PdfPCell(new Phrase("Date Received:", fontSegoeUI09Bold)) { HorizontalAlignment = 0, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                                //document.Add(tableUsers);
                            }
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Add("No rights to print sales invoice - job orders");

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Add("No rights to print sales invoice - job orders");

                    document.Add(paragraph);
                }
            }
            else
            {
                document.Add(line);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}
