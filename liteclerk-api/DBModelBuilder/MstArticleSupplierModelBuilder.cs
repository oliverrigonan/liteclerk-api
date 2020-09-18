using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleSupplierModelBuilder
    {
        public static void CreateMstArticleSupplierModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleSupplierDBSet>(entity =>
            {
                entity.ToTable("MstArticleSupplier");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleSuppliers_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Supplier).HasColumnName("Supplier").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.ContactPerson).HasColumnName("ContactPerson").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.ContactNumber).HasColumnName("ContactNumber").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();

                entity.Property(e => e.PayableAccountId).HasColumnName("PayableAccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_PayableAccountId).WithMany(f => f.MstArticleSuppliers_PayableAccountId).HasForeignKey(f => f.PayableAccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TermId).HasColumnName("TermId").HasColumnType("int");
                entity.HasOne(f => f.MstTerm_TermId).WithMany(f => f.MstArticleSuppliers_TermId).HasForeignKey(f => f.TermId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
