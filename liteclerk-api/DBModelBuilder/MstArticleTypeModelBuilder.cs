using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleTypeModelBuilder
    {
        public static void CreateMstArticleTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleTypeDBSet>(entity =>
            {
                entity.ToTable("MstArticleType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleType).HasColumnName("ArticleType").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
            });
        }
    }
}
