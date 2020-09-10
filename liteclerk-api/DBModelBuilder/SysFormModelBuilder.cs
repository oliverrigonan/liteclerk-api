using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class SysFormModelBuilder
    {
        public static void CreateSysFormModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.SysFormDBSet>(entity =>
            {
                entity.ToTable("SysForm");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Form).HasColumnName("Form").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
            });
        }
    }
}
