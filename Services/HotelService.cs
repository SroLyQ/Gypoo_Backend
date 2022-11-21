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
        private IMongoCollection<Room> _roomCollection;
        public HotelService(MongoDBService mongoDBService)
        {
            _mongoDBSevice = mongoDBService;
            _hotelCollection = _mongoDBSevice._hotelCollection;
            _roomCollection = _mongoDBSevice._roomCollection;
        }
        private async Task<List<Hotel>> updatePrice()
        {
            List<Hotel> hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var hotel in hotels)
            {
                List<Room> rooms = await _roomCollection.Find(_room => _room.idHotel == hotel.Id).ToListAsync();
                foreach (var room in rooms)
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
            List<Room> rooms = await _roomCollection.Find(_room => _room.idHotel == id).ToListAsync();
            foreach (var room in rooms)
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
        private async Task<List<Hotel>> findAvailable(string inDate, string outDate, List<Hotel> hotels)
        {
            List<Hotel> avaHotel = new List<Hotel>();
            string inDay = inDate.Split("-")[0];
            string outDay = outDate.Split("-")[0];
            int inDateInt = Int32.Parse(inDay);
            int outDateInt = Int32.Parse(outDay);
            foreach (var hotel in hotels)
            {
                List<Room> rooms = await _roomCollection.Find(_room => _room.idHotel == hotel.Id).ToListAsync();
                foreach (var room in rooms)
                {
                    Console.WriteLine(room.idRoom);
                    int startIndex = 0;
                    int stopIndex = 0;
                    List<RoomAva> room30Day = room.roomCount30Day;
                    for (int i = 0; i < 30; i++)
                    {
                        string nowAvaDate = room30Day[i].date;
                        string nowAvaDay = nowAvaDate.Split("/")[0];
                        int nowDayInt = Int32.Parse(nowAvaDay);
                        if (nowDayInt == inDateInt)
                        {
                            startIndex = i;
                        }
                        if (nowDayInt == outDateInt)
                        {
                            stopIndex = i;
                        }
                    }
                    bool foundEmpty = false;
                    for (int i = startIndex; i <= stopIndex; i++)
                    {
                        if (room30Day[i].count == 0)
                        {
                            foundEmpty = true;
                            break;
                        }
                    }
                    if (!foundEmpty)
                    {
                        avaHotel.Add(hotel);
                        break;
                    }
                }
            }
            return avaHotel;
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
        public async Task<List<Hotel>> GetHotelByTypeAsync(string type)
        {
            List<Hotel> hotels = new List<Hotel>();
            if (type == "hotel")
            {
                hotels = await updatePrice();
            }
            else if (type == "activity")
            {
                hotels = await _hotelCollection.Find(_hotel => _hotel.locationType.isTravel == true).ToListAsync();
            }
            else if (type == "restaurant")
            {
                hotels = await _hotelCollection.Find(_hotel => _hotel.locationType.isRestaurant == true).ToListAsync();
            }
            else
            {
                hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            }
            return hotels;
        }
        public async Task<Hotel> GetHotelByIdAsync(string id)
        {
            Console.WriteLine("hotel Found");
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

        public async Task AddRoomToHotelAsync(string id, string roomId)
        {
            FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("Id", id);
            Room room = await _roomCollection.Find(_room => _room.idRoom == roomId).SingleAsync();
            UpdateDefinition<Hotel> update = Builders<Hotel>.Update.AddToSet<Room>("room", room);
            await _hotelCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task<List<Hotel>> getHotelByOwnerId(string ownerID)
        {
            List<Hotel> myHotels = await _hotelCollection.Find(_hotel => _hotel.ownerID == ownerID).ToListAsync();
            return myHotels;
        }

        public async Task<List<Hotel>> getAvailableHotel(string checkInDate, string checkOutDate)
        {
            List<Hotel> hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            List<Hotel> avaHotels = await this.findAvailable(checkInDate, checkOutDate, hotels);
            return avaHotels;
        }
    }
}