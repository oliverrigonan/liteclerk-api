using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPurchaseOrderItemModelBuilder
    {
        public static void CreateTrnPurchaseOrderItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPurchaseOrderItemDBSet>(entity =>
            {
                entity.ToTable("TrnPurchaseOrderItem");

                // Header field link
                entity.HasKey(e => e.Id);
                entity.Property(e => e.POId).HasColumnName("POId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPurchaseOrder_POId).WithMany(f => f.TrnPurchaseOrderItems_POId).HasForeignKey(f => f.POId).OnDelete(DeleteBehavior.Restrict);

                // Particular field
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                // Line fields
            });
        }
    }
}
