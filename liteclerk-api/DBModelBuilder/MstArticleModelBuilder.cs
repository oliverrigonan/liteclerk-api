using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleModelBuilder
    {
        public static void CreateMstArticleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleDBSet>(entity =>
            {
                entity.ToTable("MstArticle");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleCode).HasColumnName("ArticleCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Article).HasColumnName("Article").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.ArticleTypeId).HasColumnName("ArticleTypeId").HasColumnType("int");
                entity.HasOne(f => f.ArticleType).WithMany(f => f.MstArticles_ArticleType).HasForeignKey(f => f.ArticleTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsLocked).HasColumnName("IsLocked").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CreatedByUser).WithMany(f => f.MstArticles_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.UpdatedByUser).WithMany(f => f.MstArticles_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
