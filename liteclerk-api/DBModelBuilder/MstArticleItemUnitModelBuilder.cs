using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleItemUnitModelBuilder
    {
        public static void CreateMstItemUnitModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleItemUnitDBSet>(entity =>
            {
                entity.ToTable("MstArticleItemUnit");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleItemUnits_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_UnitId).WithMany(f => f.MstArticleItemUnits_UnitId).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Multiplier).HasColumnName("Multiplier").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
