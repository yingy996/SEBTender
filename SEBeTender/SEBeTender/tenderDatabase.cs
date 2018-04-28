using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SEBeTender
{
    public class tenderDatabase
    {
        readonly SQLiteAsyncConnection database;

        public tenderDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<dbtenderItem>().Wait();
        }

        public Task<List<dbtenderItem>> getTendersAsync()
        {
            return database.Table<dbtenderItem>().ToListAsync();
        }

        public Task<dbtenderItem> getTenderAsync(string reference)
        {
            return database.Table<dbtenderItem>().Where(i => i.Reference == reference).FirstOrDefaultAsync();
        }

        public Task<int> saveTendersAsync(List<dbtenderItem> tenders)
        {
            return database.InsertAllAsync(tenders);
        }

        public void deleteTendersAsync()
        {
            database.DropTableAsync<dbtenderItem>().Wait();

        }

        
    }
}
