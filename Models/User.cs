using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Username { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;

        public byte[]? PasswordSalt { get; set; }

        public string? token { get; set; } = null!;
        public string role { get; set; } = null!;
    }
}