using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnJournalVoucherLineModelBuilder
    {
        public static void CreateTrnJournalVoucherLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnJournalVoucherLineDBSet>(entity =>
            {
                entity.ToTable("TrnJournalVoucherLine");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.JVId).HasColumnName("JVId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJournalVoucher_JVId).WithMany(f => f.TrnJournalVoucherLines_JVId).HasForeignKey(f => f.JVId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnJournalVoucherLines_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnJournalVoucherLines_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnJournalVoucherLines_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DebitAmount).HasColumnName("DebitAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.CreditAmount).HasColumnName("CreditAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
