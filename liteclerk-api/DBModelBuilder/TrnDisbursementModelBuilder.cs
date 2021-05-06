using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnDisbursementModelBuilder
    {
        public static void CreateTrnDisbursementModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnDisbursementDBSet>(entity =>
            {
                entity.ToTable("TrnDisbursement");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnDisbursements_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnDisbursements_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeCurrencyId).HasColumnName("ExchangeCurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_ExchangeCurrencyId).WithMany(f => f.TrnDisbursements_ExchangeCurrencyId).HasForeignKey(f => f.ExchangeCurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeRate).HasColumnName("ExchangeRate").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.CVNumber).HasColumnName("CVNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CVDate).HasColumnName("CVDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ManualNumber).HasColumnName("ManualNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DocumentReference).HasColumnName("DocumentReference").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.SupplierId).HasColumnName("SupplierId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_SupplierId).WithMany(f => f.TrnDisbursements_SupplierId).HasForeignKey(f => f.SupplierId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Payee).HasColumnName("Payee").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Remarks).HasColumnName("Remarks").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.PayTypeId).HasColumnName("PayTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstPayType_PayTypeId).WithMany(f => f.TrnDisbursements_PayTypeId).HasForeignKey(f => f.PayTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CheckNumber).HasColumnName("CheckNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CheckDate).HasColumnName("CheckDate").HasColumnType("datetime");
                entity.Property(e => e.CheckBank).HasColumnName("CheckBank").HasColumnType("nvarchar(255)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCrossCheck).HasColumnName("IsCrossCheck").HasColumnType("bit").IsRequired();

                entity.Property(e => e.BankId).HasColumnName("BankId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_BankId).WithMany(f => f.TrnDisbursements_BankId).HasForeignKey(f => f.BankId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.IsClear).HasColumnName("IsClear").HasColumnType("bit").IsRequired();

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.PreparedByUserId).HasColumnName("PreparedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_PreparedByUserId).WithMany(f => f.TrnDisbursements_PreparedByUserId).HasForeignKey(f => f.PreparedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CheckedByUserId).HasColumnName("CheckedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CheckedByUserId).WithMany(f => f.TrnDisbursements_CheckedByUserId).HasForeignKey(f => f.CheckedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_ApprovedByUserId).WithMany(f => f.TrnDisbursements_ApprovedByUserId).HasForeignKey(f => f.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsCancelled).HasColumnName("IsCancelled").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.TrnDisbursements_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.TrnDisbursements_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
