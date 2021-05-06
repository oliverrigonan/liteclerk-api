using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnReceivableMemoLineModelBuilder
    {
        public static void CreateTrnReceivableMemoLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnReceivableMemoLineDBSet>(entity =>
            {
                entity.ToTable("TrnReceivableMemoLine");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.RMId).HasColumnName("RMId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnReceivableMemo_RMId).WithMany(f => f.TrnReceivableMemoLines_RMId).HasForeignKey(f => f.RMId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnReceivableMemoLines_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnReceivableMemoLines_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnReceivableMemoLines_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.TrnReceivableMemoLines_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
