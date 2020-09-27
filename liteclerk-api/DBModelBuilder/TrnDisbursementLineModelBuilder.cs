using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnDisbursementLineModelBuilder
    {
        public static void CreateTrnDisbursementLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnDisbursementLineDBSet>(entity =>
            {
                entity.ToTable("TrnDisbursementLine");

                // Header link fields
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CVId).HasColumnName("CVId").HasColumnType("int").IsRequired();
                //entity.HasOne(f => f.TrnDisbursement_CVId).WithMany(f => f.TrnDisbursementLines_CVId).HasForeignKey(f => f.CVId).OnDelete(DeleteBehavior.Restrict);

                // Particular field
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                // Line fields
            });
        }
    }
}
