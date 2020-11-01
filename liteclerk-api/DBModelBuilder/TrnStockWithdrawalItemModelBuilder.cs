using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockWithdrawalItemModelBuilder
    {
        public static void CreateTrnStockWithdrawalItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockWithdrawalItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockWithdrawalItem");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SWId).HasColumnName("SWId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnStockWithdrawal_SWId).WithMany(f => f.TrnStockWithdrawalItems_SWId).HasForeignKey(f => f.SWId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnStockWithdrawalItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemInventoryId).HasColumnName("ItemInventoryId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticleItemInventory_ItemInventoryId).WithMany(f => f.TrnStockWithdrawalItems_ItemInventoryId).HasForeignKey(f => f.ItemInventoryId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnStockWithdrawalItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnStockWithdrawalItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseCost).HasColumnName("BaseCost").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
