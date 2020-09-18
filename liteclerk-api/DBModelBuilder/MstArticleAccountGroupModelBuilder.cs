using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleAccountGroupModelBuilder
    {
        public static void CreateMstArticleAccountGroupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleAccountGroupDBSet>(entity =>
            {
                entity.ToTable("MstArticleAccountGroup");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ArticleAccountGroupCode).HasColumnName("ArticleAccountGroupCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ArticleAccountGroup).HasColumnName("ArticleAccountGroup").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();

                entity.Property(e => e.AssetAccountId).HasColumnName("AssetAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AssetAccountId).WithMany(f => f.MstArticleAccountGroups_AssetAccountId).HasForeignKey(f => f.AssetAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SalesAccountId).HasColumnName("SalesAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_SalesAccountId).WithMany(f => f.MstArticleAccountGroups_SalesAccountId).HasForeignKey(f => f.SalesAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CostAccountId).HasColumnName("CostAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_CostAccountId).WithMany(f => f.MstArticleAccountGroups_CostAccountId).HasForeignKey(f => f.CostAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExpenseAccountId).HasColumnName("ExpenseAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_ExpenseAccountId).WithMany(f => f.MstArticleAccountGroups_ExpenseAccountId).HasForeignKey(f => f.ExpenseAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.MstArticleAccountGroups_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.MstArticleAccountGroups_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
