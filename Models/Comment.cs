using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GypooWebAPI.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string content { get; set; } = null!;
        public string commentBy { get; set; } = null!;
        public string commentOn { get; set; } = null!;
        public double rating { get; set; } = 0.0;
        public string? date { get; set; } = null!;
    }

}