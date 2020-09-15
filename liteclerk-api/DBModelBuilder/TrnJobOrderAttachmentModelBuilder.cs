using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnJobOrderAttachmentModelBuilder
    {
        public static void CreateTrnJobOrderAttachmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnJobOrderAttachmentDBSet>(entity =>
            {
                entity.ToTable("TrnJobOrderAttachment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JOId).HasColumnName("JOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJobOrder_JOId).WithMany(f => f.TrnJobOrderAttachments_JOId).HasForeignKey(f => f.JOId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.AttachmentCode).HasColumnName("AttachmentCode").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.AttachmentType).HasColumnName("AttachmentType").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.AttachmentURL).HasColumnName("AttachmentURL").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
            });
        }
    }
}
