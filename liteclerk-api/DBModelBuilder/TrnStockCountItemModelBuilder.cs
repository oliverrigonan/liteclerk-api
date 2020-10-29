using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockCountItemModelBuilder
    {
        public static void CreateTrnStockCountItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockCountItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockCountItem");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SCId).HasColumnName("SCId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnStockCount_SCId).WithMany(f => f.TrnStockCountItems_SCId).HasForeignKey(f => f.SCId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnStockCountItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
