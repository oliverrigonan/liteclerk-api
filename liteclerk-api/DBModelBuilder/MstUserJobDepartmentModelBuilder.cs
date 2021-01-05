using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstUserJobDepartmentModelBuilder
    {
        public static void CreateMstUserJobDepartmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstUserJobDepartmentDBSet>(entity =>
            {
                entity.ToTable("MstUserJobDepartment");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UserId).WithMany(f => f.MstUserJobDepartments_UserId).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.JobDepartmentId).HasColumnName("JobDepartmentId").HasColumnType("int");
                entity.HasOne(f => f.MstJobDepartment_JobDepartmentId).WithMany(f => f.MstUserJobDepartments_JobDepartmentId).HasForeignKey(f => f.JobDepartmentId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
