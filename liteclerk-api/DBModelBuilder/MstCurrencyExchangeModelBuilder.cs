using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstCurrencyExchangeModelBuilder
    {
        public static void CreateMstCurrencyExchangeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstCurrencyExchangeDBSet>(entity =>
            {
                entity.ToTable("MstCurrencyExchange");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_CurrencyId).WithMany(f => f.MstCurrencyExchanges_CurrencyId).HasForeignKey(f => f.CurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeCurrencyId).HasColumnName("ExchangeCurrencyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCurrency_ExchangeCurrencyId).WithMany(f => f.MstCurrencyExchanges_ExchangeCurrencyId).HasForeignKey(f => f.ExchangeCurrencyId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeDate).HasColumnName("ExchangeDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ExchangeRate).HasColumnName("ExchangeRate").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
