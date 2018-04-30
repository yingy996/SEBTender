﻿using System;
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

        public Task<List<dbTenderItem>> getTendersAsync()
        {
            return database.Table<dbTenderItem>().ToListAsync();
        }

        public Task<dbTenderItem> getTenderAsync(int id)
        {
            return database.Table<dbTenderItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        /*public Task<int> SaveTenderAsync(dbTenderItem item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }*/

        /*public Task<int> SaveTenderAsync(List<dbTenderItem> dbTenderItems)
        {
            if (dbTenderItems != null)
            {
                foreach (dbTenderItem item in dbTenderItems)
                {
                    if (item.Id != 0)
                    {
                        return database.UpdateAsync(item);
                    }
                    else
                    {
                        return database.InsertAsync(item);
                    }
                }

            }
            else
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }*/

            public Task<int> SaveTendersasync(List<dbTenderItem> dbTenderItems)
        {
            return database.InsertAllAsync(dbTenderItems);
        }

        

        public Task<int> DeleteTenderAsync(dbTenderItem item)
        {
            return database.DeleteAsync(item);
        }
    }
}
