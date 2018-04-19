using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SEBeTender
{
    public class tenderItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public tenderItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<tenderItem>().Wait();
        }

        public Task<List<tenderItem>> getTenderItems()
        {
            return database.Table<tenderItem>().ToListAsync();
        }

        public Task<tenderItem> getTenderItem(string refNo)
        {
            return database.Table<tenderItem>().Where(i => i.Reference == refNo).FirstOrDefaultAsync();
        }

        public Task<int> saveItem(tenderItem tender)
        {
            var count = database.Table<tenderItem>().CountAsync();
            if (count.Result != 0)
            {
                return database.UpdateAsync(tender);
            } else
            {
                return database.InsertAsync(tender);
            }
        }

        public Task<int> deleteItem(tenderItem tender)
        {
            return database.DeleteAsync(tender);
        }

    }
}
