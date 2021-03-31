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
    public class RepIncomeStatementAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepIncomeStatementAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("print/{startDate}/{endDate}/{companyId}")]
        public async Task<ActionResult> PrintIncomeStatement(String startDate, String endDate, Int32 companyId)
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
                    && d.SysForm_FormId.Form == "ReportIncomeStatement"
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
                        tableHeader.AddCell(new PdfPCell(new Phrase("INCOME STATEMENT", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(tableHeader);

                        var incomes = await (from d in _dbContext.SysJournalEntries
                                             where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                                             && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                                             && d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 4
                                             && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                             select new DTO.RepIncomeStatementDTO
                                             {
                                                 Document = "4 - incomes",
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

                        if (incomes.Any())
                        {
                            var incomeAccountCategories = from d in incomes
                                                          group d by new
                                                          {
                                                              d.AccountCategory.AccountCategory
                                                          } into g
                                                          select new
                                                          {
                                                              g.Key.AccountCategory,
                                                              Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                          };

                            if (incomeAccountCategories.Any())
                            {
                                Decimal totalAllIncomes = 0;

                                foreach (var incomeAccountCategory in incomeAccountCategories)
                                {
                                    PdfPTable incomeSubCategoryDescriptionTable = new PdfPTable(1);
                                    float[] widthCellsIncomeSubCategoryDescriptionTable = new float[] { 100f };
                                    incomeSubCategoryDescriptionTable.SetWidths(widthCellsIncomeSubCategoryDescriptionTable);
                                    incomeSubCategoryDescriptionTable.WidthPercentage = 100;
                                    incomeSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(incomeAccountCategory.AccountCategory, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                    document.Add(incomeSubCategoryDescriptionTable);

                                    var incomeAccountTypes = from d in incomes
                                                             where d.AccountCategory.AccountCategory == incomeAccountCategory.AccountCategory
                                                             group d by new
                                                             {
                                                                 d.AccountType.AccountType
                                                             } into g
                                                             select new
                                                             {
                                                                 g.Key.AccountType,
                                                                 Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                             };

                                    if (incomeAccountTypes.Any())
                                    {
                                        Decimal totalCurrentIncomes = 0;
                                        foreach (var incomeAccountType in incomeAccountTypes)
                                        {
                                            totalCurrentIncomes += incomeAccountType.Balance;

                                            PdfPTable incomeAccountTypeTable = new PdfPTable(3);
                                            float[] widthCellsIncomeAccountTypeTable = new float[] { 50f, 100f, 50f };
                                            incomeAccountTypeTable.SetWidths(widthCellsIncomeAccountTypeTable);
                                            incomeAccountTypeTable.WidthPercentage = 100;
                                            incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase(incomeAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase(incomeAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(incomeAccountTypeTable);

                                            var incomeAccounts = from d in incomes
                                                                 where d.AccountType.AccountType == incomeAccountType.AccountType
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

                                            if (incomeAccounts.Any())
                                            {
                                                foreach (var incomeAccount in incomeAccounts)
                                                {
                                                    totalAllIncomes += incomeAccount.Balance;

                                                    PdfPTable incomeAccountTable = new PdfPTable(3);
                                                    float[] widthCellsIncomeAccountTable = new float[] { 50f, 100f, 50f };
                                                    incomeAccountTable.SetWidths(widthCellsIncomeAccountTable);
                                                    incomeAccountTable.WidthPercentage = 100;
                                                    incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                    incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                    incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                    document.Add(incomeAccountTable);
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
                                        totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("Sub Total " + incomeAccountCategory.AccountCategory, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase(totalCurrentIncomes.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
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
                                totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("Total Income", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                totalAllIncomesTable.AddCell(new PdfPCell(new Phrase(totalAllIncomes.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                document.Add(totalAllIncomesTable);

                                totalOverallIncomes += totalAllIncomes;
                                document.Add(Chunk.Newline);
                            }
                        }

                        var expenses = await (from d in _dbContext.SysJournalEntries
                                              where d.JournalEntryDate >= Convert.ToDateTime(startDate)
                                              && d.JournalEntryDate <= Convert.ToDateTime(endDate)
                                              && d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 5
                                              && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                              select new DTO.RepIncomeStatementDTO
                                              {
                                                  Document = "4 - incomes",
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

                        if (expenses.Any())
                        {
                            var expenseAccountCategories = from d in expenses
                                                           group d by new
                                                           {
                                                               d.AccountCategory.AccountCategory
                                                           } into g
                                                           select new
                                                           {
                                                               g.Key.AccountCategory,
                                                               Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                           };

                            if (expenseAccountCategories.Any())
                            {
                                Decimal totalAllExpenses = 0;

                                foreach (var expenseAccountCategory in expenseAccountCategories)
                                {
                                    PdfPTable expenseSubCategoryDescriptionTable = new PdfPTable(1);
                                    float[] widthCellsExpenseSubCategoryDescriptionTable = new float[] { 100f };
                                    expenseSubCategoryDescriptionTable.SetWidths(widthCellsExpenseSubCategoryDescriptionTable);
                                    expenseSubCategoryDescriptionTable.WidthPercentage = 100;
                                    expenseSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(expenseAccountCategory.AccountCategory, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                    document.Add(expenseSubCategoryDescriptionTable);

                                    var expenseAccountTypes = from d in expenses
                                                              where d.AccountCategory.AccountCategory == expenseAccountCategory.AccountCategory
                                                              group d by new
                                                              {
                                                                  d.AccountType.AccountType
                                                              } into g
                                                              select new
                                                              {
                                                                  g.Key.AccountType,
                                                                  Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                              };

                                    if (expenseAccountTypes.Any())
                                    {
                                        Decimal totalCurrentExpenses = 0;
                                        foreach (var expenseAccountType in expenseAccountTypes)
                                        {
                                            totalCurrentExpenses += expenseAccountType.Balance;

                                            PdfPTable expenseAccountTypeTable = new PdfPTable(3);
                                            float[] widthCellsExpenseAccountTypeTable = new float[] { 50f, 100f, 50f };
                                            expenseAccountTypeTable.SetWidths(widthCellsExpenseAccountTypeTable);
                                            expenseAccountTypeTable.WidthPercentage = 100;
                                            expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase(expenseAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase(expenseAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(expenseAccountTypeTable);

                                            var expenseAccounts = from d in expenses
                                                                  where d.AccountType.AccountType == expenseAccountType.AccountType
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
                                                                      Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                                  };

                                            if (expenseAccounts.Any())
                                            {
                                                foreach (var expenseAccount in expenseAccounts)
                                                {
                                                    totalAllExpenses += expenseAccount.Balance;

                                                    PdfPTable expenseAccountTable = new PdfPTable(3);
                                                    float[] widthCellsExpenseAccountTable = new float[] { 50f, 100f, 50f };
                                                    expenseAccountTable.SetWidths(widthCellsExpenseAccountTable);
                                                    expenseAccountTable.WidthPercentage = 100;
                                                    expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                    expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                    expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                    document.Add(expenseAccountTable);
                                                }
                                            }
                                        }

                                        document.Add(line);

                                        PdfPTable totalCurrentExpensesTable = new PdfPTable(5);
                                        float[] widthCellsTotalCurrentExpensesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                        totalCurrentExpensesTable.SetWidths(widthCellsTotalCurrentExpensesTable);
                                        totalCurrentExpensesTable.WidthPercentage = 100;
                                        totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                        totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                        totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                        totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("Sub Total " + expenseAccountCategory.AccountCategory, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase(totalCurrentExpenses.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(totalCurrentExpensesTable);
                                    }
                                }

                                document.Add(line);

                                PdfPTable totalAllExpensesTable = new PdfPTable(5);
                                float[] widthCellsTotalAllExpensesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                totalAllExpensesTable.SetWidths(widthCellsTotalAllExpensesTable);
                                totalAllExpensesTable.WidthPercentage = 100;
                                totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                                totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("Total Expense", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                totalAllExpensesTable.AddCell(new PdfPCell(new Phrase(totalAllExpenses.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                document.Add(totalAllExpensesTable);

                                totalOverallExpenses += totalAllExpenses;
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
