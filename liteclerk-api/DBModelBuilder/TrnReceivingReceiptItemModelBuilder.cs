using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnReceivingReceiptItemModelBuilder
    {
        public static void CreateTrnReceivingReceiptItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnReceivingReceiptItemDBSet>(entity =>
            {
                entity.ToTable("TrnReceivingReceiptItem");

                // Header field link
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int").IsRequired();
                //entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.TrnReceivingReceiptItems_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Restrict);

                // Particular field
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                // Line fields
            });
        }
    }
}
