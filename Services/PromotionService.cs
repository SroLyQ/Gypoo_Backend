using GypooWebAPI.Controllers;
using GypooWebAPI.Models;
using MongoDB.Driver;

namespace GypooWebAPI.Services
{
    public class PromotionService
    {
        private readonly MongoDBService _mongoDBService;
        private readonly IMongoCollection<Promotion> _promotionCollection;
        private readonly IMongoCollection<Hotel> _hotelCollection;
        public PromotionService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
            _promotionCollection = mongoDBService._promotionCollection;
            _hotelCollection = mongoDBService._hotelCollection;
        }
        public async Task<Promotion> createPromotion(Promotion promotion)
        {
            promotion.startDate = DateTime.Now.ToString("dd/MM/yyyy");
            Hotel hotel = await _hotelCollection.Find(_hotel => _hotel.Id == promotion.hotelId).SingleAsync();
            promotion.ownerID = hotel.ownerID;
            await _promotionCollection.InsertOneAsync(promotion);
            return promotion;
        }
        public async Task<List<Promotion>> getMyPromotions(string id)
        {
            List<Promotion> _promotions = await _promotionCollection.Find(_promotion => _promotion.ownerID == id).ToListAsync();
            List<string> deletePromo = new List<string> { };
            foreach (var promotion in _promotions)
            {
                var endDate = DateTime.ParseExact(promotion.endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var nowDate = DateTime.Now;
                Console.WriteLine(nowDate.CompareTo(endDate));
                if (nowDate.CompareTo(endDate) > 0)
                {
                    deletePromo.Add(promotion.id);
                }
            }
            foreach (var promoId in deletePromo)
            {
                await _promotionCollection.DeleteOneAsync(_promo => _promo.id == promoId);
            }
            List<Promotion> __promotions = await _promotionCollection.Find(_promotion => _promotion.ownerID == id).ToListAsync();
            return __promotions;
        }
    }
}