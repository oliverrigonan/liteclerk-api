using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleAccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleAccountGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArticleAccountGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AssetAccountId = table.Column<int>(type: "int", nullable: false),
                    SalesAccountId = table.Column<int>(type: "int", nullable: false),
                    CostAccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenseAccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleAccountGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ReceivableAccountId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleCustomer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    IsInventory = table.Column<bool>(type: "bit", nullable: false),
                    ArticleAccountGroupId = table.Column<int>(type: "int", nullable: false),
                    AssetAccountId = table.Column<int>(nullable: false),
                    SalesAccountId = table.Column<int>(type: "int", nullable: false),
                    CostAccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenseAccountId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    RRVATId = table.Column<int>(type: "int", nullable: false),
                    SIVATId = table.Column<int>(type: "int", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    Kitting = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupIdId",
                        column: x => x.ArticleAccountGroupId,
                        principalTable: "MstArticleAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountCashFlowId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleItemInventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    InventoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemInventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleItemPrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    PriceDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemPrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleItemUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrnJobOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    JONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateScheduled = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    SIItemId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemJobTypeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrnJobOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    AttachmentCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentURL = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SINumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SIDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoldByUserId = table.Column<int>(type: "int", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemInventoryId = table.Column<int>(type: "int", nullable: true),
                    ItemJobTypeId = table.Column<int>(type: "int", nullable: true),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VATId = table.Column<int>(type: "int", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseNetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LineTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstArticleItemInventory_ItemInventoryIdId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstArticle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Article = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ArticleTypeId = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticle_MstArticleType_ArticleTypeIdId",
                        column: x => x.ArticleTypeId,
                        principalTable: "MstArticleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstCompanyBranch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstCompanyBranch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUser_MstCompanyBranch_BranchIdId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountCashFlow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCashFlowCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountCashFlow = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountCashFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountCashFlow_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCashFlow_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountCategory = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountCategory_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCategory_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstCurrency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(255)", maxLength: 50, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstCurrency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstCurrency_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCurrency_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Discount = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstDiscount_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstDiscount_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstJobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDepartmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobDepartment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobDepartment_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobDepartment_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstJobType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalNumberOfDays = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobType_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobType_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstTax",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstTax_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstTax_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstTerm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Term = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstTerm_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstTerm_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUnit_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUnit_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnJobOrderInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    InformationCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InformationGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    InformationByUserId = table.Column<int>(type: "int", nullable: false),
                    InformationUpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderInformation_MstUser_InformationByUserIdId",
                        column: x => x.InformationByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstCompany",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CostMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstCompany_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCompany_MstCurrency_CurrencyIdId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCompany_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnJobOrderDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusByUserId = table.Column<int>(type: "int", nullable: false),
                    StatusUpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstJobDepartment_JobDepartmentIdId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstUser_StatusByUserIdId",
                        column: x => x.StatusByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstJobTypeAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    AttachmentCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeAttachment_MstJobType_JobTypeIdId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstJobTypeDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeDepartment_MstJobDepartment_JobDepartmentIdId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobTypeDepartment_MstJobType_JobTypeIdId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstJobTypeInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    InformationCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InformationGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeInformation_MstJobType_JobTypeIdId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountIdCashFlowId",
                table: "MstAccount",
                column: "AccountCashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountIdTypeId",
                table: "MstAccount",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_CreatedByUserId",
                table: "MstAccount",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_UpdatedByUserId",
                table: "MstAccount",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCashFlow_CreatedByUserId",
                table: "MstAccountCashFlow",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCashFlow_UpdatedByUserId",
                table: "MstAccountCashFlow",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCategory_CreatedByUserId",
                table: "MstAccountCategory",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCategory_UpdatedByUserId",
                table: "MstAccountCategory",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_AccountCategoryId",
                table: "MstAccountType",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_CreatedByUserId",
                table: "MstAccountType",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_UpdatedByUserId",
                table: "MstAccountType",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticle_ArticleIdTypeId",
                table: "MstArticle",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticle_CreatedByUserId",
                table: "MstArticle",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticle_UpdatedByUserId",
                table: "MstArticle",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_AssetAccountId",
                table: "MstArticleAccountGroup",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_CostAccountId",
                table: "MstArticleAccountGroup",
                column: "CostAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_CreatedByUserId",
                table: "MstArticleAccountGroup",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_ExpenseAccountId",
                table: "MstArticleAccountGroup",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_SalesAccountId",
                table: "MstArticleAccountGroup",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_UpdatedByUserId",
                table: "MstArticleAccountGroup",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleCustomer_ArticleId",
                table: "MstArticleCustomer",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleCustomer_ReceivableAccountId",
                table: "MstArticleCustomer",
                column: "ReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleCustomer_TermId",
                table: "MstArticleCustomer",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ArticleAccountGroupId",
                table: "MstArticleItem",
                column: "ArticleAccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ArticleId",
                table: "MstArticleItem",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_AssetAccountId",
                table: "MstArticleItem",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_CostAccountId",
                table: "MstArticleItem",
                column: "CostAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ExpenseAccountId",
                table: "MstArticleItem",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_RRVATId",
                table: "MstArticleItem",
                column: "RRVATId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_SIVATId",
                table: "MstArticleItem",
                column: "SIVATId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_SalesAccountId",
                table: "MstArticleItem",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_UnitId",
                table: "MstArticleItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_WTAXId",
                table: "MstArticleItem",
                column: "WTAXId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemInventory_ArticleId",
                table: "MstArticleItemInventory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemInventory_BranchId",
                table: "MstArticleItemInventory",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemPrice_ArticleId",
                table: "MstArticleItemPrice",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemUnit_ArticleId",
                table: "MstArticleItemUnit",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemUnit_UnitId",
                table: "MstArticleItemUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_CreatedByUserId",
                table: "MstCompany",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_CurrencyId",
                table: "MstCompany",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_UpdatedByUserId",
                table: "MstCompany",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompanyBranch_CompanyId",
                table: "MstCompanyBranch",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCurrency_CreatedByUserId",
                table: "MstCurrency",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCurrency_UpdatedByUserId",
                table: "MstCurrency",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstDiscount_CreatedByUserId",
                table: "MstDiscount",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstDiscount_UpdatedByUserId",
                table: "MstDiscount",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobDepartment_CreatedByUserId",
                table: "MstJobDepartment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobDepartment_UpdatedByUserId",
                table: "MstJobDepartment",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobType_CreatedByUserId",
                table: "MstJobType",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobType_UpdatedByUserId",
                table: "MstJobType",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeAttachment_JobTypeId",
                table: "MstJobTypeAttachment",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeDepartment_JobDepartmentId",
                table: "MstJobTypeDepartment",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeDepartment_JobTypeId",
                table: "MstJobTypeDepartment",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeInformation_JobTypeId",
                table: "MstJobTypeInformation",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstTax_CreatedByUserId",
                table: "MstTax",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstTax_UpdatedByUserId",
                table: "MstTax",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstTerm_CreatedByUserId",
                table: "MstTerm",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstTerm_UpdatedByUserId",
                table: "MstTerm",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUnit_CreatedByUserId",
                table: "MstUnit",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUnit_UpdatedByUserId",
                table: "MstUnit",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_BranchId",
                table: "MstUser",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_CompanyId",
                table: "MstUser",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ApprovedByUserId",
                table: "TrnJobOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_BranchId",
                table: "TrnJobOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CheckedByUserId",
                table: "TrnJobOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CreatedByUserId",
                table: "TrnJobOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CurrencyId",
                table: "TrnJobOrder",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ItemId",
                table: "TrnJobOrder",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ItemJobTypeId",
                table: "TrnJobOrder",
                column: "ItemJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_PreparedByUserId",
                table: "TrnJobOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_SIId",
                table: "TrnJobOrder",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_SIItemId",
                table: "TrnJobOrder",
                column: "SIItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_UpdatedByUserId",
                table: "TrnJobOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderAttachment_JOId",
                table: "TrnJobOrderAttachment",
                column: "JOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_JOId",
                table: "TrnJobOrderDepartment",
                column: "JOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_JobDepartmentId",
                table: "TrnJobOrderDepartment",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_StatusByUserId",
                table: "TrnJobOrderDepartment",
                column: "StatusByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderInformation_InformationByUserId",
                table: "TrnJobOrderInformation",
                column: "InformationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderInformation_JOId",
                table: "TrnJobOrderInformation",
                column: "JOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_ApprovedByUserId",
                table: "TrnSalesInvoice",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_BranchId",
                table: "TrnSalesInvoice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CheckedByUserId",
                table: "TrnSalesInvoice",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CreatedByUserId",
                table: "TrnSalesInvoice",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CurrencyId",
                table: "TrnSalesInvoice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CustomerId",
                table: "TrnSalesInvoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_PreparedByUserId",
                table: "TrnSalesInvoice",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_SoldByUserId",
                table: "TrnSalesInvoice",
                column: "SoldByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_TermId",
                table: "TrnSalesInvoice",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_UpdatedByUserId",
                table: "TrnSalesInvoice",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_BaseUnitId",
                table: "TrnSalesInvoiceItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_DiscountId",
                table: "TrnSalesInvoiceItem",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemId",
                table: "TrnSalesInvoiceItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemInventoryId",
                table: "TrnSalesInvoiceItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemJobTypeId",
                table: "TrnSalesInvoiceItem",
                column: "ItemJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_SIId",
                table: "TrnSalesInvoiceItem",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_UnitId",
                table: "TrnSalesInvoiceItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_VATId",
                table: "TrnSalesInvoiceItem",
                column: "VATId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_WTAXId",
                table: "TrnSalesInvoiceItem",
                column: "WTAXId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstUser_CreatedByUserIdId",
                table: "MstArticleAccountGroup",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstUser_UpdatedByUserIdId",
                table: "MstArticleAccountGroup",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_AssetAccountIdId",
                table: "MstArticleAccountGroup",
                column: "AssetAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_CostAccountIdId",
                table: "MstArticleAccountGroup",
                column: "CostAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_ExpenseAccountIdId",
                table: "MstArticleAccountGroup",
                column: "ExpenseAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_SalesAccountIdId",
                table: "MstArticleAccountGroup",
                column: "SalesAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstAccount_ReceivableAccountIdId",
                table: "MstArticleCustomer",
                column: "ReceivableAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstArticle_ArticleIdId",
                table: "MstArticleCustomer",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstTerm_TermIdId",
                table: "MstArticleCustomer",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_AssetAccountIdId",
                table: "MstArticleItem",
                column: "AssetAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_CostAccountIdId",
                table: "MstArticleItem",
                column: "CostAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_ExpenseAccountIdId",
                table: "MstArticleItem",
                column: "ExpenseAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_SalesAccountIdId",
                table: "MstArticleItem",
                column: "SalesAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleIdId",
                table: "MstArticleItem",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_RRVATIdId",
                table: "MstArticleItem",
                column: "RRVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_SIVATIdId",
                table: "MstArticleItem",
                column: "SIVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_WTAXIdId",
                table: "MstArticleItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstUnit_UnitIdId",
                table: "MstArticleItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstAccountCashFlow_AccountCashFlowIdId",
                table: "MstAccount",
                column: "AccountCashFlowId",
                principalTable: "MstAccountCashFlow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstAccountType_AccountTypeIdId",
                table: "MstAccount",
                column: "AccountTypeId",
                principalTable: "MstAccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstUser_CreatedByUserIdId",
                table: "MstAccount",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstUser_UpdatedByUserIdId",
                table: "MstAccount",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstUser_CreatedByUserIdId",
                table: "MstAccountType",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstUser_UpdatedByUserIdId",
                table: "MstAccountType",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstAccountCategory_AccountCategoryIdId",
                table: "MstAccountType",
                column: "AccountCategoryId",
                principalTable: "MstAccountCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleIdId",
                table: "MstArticleItemInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstCompanyBranch_BranchIdId",
                table: "MstArticleItemInventory",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleIdId",
                table: "MstArticleItemPrice",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleIdId",
                table: "MstArticleItemUnit",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstUnit_UnitIdId",
                table: "MstArticleItemUnit",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_ApprovedByUserIdId",
                table: "TrnJobOrder",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_CheckedByUserIdId",
                table: "TrnJobOrder",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_CreatedByUserIdId",
                table: "TrnJobOrder",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_PreparedByUserIdId",
                table: "TrnJobOrder",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_UpdatedByUserIdId",
                table: "TrnJobOrder",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstArticle_ItemIdId",
                table: "TrnJobOrder",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstCompanyBranch_BranchIdId",
                table: "TrnJobOrder",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstCurrency_CurrencyIdId",
                table: "TrnJobOrder",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstJobType_ItemJobTypeIdId",
                table: "TrnJobOrder",
                column: "ItemJobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_TrnSalesInvoice_SIId",
                table: "TrnJobOrder",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_TrnSalesInvoiceItem_SIItemId",
                table: "TrnJobOrder",
                column: "SIItemId",
                principalTable: "TrnSalesInvoiceItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_ApprovedByUserIdId",
                table: "TrnSalesInvoice",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_CheckedByUserIdId",
                table: "TrnSalesInvoice",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_CreatedByUserIdId",
                table: "TrnSalesInvoice",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_PreparedByUserIdId",
                table: "TrnSalesInvoice",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_SoldByUserIdId",
                table: "TrnSalesInvoice",
                column: "SoldByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_UpdatedByUserIdId",
                table: "TrnSalesInvoice",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstArticle_CustomerIdId",
                table: "TrnSalesInvoice",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstTerm_TermIdId",
                table: "TrnSalesInvoice",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstCompanyBranch_BranchIdId",
                table: "TrnSalesInvoice",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstCurrency_CurrencyIdId",
                table: "TrnSalesInvoice",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstArticle_ItemIdId",
                table: "TrnSalesInvoiceItem",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstTax_VATIdId",
                table: "TrnSalesInvoiceItem",
                column: "VATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstTax_WTAXIdId",
                table: "TrnSalesInvoiceItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstUnit_BaseUnitIdId",
                table: "TrnSalesInvoiceItem",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstUnit_UnitIdId",
                table: "TrnSalesInvoiceItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstJobType_ItemJobTypeIdId",
                table: "TrnSalesInvoiceItem",
                column: "ItemJobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstDiscount_DiscountIdId",
                table: "TrnSalesInvoiceItem",
                column: "DiscountId",
                principalTable: "MstDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticle_MstUser_CreatedByUserIdId",
                table: "MstArticle",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticle_MstUser_UpdatedByUserIdId",
                table: "MstArticle",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompanyBranch_MstCompany_CompanyIdId",
                table: "MstCompanyBranch",
                column: "CompanyId",
                principalTable: "MstCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompany_CompanyIdId",
                table: "MstUser",
                column: "CompanyId",
                principalTable: "MstCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_CreatedByUserIdId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_UpdatedByUserIdId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCurrency_MstUser_CreatedByUserIdId",
                table: "MstCurrency");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCurrency_MstUser_UpdatedByUserIdId",
                table: "MstCurrency");

            migrationBuilder.DropTable(
                name: "MstArticleCustomer");

            migrationBuilder.DropTable(
                name: "MstArticleItem");

            migrationBuilder.DropTable(
                name: "MstArticleItemPrice");

            migrationBuilder.DropTable(
                name: "MstArticleItemUnit");

            migrationBuilder.DropTable(
                name: "MstJobTypeAttachment");

            migrationBuilder.DropTable(
                name: "MstJobTypeDepartment");

            migrationBuilder.DropTable(
                name: "MstJobTypeInformation");

            migrationBuilder.DropTable(
                name: "TrnJobOrderAttachment");

            migrationBuilder.DropTable(
                name: "TrnJobOrderDepartment");

            migrationBuilder.DropTable(
                name: "TrnJobOrderInformation");

            migrationBuilder.DropTable(
                name: "MstArticleAccountGroup");

            migrationBuilder.DropTable(
                name: "MstJobDepartment");

            migrationBuilder.DropTable(
                name: "TrnJobOrder");

            migrationBuilder.DropTable(
                name: "MstAccount");

            migrationBuilder.DropTable(
                name: "TrnSalesInvoiceItem");

            migrationBuilder.DropTable(
                name: "MstAccountCashFlow");

            migrationBuilder.DropTable(
                name: "MstAccountType");

            migrationBuilder.DropTable(
                name: "MstUnit");

            migrationBuilder.DropTable(
                name: "MstDiscount");

            migrationBuilder.DropTable(
                name: "MstArticleItemInventory");

            migrationBuilder.DropTable(
                name: "MstJobType");

            migrationBuilder.DropTable(
                name: "TrnSalesInvoice");

            migrationBuilder.DropTable(
                name: "MstTax");

            migrationBuilder.DropTable(
                name: "MstAccountCategory");

            migrationBuilder.DropTable(
                name: "MstArticle");

            migrationBuilder.DropTable(
                name: "MstTerm");

            migrationBuilder.DropTable(
                name: "MstArticleType");

            migrationBuilder.DropTable(
                name: "MstUser");

            migrationBuilder.DropTable(
                name: "MstCompanyBranch");

            migrationBuilder.DropTable(
                name: "MstCompany");

            migrationBuilder.DropTable(
                name: "MstCurrency");
        }
    }
}
