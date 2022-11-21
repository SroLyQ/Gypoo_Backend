using GypooWebAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GypooWebAPI.Services
{
    public class HistoryService
    {
        private MongoDBService _mongoDBSevice;
        private IMongoCollection<Room> _roomCollection;
        private IMongoCollection<History> _historyCollection;
        private IMongoCollection<Hotel> _hotelCollection;
        public HistoryService(MongoDBService mongoDBService)
        {
            _mongoDBSevice = mongoDBService;
            _roomCollection = _mongoDBSevice._roomCollection;
            _historyCollection = _mongoDBSevice._historyCollection;
            _hotelCollection = mongoDBService._hotelCollection;
        }

        public async Task CreateHistoryAsync(History history)
        {
            await _historyCollection.InsertOneAsync(history);
            return;
        }

        public async Task<List<History>> GetHistorysAsync()
        {
            return await _historyCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<History> GetHistoryByIdAsync(string id)
        {

            return await _historyCollection.Find(_history => _history._id == id).SingleAsync();

        }

        // public async Task<RoomAva> updateRoomAvaByIdAsync(string id, RoomAva roomAva){
        //     var update = Builders<RoomAva>.Update.Set("date", roomAva.date).Set("count", roomAva.count);
        //     Room result = await _roomCollection.FindOneAndUpdateAsync<Room>(_room => _room.idRoom == id, update);
        //     if (result == null)
        //     {
        //         return null!;
        //     }
        //     return result;
        // }

        // public async Task DeleteRoomAsync(string id)
        // {

        //     return;
        // }

        // public async Task<List<Room>> GetRoomByHotelId(string hotelId)
        // {
        //     List<Room> Rooms = await _roomCollection.Find(_Room => _Room.idHotel == hotelId).ToListAsync();
        //     return Rooms;
        // }

        // public async Task<List<Room>> GetRoomsAva()
        // {

        //     return await _roomCollection.Find(new BsonDocument()).ToListAsync();
        // }
    }
}