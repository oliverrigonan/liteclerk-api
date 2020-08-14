using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBContext
{
    public class LiteclerkDBContext : DbContext
    {
        public LiteclerkDBContext(DbContextOptions<LiteclerkDBContext> options) : base(options)
        {

        }

        public virtual DbSet<DBSets.MstUser> MstUsers { get; set; }
        public virtual DbSet<DBSets.MstCompany> MstCompanies { get; set; }
        public virtual DbSet<DBSets.MstCompanyBranch> MstCompanyBranches { get; set; }
        public virtual DbSet<DBSets.MstCurrency> MstCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DBSets.MstUser>(entity =>
            {
                entity.ToTable("MstUser");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasColumnName("Username").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Fullname).HasColumnName("Fullname").HasColumnType("nvarchar(100)").HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<DBSets.MstCompany>(entity =>
            {
                entity.ToTable("MstCompany");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyCode).HasColumnName("CompanyCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Company).HasColumnName("Company").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TIN).HasColumnName("TIN").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.Currency).WithMany(f => f.CompanyCurrencies).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CostMethod).HasColumnName("CostMethod").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CreatedByUser).WithMany(f => f.CreatedByUserCompanies).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.UpdatedByUser).WithMany(f => f.UpdatedByUserCompanies).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });

            modelBuilder.Entity<DBSets.MstCompanyBranch>(entity =>
            {
                entity.ToTable("MstCompanyBranch");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchCode).HasColumnName("BranchCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.Company).WithMany(f => f.CompanyBranches).HasForeignKey(f => f.CompanyId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Branch).HasColumnName("Branch").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TIN).HasColumnName("TIN").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<DBSets.MstCurrency>(entity =>
            {
                entity.ToTable("MstCurrency");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CurrencyCode).HasColumnName("BranchCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Currency).HasColumnName("Currency").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CreatedByUser).WithMany(f => f.CreatedByUserCurrencies).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.UpdatedByUser).WithMany(f => f.UpdatedByUserCurrencies).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
