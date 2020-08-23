using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnJobOrderDepartmentModelBuilder
    {
        public static void CreateTrnJobOrderDepartmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnJobOrderDepartmentDBSet>(entity =>
            {
                entity.ToTable("TrnJobOrderDepartment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JOId).HasColumnName("JOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJobOrder_JobOrder).WithMany(f => f.TrnJobOrderDepartments_JobOrder).HasForeignKey(f => f.JOId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.JobDepartmentId).HasColumnName("JobDepartmentId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobDepartment_JobDepartment).WithMany(f => f.TrnJobOrderDepartments_JobDepartment).HasForeignKey(f => f.JobDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.StatusByUserId).HasColumnName("StatusByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_StatusByUser).WithMany(f => f.TrnJobOrderDepartments_StatusByUser).HasForeignKey(f => f.StatusByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.StatusUpdatedDateTime).HasColumnName("StatusUpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
