using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstAccountTypeModelBuilder
    {
        public static void CreateMstAccountTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstAccountTypeDBSet>(entity =>
            {
                entity.ToTable("MstAccountType");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountTypeCode).HasColumnName("AccountTypeCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.AccountType).HasColumnName("AccountType").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();

                entity.Property(e => e.AccountCategoryId).HasColumnName("AccountCategoryId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccountCategory_AccountCategoryId).WithMany(f => f.MstAccountTypes_AccountCategoryId).HasForeignKey(f => f.AccountCategoryId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.MstAccountTypes_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.MstAccountTypes_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
