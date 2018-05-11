using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace SEBeTender
{
    public class tenderDatabase
    {
        readonly SQLiteAsyncConnection database;

        public tenderDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<dbTenderItem>().Wait();
        }

        public Task<List<dbTenderItem>> getTendersAsync(int page)
        {
            return database.Table<dbTenderItem>().Where(i => i.Page == page).ToListAsync();
        }

        public Task<dbTenderItem> getTenderAsync(int id)
        {
            return database.Table<dbTenderItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveTendersasync(List<dbTenderItem> dbTenderItems)
        {
            return await database.InsertAllAsync(dbTenderItems);
        }

        public Task<int> DeleteTenderAsync(dbTenderItem item)
        {
            return database.DeleteAsync(item);
        }

        public Task<List<dbTenderItem>> keywordSearchTenders(string keyword)
        {
            return database.QueryAsync<dbTenderItem>("select * from dbTenderItem where Reference = ? or Title = ? or OriginatingStation = ? or CLosingDate = ? or BidClosingDate = ? or TendererClass = ? or Name = ?", keyword);
        }
        public void deleteAllTenders()
        {
            //return database.QueryAsync<dbTenderItem>("DELETE * FROM dbTenderItem");
            database.DropTableAsync<dbTenderItem>().Wait();
            database.CreateTableAsync<dbTenderItem>().Wait();

        }
    }
}
