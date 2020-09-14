using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockInItemModelBuilder
    {
        public static void CreateTrnStockInItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockInItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockInItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.INId).HasColumnName("INId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnStockIn_INId).WithMany(f => f.TrnStockInItems_INId).HasForeignKey(f => f.INId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
