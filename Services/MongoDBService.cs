using GypooWebAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GypooWebAPI.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Hotel> _hotelCollection;
        public readonly IMongoCollection<User> _userCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _hotelCollection = database.GetCollection<Hotel>("Hotels");
            _userCollection = database.GetCollection<User>("Users");

        }

    }
}

