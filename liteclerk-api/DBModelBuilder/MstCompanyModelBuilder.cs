using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstCompanyModelBuilder
    {
        public static void CreateMstCompanyModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstCompanyDBSet>(entity =>
            {
                entity.ToTable("MstCompany");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyCode).HasColumnName("CompanyCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Company).HasColumnName("Company").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TIN).HasColumnName("TIN").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.MstCompanies_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CostMethod).HasColumnName("CostMethod").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.MstCompanies_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.MstCompanies_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
