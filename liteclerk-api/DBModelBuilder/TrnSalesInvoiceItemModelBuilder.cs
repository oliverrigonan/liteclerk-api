using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnSalesInvoiceItemModelBuilder
    {
        public static void CreateTrnSalesInvoiceItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnSalesInvoiceItemDBSet>(entity =>
            {
                entity.ToTable("TrnSalesInvoiceItem");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.TrnSalesInvoiceItems_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnSalesInvoiceItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemInventoryId).HasColumnName("ItemInventoryId").HasColumnType("int");
                entity.HasOne(f => f.MstArticleItemInventory_ItemInventoryId).WithMany(f => f.TrnSalesInvoiceItems_ItemInventoryId).HasForeignKey(f => f.ItemInventoryId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemJobTypeId).HasColumnName("ItemJobTypeId").HasColumnType("int");
                entity.HasOne(f => f.MstJobType_ItemJobTypeId).WithMany(f => f.TrnSalesInvoiceItems_ItemJobTypeId).HasForeignKey(f => f.ItemJobTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnSalesInvoiceItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.DiscountId).HasColumnName("DiscountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstDiscount_DiscountId).WithMany(f => f.TrnSalesInvoiceItems_DiscountId).HasForeignKey(f => f.DiscountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.DiscountRate).HasColumnName("DiscountRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.DiscountAmount).HasColumnName("DiscountAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.NetPrice).HasColumnName("NetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.VATId).HasColumnName("VATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_VATId).WithMany(f => f.TrnSalesInvoiceItems_VATId).HasForeignKey(f => f.VATId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.TrnSalesInvoiceItems_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnSalesInvoiceItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseNetPrice).HasColumnName("BaseNetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.LineTimeStamp).HasColumnName("LineTimeStamp").HasColumnType("datetime").IsRequired();
            });
        }
    }
}