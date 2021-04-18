using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysFormTableColumnModelBuilder
    {
        public static void CreateSysFormTableColumnModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysFormTableColumnDBSet>(entity =>
            {
                entity.ToTable("SysFormTableColumn");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TableId).HasColumnName("TableId").HasColumnType("int");
                entity.HasOne(f => f.SysFormTable_TableId).WithMany(f => f.SysFormTableColumns_TableId).HasForeignKey(f => f.TableId).OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Column).HasColumnName("Column").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IsDisplayed).HasColumnName("IsDisplayed").HasColumnType("bit").IsRequired();
            });
        }
    }
}
