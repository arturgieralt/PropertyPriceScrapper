using System.Collections.Generic;
using DataAccess.Models;
using MongoDB.Driver;

namespace DataAccess
{
    public class DatabaseContext
    {
        private IMongoDatabase _database { get; }
        public DatabaseContext(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Offer> Offers {
            get {
                return _database.GetCollection<Offer>("Offer");
            }
        }
    }
}