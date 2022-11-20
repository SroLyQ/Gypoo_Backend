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
        private async Task<List<Hotel>> updatePrice()
        {
            List<Hotel> hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var hotel in hotels)
            {
                foreach (var room in hotel.room)
                {
                    if (hotel.price == 0)
                    {
                        hotel.price = room.roomPrice;
                    }
                    if (room.roomPrice < hotel.price)
                    {
                        hotel.price = room.roomPrice;
                    }
                }
                var update = Builders<Hotel>.Update.Set("price", hotel.price);
                Hotel newhotel = await _hotelCollection.FindOneAndUpdateAsync(_hotel => _hotel.Id == hotel.Id, update);
            }
            return hotels;
        }
        private async Task<Hotel> updateOnePrice(string id)
        {
            Hotel hotel = await _hotelCollection.Find(_hotel => _hotel.Id == id).SingleAsync();
            foreach (var room in hotel.room)
            {
                if (hotel.price == 0)
                {
                    hotel.price = room.roomPrice;
                }
                if (room.roomPrice < hotel.price)
                {
                    hotel.price = room.roomPrice;
                }
            }
            var update = Builders<Hotel>.Update.Set("price", hotel.price);
            Hotel newHotel = await _hotelCollection.FindOneAndUpdateAsync(_hotel => _hotel.Id == id, update);
            return hotel;
        }
        public async Task CreateAsync(Hotel hotel)
        {
            await _hotelCollection.InsertOneAsync(hotel);
            return;
        }

        public async Task<List<Hotel>> GetHotelsAsync()
        {
            List<Hotel> hotels = await updatePrice();
            return hotels;
        }

        public async Task<Hotel> GetHotelByIdAsync(string id)
        {
            Hotel hotel = await _hotelCollection.Find(_hotel => _hotel.Id == id).SingleAsync();
            Hotel res = await updateOnePrice(id);
            return res;
        }

        public async Task<Hotel> updateHotelByIdAsync(string id, Hotel hotel)
        {
            var option = new FindOneAndUpdateOptions<Hotel, Hotel>
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            var update = Builders<Hotel>.Update.Set("name", hotel.name).Set("email", hotel.email).Set("phone", hotel.phone).Set("address", hotel.address).Set("about", hotel.about).Set("mapURL", hotel.mapURL).Set("ownerID", hotel.ownerID).Set("locationType", hotel.locationType);
            Hotel result = await _hotelCollection.FindOneAndUpdateAsync<Hotel>(_hotel => _hotel.Id == id, update, option);
            if (result == null)
            {
                return null!;
            }
            return result;
        }

        public async Task AddRoomToHotelAsync(string id, Room room)
        {
            FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("Id", id);
            UpdateDefinition<Hotel> update = Builders<Hotel>.Update.AddToSet<Room>("room", room);
            await _hotelCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task<List<Hotel>> getHotelByOwnerId(string ownerID)
        {
            List<Hotel> myHotels = await _hotelCollection.Find(_hotel => _hotel.ownerID == ownerID).ToListAsync();
            return myHotels;
        }


    }
}