﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using liteclerk_api.DBContext;

namespace liteclerk_api.Migrations
{
    [DbContext(typeof(LiteclerkDBContext))]
    partial class LiteclerkDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompany", b =>
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

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyBranch", b =>
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

            modelBuilder.Entity("liteclerk_api.DBSets.MstCurrency", b =>
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

            modelBuilder.Entity("liteclerk_api.DBSets.MstUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyBranchId")
                        .HasColumnName("CompanyBranchId")
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

                    b.HasIndex("CompanyBranchId");

                    b.HasIndex("CompanyId");

                    b.ToTable("MstUser");
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompany", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUser", "CreatedByUser")
                        .WithMany("CreatedByUserCompanies")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstCurrency", "Currency")
                        .WithMany("CompanyCurrencies")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUser", "UpdatedByUser")
                        .WithMany("UpdatedByUserCompanies")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCompanyBranch", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstCompany", "Company")
                        .WithMany("CompanyBranches")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstCurrency", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstUser", "CreatedByUser")
                        .WithMany("CreatedByUserCurrencies")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("liteclerk_api.DBSets.MstUser", "UpdatedByUser")
                        .WithMany("UpdatedByUserCurrencies")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("liteclerk_api.DBSets.MstUser", b =>
                {
                    b.HasOne("liteclerk_api.DBSets.MstCompanyBranch", "CompanyBranch")
                        .WithMany("CompanyBranchUsers")
                        .HasForeignKey("CompanyBranchId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("liteclerk_api.DBSets.MstCompany", "Company")
                        .WithMany("CompanyUsers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
