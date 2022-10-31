using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? idRoom { get; set; } = null!;
        public string sideBed { get; set; } = null!;

        public float price { get; set; } = 0;

        public List<string> facilities { get; set; } = null!;

        public List<string> service { get; set; } = null!;

        public string comment { get; set; } = null!;
    }
}