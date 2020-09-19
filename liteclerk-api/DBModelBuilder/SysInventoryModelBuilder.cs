using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysInventoryModelBuilder
    {
        public static void CreateSysInventoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysInventoryDBSet>(entity =>
            {
                entity.ToTable("SysInventory");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.SysInventories_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IVNumber).HasColumnName("IVNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IVDate).HasColumnName("IVDate").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.SysInventories_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ArticleItemInventoryId).HasColumnName("ArticleItemInventoryId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticleItemInventory_ArticleItemInventoryId).WithMany(f => f.SysInventories_ArticleItemInventoryId).HasForeignKey(f => f.ArticleItemInventoryId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.QuantityIn).HasColumnName("QuantityIn").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.QuantityOut).HasColumnName("QuantityOut").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.SysInventories_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.RRId).HasColumnName("RRId").HasColumnType("int");
                entity.HasOne(f => f.TrnReceivingReceipt_RRId).WithMany(f => f.SysInventories_RRId).HasForeignKey(f => f.RRId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int");
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.SysInventories_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.INId).HasColumnName("INId").HasColumnType("int");
                entity.HasOne(f => f.TrnStockIn_INId).WithMany(f => f.SysInventories_INId).HasForeignKey(f => f.INId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.OTId).HasColumnName("OTId").HasColumnType("int");
                entity.HasOne(f => f.TrnStockOut_OTId).WithMany(f => f.SysInventories_OTId).HasForeignKey(f => f.OTId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.STId).HasColumnName("STId").HasColumnType("int");
                entity.HasOne(f => f.TrnStockTransfer_STId).WithMany(f => f.SysInventories_STId).HasForeignKey(f => f.STId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.SWId).HasColumnName("SWId").HasColumnType("int");
                entity.HasOne(f => f.TrnStockWithdrawal_SWId).WithMany(f => f.SysInventories_SWId).HasForeignKey(f => f.SWId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
