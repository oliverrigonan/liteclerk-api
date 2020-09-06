using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleItemPriceModelBuilder
    {
        public static void CreateMstItemPriceModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleItemPriceDBSet>(entity =>
            {
                entity.ToTable("MstArticleItemPrice");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleItemPrices_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PriceDescription).HasColumnName("PriceDescription").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
