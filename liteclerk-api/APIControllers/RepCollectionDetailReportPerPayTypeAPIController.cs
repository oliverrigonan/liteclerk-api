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
    public class RepCollectionDetailReportPerPayTypeAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepCollectionDetailReportPerPayTypeAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> GetCollectionDetailReportPerPayTypeList(String startDate, String endDate, Int32 companyId, Int32 branchId)
        {
            try
            {
                List<DTO.TrnCollectionLineDTO> newCollectionLinesPerPayType = new List<DTO.TrnCollectionLineDTO>();

                var collectionLines = await (
                    from d in _dbContext.TrnCollectionLines
                    where d.TrnCollection_CIId.CIDate >= Convert.ToDateTime(startDate)
                    && d.TrnCollection_CIId.CIDate <= Convert.ToDateTime(endDate)
                    && d.TrnCollection_CIId.MstCompanyBranch_BranchId.CompanyId == companyId
                    && d.TrnCollection_CIId.BranchId == branchId
                    && d.TrnCollection_CIId.IsLocked == true
                    && d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true
                    && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
                    select d
                ).ToListAsync();

                if (collectionLines.Any() == true)
                {
                    var payTypes = from d in collectionLines
                                   group d by d.PayTypeId
                                   into g
                                   select g;

                    if (payTypes.Any())
                    {
                        foreach (var payType in payTypes)
                        {
                            var collectionLinesPerPayType = from d in collectionLines
                                                            where d.PayTypeId == payType.Key
                                                            select d;

                            if (collectionLinesPerPayType.Any())
                            {
                                var newCollectionLines = from d in collectionLinesPerPayType
                                                         select new DTO.TrnCollectionLineDTO
                                                         {
                                                             Id = d.Id,
                                                             CIId = d.CIId,
                                                             Collection = new DTO.TrnCollectionDTO
                                                             {
                                                                 Id = d.TrnCollection_CIId.Id,
                                                                 BranchId = d.TrnCollection_CIId.BranchId,
                                                                 Branch = new DTO.MstCompanyBranchDTO
                                                                 {
                                                                     ManualCode = d.TrnCollection_CIId.MstCompanyBranch_BranchId.ManualCode,
                                                                     Branch = d.TrnCollection_CIId.MstCompanyBranch_BranchId.Branch
                                                                 },
                                                                 CurrencyId = d.TrnCollection_CIId.CurrencyId,
                                                                 Currency = new DTO.MstCurrencyDTO
                                                                 {
                                                                     ManualCode = d.TrnCollection_CIId.MstCurrency_CurrencyId.ManualCode,
                                                                     Currency = d.TrnCollection_CIId.MstCurrency_CurrencyId.Currency
                                                                 },
                                                                 CINumber = d.TrnCollection_CIId.CINumber,
                                                                 CIDate = d.TrnCollection_CIId.CIDate.ToShortDateString(),
                                                                 ManualNumber = d.TrnCollection_CIId.ManualNumber,
                                                                 DocumentReference = d.TrnCollection_CIId.DocumentReference,
                                                                 CustomerId = d.TrnCollection_CIId.CustomerId,
                                                                 Customer = new DTO.MstArticleCustomerDTO
                                                                 {
                                                                     Article = new DTO.MstArticleDTO
                                                                     {
                                                                         ManualCode = d.TrnCollection_CIId.MstArticle_CustomerId.ManualCode
                                                                     },
                                                                     Customer = d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer
                                                                 },
                                                                 Remarks = d.TrnCollection_CIId.Remarks,
                                                                 PreparedByUserId = d.TrnCollection_CIId.PreparedByUserId,
                                                                 PreparedByUser = new DTO.MstUserDTO
                                                                 {
                                                                     Username = d.TrnCollection_CIId.MstUser_PreparedByUserId.Username,
                                                                     Fullname = d.TrnCollection_CIId.MstUser_PreparedByUserId.Fullname
                                                                 },
                                                                 CheckedByUserId = d.TrnCollection_CIId.CheckedByUserId,
                                                                 CheckedByUser = new DTO.MstUserDTO
                                                                 {
                                                                     Username = d.TrnCollection_CIId.MstUser_CheckedByUserId.Username,
                                                                     Fullname = d.TrnCollection_CIId.MstUser_CheckedByUserId.Fullname
                                                                 },
                                                                 ApprovedByUserId = d.TrnCollection_CIId.ApprovedByUserId,
                                                                 ApprovedByUser = new DTO.MstUserDTO
                                                                 {
                                                                     Username = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Username,
                                                                     Fullname = d.TrnCollection_CIId.MstUser_ApprovedByUserId.Fullname
                                                                 },
                                                                 Amount = d.TrnCollection_CIId.Amount,
                                                                 Status = d.TrnCollection_CIId.Status,
                                                                 IsCancelled = d.TrnCollection_CIId.IsCancelled,
                                                                 IsPrinted = d.TrnCollection_CIId.IsPrinted,
                                                                 IsLocked = d.TrnCollection_CIId.IsLocked,
                                                                 CreatedByUser = new DTO.MstUserDTO
                                                                 {
                                                                     Username = d.TrnCollection_CIId.MstUser_CreatedByUserId.Username,
                                                                     Fullname = d.TrnCollection_CIId.MstUser_CreatedByUserId.Fullname
                                                                 },
                                                                 CreatedDateTime = d.TrnCollection_CIId.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                                                                 UpdatedByUser = new DTO.MstUserDTO
                                                                 {
                                                                     Username = d.TrnCollection_CIId.MstUser_UpdatedByUserId.Username,
                                                                     Fullname = d.TrnCollection_CIId.MstUser_UpdatedByUserId.Fullname
                                                                 },
                                                                 UpdatedDateTime = d.TrnCollection_CIId.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                                                             },
                                                             BranchId = d.BranchId,
                                                             Branch = new DTO.MstCompanyBranchDTO
                                                             {
                                                                 ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                                                 Branch = d.MstCompanyBranch_BranchId.Branch
                                                             },
                                                             AccountId = d.AccountId,
                                                             Account = new DTO.MstAccountDTO
                                                             {
                                                                 ManualCode = d.MstAccount_AccountId.ManualCode,
                                                                 Account = d.MstAccount_AccountId.Account
                                                             },
                                                             ArticleId = d.ArticleId,
                                                             Article = new DTO.MstArticleDTO
                                                             {
                                                                 ManualCode = d.MstArticle_ArticleId.ManualCode,
                                                                 Article = d.MstArticle_ArticleId.Article
                                                             },
                                                             SIId = d.SIId,
                                                             SalesInvoice = new DTO.TrnSalesInvoiceDTO
                                                             {
                                                                 SINumber = d.TrnSalesInvoice_SIId.SINumber,
                                                                 SIDate = d.TrnSalesInvoice_SIId.SIDate.ToShortDateString(),
                                                                 ManualNumber = d.TrnSalesInvoice_SIId.ManualNumber,
                                                                 DocumentReference = d.TrnSalesInvoice_SIId.DocumentReference
                                                             },
                                                             Amount = d.Amount,
                                                             Particulars = d.Particulars,
                                                             PayTypeId = d.PayTypeId,
                                                             PayType = new DTO.MstPayTypeDTO
                                                             {
                                                                 ManualCode = d.MstPayType_PayTypeId.ManualCode,
                                                                 PayType = d.MstPayType_PayTypeId.PayType
                                                             },
                                                             CheckNumber = d.CheckNumber,
                                                             CheckDate = d.CheckDate != null ? Convert.ToDateTime(d.CheckDate).ToShortDateString() : "",
                                                             CheckBank = d.CheckBank,
                                                             BankId = d.BankId,
                                                             Bank = new DTO.MstArticleBankDTO
                                                             {
                                                                 Article = new DTO.MstArticleDTO
                                                                 {
                                                                     ManualCode = d.MstArticle_BankId.ManualCode
                                                                 },
                                                                 Bank = d.MstArticle_BankId.MstArticleBanks_ArticleId.FirstOrDefault().Bank
                                                             },
                                                             IsClear = d.IsClear,
                                                             WTAXId = d.WTAXId,
                                                             WTAX = new DTO.MstTaxDTO
                                                             {
                                                                 ManualCode = d.MstTax_WTAXId.ManualCode,
                                                                 TaxDescription = d.MstTax_WTAXId.TaxDescription
                                                             },
                                                             WTAXRate = d.WTAXRate,
                                                             WTAXAmount = d.WTAXAmount
                                                         };

                                newCollectionLinesPerPayType.AddRange(newCollectionLines.ToList());
                            }
                        }
                    }
                }

                return StatusCode(200, newCollectionLinesPerPayType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("print/byDateRange/{startDate}/{endDate}/byCompany/{companyId}/byBranch/{branchId}")]
        public async Task<ActionResult> PrintCollectionDetailReportPerPayType(String startDate, String endDate, Int32 companyId, Int32 branchId)
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
                    && d.SysForm_FormId.Form == "ReportCollectionDetailReportPerPayType"
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
                        tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"), fontSegoeUI09)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 3f });
                        tableHeader.AddCell(new PdfPCell(new Phrase("COLLECTION DETAIL REPORT (Per Pay Type)", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(tableHeader);

                        String companyBranchName = "";

                        var branch = await (
                            from d in _dbContext.MstCompanyBranches
                            where d.Id == branchId
                            select d
                        ).FirstOrDefaultAsync();

                        if (branch != null)
                        {
                            companyBranchName = branch.Branch;
                        }

                        PdfPTable tableCollection = new PdfPTable(2);
                        tableCollection.SetWidths(new float[] { 45f, 280f });
                        tableCollection.WidthPercentage = 100;
                        tableCollection.AddCell(new PdfPCell(new Phrase("Date:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableCollection.AddCell(new PdfPCell(new Phrase("From " + startDate + " To " + endDate, fontSegoeUI10)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableCollection.AddCell(new PdfPCell(new Phrase("Branch:", fontSegoeUI10Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableCollection.AddCell(new PdfPCell(new Phrase(companyBranchName, fontSegoeUI10)) { Border = 0, PaddingTop = 2f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        document.Add(tableCollection);

                        var collectionLines = await (
                           from d in _dbContext.TrnCollectionLines
                           where d.TrnCollection_CIId.CIDate >= Convert.ToDateTime(startDate)
                           && d.TrnCollection_CIId.CIDate <= Convert.ToDateTime(endDate)
                           && d.TrnCollection_CIId.MstCompanyBranch_BranchId.CompanyId == companyId
                           && d.TrnCollection_CIId.BranchId == branchId
                           && d.TrnCollection_CIId.IsLocked == true
                           && d.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.Any() == true
                           && d.MstArticle_BankId.MstArticleBanks_ArticleId.Any() == true
                           select d
                       ).ToListAsync();

                        if (collectionLines.Any())
                        {
                            PdfPTable tableCollectionLineHeaders = new PdfPTable(8);
                            tableCollectionLineHeaders.SetWidths(new float[] { 80f, 70f, 150f, 80f, 80f, 70f, 100f, 80f });
                            tableCollectionLineHeaders.WidthPercentage = 100;
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("CI No.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("CI Date", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Customer", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Manual No.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Check No.", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Check Date", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Check Bank", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineHeaders.AddCell(new PdfPCell(new Phrase("Amount", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 1, PaddingTop = 2f, PaddingBottom = 5f });
                            document.Add(tableCollectionLineHeaders);

                            var payTypes = from d in collectionLines
                                           group d by new
                                           {
                                               d.PayTypeId,
                                               d.MstPayType_PayTypeId.PayType
                                           }
                                           into g
                                           select g;

                            if (payTypes.Any() == true)
                            {
                                foreach (var payType in payTypes)
                                {
                                    PdfPTable tablePayTypes = new PdfPTable(1);
                                    tablePayTypes.SetWidths(new float[] { 100f });
                                    tablePayTypes.WidthPercentage = 100;
                                    tablePayTypes.AddCell(new PdfPCell(new Phrase(payType.Key.PayType, fontSegoeUI09Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    document.Add(tablePayTypes);

                                    var collectionLinesPerPayType = from d in collectionLines
                                                                    where d.PayTypeId == payType.Key.PayTypeId
                                                                    select d;

                                    if (collectionLinesPerPayType.Any() == true)
                                    {
                                        PdfPTable tableCollectionLines = new PdfPTable(8);
                                        tableCollectionLines.SetWidths(new float[] { 80f, 70f, 150f, 80f, 80f, 70f, 100f, 80f });
                                        tableCollectionLines.WidthPercentage = 100;

                                        foreach (var collectionLine in collectionLinesPerPayType)
                                        {
                                            String checkDate = "";

                                            if (collectionLine.CheckDate != null)
                                            {
                                                checkDate = Convert.ToDateTime(collectionLine.CheckDate).ToShortDateString();
                                            }

                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.TrnCollection_CIId.CINumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.TrnCollection_CIId.CIDate.ToShortDateString(), fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.TrnCollection_CIId.MstArticle_CustomerId.MstArticleCustomers_ArticleId.FirstOrDefault().Customer, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.TrnCollection_CIId.ManualNumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.CheckNumber, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(checkDate, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.CheckBank, fontSegoeUI09)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                            tableCollectionLines.AddCell(new PdfPCell(new Phrase(collectionLine.Amount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                        }

                                        document.Add(tableCollectionLines);
                                    }
                                }
                            }

                            PdfPTable tableCollectionLineTotals = new PdfPTable(8);
                            tableCollectionLineTotals.SetWidths(new float[] { 80f, 70f, 150f, 80f, 80f, 70f, 100f, 80f });
                            tableCollectionLineTotals.WidthPercentage = 100;
                            tableCollectionLineTotals.AddCell(new PdfPCell(new Phrase("TOTAL:", fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f, Colspan = 7 });
                            tableCollectionLineTotals.AddCell(new PdfPCell(new Phrase(collectionLines.Sum(d => d.Amount).ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER, HorizontalAlignment = 2, PaddingTop = 2f, PaddingBottom = 5f });
                            tableCollectionLineTotals.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, Colspan = 6 });
                            document.Add(tableCollectionLineTotals);
                        }
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print sales order"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print sales order"
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
