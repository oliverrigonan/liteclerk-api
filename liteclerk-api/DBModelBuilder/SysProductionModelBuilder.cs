using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysProductionModelBuilder
    {
        public static void CreateSysProductionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysProductionDBSet>(entity =>
            {
                entity.ToTable("SysProduction");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.SysProductions_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PNNumber).HasColumnName("PNNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.PNDate).HasColumnName("PNDate").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.ProductionTimeStamp).HasColumnName("ProductionTimeStamp").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UserId).WithMany(f => f.SysProductions_UserId).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.JODepartmentId).HasColumnName("JODepartmentId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJobOrderDepartment_JODepartmentId).WithMany(f => f.SysProductions_JODepartmentId).HasForeignKey(f => f.JODepartmentId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
