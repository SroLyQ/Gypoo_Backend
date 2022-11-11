using GypooWebAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
namespace GypooWebAPI.Services
{
    public class HotelService
    {
        private MongoDBService _mongoDBSevice;
        private IMongoCollection<Hotel> _hotelCollection;
        public HotelService(MongoDBService mongoDBService)
        {
            _mongoDBSevice = mongoDBService;
            _hotelCollection = _mongoDBSevice._hotelCollection;
        }

        public async Task CreateAsync(Hotel hotel)
        {
            await _hotelCollection.InsertOneAsync(hotel);
            return;
        }

        public async Task<List<Hotel>> GetHotelsAsync()
        {
            List<Hotel> hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            return hotels;
        }

        public async Task<Hotel> GetHotelByIdAsync(string id)
        {
            Hotel hotel = await _hotelCollection.Find(_hotel => _hotel.Id == id).SingleAsync();
            return hotel;
        }

        public async Task AddRoomToHotelAsync(string id, string roomId)
        {
            FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("Id", id);
            UpdateDefinition<Hotel> update = Builders<Hotel>.Update.AddToSet<string>("roomIds", roomId);
            await _hotelCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}