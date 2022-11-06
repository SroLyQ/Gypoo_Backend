using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class RoomHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; } = null!;
        public int index { get; set; } = 0;
        public bool isAviable { get; set; } = false;
        public float price { get; set; } = 0;
        public string picture { get; set; } = null!;
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string address { get; set; } = null!;
        public float latitude { get; set; } = 0;
        public float longitude { get; set; } = 0;
        public float discount { get; set; } = 0;
        public List<FacilitiesRoom_History> facilities { get; set; } = null!;
        public float rating { get; set; } = 0;
        public string reviews { get; set; } = null!;
        public string type { get; set; } = null!;
        public string period { get; set; } = null!;

        public List<CommentRoom_History> comment { get; set; } = null!;
    }
    public class FacilitiesRoom_History
    {

        // public string sideBed { get; set; } = null!;
        public bool breakFast { get; set; } = false!;
        public bool roomCleaning { get; set; } = false!;

    }
    public class CommentRoom_History
    {
        public string name { get; set; } = null!;
        public string roomType { get; set; } = null!;
        public string date { get; set; } = null!;
        public float? rating { get; set; } = 0!;
        public string content { get; set; } = null!;
    }

}