using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.DTO_s
{
    public class ReviewDTO
    {

        public int ProductId { get; set; }
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
