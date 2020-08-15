using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBContext
{
    public class LiteclerkDBContext : DbContext
    {
        public LiteclerkDBContext(DbContextOptions<LiteclerkDBContext> options) : base(options)
        {

        }

        public virtual DbSet<DBSets.MstUserDBSet> MstUsers { get; set; }
        public virtual DbSet<DBSets.MstCompanyDBSet> MstCompanies { get; set; }
        public virtual DbSet<DBSets.MstCompanyBranchDBSet> MstCompanyBranches { get; set; }
        public virtual DbSet<DBSets.MstCurrencyDBSet> MstCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DBModelBuilder.MstUserModelBuilder.CreateMstUserModel(modelBuilder);
            DBModelBuilder.MstCompanyModelBuilder.CreateMstCompanyModel(modelBuilder);
            DBModelBuilder.MstCompanyBranchModelBuilder.CreateMstCompanyBranchModel(modelBuilder);
            DBModelBuilder.MstCurrencyModelBuilder.CreateMstCurrencyModel(modelBuilder);
        }
    }
}
