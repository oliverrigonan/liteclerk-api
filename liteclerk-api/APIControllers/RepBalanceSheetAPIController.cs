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
    public class RepBalanceSheetAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public RepBalanceSheetAPIController(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("print/{dateAsOf}/{companyId}")]
        public async Task<ActionResult> PrintBalanceSheet(String dateAsOf, Int32 companyId)
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
                        Int32? companyIncomeAccountId = null;

                        if (loginUser.CompanyId != null)
                        {
                            companyName = loginUser.MstCompany_CompanyId.Company;
                            companyAddress = loginUser.MstCompany_CompanyId.Address;
                            companyTaxNumber = loginUser.MstCompany_CompanyId.TIN;
                            companyIncomeAccountId = loginUser.MstCompany_CompanyId.IncomeAccountId;
                        }

                        if (companyIncomeAccountId != null)
                        {

                            Decimal totalOverallAssets = 0;
                            Decimal totalOverallLiabilities = 0;
                            Decimal totalOverallEquities = 0;

                            String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\prime_global_logo.png";

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
                            tableHeader.AddCell(new PdfPCell(new Phrase("BALANCE SHEET", fontSegoeUI10Bold)) { Border = PdfCell.BOTTOM_BORDER, PaddingBottom = 5f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                            document.Add(tableHeader);

                            var assets = await (from d in _dbContext.SysJournalEntries
                                                where d.JournalEntryDate <= Convert.ToDateTime(dateAsOf)
                                                && d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 1
                                                && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                select new DTO.RepBalanceSheetDTO
                                                {
                                                    Document = "1 - assets",
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
                                                    Balance = d.DebitAmount - d.CreditAmount
                                                }).ToListAsync();

                            if (assets.Any())
                            {
                                var assetAccountCategories = from d in assets
                                                             group d by new
                                                             {
                                                                 d.AccountCategory.AccountCategory
                                                             } into g
                                                             select new
                                                             {
                                                                 g.Key.AccountCategory,
                                                                 Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                             };

                                if (assetAccountCategories.Any())
                                {
                                    Decimal totalAllAssets = 0;

                                    foreach (var assetAccountCategory in assetAccountCategories)
                                    {
                                        PdfPTable assetSubCategoryDescriptionTable = new PdfPTable(1);
                                        float[] widthCellsAssetSubCategoryDescriptionTable = new float[] { 100f };
                                        assetSubCategoryDescriptionTable.SetWidths(widthCellsAssetSubCategoryDescriptionTable);
                                        assetSubCategoryDescriptionTable.WidthPercentage = 100;
                                        assetSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(assetAccountCategory.AccountCategory, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                        document.Add(assetSubCategoryDescriptionTable);

                                        var assetAccountTypes = from d in assets
                                                                where d.AccountCategory.AccountCategory == assetAccountCategory.AccountCategory
                                                                group d by new
                                                                {
                                                                    d.AccountType.AccountType
                                                                } into g
                                                                select new
                                                                {
                                                                    g.Key.AccountType,
                                                                    Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                };

                                        if (assetAccountTypes.Any())
                                        {
                                            Decimal totalCurrentAssets = 0;
                                            foreach (var assetAccountType in assetAccountTypes)
                                            {
                                                totalCurrentAssets += assetAccountType.Balance;

                                                PdfPTable assetAccountTypeTable = new PdfPTable(3);
                                                float[] widthCellsAssetAccountTypeTable = new float[] { 50f, 100f, 50f };
                                                assetAccountTypeTable.SetWidths(widthCellsAssetAccountTypeTable);
                                                assetAccountTypeTable.WidthPercentage = 100;
                                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase(assetAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase(assetAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                                document.Add(assetAccountTypeTable);

                                                var assetAccounts = from d in assets
                                                                    where d.AccountType.AccountType == assetAccountType.AccountType
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

                                                if (assetAccounts.Any())
                                                {
                                                    foreach (var assetAccount in assetAccounts)
                                                    {
                                                        totalAllAssets += assetAccount.Balance;

                                                        PdfPTable assetAccountTable = new PdfPTable(3);
                                                        float[] widthCellsAssetAccountTable = new float[] { 50f, 100f, 50f };
                                                        assetAccountTable.SetWidths(widthCellsAssetAccountTable);
                                                        assetAccountTable.WidthPercentage = 100;
                                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                        document.Add(assetAccountTable);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    document.Add(line);

                                    PdfPTable totalAllAssetTable = new PdfPTable(5);
                                    float[] widthCellsTotalAllAssetTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                    totalAllAssetTable.SetWidths(widthCellsTotalAllAssetTable);
                                    totalAllAssetTable.WidthPercentage = 100;
                                    totalAllAssetTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllAssetTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllAssetTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllAssetTable.AddCell(new PdfPCell(new Phrase("Total Asset", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllAssetTable.AddCell(new PdfPCell(new Phrase(totalAllAssets.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    document.Add(totalAllAssetTable);

                                    totalOverallAssets += totalAllAssets;
                                    document.Add(Chunk.Newline);
                                }
                            }

                            var liabilities = await (from d in _dbContext.SysJournalEntries
                                                     where d.JournalEntryDate <= Convert.ToDateTime(dateAsOf)
                                                     && d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 2
                                                     && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                     select new DTO.RepBalanceSheetDTO
                                                     {
                                                         Document = "1 - liabilities",
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
                                                     }).ToListAsync();

                            if (liabilities.Any())
                            {
                                var liabilityAccountCategories = from d in liabilities
                                                                 group d by new
                                                                 {
                                                                     d.AccountCategory.AccountCategory
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     g.Key.AccountCategory,
                                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                 };

                                if (liabilityAccountCategories.Any())
                                {
                                    Decimal totalAllLiabilities = 0;

                                    foreach (var liabilityAccountCategory in liabilityAccountCategories)
                                    {
                                        PdfPTable liabilitySubCategoryDescriptionTable = new PdfPTable(1);
                                        float[] widthCellsLiabilitySubCategoryDescriptionTable = new float[] { 100f };
                                        liabilitySubCategoryDescriptionTable.SetWidths(widthCellsLiabilitySubCategoryDescriptionTable);
                                        liabilitySubCategoryDescriptionTable.WidthPercentage = 100;
                                        liabilitySubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(liabilityAccountCategory.AccountCategory, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                        document.Add(liabilitySubCategoryDescriptionTable);

                                        var liabilityAccountTypes = from d in liabilities
                                                                    where d.AccountCategory.AccountCategory == liabilityAccountCategory.AccountCategory
                                                                    group d by new
                                                                    {
                                                                        d.AccountType.AccountType
                                                                    } into g
                                                                    select new
                                                                    {
                                                                        g.Key.AccountType,
                                                                        Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                    };

                                        if (liabilityAccountTypes.Any())
                                        {
                                            Decimal totalCurrentLiabilities = 0;
                                            foreach (var liabilityAccountType in liabilityAccountTypes)
                                            {
                                                totalCurrentLiabilities += liabilityAccountType.Balance;

                                                PdfPTable liabilityAccountTypeTable = new PdfPTable(3);
                                                float[] widthCellsLiabilityAccountTypeTable = new float[] { 50f, 100f, 50f };
                                                liabilityAccountTypeTable.SetWidths(widthCellsLiabilityAccountTypeTable);
                                                liabilityAccountTypeTable.WidthPercentage = 100;
                                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase(liabilityAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase(liabilityAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                                document.Add(liabilityAccountTypeTable);

                                                var liabilityAccounts = from d in liabilities
                                                                        where d.AccountType.AccountType == liabilityAccountType.AccountType
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

                                                if (liabilityAccounts.Any())
                                                {
                                                    foreach (var liabilityAccount in liabilityAccounts)
                                                    {
                                                        totalAllLiabilities += liabilityAccount.Balance;

                                                        PdfPTable liabilityAccountTable = new PdfPTable(3);
                                                        float[] widthCellsLiabilityAccountTable = new float[] { 50f, 100f, 50f };
                                                        liabilityAccountTable.SetWidths(widthCellsLiabilityAccountTable);
                                                        liabilityAccountTable.WidthPercentage = 100;
                                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                        document.Add(liabilityAccountTable);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    document.Add(line);

                                    PdfPTable totalAllLiabilityTable = new PdfPTable(5);
                                    float[] widthCellsTotalAllLiabilityTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                    totalAllLiabilityTable.SetWidths(widthCellsTotalAllLiabilityTable);
                                    totalAllLiabilityTable.WidthPercentage = 100;
                                    totalAllLiabilityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllLiabilityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllLiabilityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllLiabilityTable.AddCell(new PdfPCell(new Phrase("Total Liability", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllLiabilityTable.AddCell(new PdfPCell(new Phrase(totalAllLiabilities.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    document.Add(totalAllLiabilityTable);

                                    totalOverallLiabilities += totalAllLiabilities;
                                    document.Add(Chunk.Newline);
                                }
                            }

                            var profitAndLoss = await (from d in _dbContext.SysJournalEntries
                                                       where d.JournalEntryDate <= Convert.ToDateTime(dateAsOf)
                                                       && (d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 4 || d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 5)
                                                       && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                       select new DTO.RepBalanceSheetDTO
                                                       {
                                                           Document = "4&5 - profitAndLoss",
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
                                                       }).ToListAsync();

                            var incomeAccount = await (from d in _dbContext.MstAccounts
                                                       where d.Id == Convert.ToInt32(companyIncomeAccountId)
                                                       select d).FirstOrDefaultAsync();

                            var retainedEarnings = from d in profitAndLoss
                                                   group d by d.Document into g
                                                   select new DTO.RepBalanceSheetDTO
                                                   {
                                                       Document = "1 - equities",
                                                       AccountCategory = new DTO.MstAccountCategoryDTO
                                                       {
                                                           ManualCode = incomeAccount.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.ManualCode,
                                                           AccountCategory = incomeAccount.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.AccountCategory
                                                       },
                                                       AccountType = new DTO.MstAccountTypeDTO
                                                       {
                                                           ManualCode = incomeAccount.MstAccountType_AccountTypeId.ManualCode,
                                                           AccountType = incomeAccount.MstAccountType_AccountTypeId.AccountType
                                                       },
                                                       Account = new DTO.MstAccountDTO
                                                       {
                                                           ManualCode = incomeAccount.ManualCode,
                                                           Account = incomeAccount.Account
                                                       },
                                                       DebitAmount = g.Sum(d => d.DebitAmount),
                                                       CreditAmount = g.Sum(d => d.CreditAmount),
                                                       Balance = g.Sum(d => d.Balance)
                                                   };

                            var equities = await (from d in _dbContext.SysJournalEntries
                                                  where d.JournalEntryDate <= Convert.ToDateTime(dateAsOf)
                                                  && d.MstAccount_AccountId.MstAccountType_AccountTypeId.MstAccountCategory_AccountCategoryId.Id == 3
                                                  && d.MstCompanyBranch_BranchId.CompanyId == companyId
                                                  select new DTO.RepBalanceSheetDTO
                                                  {
                                                      Document = "3 - equities",
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
                                                  }).ToListAsync();

                            var unionEquitiesWithRetainEarnings = equities.Union(retainedEarnings);

                            if (unionEquitiesWithRetainEarnings.Any())
                            {
                                var unionEquitiesWithRetainEarningAccountCategories = from d in unionEquitiesWithRetainEarnings
                                                                                      group d by new
                                                                                      {
                                                                                          d.AccountCategory.AccountCategory
                                                                                      } into g
                                                                                      select new
                                                                                      {
                                                                                          g.Key.AccountCategory,
                                                                                          Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                                      };

                                if (unionEquitiesWithRetainEarningAccountCategories.Any())
                                {
                                    Decimal totalAllEquities = 0;

                                    foreach (var unionEquitiesWithRetainEarningAccountCategory in unionEquitiesWithRetainEarningAccountCategories)
                                    {
                                        PdfPTable equitySubCategoryDescriptionTable = new PdfPTable(1);
                                        float[] widthCellsEquitySubCategoryDescriptionTable = new float[] { 100f };
                                        equitySubCategoryDescriptionTable.SetWidths(widthCellsEquitySubCategoryDescriptionTable);
                                        equitySubCategoryDescriptionTable.WidthPercentage = 100;
                                        equitySubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccountCategory.AccountCategory, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LightGray });
                                        document.Add(equitySubCategoryDescriptionTable);

                                        var unionEquitiesWithRetainEarningAccountTypes = from d in unionEquitiesWithRetainEarnings
                                                                                         where d.AccountCategory.AccountCategory == unionEquitiesWithRetainEarningAccountCategory.AccountCategory
                                                                                         group d by new
                                                                                         {
                                                                                             d.AccountType.AccountType
                                                                                         } into g
                                                                                         select new
                                                                                         {
                                                                                             g.Key.AccountType,
                                                                                             Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                                         };

                                        if (unionEquitiesWithRetainEarningAccountTypes.Any())
                                        {
                                            Decimal totalCurrentEquities = 0;
                                            foreach (var unionEquitiesWithRetainEarningAccountType in unionEquitiesWithRetainEarningAccountTypes)
                                            {
                                                totalCurrentEquities += unionEquitiesWithRetainEarningAccountType.Balance;

                                                PdfPTable equityAccountTypeTable = new PdfPTable(3);
                                                float[] widthCellsEquityAccountTypeTable = new float[] { 50f, 100f, 50f };
                                                equityAccountTypeTable.SetWidths(widthCellsEquityAccountTypeTable);
                                                equityAccountTypeTable.WidthPercentage = 100;
                                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccountType.AccountType, fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccountType.Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                                document.Add(equityAccountTypeTable);

                                                var unionEquitiesWithRetainEarningAccounts = from d in unionEquitiesWithRetainEarnings
                                                                                             where d.AccountType.AccountType == unionEquitiesWithRetainEarningAccountType.AccountType
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

                                                if (unionEquitiesWithRetainEarningAccounts.Any())
                                                {
                                                    foreach (var unionEquitiesWithRetainEarningAccount in unionEquitiesWithRetainEarningAccounts)
                                                    {
                                                        totalAllEquities += unionEquitiesWithRetainEarningAccount.Balance;

                                                        PdfPTable equityAccountTable = new PdfPTable(3);
                                                        float[] widthCellsEquityAccountTable = new float[] { 50f, 100f, 50f };
                                                        equityAccountTable.SetWidths(widthCellsEquityAccountTable);
                                                        equityAccountTable.WidthPercentage = 100;
                                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccount.ManualCode, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccount.Account, fontSegoeUI09)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(unionEquitiesWithRetainEarningAccount.Balance.ToString("#,##0.00"), fontSegoeUI09)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                        document.Add(equityAccountTable);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    document.Add(line);

                                    PdfPTable totalAllEquityTable = new PdfPTable(5);
                                    float[] widthCellsTotalAllEquityTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                                    totalAllEquityTable.SetWidths(widthCellsTotalAllEquityTable);
                                    totalAllEquityTable.WidthPercentage = 100;
                                    totalAllEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllEquityTable.AddCell(new PdfPCell(new Phrase("Total Equity", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    totalAllEquityTable.AddCell(new PdfPCell(new Phrase(totalAllEquities.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    document.Add(totalAllEquityTable);

                                    totalOverallEquities += totalAllEquities;
                                    document.Add(Chunk.Newline);
                                }
                            }
                            
                            Decimal totalLiabilityAndEquity = totalOverallLiabilities + totalOverallEquities;

                            document.Add(line);
                            PdfPTable totalAllLiabilityAndEquityTable = new PdfPTable(5);
                            float[] widthCellsTotalAlliabilityAndEquityTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalAllLiabilityAndEquityTable.SetWidths(widthCellsTotalAlliabilityAndEquityTable);
                            totalAllLiabilityAndEquityTable.WidthPercentage = 100;
                            totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("Total Liability and Equity", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalAllLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase(totalLiabilityAndEquity.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalAllLiabilityAndEquityTable);

                            //document.Add(Chunk.Newline);

                            Decimal Balance = totalOverallAssets - totalLiabilityAndEquity;

                            document.Add(line);
                            PdfPTable balanceTable = new PdfPTable(5);
                            float[] widthCellsBalanceTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            balanceTable.SetWidths(widthCellsBalanceTable);
                            balanceTable.WidthPercentage = 100;
                            balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            balanceTable.AddCell(new PdfPCell(new Phrase("", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            balanceTable.AddCell(new PdfPCell(new Phrase("Balance", fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            balanceTable.AddCell(new PdfPCell(new Phrase(Balance.ToString("#,##0.00"), fontSegoeUI09Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(balanceTable);
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
