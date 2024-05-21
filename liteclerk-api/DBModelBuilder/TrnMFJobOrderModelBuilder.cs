using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnMFJobOrderModelBuilder
    {
        public static void CreateTrnMFJobOrderModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnMFJobOrderDBSet>(entity =>
            {
                entity.ToTable("TrnMFJobOrder");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnMFJobOrders_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.JONumber).HasColumnName("JONumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.JODate).HasColumnName("JODate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.DateScheduled).HasColumnName("DateScheduled").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.DateNeeded).HasColumnName("DateNeeded").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerId").HasColumnType("int");
                entity.HasOne(f => f.MstArticle_CustomerId).WithMany(f => f.TrnMFJobOrders_Customer).HasForeignKey(f => f.CustomerId);

                entity.Property(e => e.Accessories).HasColumnName("Accessories").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Engineer).HasColumnName("Engineer").HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Complaint).HasColumnName("Complaint").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUserId).WithMany(f => f.TrnMFJobOrders_PreparedByUserId).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUserId).WithMany(f => f.TrnMFJobOrder_CheckedByUserId).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUserId).WithMany(f => f.TrnMFJobOrders_ApprovedByUserId).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnMFJobOrders_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnMFJobOrders_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
