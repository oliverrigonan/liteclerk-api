using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnMFJobOrderLineModelBuilder
    {
        public static void CreateMFTrnJobOrderLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnMFJobOrderLineDBSet>(entity =>
            {
                entity.ToTable("TrnMFJobOrderLine");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MFJOId).HasColumnName("MFJOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnMFJobOrder_MFJOId).WithMany(f => f.TrnMFJobOrderLine_MFJOId).HasForeignKey(f => f.MFJOId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Brand).HasColumnName("Brand").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Serial).HasColumnName("Serial").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
