using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstJobTypeInformationModelBuilder
    {
        public static void CreateMstJobTypeInformationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstJobTypeInformationDBSet>(entity =>
            {
                entity.ToTable("MstJobTypeInformation");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobType_JobTypeId).WithMany(f => f.MstJobTypeInformations_JobTypeId).HasForeignKey(f => f.JobTypeId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.InformationCode).HasColumnName("InformationCode").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.InformationGroup).HasColumnName("InformationGroup").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
            });
        }
    }
}
