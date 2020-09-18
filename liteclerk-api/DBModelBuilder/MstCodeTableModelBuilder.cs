using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBModelBuilder
{
    public class MstCodeTableModelBuilder
    {
        public static void CreateMstCodeTableModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBSets.MstCodeTableDBSet>(entity =>
            {
                entity.ToTable("MstCodeTable");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Code).HasColumnName("Code").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.CodeValue).HasColumnName("CodeValue").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Category).HasColumnName("Category").HasColumnType("nvarchar(50)").HasMaxLength(50).IsRequired();
            });
        }
    }
}
