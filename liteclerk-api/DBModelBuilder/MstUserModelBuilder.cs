using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstUserModelBuilder
    {
        public static void CreateMstUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstUserDBSet>(entity =>
            {
                entity.ToTable("MstUser");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasColumnName("Username").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Fullname).HasColumnName("Fullname").HasColumnType("nvarchar(100)").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId").HasColumnType("int");
                entity.HasOne(f => f.Company).WithMany(f => f.MstUsers_Company).HasForeignKey(f => f.CompanyId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int");
                entity.HasOne(f => f.CompanyBranch).WithMany(f => f.MstUsers_CompanyBranch).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
