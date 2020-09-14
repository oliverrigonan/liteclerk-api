using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnPurchaseRequestItemModelBuilder
    {
        public static void CreateTrnPurchaseRequestItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnPurchaseRequestItemDBSet>(entity =>
            {
                entity.ToTable("TrnPurchaseRequestItem");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PRId).HasColumnName("PRId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnPurchaseRequest_PRId).WithMany(f => f.TrnPurchaseRequestItems_PRId).HasForeignKey(f => f.PRId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
