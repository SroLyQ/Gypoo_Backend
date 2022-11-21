using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class History
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; } = null!; // id ของประวัติให้ที่พัก
        public int? index { get; set; } = 0;
        public double price { get; set; } = 0;
        public List<string> picture { get; set; } = new List<string>(); // Hotel Picture
        public string name { get; set; } = null!; // Hotel name
        public string type { get; set; } = null!; // type room
        public string checkin { get; set; } = null!;
        public string checkout { get; set; } = null!;


    }
    // public class FacilitiesRoom_History
    // {

    //     // public string sideBed { get; set; } = null!;
    //     public bool breakFast { get; set; } = false!;
    //     public bool roomCleaning { get; set; } = false!;

    // }
    // public class CommentRoom_History
    // {
    //     public string name { get; set; } = null!;
    //     public string roomType { get; set; } = null!;
    //     public string date { get; set; } = null!;
    //     public float? rating { get; set; } = 0!;
    //     public string content { get; set; } = null!;
    // }

}