﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleCustomerModelBuilder
    {
        public static void CreateMstArticleCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleCustomerDBSet>(entity =>
            {
                entity.ToTable("MstArticleCustomer");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleCustomers_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Customer).HasColumnName("Customer").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.ContactPerson).HasColumnName("ContactPerson").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.ContactNumber).HasColumnName("ContactNumber").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Category).HasColumnName("Category").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();

                entity.Property(e => e.ReceivableAccountId).HasColumnName("ReceivableAccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_ReceivableAccountId).WithMany(f => f.MstArticleCustomers_ReceivableAccountId).HasForeignKey(f => f.ReceivableAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TermId).HasColumnName("TermId").HasColumnType("int");
                entity.HasOne(f => f.MstTerm_TermId).WithMany(f => f.MstArticleCustomers_TermId).HasForeignKey(f => f.TermId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreditLimit).HasColumnName("CreditLimit").HasColumnType("decimal(18,5)");
            });
        }
    }
}
