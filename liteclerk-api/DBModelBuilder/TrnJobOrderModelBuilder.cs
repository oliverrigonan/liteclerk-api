using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnJobOrderModelBuilder
    {
        public static void CreateTrnJobOrderModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnJobOrderDBSet>(entity =>
            {
                entity.ToTable("TrnJobOrder");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnJobOrders_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnJobOrders_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.JONumber).HasColumnName("JONumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.JODate).HasColumnName("JODate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DateScheduled).HasColumnName("DateScheduled").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.DateNeeded).HasColumnName("DateNeeded").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int");
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.TrnJobOrders_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SIItemId).HasColumnName("SIItemId").HasColumnType("int");
                entity.HasOne(f => f.TrnSalesInvoiceItem_SIItemId).WithMany(f => f.TrnJobOrders_SIIdItem).HasForeignKey(f => f.SIItemId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnJobOrders_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemJobTypeId).HasColumnName("ItemJobTypeId").HasColumnType("int");
                entity.HasOne(f => f.MstJobType_ItemJobTypeId).WithMany(f => f.TrnJobOrders_ItemJobTypeId).HasForeignKey(f => f.ItemJobTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnJobOrders_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnJobOrders_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUserId).WithMany(f => f.TrnJobOrders_PreparedByUserId).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUserId).WithMany(f => f.TrnJobOrders_CheckedByUserId).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUserId).WithMany(f => f.TrnJobOrders_ApprovedByUserId).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnJobOrders_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnJobOrders_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
