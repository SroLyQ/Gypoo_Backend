using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace GypooWebAPI.Models
{
    public class Hotel{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get;set;}

        public string Name {get; set;} = null!;

        
        public List<string> roomIds {get; set;} = null!;
    }
}