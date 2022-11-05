using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;


namespace GypooWebAPI.Models
{
    public class Hotel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int index { get; set; } = 0;
        public bool isAvailable { get; set; } = true;
        public List<string> picture { get; set; } = null!;
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string address { get; set; } = null!;
        public string about { get; set; } = null!;

        public string latitude { get; set; } = null!;
        public string longtitude { get; set; } = null!;
        public float discount { get; set; } = 0;
        public List<string> facilities { get; set; } = null!;

        public float rating { get; set; } = 0;

        public int review { get; set; } = 0;
        public List<string> comment { get; set; } = null!;
        public List<string> roomIds { get; set; } = null!;
    }
}