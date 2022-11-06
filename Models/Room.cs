using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? idRoom { get; set; } = null!;

        // public string sideBed { get; set; } = null!;

        public float price { get; set; } = 0;

        public List<string> facilities { get; set; } = null!;

        public List<ServiceRoom> service { get; set; } = null!;

        public string comment { get; set; } = null!;
    }

    public class ServiceRoom
    {

        // public string sideBed { get; set; } = null!;
        public bool breakFast { get; set; } = false!;
        public bool roomCleaning { get; set; } = false!;

    }
    public class Comment
    {
        public string name { get; set; } = null!;
        public string roomType { get; set; } = null!;
        public string date { get; set; } = null!;
        public float? rating { get; set; } = 0!;
        public string content { get; set; } = null!;
    }

}