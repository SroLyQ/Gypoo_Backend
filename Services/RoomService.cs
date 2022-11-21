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
        public async Task UpdateBooking(Room room) // อัพเดทวันเวลาที่จอง
        {

            var roomBooking = room.roomCount30Day;
            var nowDate = DateTime.Now.ToString("dd/MM/yyyy");
            int countRemove = 30;
            for (int i = 0; i < 30; i++)
            {
                var checkDate = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");
                int month = Int32.Parse(room.roomCount30Day[i].date.Split("/")[1]);
                int monthNow = Int32.Parse(checkDate.Split("/")[1]);
                int date = Int32.Parse(room.roomCount30Day[i].date.Split("/")[0]);
                int dateNow = Int32.Parse(checkDate.Split("/")[0]);
                // var date = room.roomCount30Day[i].date;
                // var dateNow = nowDate;
                Console.WriteLine("date = {0}", date);
                Console.WriteLine("dateNow = {0}", dateNow);
                Console.WriteLine("i = {0}", i);
                if (month < monthNow)
                {
                    room.roomCount30Day.RemoveAt(0);
                    countRemove--;
                    // Console.WriteLine("RoomBookingCheckMonth");
                }
                else if (date < dateNow)
                {
                    room.roomCount30Day.RemoveAt(0);
                    countRemove--;
                    // Console.WriteLine("RoomBookingCheckDate");
                }
            }
            for (int i = 0; i < 30 - countRemove; i++)
            {
                Console.WriteLine("----------------- Add Date ---------------------");
                // int lastDate = Int32.Parse(room.roomCount30Day[-1].date.Split("/")[0]); //วันสุดท้ายหลังเคลียวัน

                var lastDate = DateTime.Now.AddDays(countRemove + i).ToString("dd/MM/yyyy");//เพิ่มต่อจากวันสุดท้าย
                RoomAva nowAva = new RoomAva();
                nowAva.date = lastDate;
                nowAva.count = room.roomCount;
                room.roomCount30Day.Add(nowAva);
                Console.WriteLine("i = {0}", i);
                Console.WriteLine("lastDate = {0}", lastDate);
                // var nowDate = room.roomCount30Day.Add(countRemove).ToString("dd/MM/yyyy");

            }
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
            List<Room> rooms = await _roomCollection.Find(new BsonDocument()).ToListAsync();

            foreach (var room in rooms)
            {

                UpdateBooking(room);
                // RoomAva nowAva = new RoomAva();
                // var update = Builders<Room>.Update.Set("date", nowAva.date);
                // Room result = await _roomCollection.FindOneAndUpdateAsync<Room>(_room => _room.roomCount30Day == id, update);
                // var update = Builders<Room>.Update.Set("roomCount30Day", room.roomCount30Day);
                // Room newRoom = await _roomCollection.FindOneAndUpdateAsync(_room => _room.roomCount30Day == room.roomCount30Day, update);
            }

            return rooms;
            // return await _roomCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(string id)
        {
            // List<Room> rooms = await UpdateBooking();
            // List<Room> rooms = await _roomCollection.Find(new BsonDocument()).ToListAsync();
            // foreach (var room in rooms)
            // {
            //     var update = Builders<Room>.Update.Set("roomCount30Day", room.roomCount30Day);
            //     Room newRoom = await _roomCollection.FindOneAndUpdateAsync(_room => _room.roomCount30Day == room.roomCount30Day, update);
            // }

            var room = await _roomCollection.Find(_room => _room.idRoom == id).SingleAsync();
            UpdateBooking(room);
            return room;
            // return await _roomCollection.Find(_room => _room.idRoom == id).SingleAsync();

        }

        public async Task AddDetailToRoomAsync(string id, string roomId)
        {
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq("Id", id);
            UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet<string>("roomIds", roomId);
            await _roomCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task<Room> updateRoomByIdAsync(string id, Room room)
        {
            // var option = new FindOneAndUpdateOptions<Room, Room>
            // {
            //     IsUpsert = false,
            //     ReturnDocument = ReturnDocument.After
            // };
            var update = Builders<Room>.Update.Set("roomType", room.roomType).Set("guest", room.guest).Set("roomCount", room.roomCount).Set("currentRoom", room.currentRoom).Set("roomPrice", room.roomPrice).Set("service", room.service);
            Room result = await _roomCollection.FindOneAndUpdateAsync<Room>(_room => _room.idRoom == id, update);
            if (result == null)
            {
                return null!;
            }
            return result;
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