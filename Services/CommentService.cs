using GypooWebAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
namespace GypooWebAPI.Services
{
    public class CommentService
    {
        private MongoDBService _mongoDBService;
        private IMongoCollection<Comment> _commentCollection;
        private IMongoCollection<Hotel> _hotelCollection;
        public CommentService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
            _commentCollection = mongoDBService._commentCollection;
            _hotelCollection = mongoDBService._hotelCollection;
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
            List<Comment> comments = await this.GetCommentsByHotelId(comment.commentOn); double rating = 0;
            foreach (var arrcomment in comments)
            {
                rating += arrcomment.rating;
            }
            rating /= comments.Count;
            rating = Math.Round(rating, 2);
            UpdateDefinition<Hotel> update = Builders<Hotel>.Update.Set<double>("rating", rating);
            await _hotelCollection.UpdateOneAsync(_hotel => _hotel.Id == comment.commentOn, update);
            return _comment;
        }
        public async Task<List<Comment>> GetCommentsByHotelId(string hotelId)
        {
            List<Comment> comments = await _commentCollection.Find(_comment => _comment.commentOn == hotelId).ToListAsync();
            return comments;
        }
    }
}