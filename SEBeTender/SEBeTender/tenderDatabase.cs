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
            string dbkeyword = "%" + keyword + "%";
            /*return database.QueryAsync<dbTenderItem>("select * from dbTenderItem where Reference like '" + dbkeyword + "' or Title like '" + dbkeyword + "' or OriginatingStation like '" + dbkeyword + "' or CLosingDate like '" + dbkeyword + "' or BidClosingDate like '" + dbkeyword + "' or TendererClass like '" + dbkeyword + "' or Name like '" + dbkeyword + "'");*/
            return database.QueryAsync<dbTenderItem>("select * from dbTenderItem where Reference like '" + dbkeyword + "' or Title like '" + dbkeyword + "' or Category like '" + dbkeyword + "' or originatingSource like '" + dbkeyword + "' or Agency like '" + dbkeyword + "'");


            
            //return database.QueryAsync<dbTenderItem>("select * from dbTenderItem where Title like '%" + keyword + "%'");
            //return database.Table<dbTenderItem>().Where(i => i.Title.ToLower().Contains(dbkeyword)).ToListAsync();

        }
        public void deleteAllTenders()
        {
            //return database.QueryAsync<dbTenderItem>("DELETE * FROM dbTenderItem");
            database.DropTableAsync<dbTenderItem>().Wait();
            database.CreateTableAsync<dbTenderItem>().Wait();

        }
    }
}
