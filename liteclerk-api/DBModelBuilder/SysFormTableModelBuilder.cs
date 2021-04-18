using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysFormTableModelBuilder
    {
        public static void CreateSysFormTableModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysFormTableDBSet>(entity =>
            {
                entity.ToTable("SysFormTable");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FormId).HasColumnName("FormId").HasColumnType("int");
                entity.HasOne(f => f.SysForm_FormId).WithMany(f => f.SysFormTables_FormId).HasForeignKey(f => f.FormId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Table).HasColumnName("Table").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
            });
        }
    }
}
