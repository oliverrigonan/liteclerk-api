using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
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
    public class RepMFJobOrderReportAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepMFJobOrderReportAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byBranch/{branchId}/{status}")]
        public async Task<ActionResult> GetSalesOrderSummaryReportList(String startDate, String endDate, Int32 branchId,String status)
        {
            try
            {

                var jobOrders = await (
                    from d in _dbContext.TrnMFJobOrderLines
                    where d.TrnMFJobOrder_MFJOId.JODate >= Convert.ToDateTime(startDate)
                    && d.TrnMFJobOrder_MFJOId.JODate <= Convert.ToDateTime(endDate)
                    && d.TrnMFJobOrder_MFJOId.BranchId == branchId
                    && d.TrnMFJobOrder_MFJOId.IsLocked == true
                    && (status.ToUpper() != "ALL" ? d.TrnMFJobOrder_MFJOId.Status == status : true)
                    select new DTO.RepMFJobOrderDTO
                    {
                        Id = d.Id,
                        BranchId = d.TrnMFJobOrder_MFJOId.BranchId,
                        Branch = new DTO.MstCompanyBranchDTO
                        {
                            ManualCode = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.ManualCode,
                            Branch = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.Branch
                        },
                        SINumber = d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.Any() ?
                        d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.FirstOrDefault().TrnSalesInvoice_SIId.SINumber : "",
                        JONumber = d.TrnMFJobOrder_MFJOId.JONumber,
                        JODate = d.TrnMFJobOrder_MFJOId.JODate.ToShortDateString(),
                        ManualNumber = d.TrnMFJobOrder_MFJOId.ManualNumber,
                        DocumentReference = d.TrnMFJobOrder_MFJOId.DocumentReference,
                        CustomerId = d.TrnMFJobOrder_MFJOId.CustomerId,
                        Customer = new DTO.MstArticleDTO
                        {
                            Article = d.TrnMFJobOrder_MFJOId.MstArticle_CustomerId.Article
                        },
                        DateNeeded = d.TrnMFJobOrder_MFJOId.DateNeeded.ToShortDateString(),
                        DateScheduled = d.TrnMFJobOrder_MFJOId.DateScheduled.ToShortDateString(),
                        Remarks = d.TrnMFJobOrder_MFJOId.Remarks,
                        Complaint = d.TrnMFJobOrder_MFJOId.Complaint,
                        Accessories = d.TrnMFJobOrder_MFJOId.Accessories,
                        Engineer = d.TrnMFJobOrder_MFJOId.Engineer,
                        Status = d.TrnMFJobOrder_MFJOId.Status,
                        NoOfDays = (DateTime.Now - d.TrnMFJobOrder_MFJOId.JODate).Days,
                        Description = d.Description,
                        Brand = d.Brand,
                        Serial = d.Serial,
                        Quantity = d.Quantity,
                    }
                ).ToListAsync();

                return StatusCode(200, jobOrders);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }


        [HttpGet("print/{startDate}/{endDate}/byBranch/{branchId}/{status}")]
        public async Task<ActionResult> Print(String startDate, String endDate, Int32 branchId, String status)
        {
            FontFactory.RegisterDirectories();

            Font fontSegoeUI08 = FontFactory.GetFont("Segoe UI light", 8);
            Font fontSegoeUI08Bold = FontFactory.GetFont("Segoe UI light", 8, Font.BOLD);
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

            Document document = new Document(PageSize.Letter.Rotate(), 30f, 30f, 30f, 30f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.Black, Element.ALIGN_MIDDLE, 5F)));

            Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            DBSets.MstUserDBSet loginUser = await (
                from d in _dbContext.MstUsers
                where d.Id == loginUserId
                select d
            ).FirstOrDefaultAsync();

            if (loginUser != null)
            {
                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ReportBalanceSheet"
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
                        Int32? companyIncomeAccountId = null;

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyImageURL = loginUser.MstCompany_CompanyId.ImageURL;
                            companyIncomeAccountId = loginUser.MstCompany_CompanyId.IncomeAccountId;
                        }

                        if (companyIncomeAccountId != null)
                        {

                            Decimal totalOverallAssets = 0;
                            Decimal totalOverallLiabilities = 0;
                            Decimal totalOverallEquities = 0;

                            //String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\prime_global_logo.png";
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"), fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                            tableHeader.AddCell(new PdfPCell(new Phrase("JOB ORDER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            var jobOrders = await (
                                from d in _dbContext.TrnMFJobOrderLines
                                where d.TrnMFJobOrder_MFJOId.JODate >= Convert.ToDateTime(startDate)
                                && d.TrnMFJobOrder_MFJOId.JODate <= Convert.ToDateTime(endDate)
                                && d.TrnMFJobOrder_MFJOId.BranchId == branchId
                                && d.TrnMFJobOrder_MFJOId.IsLocked == true
                                && (status.ToUpper() != "ALL" ? d.TrnMFJobOrder_MFJOId.Status == status : true)
                                select new DTO.RepMFJobOrderDTO
                                {
                                    Id = d.Id,
                                    BranchId = d.TrnMFJobOrder_MFJOId.BranchId,
                                    Branch = new DTO.MstCompanyBranchDTO
                                    {
                                        ManualCode = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.ManualCode,
                                        Branch = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.Branch
                                    },
                                    SINumber = d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.Any() ?
                                    d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.FirstOrDefault().TrnSalesInvoice_SIId.SINumber : "",
                                    JONumber = d.TrnMFJobOrder_MFJOId.JONumber,
                                    JODate = d.TrnMFJobOrder_MFJOId.JODate.ToShortDateString(),
                                    ManualNumber = d.TrnMFJobOrder_MFJOId.ManualNumber,
                                    DocumentReference = d.TrnMFJobOrder_MFJOId.DocumentReference,
                                    CustomerId = d.TrnMFJobOrder_MFJOId.CustomerId,
                                    Customer = new DTO.MstArticleDTO
                                    {
                                        Article = d.TrnMFJobOrder_MFJOId.MstArticle_CustomerId.Article
                                    },
                                    DateNeeded = d.TrnMFJobOrder_MFJOId.DateNeeded.ToShortDateString(),
                                    Remarks = d.TrnMFJobOrder_MFJOId.Remarks,
                                    Complaint = d.TrnMFJobOrder_MFJOId .Complaint,
                                    Accessories = d.TrnMFJobOrder_MFJOId.Accessories,
                                    Engineer = d.TrnMFJobOrder_MFJOId.Engineer,
                                    Status = d.TrnMFJobOrder_MFJOId.Status,
                                    NoOfDays = (DateTime.Now - d.TrnMFJobOrder_MFJOId.JODate).Days,
                                    Description = d.Description,
                                    Brand = d.Brand,
                                    Serial = d.Serial,
                                    Quantity = d.Quantity,
                                }
                            ).ToListAsync();
                            if (jobOrders.Any())
                            {

                                PdfPTable assetAccountTypeTable = new PdfPTable(13);
                                float[] widthCellsAssetAccountTypeTable = new float[] { 50f, 50f, 50f, 20f, 50f, 50F, 50F, 50F, 50, 50F, 30f, 50f, 30f};
                                assetAccountTypeTable.SetWidths(widthCellsAssetAccountTypeTable);
                                assetAccountTypeTable.WidthPercentage = 100;
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("JO Date", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("JO Number", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Customer", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Qty", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Description", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Brand", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Serial", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Complaint", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Accesories", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Engineer", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Status", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Date Needed", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("No Of Days", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(assetAccountTypeTable);

                                foreach (var jobOrder in jobOrders)
                                {


                                    PdfPTable assetAccountTable = new PdfPTable(13);
                                    float[] widthCellsAssetAccountTable = new float[] { 50f, 50f, 50f, 20f, 50f, 50F, 50F, 50F, 50, 50F, 30f, 50f, 30f };
                                    assetAccountTable.SetWidths(widthCellsAssetAccountTable);
                                    assetAccountTable.WidthPercentage = 100;
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.JODate, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.JONumber, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Customer.Article, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Quantity.ToString("#,##0.00"), fontSegoeUI08)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Description, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Brand, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Serial, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Complaint, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Accessories, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Engineer, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Status, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.DateNeeded, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.NoOfDays.ToString("#,##0.00"), fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    document.Add(assetAccountTable);
                                }
                            }

                            //document.Add(line);
                            //PdfPTable totalAllLiabilityAndEquityTable = new PdfPTable(5);
                            //float[] widthCellsTotalAlliabilityAndEquityTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            //totalAllLiabilityAndEquityTable.SetWidths(widthCellsTotalAlliabilityAndEquityTable);
                            //totalAllLiabilityAndEquityTable.WidthPercentage = 100;
                            //totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("Total Liability and Equity", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //document.Add(totalAllLiabilityAndEquityTable);

                            //document.Add(Chunk.Newline);


                            //document.Add(line);
                            //PdfPTable balanceTable = new PdfPTable(5);
                            //float[] widthCellsBalanceTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            //balanceTable.SetWidths(widthCellsBalanceTable);
                            //balanceTable.WidthPercentage = 100;
                            //balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //balanceTable.AddCell(new PdfPCell(new Phrase("Balance", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            //document.Add(balanceTable);
                        }
                        else
                        {
                            Paragraph paragraph = new Paragraph
                            {
                                "Income account is not define in company settings."
                            };

                            document.Add(paragraph);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print sales invoice"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales invoice"
                    };

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



        [HttpGet("print/perEngineer/{startDate}/{endDate}/byBranch/{branchId}")]
        public async Task<ActionResult> PrintGroupperEngineer(String startDate, String endDate, Int32 branchId, String status)
        {
            FontFactory.RegisterDirectories();

            Font fontSegoeUI08 = FontFactory.GetFont("Segoe UI light", 8);
            Font fontSegoeUI08Bold = FontFactory.GetFont("Segoe UI light", 8, Font.BOLD);
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

            Document document = new Document(PageSize.Letter.Rotate(), 30f, 30f, 30f, 30f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.Black, Element.ALIGN_MIDDLE, 5F)));

            Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

            DBSets.MstUserDBSet loginUser = await (
                from d in _dbContext.MstUsers
                where d.Id == loginUserId
                select d
            ).FirstOrDefaultAsync();

            if (loginUser != null)
            {
                DBSets.MstUserFormDBSet loginUserForm = await (
                    from d in _dbContext.MstUserForms
                    where d.UserId == loginUserId
                    && d.SysForm_FormId.Form == "ReportBalanceSheet"
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
                        Int32? companyIncomeAccountId = null;

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyImageURL = loginUser.MstCompany_CompanyId.ImageURL;
                            companyIncomeAccountId = loginUser.MstCompany_CompanyId.IncomeAccountId;
                        }

                        if (companyIncomeAccountId != null)
                        {

                            Decimal totalOverallAssets = 0;
                            Decimal totalOverallLiabilities = 0;
                            Decimal totalOverallEquities = 0;

                            //String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\prime_global_logo.png";
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
                            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"), fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                            tableHeader.AddCell(new PdfPCell(new Phrase("JOB ORDER GROUPED BY ENGINEER", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            var jobOrders = await (
                                from d in _dbContext.TrnMFJobOrderLines
                                where d.TrnMFJobOrder_MFJOId.JODate >= Convert.ToDateTime(startDate)
                                && d.TrnMFJobOrder_MFJOId.JODate <= Convert.ToDateTime(endDate)
                                && d.TrnMFJobOrder_MFJOId.BranchId == branchId
                                && d.TrnMFJobOrder_MFJOId.IsLocked == true
                                select new DTO.RepMFJobOrderDTO
                                {
                                    Id = d.Id,
                                    BranchId = d.TrnMFJobOrder_MFJOId.BranchId,
                                    Branch = new DTO.MstCompanyBranchDTO
                                    {
                                        ManualCode = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.ManualCode,
                                        Branch = d.TrnMFJobOrder_MFJOId.MstCompanyBranch_BranchId.Branch
                                    },
                                    SINumber = d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.Any() ?
                                    d.TrnMFJobOrder_MFJOId.TrnSalesInvoiceMFJOItem_MFJOSIId.FirstOrDefault().TrnSalesInvoice_SIId.SINumber : "",
                                    JONumber = d.TrnMFJobOrder_MFJOId.JONumber,
                                    JODate = d.TrnMFJobOrder_MFJOId.JODate.ToShortDateString(),
                                    ManualNumber = d.TrnMFJobOrder_MFJOId.ManualNumber,
                                    DocumentReference = d.TrnMFJobOrder_MFJOId.DocumentReference,
                                    CustomerId = d.TrnMFJobOrder_MFJOId.CustomerId,
                                    Customer = new DTO.MstArticleDTO
                                    {
                                        Article = d.TrnMFJobOrder_MFJOId.MstArticle_CustomerId.Article
                                    },
                                    DateNeeded = d.TrnMFJobOrder_MFJOId.DateNeeded.ToShortDateString(),
                                    Remarks = d.TrnMFJobOrder_MFJOId.Remarks,
                                    Complaint = d.TrnMFJobOrder_MFJOId.Complaint,
                                    Accessories = d.TrnMFJobOrder_MFJOId.Accessories,
                                    Engineer = d.TrnMFJobOrder_MFJOId.Engineer,
                                    Status = d.TrnMFJobOrder_MFJOId.Status,
                                    NoOfDays = (DateTime.Now - d.TrnMFJobOrder_MFJOId.JODate).Days,
                                    Description = d.Description,
                                    Brand = d.Brand,
                                    Serial = d.Serial,
                                    Quantity = d.Quantity,
                                }
                            ).ToListAsync();
                            if (jobOrders.Any())
                            {
                                var engineerGrouped = from d in jobOrders
                                                      group d by d.Engineer into g
                                                      select new
                                                      {
                                                          Engineer = g.Key
                                                      };
                                foreach (var engineer in engineerGrouped)
                                {


                                    PdfPTable assetAccountTypeTable = new PdfPTable(12);
                                    float[] widthCellsAssetAccountTypeTable = new float[] { 50f, 50f, 50f, 20f, 50f, 50F, 50F, 50F, 50F, 30f, 50f, 30f };
                                    assetAccountTypeTable.SetWidths(widthCellsAssetAccountTypeTable);
                                    assetAccountTypeTable.WidthPercentage = 100;
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI08Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f, Colspan=12 });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Engineer: ", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f});
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase(engineer.Engineer, fontSegoeUI08)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f, Colspan=11 });


                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("JO Date", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("JO Number", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Customer", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Qty", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER , HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Description", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER , HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Brand", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Serial", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Complaint", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Accesories", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Status", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("Date Needed", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("No Of Days", fontSegoeUI08Bold)) { Border = PdfCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 10f, PaddingBottom = 5f });
                                    document.Add(assetAccountTypeTable);

                                    foreach (var jobOrder in jobOrders.Where(x => x.Engineer == engineer.Engineer))
                                    {
                                        PdfPTable assetAccountTable = new PdfPTable(12);
                                        float[] widthCellsAssetAccountTable = new float[] { 50f, 50f, 50f, 20f, 50f, 50F, 50F, 50F, 50F, 30f, 50f, 30f };
                                        assetAccountTable.SetWidths(widthCellsAssetAccountTable);
                                        assetAccountTable.WidthPercentage = 100;
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.JODate, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.JONumber, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Customer.Article, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Quantity.ToString("#,##0.00"), fontSegoeUI08)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Description, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Brand, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Serial, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Complaint, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Accessories, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.Status, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.DateNeeded, fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(jobOrder.NoOfDays.ToString("#,##0.00"), fontSegoeUI08)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        document.Add(assetAccountTable);
                                    }

                                }
 }

                        }
                        else
                        {
                            Paragraph paragraph = new Paragraph
                            {
                                "Income account is not define in company settings."
                            };

                            document.Add(paragraph);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print sales invoice"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales invoice"
                    };

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
