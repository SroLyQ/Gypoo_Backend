using GypooWebAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GypooWebAPI.Services
{
    public class RoomService
    {
        private MongoDBService _mongoDBSevice;
        private IMongoCollection<Room> _roomCollection;
        public RoomService(MongoDBService mongoDBService)
        {
            _mongoDBSevice = mongoDBService;
            _roomCollection = _mongoDBSevice._roomCollection;
        }

        public async Task CreateRoomAsync(Room room)
        {
            await _roomCollection.InsertOneAsync(room);
            return;
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            return await _roomCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(string id)
        {
            return await _roomCollection.Find(_room => _room.idRoom == id).SingleAsync();
        }

        public async Task AddDetailToRoomAsync(string id, string roomId)
        {
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq("Id", id);
            UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet<string>("roomIds", roomId);
            await _roomCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}