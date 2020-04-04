using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repositories
{
    public class OfferRepository
    {
        private DatabaseContext _context;
        public OfferRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetAllAsync()
        {
            return await _context
                .Offers
                .FindSync(FilterDefinition<Offer>.Empty)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<AggregatedOffer>> GetAllAggregatedAsync()
        {

            // var parseDate = new BsonDocument{ 
            //     { "$project", new BsonDocument{ 
            //         {"DateAdded", 
            //         }}
            //     }}; 

            var groupByDate = new BsonDocument{ 
                { "$group", new BsonDocument{ 
                        { "_id", new BsonDocument{
                            {"Type", "$Type"},
                            {"City", "$City"},
                            {"Location", "$Location"},
                            {"DateAdded", new BsonDocument{
                                {"$dateToString",new BsonDocument{
                                    { "format", "%Y-%m-%d"}, {"date", "$CreatedOn"} } 
                                }}
                            }}
                        },
                        { "AveragePrice", new BsonDocument{
                            { "$avg", "$Price"} } 
                        },
                        { "AverageArea", new BsonDocument{
                            { "$avg", "$Area"}} 
                        },
                        { "Count", new BsonDocument{
                            { "$sum", 1}} 
                        }}
                }}; 

            var chooseProperties = new BsonDocument{ 
                { "$project", new BsonDocument{ 
                    {"AveragePrice", 1 },
                    {"AverageArea", 1 },
                    {"AveragePricePerUnit", new BsonDocument{
                            { "$divide", new BsonArray{"$AveragePrice", "$AverageArea"}} } 
                        } ,
                    {"Count", 1 },
                    {"CreatedOn", "$_id.DateAdded" },
                    {"City", "$_id.City"},
                    {"Location", "$_id.Location"},
                    {"Type", "$_id.Type" },
                    {"_id", 0}
                }}
            };

            PipelineDefinition<Offer, AggregatedOffer> pipeline = new BsonDocument[]{
                groupByDate,
                chooseProperties
            };

            return await _context
                .Offers
                .Aggregate(pipeline)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task InsertManyAsync(IEnumerable<Offer> offers)
        {
            await _context
            .Offers
            .InsertManyAsync(offers)
            .ConfigureAwait(false);
        }
    }
}