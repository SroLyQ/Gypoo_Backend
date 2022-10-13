using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;
using GypooWebAPI.Models;
namespace GypooWebAPI.Services
{
    public class UserService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<User> _userCollection;

        public UserService(MongoDBService mongoDBService)
        {
            _mongoDBservice = mongoDBService;
            _userCollection = _mongoDBservice._userCollection;
        }
        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<User> registerAsync(UserDTO user)
        {
            createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var _user = new User();

            _user.Username = user.Username;
            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = passwordSalt;
            await _userCollection.InsertOneAsync(_user);

            return _user;
        }
    }
}