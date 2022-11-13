using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace GypooWebAPI.Models
{
    public class UserNoPW
    {
        public static explicit operator UserNoPW(User obj)
        {
            return JsonConvert.DeserializeObject<UserNoPW>(JsonConvert.SerializeObject(obj));
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Username { get; set; } = null!;
        public string role { get; set; } = null!;
        public string token { get; set; } = null!;
    }
}