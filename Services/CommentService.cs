using GypooWebAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
namespace GypooWebAPI.Services
{
    public class CommentService
    {
        private MongoDBService _mongoDBService;
        private IMongoCollection<Comment> _commentCollection;
        public CommentService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
            _commentCollection = mongoDBService._commentCollection;
        }

        public async Task<Comment> GetCommentAsync(string id)
        {
            Comment comment = await _commentCollection.Find(_comment => _comment.id == id).SingleAsync();
            return comment;
        }
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            DateTime nowDate = DateTime.Now;
            comment.date = nowDate.ToString("dd/MM/yyyy hh:mm tt");
            await _commentCollection.InsertOneAsync(comment);
            Comment _comment = comment;
            return _comment;
        }
        public async Task<List<Comment>> GetCommentsByHotelId(string hotelId)
        {
            List<Comment> comments = await _commentCollection.Find(_comment => _comment.commentOn == hotelId).ToListAsync();
            return comments;
        }
    }
}