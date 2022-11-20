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
        public string ownerID { get; set; } = null!;
        public LocationType locationType { get; set; } = null!;
        public List<string> picture { get; set; } = new List<string>();
        public double? price { get; set; } = 0;
        public float? discount { get; set; } = 0;
        public float? rating { get; set; } = 0;
        public int? review { get; set; } = 0;
        public List<Room>? room { get; set; } = new List<Room>();
        public List<Comment>? comments { get; set; } = new List<Comment>();
    }
    public class LocationType
    {
        public Boolean isHotel { get; set; } = false;
        public Boolean isRestaurant { get; set; } = false;
        public Boolean isTravel { get; set; } = false;
    }
}