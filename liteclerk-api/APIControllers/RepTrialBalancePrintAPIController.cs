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
    public class RepTrialBalancePrintAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepTrialBalancePrintAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("print/{startDate}/{endDate}/{companyId}")]
        public async Task<ActionResult> PrintTrialBalance(String startDate, String endDate, Int32 companyId)
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

            List<DTO.SysJournalEntryDTO> newJournals = new List<DTO.SysJournalEntryDTO>();

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
                    && d.SysForm_FormId.Form == "ReportTrialBalance"
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
                        tableHeader.AddCell(new PdfPCell(new Phrase("TRIAL BALANCE", fontSegoeUI13Bold)) { Border = 0,PaddingBottom = 10f, PaddingTop = 8f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(tableHeader);


                        var journals = await (
                            from d in _dbContext.SysJournalEntries
                            where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                            && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                            && d.MstCompanyBranch_BranchId.CompanyId == companyId
                            select d
                        ).ToListAsync();

                        if (journals.Any() == true)
                        {
                            var groupedJournals = from d in journals
                                                  group d by new
                                                  {
                                                      Company = d.MstCompanyBranch_BranchId.MstCompany_CompanyId.Company,
                                                      BranchId = d.BranchId,
                                                      BranchManualCode = d.MstCompanyBranch_BranchId.ManualCode,
                                                      Branch = d.MstCompanyBranch_BranchId.Branch,
                                                      AccountId = d.AccountId,
                                                      AccountManualCode = d.MstAccount_AccountId.ManualCode,
                                                      Account = d.MstAccount_AccountId.Account
                                                  } into g
                                                  select new
                                                  {
                                                      Company = g.Key.Company,
                                                      BranchId = g.Key.BranchId,
                                                      BranchManualCode = g.Key.BranchManualCode,
                                                      Branch = g.Key.Branch,
                                                      AccountId = g.Key.AccountId,
                                                      AccountManualCode = g.Key.AccountManualCode,
                                                      Account = g.Key.Account,
                                                      TotalDebitAmount = g.Sum(s => s.DebitAmount),
                                                      TotalCreditAmount = g.Sum(s => s.CreditAmount)
                                                  };

                            foreach (var groupedJournal in groupedJournals.ToList())
                            {
                                newJournals.Add(new DTO.SysJournalEntryDTO
                                {
                                    Id = 0,
                                    BranchId = groupedJournal.BranchId,
                                    Branch = new DTO.MstCompanyBranchDTO
                                    {
                                        ManualCode = groupedJournal.BranchManualCode,
                                        Branch = groupedJournal.Branch,
                                        Company = new DTO.MstCompanyDTO
                                        {
                                            ManualCode = "",
                                            Company = groupedJournal.Company
                                        }
                                    },
                                    JournalEntryDate = "",
                                    ArticleId = 0,
                                    Article = new DTO.MstArticleDTO
                                    {
                                        ManualCode = "",
                                        Article = ""
                                    },
                                    AccountId = groupedJournal.AccountId,
                                    Account = new DTO.MstAccountDTO
                                    {
                                        ManualCode = groupedJournal.AccountManualCode,
                                        Account = groupedJournal.Account
                                    },
                                    DebitAmount = groupedJournal.TotalDebitAmount,
                                    CreditAmount = groupedJournal.TotalCreditAmount,
                                    Particulars = "",
                                    RRId = null,
                                    ReceivingReceipt = new DTO.TrnReceivingReceiptDTO
                                    {

                                    },
                                    SIId = null,
                                    SalesInvoice = new DTO.TrnSalesInvoiceDTO
                                    {

                                    },
                                    CIId = null,
                                    Collection = new DTO.TrnCollectionDTO
                                    {

                                    },
                                    CVId = null,
                                    Disbursement = new DTO.TrnDisbursementDTO
                                    {

                                    },
                                    PMId = null,
                                    PayableMemo = new DTO.TrnPayableMemoDTO
                                    {

                                    },
                                    RMId = null,
                                    ReceivableMemo = new DTO.TrnReceivableMemoDTO
                                    {

                                    },
                                    JVId = null,
                                    JournalVoucher = new DTO.TrnJournalVoucherDTO
                                    {

                                    },
                                    ILId = null,
                                    InventoryLedger = new DTO.TrnInventoryDTO
                                    {

                                    }
                                });
                            }
                        }

                        PdfPTable trialBalanceHeaderCells = new PdfPTable(5);
                        float[] widthCellsTrialBalanceTable = new float[] { 100f, 50f, 100f, 60f, 60f };
                        trialBalanceHeaderCells.SetWidths(widthCellsTrialBalanceTable);
                        trialBalanceHeaderCells.WidthPercentage = 100;
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Branch", fontSegoeUI09Bold)) {HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Account Code", fontSegoeUI09Bold)) {HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Account", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Debit Amount", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        trialBalanceHeaderCells.AddCell(new PdfPCell(new Phrase("Credit Amount", fontSegoeUI09Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        document.Add(trialBalanceHeaderCells);

                        PdfPTable trialBalanceDetailCells = new PdfPTable(5);
                        float[] widthCellsDetailTrialBalanceTable = new float[] { 100f, 50f, 100f, 60f, 60f };
                        trialBalanceDetailCells.SetWidths(widthCellsDetailTrialBalanceTable);
                        trialBalanceDetailCells.WidthPercentage = 100;

                        Decimal totalDebitAmount = 0;
                        Decimal totalCreditAmount = 0;
                        Decimal balanceAmount = 0;

                        if (newJournals.Any())
                        {
                            foreach (var newJournal in newJournals.ToList())
                            {
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(newJournal.Branch.Branch, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(newJournal.Account.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(newJournal.Account.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(newJournal.DebitAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                trialBalanceDetailCells.AddCell(new PdfPCell(new Phrase(newJournal.CreditAmount.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                                totalDebitAmount += newJournal.DebitAmount;
                                totalCreditAmount += newJournal.CreditAmount;
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
                            "No rights to print trial balance"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print trial balance"
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
