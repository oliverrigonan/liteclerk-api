using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysJournalEntryBuilder
    {
        public static void CreateSysJournalEntryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysJournalEntryDBSet>(entity =>
            {
                entity.ToTable("SysJournalEntry");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnJournalEntries_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.TrnJournalEntries_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.GLNumber).HasColumnName("GLNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.GLDate).HasColumnName("GLDate").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnJournalEntries_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnJournalEntries_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DebitAmount).HasColumnName("DebitAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.CreditAmount).HasColumnName("CreditAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.SysJournalEntries_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CVId).HasColumnName("CVId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnDisbursement_CVId).WithMany(f => f.SysJournalEntries_CVId).HasForeignKey(f => f.CVId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PMId).HasColumnName("PMId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPayableMemo_PMId).WithMany(f => f.SysJournalEntries_PMId).HasForeignKey(f => f.PMId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.SysJournalEntries_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CIId).HasColumnName("CIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnCollection_CIId).WithMany(f => f.SysJournalEntries_CIId).HasForeignKey(f => f.CIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.RMId).HasColumnName("RMId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnReceivableMemo_RMId).WithMany(f => f.SysJournalEntries_RMId).HasForeignKey(f => f.CIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.JVId).HasColumnName("JVId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJournalVoucher_JVId).WithMany(f => f.SysJournalEntries_JVId).HasForeignKey(f => f.JVId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
