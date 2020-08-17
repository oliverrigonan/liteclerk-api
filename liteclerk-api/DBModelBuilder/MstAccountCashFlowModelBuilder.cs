using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstAccountCashFlowModelBuilder
    {
        public static void CreateMstAccountCashFlowModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstAccountCashFlowDBSet>(entity =>
            {
                entity.ToTable("MstAccountCashFlow");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountCashFlowCode).HasColumnName("AccountCashFlowCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.AccountCashFlow).HasColumnName("AccountCashFlow").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUser).WithMany(f => f.MstAccountCashFlows_CreatedByUser).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedByDateTime).HasColumnName("CreatedByDateTime").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUser).WithMany(f => f.MstAccountCashFlows_UpdatedByUser).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedByDateTime).HasColumnName("UpdatedByDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
