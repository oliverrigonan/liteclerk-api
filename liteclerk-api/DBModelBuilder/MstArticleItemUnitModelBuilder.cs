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
                entity.HasOne(f => f.MstArticle_Article).WithMany(f => f.MstArticleItemUnits_Article).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UnitId).HasColumnName("UnitId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUnit_Unit).WithMany(f => f.MstArticleItemUnits_Unit).HasForeignKey(f => f.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Mutliplier).HasColumnName("Mutliplier").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
