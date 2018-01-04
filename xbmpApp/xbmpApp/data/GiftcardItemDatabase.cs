using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using xbmpApp.model;

namespace xbmpApp
{
    public class GiftcardItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public GiftcardItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<GiftcardItem>().Wait();
        }

        public Task<List<GiftcardItem>> GetItemsAsync()
        {
            return database.Table<GiftcardItem>().ToListAsync();
        }

        public Task<GiftcardItem> GetItemAsync(int id)
        {
            return database.Table<GiftcardItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<GiftcardItem> GetItemAsync(string code)
        {
            return database.Table<GiftcardItem>().Where(i => i.Code == code).FirstOrDefaultAsync();

        }

        public Task<int> SaveItemAsync(GiftcardItem item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(GiftcardItem item)
        {
            return database.DeleteAsync(item);
        }
    }
}
