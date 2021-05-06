using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPurchaseRequestModelBuilder
    {
        public static void CreateTrnPurchaseRequestModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPurchaseRequestDBSet>(entity =>
            {
                entity.ToTable("TrnPurchaseRequest");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnPurchaseRequests_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnPurchaseRequests_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeCurrencyId).HasColumnName("ExchangeCurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_ExchangeCurrencyId).WithMany(f => f.TrnPurchaseRequests_ExchangeCurrencyId).HasForeignKey(f => f.ExchangeCurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeRate).HasColumnName("ExchangeRate").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.PRNumber).HasColumnName("PRNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.PRDate).HasColumnName("PRDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.SupplierId).HasColumnName("SupplierId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_SupplierId).WithMany(f => f.TrnPurchaseRequests_SupplierId).HasForeignKey(f => f.SupplierId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TermId).HasColumnName("TermId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTerm_TermId).WithMany(f => f.TrnPurchaseRequests_TermId).HasForeignKey(f => f.TermId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DateNeeded).HasColumnName("DateNeeded").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.RequestedByUserId).HasColumnName("RequestedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_RequestedByUserId).WithMany(f => f.TrnPurchaseRequests_RequestedByUserId).HasForeignKey(f => f.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUserId).WithMany(f => f.TrnPurchaseRequests_PreparedByUserId).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUserId).WithMany(f => f.TrnPurchaseRequests_CheckedByUserId).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUserId).WithMany(f => f.TrnPurchaseRequests_ApprovedByUserId).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnPurchaseRequests_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnPurchaseRequests_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
