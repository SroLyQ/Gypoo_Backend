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
        public string idHotel { get; set; } = null!;
        public string idRoom { get; set; } = null!;
        public string idUser { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string payment { get; set; } = null!;
        public string roomBooking { get; set; } = null!; // จำนวนห้องที่จะจอง
        public string dateBooking { get; set; } = null!; // วันเวลาที่จองทั้งหมด
        // public string checkin { get; set; } = null!;
        // public string checkout { get; set; } = null!;
        public string price { get; set; } = null!;


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