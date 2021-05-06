using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnDisbursementLineModelBuilder
    {
        public static void CreateTrnDisbursementLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnDisbursementLineDBSet>(entity =>
            {
                entity.ToTable("TrnDisbursementLine");

                entity.HasKey(e => e.Id);
               
                entity.Property(e => e.CVId).HasColumnName("CVId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnDisbursement_CVId).WithMany(f => f.TrnDisbursementLines_CVId).HasForeignKey(f => f.CVId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnDisbursementLines_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnDisbursementLines_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnDisbursementLines_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int");
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.TrnDisbursementLines_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.TrnDisbursementLines_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXRate).HasColumnName("WTAXRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.WTAXAmount).HasColumnName("WTAXAmount").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
