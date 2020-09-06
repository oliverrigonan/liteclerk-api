using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnJobOrderInformationModelBuilder
    {
        public static void CreateTrnJobOrderInformationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnJobOrderInformationDBSet>(entity =>
            {
                entity.ToTable("TrnJobOrderInformation");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JOId).HasColumnName("JOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnJobOrder_JOId).WithMany(f => f.TrnJobOrderInformations_JOId).HasForeignKey(f => f.JOId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.InformationCode).HasColumnName("InformationCode").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.InformationGroup).HasColumnName("InformationGroup").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Value).HasColumnName("Value").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsPrinted).HasColumnName("IsPrinted").HasColumnType("bit").IsRequired();
                entity.Property(e => e.InformationByUserId).HasColumnName("InformationByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_InformationByUserId).WithMany(f => f.TrnJobOrderInformations_InformationByUserId).HasForeignKey(f => f.InformationByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.InformationUpdatedDateTime).HasColumnName("InformationUpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
