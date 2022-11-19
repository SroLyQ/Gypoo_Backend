using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GypooWebAPI.Models
{
    public class Promotion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; } = null!;
        public string hotelId {get; set;} = null!;
        public string title { get; set; } = null!;
        public int percent { get; set; } = 0;
        public string description { get; set; } = null!;
        public string? startDate { get; set; } = null!;
        public string endDate { get; set; } = null!;
        public string? ownerID {get;set;} = null!;
    }
}