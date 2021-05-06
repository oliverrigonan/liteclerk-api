using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPayableMemoLineModelBuilder
    {
        public static void CreateTrnPayableMemoLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPayableMemoLineDBSet>(entity =>
            {
                entity.ToTable("TrnPayableMemoLine");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PMId).HasColumnName("PMId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPayableMemo_PMId).WithMany(f => f.TrnPayableMemoLines_PMId).HasForeignKey(f => f.PMId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnPayableMemoLines_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnPayableMemoLines_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnPayableMemoLines_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.TrnPayableMemoLines_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
