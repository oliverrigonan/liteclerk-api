using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstUserBranchModelBuilder
    {
        public static void CreateMstUserBranchModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstUserBranchDBSet>(entity =>
            {
                entity.ToTable("MstUserBranch");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UserId).WithMany(f => f.MstUserBranches_UserId).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.BranchId).HasColumnName("BranchId").HasColumnType("int");
                entity.HasOne(f => f.MstCompanyBranch_BranchId).WithMany(f => f.MstUserBranches_BranchId).HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
