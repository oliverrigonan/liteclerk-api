using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstUserFormModelBuilder
    {
        public static void CreateMstUserFormModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstUserFormDBSet>(entity =>
            {
                entity.ToTable("MstUserForm");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("int").IsRequired();
                entity.HasOne(f => f.MstUser_UserId).WithMany(f => f.MstUserForms_UserId).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.FormId).HasColumnName("FormId").HasColumnType("int");
                entity.HasOne(f => f.SysForm_FormId).WithMany(f => f.MstUserForms_FormId).HasForeignKey(f => f.FormId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CanAdd).HasColumnName("CanAdd").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanEdit).HasColumnName("CanEdit").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanDelete).HasColumnName("CanDelete").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanLock).HasColumnName("CanLock").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanUnlock).HasColumnName("CanUnlock").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanCancel).HasColumnName("CanCancel").HasColumnType("bit").IsRequired();
                entity.Property(e => e.CanPrint).HasColumnName("CanPrint").HasColumnType("bit").IsRequired();
            });
        }
    }
}
