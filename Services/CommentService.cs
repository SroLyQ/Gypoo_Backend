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
        private IMongoCollection<User> _userCollection;
        private double rateCalculation(List<Comment> comments)
        {
            double rating = 0;
            foreach (var arrcomment in comments)
            {
                rating += arrcomment.rating;
            }
            rating /= comments.Count;
            rating = Math.Round(rating, 2);
            return rating;
        }
        public CommentService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
            _commentCollection = mongoDBService._commentCollection;
            _hotelCollection = mongoDBService._hotelCollection;
            _userCollection = mongoDBService._userCollection;
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
            if (comment.commentBy == "")
            {
                comment.usernameComment = "Guest";
            }
            else
            {
                User _user = await _userCollection.Find(_user => _user.Id == comment.commentBy).SingleAsync();
                comment.usernameComment = _user.username;
            }
            await _commentCollection.InsertOneAsync(comment);
            Comment _comment = comment;
            List<Comment> comments = await this.GetCommentsByHotelId(comment.commentOn);
            double rating = this.rateCalculation(comments);
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