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
                entity.HasOne(f => f.TrnSalesInvoice_SalesInvoice).WithMany(f => f.TrnSalesInvoiceItems_SalesInvoice).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_Item).WithMany(f => f.TrnSalesInvoiceItems_Item).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemInventoryId).HasColumnName("ItemInventoryId").HasColumnType("int");
                entity.HasOne(f => f.MstArticleItemInventory_ItemInventory).WithMany(f => f.TrnSalesInvoiceItems_ItemInventory).HasForeignKey(f => f.ItemInventoryId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ItemJobTypeId).HasColumnName("ItemJobTypeId").HasColumnType("int");
                entity.HasOne(f => f.MstJobType_ItemJobType).WithMany(f => f.TrnSalesInvoiceItems_ItemJobType).HasForeignKey(f => f.ItemJobTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_Unit).WithMany(f => f.TrnSalesInvoiceItems_Unit).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.DiscountId).HasColumnName("DiscountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstDiscount_Discount).WithMany(f => f.TrnSalesInvoiceItems_Discount).HasForeignKey(f => f.DiscountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.DiscountRate).HasColumnName("DiscountRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.DiscountAmount).HasColumnName("DiscountAmount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.NetPrice).HasColumnName("NetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.VATId).HasColumnName("VATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_VAT).WithMany(f => f.TrnSalesInvoiceItems_VAT).HasForeignKey(f => f.VATId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAX).WithMany(f => f.TrnSalesInvoiceItems_WTAX).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnit).WithMany(f => f.TrnSalesInvoiceItems_BaseUnit).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseNetPrice).HasColumnName("BaseNetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.LineTimeStamp).HasColumnName("LineTimeStamp").HasColumnType("datetime").IsRequired();
            });
        }
    }
}