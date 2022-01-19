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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RepAccountLedgerPrintAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepAccountLedgerPrintAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("print/{startDate}/{endDate}/{companyId}/{branchId}/{accountId}")]
        public async Task<ActionResult> PrintTrialBalance(String startDate, String endDate, Int32 companyId, Int32 branchId, Int32 accountId)
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
                    && d.SysForm_FormId.Form == "ReportAccountLedger"
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
                        tableHeader.AddCell(new PdfPCell(new Phrase("ACCOUNT LEDGER", fontSegoeUI13Bold)) { Border = 0,PaddingBottom = 10f, PaddingTop = 8f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(tableHeader);


                        var journalEntries = await (
                                           from d in _dbContext.SysJournalEntries
                                           where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                                           && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                                           && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                           && d.BranchId == branchId
                                           && d.AccountId == accountId
                                           select new DTO.SysJournalEntryDTO
                                           {
                                               Id = d.Id,
                                               BranchId = d.BranchId,
                                               Branch = new DTO.MstCompanyBranchDTO
                                               {
                                                   ManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                                   Branch = d.MstCompanyBranch_BranchId.Branch
                                               },
                                               JournalEntryDate = d.JournalEntryDate.ToShortDateString(),
                                               ArticleId = d.ArticleId,
                                               Article = new DTO.MstArticleDTO
                                               {
                                                   ManualCode = d.MstArticle_ArticleId.ManualCode,
                                                   Article = d.MstArticle_ArticleId.Article
                                               },
                                               AccountId = d.AccountId,
                                               Account = new DTO.MstAccountDTO
                                               {
                                                   ManualCode = d.MstAccount_AccountId.ManualCode,
                                                   Account = d.MstAccount_AccountId.Account
                                               },
                                               DebitAmount = d.DebitAmount,
                                               CreditAmount = d.CreditAmount,
                                               Particulars = d.Particulars,
                                               RRId = d.RRId,
                                               ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                                               {
                                                   RRNumber = d.RRId != null ? d.TrnReceivingReceipt_RRId.RRNumber : "",
                                                   RRDate = d.RRId != null ? d.TrnReceivingReceipt_RRId.RRDate.ToShortDateString() : ""
                                               },
                                               SIId = d.SIId,
                                               SalesInvoice = new DTO.TrnSalesInvoiceDTO
                                               {
                                                   SINumber = d.SIId != null ? d.TrnSalesInvoice_SIId.SINumber : "",
                                                   SIDate = d.SIId != null ? d.TrnSalesInvoice_SIId.SIDate.ToShortDateString() : ""
                                               },
                                               CIId = d.CIId,
                                               Collection = new DTO.TrnCollectionDTO
                                               {
                                                   CINumber = d.CIId != null ? d.TrnCollection_CIId.CINumber : "",
                                                   CIDate = d.CIId != null ? d.TrnCollection_CIId.CIDate.ToShortDateString() : ""
                                               },
                                               CVId = d.CVId,
                                               Disbursement = new DTO.TrnDisbursementDTO
                                               {
                                                   CVNumber = d.CVId != null ? d.TrnDisbursement_CVId.CVNumber : "",
                                                   CVDate = d.CVId != null ? d.TrnDisbursement_CVId.CVDate.ToShortDateString() : ""
                                               },
                                               PMId = d.PMId,
                                               PayableMemo = new DTO.TrnPayableMemoDTO
                                               {
                                                   PMNumber = d.PMId != null ? d.TrnPayableMemo_PMId.PMNumber : "",
                                                   PMDate = d.PMId != null ? d.TrnPayableMemo_PMId.PMDate.ToShortDateString() : ""
                                               },
                                               RMId = d.RMId,
                                               ReceivableMemo = new DTO.TrnReceivableMemoDTO
                                               {
                                                   RMNumber = d.RMId != null ? d.TrnReceivableMemo_RMId.RMNumber : "",
                                                   RMDate = d.RMId != null ? d.TrnReceivableMemo_RMId.RMDate.ToShortDateString() : ""
                                               },
                                               JVId = d.JVId,
                                               JournalVoucher = new DTO.TrnJournalVoucherDTO
                                               {
                                                   JVNumber = d.RMId != null ? d.TrnJournalVoucher_JVId.JVNumber : "",
                                                   JVDate = d.RMId != null ? d.TrnJournalVoucher_JVId.JVDate.ToShortDateString() : ""
                                               },
                                               ILId = d.ILId,
                                               InventoryLedger = new DTO.TrnInventoryDTO
                                               {
                                                   ILNumber = d.RMId != null ? d.TrnInventory_ILId.ILNumber : "",
                                                   ILDate = d.RMId != null ? d.TrnInventory_ILId.ILDate.ToShortDateString() : ""
                                               }
                                           }
                                       ).ToListAsync();

                        PdfPTable trialBalanceHeaderCells = new PdfPTable(6);
                        float[] widthCellsTrialBalanceTable = new float[] { 60f, 100f, 40f, 100f, 50f, 50f };
                        trialBalanceHeaderCells.SetWidths(widthCellsTrialBalanceTable);
                        trialBalanceHeaderCells.WidthPercentage = 100;
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Branch", fontSegoeUI09Bold)) {HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Document", fontSegoeUI09Bold)) {HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Article Code", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Article", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Debit Amount", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Credit Amount", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        document.Add(trialBalanceHeaderCells);

                        PdfPTable trialBalanceDetailCells = new PdfPTable(6);
                        float[] widthCellsDetailTrialBalanceTable = new float[] { 60f, 100f, 40f, 100f, 50f, 50f };
                        trialBalanceDetailCells.SetWidths(widthCellsDetailTrialBalanceTable);
                        trialBalanceDetailCells.WidthPercentage = 100;

                        Decimal totalDebitAmount = 0;
                        Decimal totalCreditAmount = 0;
                        Decimal balanceAmount = 0;
                        var documentReference = "";

                        if (journalEntries.Any())
                        {
                            foreach (var journalEntry in journalEntries.ToList())
                            {
                                if (journalEntry.RRId != null)
                                {
                                    documentReference = "RR-" + journalEntry.Branch.ManualCode + "-" + journalEntry.ReceivingReceipt.RRNumber;
                                }
                                else if (journalEntry.SIId != null)
                                {
                                    documentReference = "SI-" + journalEntry.Branch.ManualCode + "-" + journalEntry.SalesInvoice.SINumber;
                                }
                                else if (journalEntry.CIId != null)
                                {
                                    documentReference = "CI-" + journalEntry.Branch.ManualCode + "-" + journalEntry.Collection.CINumber;
                                }
                                else if (journalEntry.CVId != null)
                                {
                                    documentReference = "CV-" + journalEntry.Branch.ManualCode + "-" + journalEntry.Disbursement.CVNumber;
                                }
                                else if (journalEntry.PMId != null)
                                {
                                    documentReference = "PM-" + journalEntry.Branch.ManualCode + "-" + journalEntry.PayableMemo.PMNumber;
                                }
                                else if (journalEntry.PMId != null)
                                {
                                    documentReference = "RM-" + journalEntry.Branch.ManualCode + "-" + journalEntry.ReceivableMemo.RMNumber;
                                }
                                else if (journalEntry.JVId != null)
                                {
                                    documentReference = "JV-" + journalEntry.Branch.ManualCode + "-" + journalEntry.JournalVoucher.JVNumber;
                                }
                                else if (journalEntry.ILId != null)
                                {
                                    documentReference = "IL-" + journalEntry.Branch.ManualCode + "-" + journalEntry.InventoryLedger.ILNumber;
                                }

                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(journalEntry.Branch.Branch, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(documentReference, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(journalEntry.Article.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(journalEntry.Article.Article, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(journalEntry.DebitAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(journalEntry.CreditAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                                totalDebitAmount += journalEntry.DebitAmount;
                                totalCreditAmount += journalEntry.CreditAmount;
                            }
                            document.Add(trialBalanceDetailCells);

                            balanceAmount = totalDebitAmount - totalCreditAmount;
                        }

                        PdfPTable trialBalanceFooterCells = new PdfPTable(4);
                        float[] widthCellsFooterTrialBalanceTable = new float[] { 100f, 100f, 50f, 50f};
                        trialBalanceFooterCells.SetWidths(widthCellsFooterTrialBalanceTable);
                        trialBalanceFooterCells.WidthPercentage = 100;

                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase(" ", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingBottom = 30f, Colspan = 4 });

                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase("Debit Amount :", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Colspan = 3});
                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f});

                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase("Credit Amount :", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Colspan = 3 });
                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase("Balance Amount :", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Colspan = 3 });
                        trialBalanceFooterCells.AddCell(new PdfPCell(new Phrase(balanceAmount.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(trialBalanceFooterCells);
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            "No rights to print account ledger"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print account ledger"
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
