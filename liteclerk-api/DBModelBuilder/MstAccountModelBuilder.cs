using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstAccountModelBuilder
    {
        public static void CreateMstAccountModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstAccountDBSet>(entity =>
            {
                entity.ToTable("MstAccount");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountCode).HasColumnName("AccountCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Account).HasColumnName("Account").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.AccountTypeId).HasColumnName("AccountTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccountType_AccountType).WithMany(f => f.MstAccounts_AccountType).HasForeignKey(f => f.AccountTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AccountCashFlowId).HasColumnName("AccountCashFlowId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccountCashFlow_AccountCashFlow).WithMany(f => f.MstAccounts_AccountCashFlow).HasForeignKey(f => f.AccountCashFlowId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUser).WithMany(f => f.MstAccounts_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUser).WithMany(f => f.MstAccounts_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
