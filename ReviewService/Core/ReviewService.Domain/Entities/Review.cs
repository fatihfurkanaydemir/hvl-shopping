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
        public string Id { get; set; }
        public int ProductId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
    }
}
