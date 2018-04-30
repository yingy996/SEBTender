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
            //database.CreateTableAsync<dbtenderItem>().Wait();
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
            
            //database.InsertAllAsync(tenders);
            try
            {
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
            } catch (Exception ex)
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
