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
                entity.HasOne(f => f.TrnCollection_CIId).WithMany(f => f.TrnCollectionLines_CIId).HasForeignKey(f => f.CIId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.TrnCollectionLines_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AccountId).HasColumnName("AccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_AccountId).WithMany(f => f.TrnCollectionLines_AccountId).HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.TrnCollectionLines_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.SIId).HasColumnName("SIId").HasColumnType("int");
                entity.HasOne(f => f.TrnSalesInvoice_SIId).WithMany(f => f.TrnCollectionLines_SIId).HasForeignKey(f => f.SIId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.BaseAmount).HasColumnName("BaseAmount").HasColumnType("decimal(18,5)").IsRequired();

                entity.Property(e => e.Particulars).HasColumnName("Particulars").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.PayTypeId).HasColumnName("PayTypeId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstPayType_PayTypeId).WithMany(f => f.TrnCollectionLines_PayTypeId).HasForeignKey(f => f.PayTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CheckNumber).HasColumnName("CheckNumber").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CheckDate).HasColumnName("CheckDate").HasColumnType("datetime");
                entity.Property(e => e.CheckBank).HasColumnName("CheckBank").HasColumnType("nvarchar(255)").HasMaxLength(50).IsRequired();

                entity.Property(e => e.BankId).HasColumnName("BankId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_BankId).WithMany(f => f.TrnCollectionLines_BankId).HasForeignKey(f => f.BankId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.IsClear).HasColumnName("IsClear").HasColumnType("bit").IsRequired();

                entity.Property(e => e.WTAXId).HasColumnName("WTAXId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstTax_WTAXId).WithMany(f => f.TrnCollectionLines_WTAXId).HasForeignKey(f => f.WTAXId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.WTAXRate).HasColumnName("WTAXRate").HasColumnType("decimal(18,5)").IsRequired();
                entity.Property(e => e.WTAXAmount).HasColumnName("WTAXAmount").HasColumnType("decimal(18,5)").IsRequired();
            });
        }
    }
}
