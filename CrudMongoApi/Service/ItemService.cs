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

        public async Task<Item?> UpdateAsync(string id, Item item)
        {
            // Garante que o ID do objeto a ser atualizado seja consistente com o filtro
            item.Id = id;

            // Realiza a substituição
            var result = await _itemsCollection.ReplaceOneAsync(i => i.Id == id, item);

            // Verifica se algum documento foi modificado
            if (result.ModifiedCount > 0)
            {
                // Retorna o item atualizado
                return await _itemsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
            }

            // Retorna null se nenhum documento foi encontrado ou modificado
            return null;
        }


        public async Task DeleteAsync(string id) => await _itemsCollection.DeleteOneAsync(i => i.Id == id);


    }
}