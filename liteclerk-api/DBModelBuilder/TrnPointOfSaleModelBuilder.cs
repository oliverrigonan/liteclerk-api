using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPointOfSaleModelBuilder
    {
        public static void CreateTrnPointOfSaleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPointOfSaleDBSet>(entity =>
            {
                entity.ToTable("TrnPointOfSale");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnPointOfSales_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TerminalCode).HasColumnName("TerminalCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.POSNumber).HasColumnName("POSNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.POSDate).HasColumnName("POSDate").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.OrderNumber).HasColumnName("OrderNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.CustomerCode).HasColumnName("CustomerCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId").HasColumnType("int");
                entity.HasOne(f => f.MstArticle_CustomerId).WithMany(f => f.TrnPointOfSales_CustomerId).HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ItemCode).HasColumnName("ItemCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ItemId).HasColumnName("ItemId").HasColumnType("int");
                entity.HasOne(f => f.MstArticle_ItemId).WithMany(f => f.TrnPointOfSales_ItemId).HasForeignKey(f => f.ItemId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Discount).HasColumnName("Discount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.NetPrice).HasColumnName("NetPrice").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.TaxCode).HasColumnName("TaxCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.TaxId).HasColumnName("TaxId").HasColumnType("int");
                entity.HasOne(f => f.MstTax_TaxId).WithMany(f => f.TrnPointOfSales_TaxId).HasForeignKey(f => f.TaxId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.TaxAmount).HasColumnName("TaxAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.CashierUserCode).HasColumnName("CashierUserCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CashierUserId).HasColumnName("CashierUserId").HasColumnType("int");
                entity.HasOne(f => f.MstUser_CashierUserId).WithMany(f => f.TrnPointOfSales_CashierUserId).HasForeignKey(f => f.CashierUserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TimeStamp).HasColumnName("TimeStamp").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.PostCode).HasColumnName("PostCode").HasColumnType("nvarchar(50)").HasMaxLength(50);
            });
        }
    }
}
