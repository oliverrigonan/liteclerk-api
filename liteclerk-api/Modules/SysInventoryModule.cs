using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysInventoryModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysInventoryModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task UpdateArticleInventory(Int32 articleInventoryId)
        //{
        //    try
        //    {
        //        DBSets.TrnSalesInvoiceDBSet salesInvoice = await (
        //            from d in _dbContext.TrnSalesInvoices
        //            where d.Id == articleInventoryId
        //            select d
        //        ).FirstOrDefaultAsync();

        //        if (salesInvoice != null)
        //        {
                   
        //            await _dbContext.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public async Task InsertStockInInventory(Int32 INId)
        //{

        //}
    }
}
