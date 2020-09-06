using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class TrnCollectionLineModelBuilder
    {
        public static void CreateTrnCollectionLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.TrnCollectionLineDBSet>(entity =>
            {
                entity.ToTable("TrnCollectionLine");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CIId).HasColumnName("CIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnCollection_Collection).WithMany(f => f.TrnCollectionLines_Collection).HasForeignKey(f => f.CIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_Branch).WithMany(f => f.TrnCollectionLines_Branch).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_Account).WithMany(f => f.TrnCollectionLines_Account).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_Article).WithMany(f => f.TrnCollectionLines_Article).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.TrnSalesInvoice_SalesInvoice).WithMany(f => f.TrnCollectionLines_SalesInvoice).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.PayTypeId).HasColumnName("PayTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstPayType_PayType).WithMany(f => f.TrnCollectionLines_PayType).HasForeignKey(f => f.PayTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CheckNumber).HasColumnName("CheckNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CheckDate).HasColumnName("CheckDate").HasColumnType("datetime").IsRequired();
                entity.Property(e => e.CheckBank).HasColumnName("CheckBank").HasColumnType("nvarchar(255)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.BankId).HasColumnName("BankId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_Bank).WithMany(f => f.TrnCollectionLines_Bank).HasForeignKey(f => f.BankId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsClear).HasColumnName("IsClear").HasColumnType("bit").IsRequired();
                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAX).WithMany(f => f.TrnCollectionLines_WTAX).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
