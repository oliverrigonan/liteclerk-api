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
                entity.HasOne(f => f.MstAccountCategory_AccountCategory).WithMany(f => f.MstAccountTypes_AccountCategory).HasForeignKey(f => f.AccountCategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUser).WithMany(f => f.MstAccountTypes_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUser).WithMany(f => f.MstAccountTypes_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
