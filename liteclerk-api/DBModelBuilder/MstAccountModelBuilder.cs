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
                entity.HasOne(f => f.MstAccountType_AccountTypeId).WithMany(f => f.MstAccounts_AccountTypeId).HasForeignKey(f => f.AccountTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AccountCashFlowId).HasColumnName("AccountCashFlowId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccountCashFlow_AccountCashFlowId).WithMany(f => f.MstAccounts_AccountCashFlowId).HasForeignKey(f => f.AccountCashFlowId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.MstAccounts_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.MstAccounts_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
