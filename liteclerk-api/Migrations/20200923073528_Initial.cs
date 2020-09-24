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
                name: "MstCodeTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CodeValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstCodeTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountArticleType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountArticleType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountArticleType_MstArticleType_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "MstArticleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "MstArticleBank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Bank = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeOfAccount = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CashInBankAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleBank", x => x.Id);
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
                    Category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
                        name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                        column: x => x.ArticleAccountGroupId,
                        principalTable: "MstArticleAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleSupplier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PayableAccountId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleSupplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstPayType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstPayType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysInventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IVNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IVDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ArticleItemInventoryId = table.Column<int>(type: "int", nullable: false),
                    QuantityIn = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    QuantityOut = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    RRId = table.Column<int>(type: "int", nullable: true),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    INId = table.Column<int>(type: "int", nullable: true),
                    OTId = table.Column<int>(type: "int", nullable: true),
                    STId = table.Column<int>(type: "int", nullable: true),
                    SWId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysInventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrnCollectionLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PayTypeId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckBank = table.Column<string>(type: "nvarchar(255)", maxLength: 50, nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    IsClear = table.Column<bool>(type: "bit", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    WTAXRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnCollectionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstPayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalTable: "MstPayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockIn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    INNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockIn", x => x.Id);
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
                name: "MstArticleItemComponent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ComponentArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemComponent", x => x.Id);
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
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
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
                name: "TrnCollection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CINumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CIDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnCollection", x => x.Id);
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
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrnPointOfSale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TerminalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    POSDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    POSNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashierUserCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CashierUserId = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnPointOfSale", x => x.Id);
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
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    VATRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VATAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    WTAXRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseNetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LineTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoldByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnSalesOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesOrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SOId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemInventoryId = table.Column<int>(type: "int", nullable: true),
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
                    VATRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VATAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    WTAXRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseNetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LineTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_TrnSalesOrder_SOId",
                        column: x => x.SOId,
                        principalTable: "TrnSalesOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockInItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseCost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnStockInItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockInItem_TrnStockIn_INId",
                        column: x => x.INId,
                        principalTable: "TrnStockIn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_MstArticle_MstArticleType_ArticleTypeId",
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUser_MstCompanyBranch_BranchId",
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
                        name: "FK_MstAccountCashFlow_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCashFlow_MstUser_UpdatedByUserId",
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
                        name: "FK_MstAccountCategory_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCategory_MstUser_UpdatedByUserId",
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
                        name: "FK_MstCurrency_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCurrency_MstUser_UpdatedByUserId",
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
                        name: "FK_MstDiscount_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstDiscount_MstUser_UpdatedByUserId",
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
                        name: "FK_MstJobDepartment_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobDepartment_MstUser_UpdatedByUserId",
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
                        name: "FK_MstJobType_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobType_MstUser_UpdatedByUserId",
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
                        name: "FK_MstTax_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstTax_MstUser_UpdatedByUserId",
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
                        name: "FK_MstTerm_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstTerm_MstUser_UpdatedByUserId",
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
                        name: "FK_MstUnit_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUnit_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstUserBranch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUserBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUserBranch_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUserBranch_MstUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstUserForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    CanAdd = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanLock = table.Column<bool>(type: "bit", nullable: false),
                    CanUnlock = table.Column<bool>(type: "bit", nullable: false),
                    CanCancel = table.Column<bool>(type: "bit", nullable: false),
                    CanPrint = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUserForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUserForm_SysForm_FormId",
                        column: x => x.FormId,
                        principalTable: "SysForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUserForm_MstUser_UserId",
                        column: x => x.UserId,
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
                        name: "FK_TrnJobOrderInformation_MstUser_InformationByUserId",
                        column: x => x.InformationByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        name: "FK_MstCompany_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCompany_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCompany_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnReceivingReceipt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    RRNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RRDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceivedByUserId = table.Column<int>(type: "int", nullable: false),
                    MstUser_ReceivedByUserIdId = table.Column<int>(nullable: true),
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
                    table.PrimaryKey("PK_TrnReceivingReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_MstUser_ReceivedByUserIdId",
                        column: x => x.MstUser_ReceivedByUserIdId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    OTNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OTDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnStockOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    STNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnStockTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockWithdrawal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SWNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SWDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnStockWithdrawal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstUserJobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUserJobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUserJobDepartment_MstJobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUserJobDepartment_MstUser_UserId",
                        column: x => x.UserId,
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
                    StatusUpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstUser_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstJobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstUser_StatusByUserId",
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
                        name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
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
                        name: "FK_MstJobTypeDepartment_MstJobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
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
                        name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysProduction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    PNNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PNDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JODepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysProduction_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                        column: x => x.JODepartmentId,
                        principalTable: "TrnJobOrderDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysProduction_MstUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountCashFlowId",
                table: "MstAccount",
                column: "AccountCashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountTypeId",
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
                name: "IX_MstAccountArticleType_AccountId",
                table: "MstAccountArticleType",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountArticleType_ArticleTypeId",
                table: "MstAccountArticleType",
                column: "ArticleTypeId");

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
                name: "IX_MstArticle_ArticleTypeId",
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
                name: "IX_MstArticleBank_ArticleId",
                table: "MstArticleBank",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleBank_CashInBankAccountId",
                table: "MstArticleBank",
                column: "CashInBankAccountId");

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
                name: "IX_MstArticleItemComponent_ArticleId",
                table: "MstArticleItemComponent",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemComponent_ComponentArticleId",
                table: "MstArticleItemComponent",
                column: "ComponentArticleId");

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
                name: "IX_MstArticleSupplier_ArticleId",
                table: "MstArticleSupplier",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleSupplier_PayableAccountId",
                table: "MstArticleSupplier",
                column: "PayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleSupplier_TermId",
                table: "MstArticleSupplier",
                column: "TermId");

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
                name: "IX_MstPayType_AccountId",
                table: "MstPayType",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstPayType_CreatedByUserId",
                table: "MstPayType",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstPayType_UpdatedByUserId",
                table: "MstPayType",
                column: "UpdatedByUserId");

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
                name: "IX_MstUserBranch_BranchId",
                table: "MstUserBranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranch_UserId",
                table: "MstUserBranch",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserForm_FormId",
                table: "MstUserForm",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserForm_UserId",
                table: "MstUserForm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserJobDepartment_JobDepartmentId",
                table: "MstUserJobDepartment",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserJobDepartment_UserId",
                table: "MstUserJobDepartment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_AccountId",
                table: "SysInventory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_ArticleId",
                table: "SysInventory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_ArticleItemInventoryId",
                table: "SysInventory",
                column: "ArticleItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_BranchId",
                table: "SysInventory",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_INId",
                table: "SysInventory",
                column: "INId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_OTId",
                table: "SysInventory",
                column: "OTId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_RRId",
                table: "SysInventory",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_SIId",
                table: "SysInventory",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_STId",
                table: "SysInventory",
                column: "STId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_SWId",
                table: "SysInventory",
                column: "SWId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_BranchId",
                table: "SysProduction",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_JODepartmentId",
                table: "SysProduction",
                column: "JODepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_UserId",
                table: "SysProduction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_ApprovedByUserId",
                table: "TrnCollection",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_BranchId",
                table: "TrnCollection",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CheckedByUserId",
                table: "TrnCollection",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CreatedByUserId",
                table: "TrnCollection",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CurrencyId",
                table: "TrnCollection",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CustomerId",
                table: "TrnCollection",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_PreparedByUserId",
                table: "TrnCollection",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_UpdatedByUserId",
                table: "TrnCollection",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_AccountId",
                table: "TrnCollectionLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_ArticleId",
                table: "TrnCollectionLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_BankId",
                table: "TrnCollectionLine",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_BranchId",
                table: "TrnCollectionLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_CIId",
                table: "TrnCollectionLine",
                column: "CIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_PayTypeId",
                table: "TrnCollectionLine",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_SIId",
                table: "TrnCollectionLine",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_WTAXId",
                table: "TrnCollectionLine",
                column: "WTAXId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ApprovedByUserId",
                table: "TrnJobOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_BaseUnitId",
                table: "TrnJobOrder",
                column: "BaseUnitId");

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
                name: "IX_TrnJobOrder_UnitId",
                table: "TrnJobOrder",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_UpdatedByUserId",
                table: "TrnJobOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderAttachment_JOId",
                table: "TrnJobOrderAttachment",
                column: "JOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_AssignedToUserId",
                table: "TrnJobOrderDepartment",
                column: "AssignedToUserId");

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
                name: "IX_TrnPointOfSale_BranchId",
                table: "TrnPointOfSale",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_CashierUserId",
                table: "TrnPointOfSale",
                column: "CashierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_CustomerId",
                table: "TrnPointOfSale",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_ItemId",
                table: "TrnPointOfSale",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_TaxId",
                table: "TrnPointOfSale",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_ApprovedByUserId",
                table: "TrnReceivingReceipt",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_BranchId",
                table: "TrnReceivingReceipt",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CheckedByUserId",
                table: "TrnReceivingReceipt",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CreatedByUserId",
                table: "TrnReceivingReceipt",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CurrencyId",
                table: "TrnReceivingReceipt",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt",
                column: "MstUser_ReceivedByUserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_PreparedByUserId",
                table: "TrnReceivingReceipt",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_UpdatedByUserId",
                table: "TrnReceivingReceipt",
                column: "UpdatedByUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_ApprovedByUserId",
                table: "TrnSalesOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_BranchId",
                table: "TrnSalesOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CheckedByUserId",
                table: "TrnSalesOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CreatedByUserId",
                table: "TrnSalesOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CurrencyId",
                table: "TrnSalesOrder",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CustomerId",
                table: "TrnSalesOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_PreparedByUserId",
                table: "TrnSalesOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_SoldByUserId",
                table: "TrnSalesOrder",
                column: "SoldByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_TermId",
                table: "TrnSalesOrder",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_UpdatedByUserId",
                table: "TrnSalesOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_BaseUnitId",
                table: "TrnSalesOrderItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_DiscountId",
                table: "TrnSalesOrderItem",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemId",
                table: "TrnSalesOrderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemInventoryId",
                table: "TrnSalesOrderItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_SOId",
                table: "TrnSalesOrderItem",
                column: "SOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_UnitId",
                table: "TrnSalesOrderItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_VATId",
                table: "TrnSalesOrderItem",
                column: "VATId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_WTAXId",
                table: "TrnSalesOrderItem",
                column: "WTAXId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_AccountId",
                table: "TrnStockIn",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_ApprovedByUserId",
                table: "TrnStockIn",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_ArticleId",
                table: "TrnStockIn",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_BranchId",
                table: "TrnStockIn",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_CheckedByUserId",
                table: "TrnStockIn",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_CreatedByUserId",
                table: "TrnStockIn",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_CurrencyId",
                table: "TrnStockIn",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_PreparedByUserId",
                table: "TrnStockIn",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_UpdatedByUserId",
                table: "TrnStockIn",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_BaseUnitId",
                table: "TrnStockInItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_INId",
                table: "TrnStockInItem",
                column: "INId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_ItemId",
                table: "TrnStockInItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_UnitId",
                table: "TrnStockInItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_ApprovedByUserId",
                table: "TrnStockOut",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_BranchId",
                table: "TrnStockOut",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CheckedByUserId",
                table: "TrnStockOut",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CreatedByUserId",
                table: "TrnStockOut",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CurrencyId",
                table: "TrnStockOut",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_PreparedByUserId",
                table: "TrnStockOut",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_UpdatedByUserId",
                table: "TrnStockOut",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_ApprovedByUserId",
                table: "TrnStockTransfer",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_BranchId",
                table: "TrnStockTransfer",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CheckedByUserId",
                table: "TrnStockTransfer",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CreatedByUserId",
                table: "TrnStockTransfer",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CurrencyId",
                table: "TrnStockTransfer",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_PreparedByUserId",
                table: "TrnStockTransfer",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_UpdatedByUserId",
                table: "TrnStockTransfer",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_ApprovedByUserId",
                table: "TrnStockWithdrawal",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_BranchId",
                table: "TrnStockWithdrawal",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CheckedByUserId",
                table: "TrnStockWithdrawal",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CreatedByUserId",
                table: "TrnStockWithdrawal",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CurrencyId",
                table: "TrnStockWithdrawal",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_PreparedByUserId",
                table: "TrnStockWithdrawal",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_UpdatedByUserId",
                table: "TrnStockWithdrawal",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountArticleType_MstAccount_AccountId",
                table: "MstAccountArticleType",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstUser_CreatedByUserId",
                table: "MstArticleAccountGroup",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstUser_UpdatedByUserId",
                table: "MstArticleAccountGroup",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_AssetAccountId",
                table: "MstArticleAccountGroup",
                column: "AssetAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_CostAccountId",
                table: "MstArticleAccountGroup",
                column: "CostAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_ExpenseAccountId",
                table: "MstArticleAccountGroup",
                column: "ExpenseAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstAccount_SalesAccountId",
                table: "MstArticleAccountGroup",
                column: "SalesAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleBank_MstAccount_CashInBankAccountId",
                table: "MstArticleBank",
                column: "CashInBankAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleBank_MstArticle_ArticleId",
                table: "MstArticleBank",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstAccount_ReceivableAccountId",
                table: "MstArticleCustomer",
                column: "ReceivableAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstArticle_ArticleId",
                table: "MstArticleCustomer",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleCustomer_MstTerm_TermId",
                table: "MstArticleCustomer",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_AssetAccountId",
                table: "MstArticleItem",
                column: "AssetAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_CostAccountId",
                table: "MstArticleItem",
                column: "CostAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_ExpenseAccountId",
                table: "MstArticleItem",
                column: "ExpenseAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstAccount_SalesAccountId",
                table: "MstArticleItem",
                column: "SalesAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleId",
                table: "MstArticleItem",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_RRVATId",
                table: "MstArticleItem",
                column: "RRVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_SIVATId",
                table: "MstArticleItem",
                column: "SIVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_WTAXId",
                table: "MstArticleItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstUnit_UnitId",
                table: "MstArticleItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleSupplier_MstAccount_PayableAccountId",
                table: "MstArticleSupplier",
                column: "PayableAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleSupplier_MstArticle_ArticleId",
                table: "MstArticleSupplier",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleSupplier_MstTerm_TermId",
                table: "MstArticleSupplier",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstPayType_MstUser_CreatedByUserId",
                table: "MstPayType",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstPayType_MstUser_UpdatedByUserId",
                table: "MstPayType",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstPayType_MstAccount_AccountId",
                table: "MstPayType",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstAccount_AccountId",
                table: "SysInventory",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstCompanyBranch_BranchId",
                table: "SysInventory",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                table: "SysInventory",
                column: "ArticleItemInventoryId",
                principalTable: "MstArticleItemInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockIn_INId",
                table: "SysInventory",
                column: "INId",
                principalTable: "TrnStockIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockOut_OTId",
                table: "SysInventory",
                column: "OTId",
                principalTable: "TrnStockOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnReceivingReceipt_RRId",
                table: "SysInventory",
                column: "RRId",
                principalTable: "TrnReceivingReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnSalesInvoice_SIId",
                table: "SysInventory",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockTransfer_STId",
                table: "SysInventory",
                column: "STId",
                principalTable: "TrnStockTransfer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockWithdrawal_SWId",
                table: "SysInventory",
                column: "SWId",
                principalTable: "TrnStockWithdrawal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_MstAccount_AccountId",
                table: "TrnCollectionLine",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_MstArticle_ArticleId",
                table: "TrnCollectionLine",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_MstArticle_BankId",
                table: "TrnCollectionLine",
                column: "BankId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_MstTax_WTAXId",
                table: "TrnCollectionLine",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_MstCompanyBranch_BranchId",
                table: "TrnCollectionLine",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_TrnSalesInvoice_SIId",
                table: "TrnCollectionLine",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_TrnCollection_CIId",
                table: "TrnCollectionLine",
                column: "CIId",
                principalTable: "TrnCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_ApprovedByUserId",
                table: "TrnStockIn",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_CheckedByUserId",
                table: "TrnStockIn",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_CreatedByUserId",
                table: "TrnStockIn",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_PreparedByUserId",
                table: "TrnStockIn",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_UpdatedByUserId",
                table: "TrnStockIn",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstAccount_AccountId",
                table: "TrnStockIn",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstArticle_ArticleId",
                table: "TrnStockIn",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstCompanyBranch_BranchId",
                table: "TrnStockIn",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstCurrency_CurrencyId",
                table: "TrnStockIn",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstAccountCashFlow_AccountCashFlowId",
                table: "MstAccount",
                column: "AccountCashFlowId",
                principalTable: "MstAccountCashFlow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstAccountType_AccountTypeId",
                table: "MstAccount",
                column: "AccountTypeId",
                principalTable: "MstAccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstUser_CreatedByUserId",
                table: "MstAccount",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccount_MstUser_UpdatedByUserId",
                table: "MstAccount",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstUser_CreatedByUserId",
                table: "MstAccountType",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstUser_UpdatedByUserId",
                table: "MstAccountType",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstAccountType_MstAccountCategory_AccountCategoryId",
                table: "MstAccountType",
                column: "AccountCategoryId",
                principalTable: "MstAccountCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_ArticleId",
                table: "MstArticleItemComponent",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_ComponentArticleId",
                table: "MstArticleItemComponent",
                column: "ComponentArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                table: "MstArticleItemInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstCompanyBranch_BranchId",
                table: "MstArticleItemInventory",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleId",
                table: "MstArticleItemPrice",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                table: "MstArticleItemUnit",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstUnit_UnitId",
                table: "MstArticleItemUnit",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstUser_ApprovedByUserId",
                table: "TrnCollection",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstUser_CheckedByUserId",
                table: "TrnCollection",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstUser_CreatedByUserId",
                table: "TrnCollection",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstUser_PreparedByUserId",
                table: "TrnCollection",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstUser_UpdatedByUserId",
                table: "TrnCollection",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstArticle_CustomerId",
                table: "TrnCollection",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstCompanyBranch_BranchId",
                table: "TrnCollection",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstCurrency_CurrencyId",
                table: "TrnCollection",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_ApprovedByUserId",
                table: "TrnJobOrder",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_CheckedByUserId",
                table: "TrnJobOrder",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_CreatedByUserId",
                table: "TrnJobOrder",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_PreparedByUserId",
                table: "TrnJobOrder",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUser_UpdatedByUserId",
                table: "TrnJobOrder",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstArticle_ItemId",
                table: "TrnJobOrder",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUnit_BaseUnitId",
                table: "TrnJobOrder",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUnit_UnitId",
                table: "TrnJobOrder",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstCompanyBranch_BranchId",
                table: "TrnJobOrder",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstCurrency_CurrencyId",
                table: "TrnJobOrder",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstJobType_ItemJobTypeId",
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
                name: "FK_TrnPointOfSale_MstUser_CashierUserId",
                table: "TrnPointOfSale",
                column: "CashierUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPointOfSale_MstArticle_CustomerId",
                table: "TrnPointOfSale",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPointOfSale_MstArticle_ItemId",
                table: "TrnPointOfSale",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPointOfSale_MstTax_TaxId",
                table: "TrnPointOfSale",
                column: "TaxId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPointOfSale_MstCompanyBranch_BranchId",
                table: "TrnPointOfSale",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_ApprovedByUserId",
                table: "TrnSalesInvoice",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_CheckedByUserId",
                table: "TrnSalesInvoice",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_CreatedByUserId",
                table: "TrnSalesInvoice",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_PreparedByUserId",
                table: "TrnSalesInvoice",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_SoldByUserId",
                table: "TrnSalesInvoice",
                column: "SoldByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstUser_UpdatedByUserId",
                table: "TrnSalesInvoice",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstArticle_CustomerId",
                table: "TrnSalesInvoice",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstTerm_TermId",
                table: "TrnSalesInvoice",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstCompanyBranch_BranchId",
                table: "TrnSalesInvoice",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstCurrency_CurrencyId",
                table: "TrnSalesInvoice",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstArticle_ItemId",
                table: "TrnSalesInvoiceItem",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstTax_VATId",
                table: "TrnSalesInvoiceItem",
                column: "VATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstTax_WTAXId",
                table: "TrnSalesInvoiceItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstUnit_BaseUnitId",
                table: "TrnSalesInvoiceItem",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstUnit_UnitId",
                table: "TrnSalesInvoiceItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstJobType_ItemJobTypeId",
                table: "TrnSalesInvoiceItem",
                column: "ItemJobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_MstDiscount_DiscountId",
                table: "TrnSalesInvoiceItem",
                column: "DiscountId",
                principalTable: "MstDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_ApprovedByUserId",
                table: "TrnSalesOrder",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_CheckedByUserId",
                table: "TrnSalesOrder",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_CreatedByUserId",
                table: "TrnSalesOrder",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_PreparedByUserId",
                table: "TrnSalesOrder",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_SoldByUserId",
                table: "TrnSalesOrder",
                column: "SoldByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstUser_UpdatedByUserId",
                table: "TrnSalesOrder",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstArticle_CustomerId",
                table: "TrnSalesOrder",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstTerm_TermId",
                table: "TrnSalesOrder",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstCompanyBranch_BranchId",
                table: "TrnSalesOrder",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstCurrency_CurrencyId",
                table: "TrnSalesOrder",
                column: "CurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstArticle_ItemId",
                table: "TrnSalesOrderItem",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstTax_VATId",
                table: "TrnSalesOrderItem",
                column: "VATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstTax_WTAXId",
                table: "TrnSalesOrderItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstUnit_BaseUnitId",
                table: "TrnSalesOrderItem",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstUnit_UnitId",
                table: "TrnSalesOrderItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstDiscount_DiscountId",
                table: "TrnSalesOrderItem",
                column: "DiscountId",
                principalTable: "MstDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_MstArticle_ItemId",
                table: "TrnStockInItem",
                column: "ItemId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_MstUnit_BaseUnitId",
                table: "TrnStockInItem",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_MstUnit_UnitId",
                table: "TrnStockInItem",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticle_MstUser_CreatedByUserId",
                table: "MstArticle",
                column: "CreatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticle_MstUser_UpdatedByUserId",
                table: "MstArticle",
                column: "UpdatedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompanyBranch_MstCompany_CompanyId",
                table: "MstCompanyBranch",
                column: "CompanyId",
                principalTable: "MstCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompany_CompanyId",
                table: "MstUser",
                column: "CompanyId",
                principalTable: "MstCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_CreatedByUserId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_UpdatedByUserId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCurrency_MstUser_CreatedByUserId",
                table: "MstCurrency");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCurrency_MstUser_UpdatedByUserId",
                table: "MstCurrency");

            migrationBuilder.DropTable(
                name: "MstAccountArticleType");

            migrationBuilder.DropTable(
                name: "MstArticleBank");

            migrationBuilder.DropTable(
                name: "MstArticleCustomer");

            migrationBuilder.DropTable(
                name: "MstArticleItem");

            migrationBuilder.DropTable(
                name: "MstArticleItemComponent");

            migrationBuilder.DropTable(
                name: "MstArticleItemPrice");

            migrationBuilder.DropTable(
                name: "MstArticleItemUnit");

            migrationBuilder.DropTable(
                name: "MstArticleSupplier");

            migrationBuilder.DropTable(
                name: "MstCodeTable");

            migrationBuilder.DropTable(
                name: "MstJobTypeAttachment");

            migrationBuilder.DropTable(
                name: "MstJobTypeDepartment");

            migrationBuilder.DropTable(
                name: "MstJobTypeInformation");

            migrationBuilder.DropTable(
                name: "MstUserBranch");

            migrationBuilder.DropTable(
                name: "MstUserForm");

            migrationBuilder.DropTable(
                name: "MstUserJobDepartment");

            migrationBuilder.DropTable(
                name: "SysInventory");

            migrationBuilder.DropTable(
                name: "SysProduction");

            migrationBuilder.DropTable(
                name: "TrnCollectionLine");

            migrationBuilder.DropTable(
                name: "TrnJobOrderAttachment");

            migrationBuilder.DropTable(
                name: "TrnJobOrderInformation");

            migrationBuilder.DropTable(
                name: "TrnPointOfSale");

            migrationBuilder.DropTable(
                name: "TrnSalesOrderItem");

            migrationBuilder.DropTable(
                name: "TrnStockInItem");

            migrationBuilder.DropTable(
                name: "MstArticleAccountGroup");

            migrationBuilder.DropTable(
                name: "SysForm");

            migrationBuilder.DropTable(
                name: "TrnStockOut");

            migrationBuilder.DropTable(
                name: "TrnReceivingReceipt");

            migrationBuilder.DropTable(
                name: "TrnStockTransfer");

            migrationBuilder.DropTable(
                name: "TrnStockWithdrawal");

            migrationBuilder.DropTable(
                name: "TrnJobOrderDepartment");

            migrationBuilder.DropTable(
                name: "TrnCollection");

            migrationBuilder.DropTable(
                name: "MstPayType");

            migrationBuilder.DropTable(
                name: "TrnSalesOrder");

            migrationBuilder.DropTable(
                name: "TrnStockIn");

            migrationBuilder.DropTable(
                name: "TrnJobOrder");

            migrationBuilder.DropTable(
                name: "MstJobDepartment");

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
