using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleItemInventoryModelBuilder
    {
        public static void CreateMstArticleItemInventoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleItemInventoryDBSet>(entity =>
            {
                entity.ToTable("MstArticleItemInventory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_Article).WithMany(f => f.MstArticleItemInventories_Article).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_Branch).WithMany(f => f.MstArticleItemInventories_Branch).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.InventoryCode).HasColumnName("InventoryCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
