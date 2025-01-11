using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudMongoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CrudMongoApi.Service
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _itemsCollection;

        public ItemService(IOptions<DatabaseSettings> dbSettings)
        {
            var client = new MongoClient(dbSettings.Value.ConnectionString);
            var database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _itemsCollection = database.GetCollection<Item>("Items");
        }

        public async Task<List<Item>> GetAllAsync() => await _itemsCollection.Find(_ => true).ToListAsync();

        public async Task<Item> GetByIdAsync(string id) => await _itemsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Item item) => await _itemsCollection.InsertOneAsync(item);

        public async Task UpdateAsync(string id, Item item) => await _itemsCollection.ReplaceOneAsync(i => i.Id == id, item);

        public async Task DeleteAsync(string id) => await _itemsCollection.DeleteOneAsync(i => i.Id == id);


    }
}