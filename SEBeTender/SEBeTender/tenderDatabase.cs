using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace SEBeTender
{
    public class tenderDatabase
    {
        readonly SQLiteAsyncConnection database;

        public tenderDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            if (Device.RuntimePlatform == Device.iOS) {
                deleteTendersAsync();
                database.CreateTableAsync<dbtenderItem>().Wait();
            } else
            {
                deleteTendersAsync();
                try
                {
                    database.CreateTableAsync<dbtenderItem>().Wait();
                } catch (Exception ex)
                {
                    Console.WriteLine("Create table error: " + ex);
                }
                //database.ExecuteAsync("CREATE TABLE dbtenderItem(id integer primary key autoincrement not null, Reference varchar, Title varchar, OriginatingStation varchar, ClosingDate varchar, BidClosingDate varchar, FeeBeforeGST varchar, FeeAfterGST varchar, FeeGST varchar, TendererClass varchar, Name varchar, OffinePhone varchar, Extension varchar, MobilePhone varchar, Email varchar, Fax varchar, jsonfileLinks varchar, CheckedValue varchar, AddToCartQuantity varchar, BookmarkImage varchar)").Wait();
                //database.CreateTableAsync<dbtenderItem>().Wait();
            }
            //database.CreateTableAsync<dbtenderItem>().Wait();




        }

        public void createTableAsync()
        {
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

        public void saveTendersAsync(List<dbtenderItem> tenders)
        {
            
            
            try
            {
                Console.WriteLine("Im testing here");
                //database.InsertAllAsync(tenders);
                foreach (dbtenderItem item in tenders)
                {
                    if (getTenderAsync(item.Reference).Result != null)
                    {
                        Console.WriteLine("Need to update item to db yo");
                        database.UpdateAsync(item);
                    }
                    else
                    {
                        Console.WriteLine("Need to insert item to db yo");

                        if (database != null)
                        {
                            Console.WriteLine("Database is not null, insert item!");
                            if (item != null)
                            {
                                Console.WriteLine("Item not null! Inserting");
                                if (database.Table<dbtenderItem>() != null)
                                {
                                    Console.WriteLine("table is not null! Inserting");
                                    database.InsertAsync(item);
                                }

                            }

                        }
                        else
                        {
                            Console.WriteLine("Database is null, not inserting item!");
                        }
                        //database.InsertAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs while saving tender into db: " + ex);
            }
            
        }

        public void deleteTendersAsync()
        {
            database.DropTableAsync<dbtenderItem>().Wait();
        }

        
    }
}
