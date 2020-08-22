using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstJobTypeDepartmentModelBuilder
    {
        public static void CreateMstJobTypeDepartmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstJobTypeDepartmentDBSet>(entity =>
            {
                entity.ToTable("MstJobTypeDepartment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobType_JobType).WithMany(f => f.MstJobTypeDepartments_JobType).HasForeignKey(f => f.JobTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.JobDepartmentId).HasColumnName("JobDepartmentId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobDepartment_JobDepartment).WithMany(f => f.MstJobTypeDepartments_JobDepartment).HasForeignKey(f => f.JobDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.NumberOfDays).HasColumnName("NumberOfDays").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
