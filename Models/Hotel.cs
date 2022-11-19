using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GypooWebAPI.Models;
using System.Text.Json.Serialization;


namespace GypooWebAPI.Models
{
    public class Hotel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public bool? isAvailable { get; set; } = true;
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string address { get; set; } = null!;
        public string about { get; set; } = null!;
        public string mapURL { get; set; } = null!;
        public List<string> picture { get; set; } = null!;
        public double? price { get; set; } = 0;
        public float? discount { get; set; } = 0;
        public float? rating { get; set; } = 0;
        public int? review { get; set; } = 0;

        public List<Room>? room { get; set; } = null!;
        public List<Comment>? comments {get;set;} = null!;
    }
}