using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models
{
    public enum AreaUnit {
        SquareMeter
    }

    public class Offer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Title { get; set; }
        public double Area { get; set; }
        public string AreaUnit { get; set; }
        public double PricePerUnit { get; set; }
        public double Price { get; set; }

        public string Url { get; set; }
        public string Location { get; set; }
        public string OfferedBy {get; set; }
    }
}