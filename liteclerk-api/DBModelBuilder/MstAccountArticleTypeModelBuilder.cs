using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstAccountArticleTypeModelBuilder
    {
        public static void CreateMstAccountArticleTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstAccountArticleTypeDBSet>(entity =>
            {
                entity.ToTable("MstAccountArticleType");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.MstAccountArticleTypes_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleTypeId).HasColumnName("ArticleTypeId").HasColumnType("int");
                entity.HasOne(f => f.MstArticleType_ArticleTypeId).WithMany(f => f.MstAccountArticleTypes_ArticleTypeId).HasForeignKey(f => f.ArticleTypeId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
