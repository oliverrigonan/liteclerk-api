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
                entity.HasOne(f => f.TrnJobOrder_JOId).WithMany(f => f.TrnJobOrderDepartments_JOId).HasForeignKey(f => f.JOId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.JobDepartmentId).HasColumnName("JobDepartmentId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobDepartment_JobDepartmentId).WithMany(f => f.TrnJobOrderDepartments_JobDepartmentId).HasForeignKey(f => f.JobDepartmentId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.StatusByUserId).HasColumnName("StatusByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_StatusByUserId).WithMany(f => f.TrnJobOrderDepartments_StatusByUserId).HasForeignKey(f => f.StatusByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.StatusUpdatedDateTime).HasColumnName("StatusUpdatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.AssignedToUserId).HasColumnName("AssignedToUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_AssignedToUserId).WithMany(f => f.TrnJobOrderDepartments_AssignedToUserId).HasForeignKey(f => f.AssignedToUserId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
