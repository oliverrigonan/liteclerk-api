using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnSalesInvoiceMFJOItemModelBuilder
    {
        public static void CreateMFTrnSalesInvoiceJOItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnSalesInvoiceMFJOItemDBSet>(entity =>
            {
                entity.ToTable("TrnSalesInvoiceMFJOItem");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.TrnSalesInvoiceMFJOItems_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.MFJOId).HasColumnName("MFJOId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnMFJobOrder_MFJOItemId).WithMany(f => f.TrnSalesInvoiceMFJOItem_MFJOSIId).HasForeignKey(f => f.MFJOId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}