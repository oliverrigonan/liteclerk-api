using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleItemModelBuilder
    {
        public static void CreateMstArticleItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleItemDBSet>(entity =>
            {
                entity.ToTable("MstArticleItem");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.Article).WithMany(f => f.MstArticleItems_Article).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SKUCode).HasColumnName("SKUCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.BarCode).HasColumnName("BarCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.Unit).WithMany(f => f.MstArticleItems_Unit).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsJob).HasColumnName("IsJob").HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsInventory).HasColumnName("IsInventory").HasColumnType("bit").IsRequired();
                entity.Property(e => e.AssetAccountId).HasColumnName("AssetAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.AssetAccount).WithMany(f => f.MstArticleItems_AssetAccount).HasForeignKey(f => f.AssetAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SalesAccountId).HasColumnName("SalesAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.SalesAccount).WithMany(f => f.MstArticleItems_SalesAccount).HasForeignKey(f => f.SalesAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CostAccountId).HasColumnName("CostAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CostAccount).WithMany(f => f.MstArticleItems_CostAccount).HasForeignKey(f => f.CostAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ExpenseAccountId).HasColumnName("ExpenseAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.ExpenseAccount).WithMany(f => f.MstArticleItems_ExpenseAccount).HasForeignKey(f => f.ExpenseAccountId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
