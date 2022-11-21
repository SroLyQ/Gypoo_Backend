using GypooWebAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GypooWebAPI.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Hotel> _hotelCollection;
        public readonly IMongoCollection<User> _userCollection;
        public readonly IMongoCollection<Room> _roomCollection;
        public readonly IMongoCollection<History> _historyCollection;
        public readonly IMongoCollection<Comment> _commentCollection;
        public readonly IMongoCollection<Promotion> _promotionCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _hotelCollection = database.GetCollection<Hotel>("Hotels");
            _userCollection = database.GetCollection<User>("Users");
            _roomCollection = database.GetCollection<Room>("Rooms");
            _historyCollection = database.GetCollection<History>("Historys");
            _commentCollection = database.GetCollection<Comment>("Comments");
            _promotionCollection = database.GetCollection<Promotion>("Promotion");
        }

    }
}

