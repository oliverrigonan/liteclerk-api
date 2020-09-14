using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnSalesOrderItemModelBuilder
    {
        public static void CreateTrnSalesOrderItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnSalesOrderItemDBSet>(entity =>
            {
                entity.ToTable("TrnSalesOrderItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SOId).HasColumnName("SOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesOrder_SOId).WithMany(f => f.TrnSalesOrderItems_SOId).HasForeignKey(f => f.SOId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
