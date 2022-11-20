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
        private IMongoCollection<Hotel> _hotelCollection;
        public RoomService(MongoDBService mongoDBService)
        {
            _mongoDBSevice = mongoDBService;
            _roomCollection = _mongoDBSevice._roomCollection;
            _hotelCollection = mongoDBService._hotelCollection;
        }

        public async Task CreateRoomAsync(Room room)
        {
            room.roomCount30Day = new List<RoomAva>();
            for (int i = 0; i < 30; i++)
            {
                var nowDate = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");
                RoomAva nowAva = new RoomAva();
                nowAva.date = nowDate;
                nowAva.count = room.roomCount;
                room.roomCount30Day.Add(nowAva);
            }
            await _roomCollection.InsertOneAsync(room);
            // // ตอนสร้าง room แล้วให้เพิ่ม hotel ลงใน room
            UpdateDefinition<Hotel> update = Builders<Hotel>.Update.AddToSet("room", room);
            await _hotelCollection.UpdateOneAsync(_hotel => _hotel.Id == room.idHotel, update);
        }

        private async Task<List<Room>> updateDiscount() //ยังไม่ได้ใช้
        {
            // Room room = await _roomCollection.Find(_room => _room.idRoom == id).SingleAsync();
            // Hotel hotel = await _hotelCollection.Find(_hotel => _hotel.Id == id).SingleAsync();
            List<Room> rooms = await _roomCollection.Find(new BsonDocument()).ToListAsync();
            List<Hotel> hotels = await _hotelCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var hotel in hotels)
            {
                var update = Builders<Room>.Update.Set("discount", hotel.discount);
                Room newRoom = await _roomCollection.FindOneAndUpdateAsync(_room => _room.idRoom == _room.idRoom, update);
            }
            return rooms;
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            // List<Room> rooms = await updateDiscount();
            // return rooms;
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