using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnCollectionModelBuilder
    {
        public static void CreateTrnCollectionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnCollectionDBSet>(entity =>
            {
                entity.ToTable("TrnCollection");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_Branch).WithMany(f => f.TrnCollections_Branch).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_Currency).WithMany(f => f.TrnCollections_Currency).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CINumber).HasColumnName("CINumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CIDate).HasColumnName("CIDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_Customer).WithMany(f => f.TrnCollections_Customer).HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUser).WithMany(f => f.TrnCollections_PreparedByUser).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUser).WithMany(f => f.TrnCollections_CheckedByUser).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUser).WithMany(f => f.TrnCollections_ApprovedByUser).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUser).WithMany(f => f.TrnCollections_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUser).WithMany(f => f.TrnCollections_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
