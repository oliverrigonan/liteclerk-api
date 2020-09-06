using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstArticleBankModelBuilder
    {
        public static void CreateMstArticleBankModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstArticleBankDBSet>(entity =>
            {
                entity.ToTable("MstArticleBank");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ArticleId).HasColumnName("ArticleId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstArticle_ArticleId).WithMany(f => f.MstArticleBanks_ArticleId).HasForeignKey(f => f.ArticleId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Bank).HasColumnName("Bank").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.AccountNumber).HasColumnName("AccountNumber").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TypeOfAccount).HasColumnName("TypeOfAccount").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.ContactPerson).HasColumnName("ContactPerson").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.ContactNumber).HasColumnName("ContactNumber").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.CashInBankAccountId).HasColumnName("CashInBankAccountId").HasColumnType("int");
                entity.HasOne(f => f.MstAccount_CashInBankAccountId).WithMany(f => f.MstArticleBanks_CashInBankAccountId).HasForeignKey(f => f.CashInBankAccountId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
