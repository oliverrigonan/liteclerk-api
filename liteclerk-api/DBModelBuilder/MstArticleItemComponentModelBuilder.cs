using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleItemComponentModelBuilder
    {
        public static void CreateMstArticleItemComponentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleItemComponentDBSet>(entity =>
            {
                entity.ToTable("MstArticleItemComponent");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleItemComponents_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ComponentArticleId).HasColumnName("ComponentArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ComponentArticleId).WithMany(f => f.MstArticleItemComponents_ComponentArticleId).HasForeignKey(f => f.ComponentArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
