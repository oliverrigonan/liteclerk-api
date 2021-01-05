using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPurchaseRequestItemModelBuilder
    {
        public static void CreateTrnPurchaseRequestItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPurchaseRequestItemDBSet>(entity =>
            {
                entity.ToTable("TrnPurchaseRequestItem");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PRId).HasColumnName("PRId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPurchaseRequest_PRId).WithMany(f => f.TrnPurchaseRequestItems_PRId).HasForeignKey(f => f.PRId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnPurchaseRequestItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnPurchaseRequestItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnPurchaseRequestItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseCost).HasColumnName("BaseCost").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
