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
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleItems_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SKUCode).HasColumnName("SKUCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.BarCode).HasColumnName("BarCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.MstArticleItems_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.IsInventory).HasColumnName("IsInventory").HasColumnType("bit").IsRequired();

                entity.Property(e => e.ArticleAccountGroupId).HasColumnName("ArticleAccountGroupId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticleAccountGroup_ArticleAccountGroupId).WithMany(f => f.MstArticleItems_ArticleAccountGroupId).HasForeignKey(f => f.ArticleAccountGroupId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SalesAccountId).HasColumnName("SalesAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AssetAccountId).WithMany(f => f.MstArticleItems_AssetAccountId).HasForeignKey(f => f.AssetAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SalesAccountId).HasColumnName("SalesAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_SalesAccountId).WithMany(f => f.MstArticleItems_SalesAccountId).HasForeignKey(f => f.SalesAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CostAccountId).HasColumnName("CostAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_CostAccountId).WithMany(f => f.MstArticleItems_CostAccountId).HasForeignKey(f => f.CostAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExpenseAccountId).HasColumnName("ExpenseAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_ExpenseAccountId).WithMany(f => f.MstArticleItems_ExpenseAccountId).HasForeignKey(f => f.ExpenseAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.RRVATId).HasColumnName("RRVATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_RRVATId).WithMany(f => f.MstArticleItems_RRVATId).HasForeignKey(f => f.RRVATId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SIVATId).HasColumnName("SIVATId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_SIVATId).WithMany(f => f.MstArticleItems_SIVATId).HasForeignKey(f => f.SIVATId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.MstArticleItems_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Kitting).HasColumnName("Kitting").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
            });
        }
    }
}
