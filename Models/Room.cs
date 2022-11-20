using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GypooWebAPI.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? idRoom { get; set; } = null!;
        public string idHotel { get; set; } = null!;
        public string roomType { get; set; } = null!;
        public int guest { get; set; } = 0!;
        public int roomCount { get; set; } = 0!;
        public int currentRoom { get; set; } = 0!;
        public List<string> picture { get; set; } = new List<string>();
        public float roomPrice { get; set; } = 0!;
        public int discount { get; set; } = 0!;

        // public List<string> facilities { get; set; } = null!;
        public List<RoomAva> roomCount30Day { get; set; } = new List<RoomAva>();
        public ServiceRoom service { get; set; } = null!;

        // public string comment { get; set; } = null!;
    }
    public class RoomAva
    {
        public string date { get; set; } = null!;
        public int count { get; set; } = 0;
    }
    public class ServiceRoom
    {

        public bool isWifi { get; set; } = false!;
        public bool isParking { get; set; } = false!;
        public bool isAnimals { get; set; } = false!;
        public bool isBreakFast { get; set; } = false!;
        public bool isBuffet { get; set; } = false!;
        public bool isOther { get; set; } = false!;

    }

    // public class Comment
    // {
    //     public string name { get; set; } = null!;
    //     public string roomType { get; set; } = null!;
    //     public string date { get; set; } = null!;
    //     public float? rating { get; set; } = 0!;
    //     public string content { get; set; } = null!;
    // }

}