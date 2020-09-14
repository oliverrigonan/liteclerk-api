using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockOutModelBuilder
    {
        public static void CreateTrnStockOutModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockOutDBSet>(entity =>
            {
                // Header information <do not modify>
                entity.ToTable("TrnStockOut");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnStockOuts_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnStockOuts_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.OTNumber).HasColumnName("CRNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.OTDate).HasColumnName("CRDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                
                // Header fields


                // Header user and audit fields <do not modify>
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnStockOuts_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnStockOuts_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
