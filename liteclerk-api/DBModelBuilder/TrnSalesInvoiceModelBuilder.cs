using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnSalesInvoiceModelBuilder
    {
        public static void CreateTrnSalesInvoiceModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnSalesInvoiceDBSet>(entity =>
            {
                entity.ToTable("TrnSalesInvoice");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnSalesInvoices_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnSalesInvoices_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SINumber).HasColumnName("SINumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.SIDate).HasColumnName("SIDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_CustomerId).WithMany(f => f.TrnSalesInvoices_CustomerId).HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.TermId).HasColumnName("TermId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTerm_TermId).WithMany(f => f.TrnSalesInvoices_TermId).HasForeignKey(f => f.TermId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.DateNeeded).HasColumnName("DateNeeded").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.SoldByUserId).HasColumnName("SoldByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_SoldByUserId).WithMany(f => f.TrnSalesInvoices_SoldByUserId).HasForeignKey(f => f.SoldByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUserId).WithMany(f => f.TrnSalesInvoices_PreparedByUserId).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUserId).WithMany(f => f.TrnSalesInvoices_CheckedByUserId).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUserId).WithMany(f => f.TrnSalesInvoices_ApprovedByUserId).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.PaidAmount).HasColumnName("PaidAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.AdjustmentAmount).HasColumnName("AdjustmentAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BalanceAmount).HasColumnName("BalanceAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnSalesInvoices_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnSalesInvoices_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
