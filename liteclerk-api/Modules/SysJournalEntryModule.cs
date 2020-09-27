using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public class SysJournalEntryModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;

        public SysJournalEntryModule(DBContext.LiteclerkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertInventoryJournalEntry(Int32 ILId)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertSalesInvoiceJournalEntry(Int32 SIId)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task InsertCollectionJournalEntry(Int32 SIId)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
