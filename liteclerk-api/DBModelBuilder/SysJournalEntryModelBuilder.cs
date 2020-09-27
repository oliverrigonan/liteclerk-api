using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysJournalEntryModelBuilder
    {
        public static void CreateSysJournalEntryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysJournalEntryDBSet>(entity =>
            {
                entity.ToTable("SysJournalEntry");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.SysJournalEntries_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.JournalEntryDate).HasColumnName("JournalEntryDate").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.SysJournalEntries_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.SysJournalEntries_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DebitAmount).HasColumnName("DebitAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.CreditAmount).HasColumnName("CreditAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int");
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.SysJournalEntries_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int");
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.SysJournalEntries_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.CIId).HasColumnName("CIId").HasColumnType("int");
                entity.HasOne(f => f.TrnCollection_CIId).WithMany(f => f.SysJournalEntries_CIId).HasForeignKey(f => f.CIId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.CVId).HasColumnName("CVId").HasColumnType("int");
                entity.HasOne(f => f.TrnDisbursement_CVId).WithMany(f => f.SysJournalEntries_CVId).HasForeignKey(f => f.CVId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.PMId).HasColumnName("PMId").HasColumnType("int");
                entity.HasOne(f => f.TrnPayableMemo_PMId).WithMany(f => f.SysJournalEntries_PMId).HasForeignKey(f => f.PMId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.RMId).HasColumnName("RMId").HasColumnType("int");
                entity.HasOne(f => f.TrnReceivableMemo_RMId).WithMany(f => f.SysJournalEntries_RMId).HasForeignKey(f => f.RMId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.JVId).HasColumnName("JVId").HasColumnType("int");
                entity.HasOne(f => f.TrnJournalVoucher_JVId).WithMany(f => f.SysJournalEntries_JVId).HasForeignKey(f => f.JVId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ILId).HasColumnName("ILId").HasColumnType("int");
                entity.HasOne(f => f.TrnInventory_ILId).WithMany(f => f.SysJournalEntries_ILId).HasForeignKey(f => f.ILId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
