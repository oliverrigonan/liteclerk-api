using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockTransferItemModelBuilder
    {
        public static void CreateTrnStockTransferItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockTransferItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockTransferItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.STId).HasColumnName("STId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnStockTransfer_STId).WithMany(f => f.TrnStockTransferItems_STId).HasForeignKey(f => f.STId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
