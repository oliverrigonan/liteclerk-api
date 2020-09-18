using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnStockWithdrawalItemModelBuilder
    {
        public static void CreateTrnStockWithdrawalItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnStockWithdrawalItemDBSet>(entity =>
            {
                entity.ToTable("TrnStockWithdrawalItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SWId).HasColumnName("SWId").HasColumnType("int").IsRequired();
                //entity.HasOne(f => f.TrnStockWithdrawal_SWId).WithMany(f => f.TrnStockWithdrawalItems_SWId).HasForeignKey(f => f.SWId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
