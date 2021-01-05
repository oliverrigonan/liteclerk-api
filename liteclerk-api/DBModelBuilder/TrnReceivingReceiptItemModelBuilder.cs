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

                entity.HasKey(e => e.Id);

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.TrnReceivingReceiptItems_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnReceivingReceiptItems_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.POId).HasColumnName("POId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPurchaseOrder_POId).WithMany(f => f.TrnReceivingReceiptItems_POId).HasForeignKey(f => f.POId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnReceivingReceiptItems_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.TrnReceivingReceiptItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.VATId).HasColumnName("VATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_VATId).WithMany(f => f.TrnReceivingReceiptItems_VATId).HasForeignKey(f => f.VATId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.VATRate).HasColumnName("VATRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.VATAmount).HasColumnName("VATAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.TrnReceivingReceiptItems_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXRate).HasColumnName("WTAXRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.WTAXAmount).HasColumnName("WTAXAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.BaseQuantity).HasColumnName("BaseQuantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_BaseUnitId).WithMany(f => f.TrnReceivingReceiptItems_BaseUnitId).HasForeignKey(f => f.BaseUnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BaseCost).HasColumnName("BaseCost").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
