using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReviewService.Application.DTO_s;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Infrastructure.Persistence
{
    public class ReviewServices
    {
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<ReviewDTO> _reviewCollectionDTO;

        public ReviewServices(
            IOptions<ReviewDatabaseSettings> reviewDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                reviewDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
            reviewDatabaseSettings.Value.DatabaseName);

            _reviewCollection = mongoDatabase.GetCollection<Review>(
                reviewDatabaseSettings.Value.ReviewCollectionName);

            _reviewCollectionDTO = mongoDatabase.GetCollection<ReviewDTO>(
            reviewDatabaseSettings.Value.ReviewCollectionName);
        }

        public async Task<List<Review>> GetAsync() =>
            await _reviewCollection.Find(_ => true).ToListAsync();

        public async Task<Review> GetAsync(string id) =>
            await _reviewCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ReviewDTO newComment) =>
            await _reviewCollectionDTO.InsertOneAsync(newComment);

        public async Task UpdateAsync(string id, Review updatedComment) =>
            await _reviewCollection.ReplaceOneAsync(x => x.Id == id, updatedComment);

        public async Task RemoveAsync(string id) =>
            await _reviewCollection.DeleteOneAsync(x => x.Id == id);
    }
}
