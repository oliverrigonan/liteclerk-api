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

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnStockTransferItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemInventoryId).HasColumnName("ItemInventoryId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticleItemInventory_ItemInventoryId).WithMany(f => f.TrnStockTransferItems_ItemInventoryId).HasForeignKey(f => f.ItemInventoryId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnStockTransferItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnStockTransferItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseCost).HasColumnName("BaseCost").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
