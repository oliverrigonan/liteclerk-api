using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnSalesOrderItemModelBuilder
    {
        public static void CreateTrnSalesOrderItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnSalesOrderItemDBSet>(entity =>
            {
                entity.ToTable("TrnSalesOrderItem");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SOId).HasColumnName("SOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesOrder_SOId).WithMany(f => f.TrnSalesOrderItems_SOId).HasForeignKey(f => f.SOId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnSalesOrderItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemInventoryId).HasColumnName("ItemInventoryId").HasColumnType("int");
                entity.HasOne(f => f.MstArticleItemInventory_ItemInventoryId).WithMany(f => f.TrnSalesOrderItems_ItemInventoryId).HasForeignKey(f => f.ItemInventoryId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnSalesOrderItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.DiscountId).HasColumnName("DiscountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstDiscount_DiscountId).WithMany(f => f.TrnSalesOrderItems_DiscountId).HasForeignKey(f => f.DiscountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.DiscountRate).HasColumnName("DiscountRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.DiscountAmount).HasColumnName("DiscountAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.NetPrice).HasColumnName("NetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.VATId).HasColumnName("VATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_VATId).WithMany(f => f.TrnSalesOrderItems_VATId).HasForeignKey(f => f.VATId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.VATRate).HasColumnName("VATRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.VATAmount).HasColumnName("VATAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.TrnSalesOrderItems_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXRate).HasColumnName("WTAXRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.WTAXAmount).HasColumnName("WTAXAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnSalesOrderItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseNetPrice).HasColumnName("BaseNetPrice").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.LineTimeStamp).HasColumnName("LineTimeStamp").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
