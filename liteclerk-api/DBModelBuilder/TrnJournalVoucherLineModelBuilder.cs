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
                entity.HasOne(f => f.TrnJournalVoucher_JVId).WithMany(f => f.TrnJournalVoucherLines_JVId).HasForeignKey(f => f.JVId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
