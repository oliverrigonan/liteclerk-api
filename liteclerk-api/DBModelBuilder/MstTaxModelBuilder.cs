﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstTaxModelBuilder
    {
        public static void CreateMstTaxModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstTaxDBSet>(entity =>
            {
                entity.ToTable("MstTax");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TaxCode).HasColumnName("TaxCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.TaxDescription).HasColumnName("TaxDescription").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TaxRate).HasColumnName("TaxRate").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.MstTaxes_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_CreatedByUserId).WithMany(f => f.MstTaxes_CreatedByUserId).HasForeignKey(f => f.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedDateTime).HasColumnName("CreatedDateTime").HasColumnType("datetime").IsRequired();

                entity.Property(e => e.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UpdatedByUserId).WithMany(f => f.MstTaxes_UpdatedByUserId).HasForeignKey(f => f.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime").IsRequired();
            });
        }
    }
}
