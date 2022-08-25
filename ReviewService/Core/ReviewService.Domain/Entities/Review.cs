using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Domain.Entities
{
    public class Review
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("Date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [BsonElement("ProductId")]
        public int ProductId { get; set; }
        [BsonElement("CustomerIdentityId")]
        public string CustomerIdentityId { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("Comment")]
        public string Comment { get; set; }
        [BsonElement("Rate")]
        public int Rate { get; set; }
    }
}
