using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models
{
    public class AggregatedOffer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public double AverageArea { get; set; }
        public string AreaUnit { get; set; }
        public string PriceUnit {get; set; }
        public double AveragePricePerUnit {get; set; }
        public string Type { get; set; }
        public string City { get; set ;}
        public double AveragePrice { get; set; }
        public string CreatedOn {get; set; }
        public int Count {get; set; }

    }
}