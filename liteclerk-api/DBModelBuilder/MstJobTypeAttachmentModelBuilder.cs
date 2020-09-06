using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstJobTypeAttachmentModelBuilder
    {
        public static void CreateMstJobTypeAttachmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstJobTypeAttachmentDBSet>(entity =>
            {
                entity.ToTable("MstJobTypeAttachment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstJobType_JobTypeId).WithMany(f => f.MstJobTypeAttachments_JobTypeId).HasForeignKey(f => f.JobTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AttachmentCode).HasColumnName("AttachmentCode").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.AttachmentType).HasColumnName("AttachmentType").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
            });
        }
    }
}