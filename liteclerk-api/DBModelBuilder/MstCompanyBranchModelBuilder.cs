using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstCompanyBranchModelBuilder
    {
        public static void CreateMstCompanyBranchModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstCompanyBranchDBSet>(entity =>
            {
                entity.ToTable("MstCompanyBranch");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BranchCode).HasColumnName("BranchCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ManualCode).HasColumnName("ManualCode").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstCompany_CompanyId).WithMany(f => f.MstCompanyBranches_CompanyId).HasForeignKey(f => f.CompanyId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Branch).HasColumnName("Branch").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(max)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TIN).HasColumnName("TIN").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
            });
        }
    }
}
