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
    public class RepCashFlowAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepCashFlowAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("print/{startDate}/{endDate}/{companyId}")]
        public async Task<ActionResult> PrintCashFlow(String startDate, String endDate, Int32 companyId)
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
                    && d.SysForm_FormId.Form == "ReportCashFlow"
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
                            Decimal totalOverallIncomes = 0;
                            Decimal totalOverallExpenses = 0;

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
                            tableHeader.AddCell(new PdfPCell(new Phrase("CASH FLOW", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            var cashFlowIncomeStatements = await (from d in _dbContext.SysJournalEntries
                                                                  where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                                                                  && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                                                                  && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                                  && (d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountCategoryId == 4
                                                                  || d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountCategoryId == 5)
                                                                  select new DTO.RepCashFlowDTO
                                                                  {
                                                                      Document = "Income Statement",
                                                                      AccountCashFlow = new DTO.MstAccountCashFlowDTO
                                                                      {
                                                                          ManualCode = d.MstAccount_AccountId.MstAccountCashFlow_AccountCashFlowId.ManualCode,
                                                                          AccountCashFlow = d.MstAccount_AccountId.MstAccountCashFlow_AccountCashFlowId.AccountCashFlow
                                                                      },
                                                                      AccountCategory = new DTO.MstAccountCategoryDTO
                                                                      {
                                                                          ManualCode = d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.ManualCode,
                                                                          AccountCategory = d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.AccountCategory
                                                                      },
                                                                      AccountType = new DTO.MstAccountTypeDTO
                                                                      {
                                                                          ManualCode = d.MstAccount_AccountId.MstAccountType_AccountTypeId.ManualCode,
                                                                          AccountType = d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountType
                                                                      },
                                                                      Account = new DTO.MstAccountDTO
                                                                      {
                                                                          ManualCode = d.MstAccount_AccountId.ManualCode,
                                                                          Account = d.MstAccount_AccountId.Account
                                                                      },
                                                                      DebitAmount = d.DebitAmount,
                                                                      CreditAmount = d.CreditAmount,
                                                                      Balance = d.CreditAmount - d.DebitAmount
                                                                  }).OrderBy(d => d.Account.ManualCode).ToListAsync();

                            var cashFlowBalanceSheets = await (from d in _dbContext.SysJournalEntries
                                                               where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                                                               && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                                                               && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                               && d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountCategoryId < 4
                                                               && d.MstAccount_AccountId.AccountCashFlowId <= 3
                                                               select new DTO.RepCashFlowDTO
                                                               {
                                                                   Document = "Balance Sheet",
                                                                   AccountCashFlow = new DTO.MstAccountCashFlowDTO
                                                                   {
                                                                       ManualCode = d.MstAccount_AccountId.MstAccountCashFlow_AccountCashFlowId.ManualCode,
                                                                       AccountCashFlow = d.MstAccount_AccountId.MstAccountCashFlow_AccountCashFlowId.AccountCashFlow
                                                                   },
                                                                   AccountCategory = new DTO.MstAccountCategoryDTO
                                                                   {
                                                                       ManualCode = d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.ManualCode,
                                                                       AccountCategory = d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.AccountCategory
                                                                   },
                                                                   AccountType = new DTO.MstAccountTypeDTO
                                                                   {
                                                                       ManualCode = d.MstAccount_AccountId.MstAccountType_AccountTypeId.ManualCode,
                                                                       AccountType = d.MstAccount_AccountId.MstAccountType_AccountTypeId.AccountType
                                                                   },
                                                                   Account = new DTO.MstAccountDTO
                                                                   {
                                                                       ManualCode = d.MstAccount_AccountId.ManualCode,
                                                                       Account = d.MstAccount_AccountId.Account
                                                                   },
                                                                   DebitAmount = d.DebitAmount,
                                                                   CreditAmount = d.CreditAmount,
                                                                   Balance = d.CreditAmount - d.DebitAmount
                                                               }).OrderBy(d => d.Account.ManualCode).ToListAsync();

                            var unionCashFlows = cashFlowIncomeStatements.Union(cashFlowBalanceSheets);

                            if (unionCashFlows.Any())
                            {
                                var cashFlows = from d in unionCashFlows
                                                group d by new
                                                {
                                                    d.AccountCashFlow.AccountCashFlow
                                                } into g
                                                select new
                                                {
                                                    g.Key.AccountCashFlow,
                                                    Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                };

                                if (cashFlows.Any())
                                {
                                    Decimal totalAllCashFlow = 0;

                                    foreach (var cashFlow in cashFlows)
                                    {
                                        PdfPTable cashFlowAccountCashFlowDescriptionTable = new PdfPTable(1);
                                        float[] cashFlowAccountCashFlowDescriptioWidths = new float[] { 100f };
                                        cashFlowAccountCashFlowDescriptionTable.SetWidths(cashFlowAccountCashFlowDescriptioWidths);
                                        cashFlowAccountCashFlowDescriptionTable.WidthPercentage = 100;
                                        cashFlowAccountCashFlowDescriptionTable.AddCell(new PdfPCell(new Phrase(cashFlow.AccountCashFlow, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                        document.Add(cashFlowAccountCashFlowDescriptionTable);

                                        var incomeAccount = await (from d in _dbContext.MstAccounts
                                                                   where d.Id == Convert.ToInt32(companyIncomeAccountId)
                                                                   select d).FirstOrDefaultAsync();

                                        var cashFlowRetainEarningsAccountTypes = from d in cashFlowIncomeStatements
                                                                                 group d by new
                                                                                 {
                                                                                     incomeAccount.MstAccountType_AccountTypeId.AccountType
                                                                                 } into g
                                                                                 select new
                                                                                 {
                                                                                     g.Key.AccountType,
                                                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                                 };

                                        var cashFlowAccountTypes = from d in unionCashFlows
                                                                   where d.Document == "Balance Sheet"
                                                                   && d.AccountCashFlow.AccountCashFlow == cashFlow.AccountCashFlow
                                                                   group d by new
                                                                   {
                                                                       d.AccountType.AccountType
                                                                   } into g
                                                                   select new
                                                                   {
                                                                       g.Key.AccountType,
                                                                       Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                   };

                                        var unionCashFlowAccountTypes = cashFlowRetainEarningsAccountTypes.Union(cashFlowAccountTypes);

                                        if (unionCashFlowAccountTypes.Any())
                                        {
                                            Decimal totalCurrentCashFlows = 0;

                                            foreach (var cashFlowAccountType in unionCashFlowAccountTypes)
                                            {
                                                totalCurrentCashFlows += cashFlowAccountType.Balance;

                                                PdfPTable cashFlowAccountTypeTable = new PdfPTable(3);
                                                float[] widthCellsIncomeAccountTypeTable = new float[] { 50f, 100f, 50f };
                                                cashFlowAccountTypeTable.SetWidths(widthCellsIncomeAccountTypeTable);
                                                cashFlowAccountTypeTable.WidthPercentage = 100;
                                                cashFlowAccountTypeTable.AddCell(new PdfPCell(new Phrase(cashFlowAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                                cashFlowAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                                cashFlowAccountTypeTable.AddCell(new PdfPCell(new Phrase(cashFlowAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                                document.Add(cashFlowAccountTypeTable);

                                                if (incomeAccount.MstAccountType_AccountTypeId.AccountType == cashFlowAccountType.AccountType)
                                                {
                                                    var cashFlowRetainEarningsAccounts = from d in cashFlowIncomeStatements
                                                                                         group d by new
                                                                                         {
                                                                                             incomeAccount.ManualCode,
                                                                                             incomeAccount.Account
                                                                                         } into g
                                                                                         select new
                                                                                         {
                                                                                             g.Key.ManualCode,
                                                                                             g.Key.Account,
                                                                                             DebitAmount = g.Sum(d => d.DebitAmount),
                                                                                             CreditAmount = g.Sum(d => d.CreditAmount),
                                                                                             Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                                         };

                                                    if (cashFlowRetainEarningsAccounts.Any())
                                                    {
                                                        foreach (var cashFlowRetainEarningsAccount in cashFlowRetainEarningsAccounts)
                                                        {
                                                            totalAllCashFlow += cashFlowRetainEarningsAccount.Balance;

                                                            PdfPTable cashFlowAccountTable = new PdfPTable(3);
                                                            float[] widthCellsIncomeAccountTable = new float[] { 50f, 100f, 50f };
                                                            cashFlowAccountTable.SetWidths(widthCellsIncomeAccountTable);
                                                            cashFlowAccountTable.WidthPercentage = 100;
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowRetainEarningsAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowRetainEarningsAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowRetainEarningsAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(cashFlowAccountTable);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var cashFlowAccounts = from d in unionCashFlows
                                                                           where d.Document == "Balance Sheet"
                                                                           && d.AccountType.AccountType == cashFlowAccountType.AccountType
                                                                           group d by new
                                                                           {
                                                                               d.Account.ManualCode,
                                                                               d.Account.Account
                                                                           } into g
                                                                           select new
                                                                           {
                                                                               g.Key.ManualCode,
                                                                               g.Key.Account,
                                                                               DebitAmount = g.Sum(d => d.DebitAmount),
                                                                               CreditAmount = g.Sum(d => d.CreditAmount),
                                                                               Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                           };

                                                    if (cashFlowAccounts.Any())
                                                    {
                                                        foreach (var cashFlowAccount in cashFlowAccounts)
                                                        {
                                                            totalAllCashFlow += cashFlowAccount.Balance;

                                                            PdfPTable cashFlowAccountTable = new PdfPTable(3);
                                                            float[] widthCellsIncomeAccountTable = new float[] { 50f, 100f, 50f };
                                                            cashFlowAccountTable.SetWidths(widthCellsIncomeAccountTable);
                                                            cashFlowAccountTable.WidthPercentage = 100;
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            cashFlowAccountTable.AddCell(new PdfPCell(new Phrase(cashFlowAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(cashFlowAccountTable);
                                                        }
                                                    }
                                                }
                                            }

                                            document.Add(line);

                                            PdfPTable totalCurrentIncomesTable = new PdfPTable(5);
                                            float[] widthCellsTotalCurrentIncomesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                            totalCurrentIncomesTable.SetWidths(widthCellsTotalCurrentIncomesTable);
                                            totalCurrentIncomesTable.WidthPercentage = 100;
                                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("Total " + cashFlow.AccountCashFlow, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase(totalCurrentCashFlows.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                            document.Add(totalCurrentIncomesTable);
                                        }
                                    }

                                    document.Add(line);

                                    PdfPTable totalAllIncomesTable = new PdfPTable(5);
                                    float[] widthCellsTotalAllIncomesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                    totalAllIncomesTable.SetWidths(widthCellsTotalAllIncomesTable);
                                    totalAllIncomesTable.WidthPercentage = 100;
                                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("Cash Balance", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase(totalAllCashFlow.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    document.Add(totalAllIncomesTable);

                                    totalOverallIncomes += totalAllCashFlow;
                                    document.Add(Chunk.Newline);
                                }
                            }

                            document.Add(Chunk.Newline);

                            Decimal NetIncome = totalOverallIncomes - totalOverallExpenses;

                            document.Add(line);
                            PdfPTable netIncomeTable = new PdfPTable(5);
                            float[] widthCellsNetIncomeTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            netIncomeTable.SetWidths(widthCellsNetIncomeTable);
                            netIncomeTable.WidthPercentage = 100;
                            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            netIncomeTable.AddCell(new PdfPCell(new Phrase("Net Income (Loss)", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            netIncomeTable.AddCell(new PdfPCell(new Phrase(NetIncome.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(netIncomeTable);
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
                            "No rights to print cash flow"
                        };

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph
                    {
                        "No rights to print cash flow"
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
