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
                entity.HasOne(f => f.AssetAccount).WithMany(f => f.MstArticleAccountGroups_AssetAccount).HasForeignKey(f => f.AssetAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SalesAccountId).HasColumnName("SalesAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.SalesAccount).WithMany(f => f.MstArticleAccountGroups_SalesAccount).HasForeignKey(f => f.SalesAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CostAccountId).HasColumnName("CostAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CostAccount).WithMany(f => f.MstArticleAccountGroups_CostAccount).HasForeignKey(f => f.CostAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ExpenseAccountId).HasColumnName("ExpenseAccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.ExpenseAccount).WithMany(f => f.MstArticleAccountGroups_ExpenseAccount).HasForeignKey(f => f.ExpenseAccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.CreatedByUser).WithMany(f => f.MstArticleAccountGroups_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.UpdatedByUser).WithMany(f => f.MstArticleAccountGroups_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
