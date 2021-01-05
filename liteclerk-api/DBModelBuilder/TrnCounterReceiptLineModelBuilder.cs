using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnCounterReceiptLineModelBuilder
    {
        public static void CreateTrnCounterReceiptLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnCounterReceiptLineDBSet>(entity =>
            {
                entity.ToTable("TrnCounterReceiptLine");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CRId).HasColumnName("CIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnCounterReceipt_CRId).WithMany(f => f.TrnCounterReceiptLines_CRId).HasForeignKey(f => f.CRId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
