﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using liteclerk_api.DBContext;

namespace liteclerk_api.Migrations
{
    [DbContext(typeof(LiteclerkDBContext))]
    [Migration("20200816120416_AddedAllMstAccountingTables")]
    partial class AddedAllMstAccountingTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountCashFlowDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountCashFlow")
                        .IsRequired()
                        .HasColumnName("AccountCashFlow")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("AccountCashFlowCode")
                        .IsRequired()
                        .HasColumnName("AccountCashFlowCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstAccountCashFlow");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountCategoryDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountCategory")
                        .IsRequired()
                        .HasColumnName("AccountCategory")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("AccountCategoryCode")
                        .IsRequired()
                        .HasColumnName("AccountCategoryCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstAccountCategory");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnName("Account")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("AccountCashFlowId")
                        .HasColumnName("AccountCashFlowId")
                        .HasColumnType("int");

                    b.Property<string>("AccountCode")
                        .IsRequired()
                        .HasColumnName("AccountCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("AccountTypeId")
                        .HasColumnName("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountCashFlowId");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstAccount");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountTypeDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountCategoryId")
                        .HasColumnName("AccountCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnName("AccountType")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("AccountTypeCode")
                        .IsRequired()
                        .HasColumnName("AccountTypeCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountCategoryId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstAccountType");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstArticleDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Article")
                        .IsRequired()
                        .HasColumnName("Article")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("ArticleCode")
                        .IsRequired()
                        .HasColumnName("ArticleCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("ArticleTypeId")
                        .HasColumnName("ArticleTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLocked")
                        .HasColumnName("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleTypeId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstArticle");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstArticleTypeDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArticleType")
                        .IsRequired()
                        .HasColumnName("ArticleType")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("MstArticleType");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyBranchDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("Address")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnName("Branch")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnName("BranchCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("CompanyId")
                        .HasColumnName("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TIN")
                        .IsRequired()
                        .HasColumnName("TIN")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("MstCompanyBranch");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("Address")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnName("Company")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("CompanyCode")
                        .IsRequired()
                        .HasColumnName("CompanyCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CostMethod")
                        .IsRequired()
                        .HasColumnName("CostMethod")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnName("CurrencyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLocked")
                        .HasColumnName("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TIN")
                        .IsRequired()
                        .HasColumnName("TIN")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstCompany");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCurrencyDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnName("Currency")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnName("CurrencyCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("IsLocked")
                        .HasColumnName("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstCurrency");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstTermDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedByDateTime")
                        .HasColumnName("CreatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnName("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ManualCode")
                        .IsRequired()
                        .HasColumnName("ManualCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("NumberOfDays")
                        .HasColumnName("NumberOfDays")
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("Term")
                        .IsRequired()
                        .HasColumnName("Term")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("TermCode")
                        .IsRequired()
                        .HasColumnName("TermCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedByDateTime")
                        .HasColumnName("UpdatedByDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UpdatedByUserId")
                        .HasColumnName("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("MstTerm");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstUserDBSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchId")
                        .HasColumnName("BranchId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyId")
                        .HasColumnName("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnName("Fullname")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("Username")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("CompanyId");

                    b.ToTable("MstUser");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountCashFlowDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstAccountCashFlows_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstAccountCashFlows_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountCategoryDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstAccountCategories_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstAccountCategories_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstAccountCashFlowDBSet", "AccountCashFlow")
                        .WithMany("MstAccounts_AccountCashFlow")
                        .HasForeignKey("AccountCashFlowId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstAccountTypeDBSet", "AccountType")
                        .WithMany("MstAccounts_AccountType")
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstAccounts_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstAccounts_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstAccountTypeDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstAccountCategoryDBSet", "AccountCategory")
                        .WithMany("MstAccountTypes_AccountCategory")
                        .HasForeignKey("AccountCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstAccountTypes_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstAccountTypes_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstArticleDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstArticleTypeDBSet", "ArticleType")
                        .WithMany("MstArticles_ArticleType")
                        .HasForeignKey("ArticleTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstArticles_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstArticles_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyBranchDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstCompanyDBSet", "Company")
                        .WithMany("MstCompanyBranches_Company")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstCompanies_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstCurrencyDBSet", "Currency")
                        .WithMany("MstCompanies_Currency")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstCompanies_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCurrencyDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstCurrencies_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstCurrencies_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstTermDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "CreatedByUser")
                        .WithMany("MstTerms_CreatedByUser")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUserDBSet", "UpdatedByUser")
                        .WithMany("MstTerms_UpdatedByUser")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstUserDBSet", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstCompanyBranchDBSet", "CompanyBranch")
                        .WithMany("MstUsers_CompanyBranch")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("liteclerk_api.DBSets.MstCompanyDBSet", "Company")
                        .WithMany("MstUsers_Company")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
