using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockOutItemModelBuilder
    {
        public static void CreateTrnStockOutItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockOutItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockOutItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.OTId).HasColumnName("OTId").HasColumnType("int").IsRequired();
                //entity.HasOne(f => f.TrnStockOut_OTId).WithMany(f => f.TrnStockOutItems_OTId).HasForeignKey(f => f.OTId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
