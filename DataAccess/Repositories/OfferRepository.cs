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
            return await _context
                .Offers
                .Aggregate(CreatePipeline())
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

        private PipelineDefinition<Offer, AggregatedOffer> CreatePipeline()
        {
            var groupStage = new BsonDocument{ 
                { "$group", new BsonDocument{ 
                        { "_id", new BsonDocument{
                            {"Type", "$Type"},
                            {"City", "$City"},
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
                        { "AreaUnit", new BsonDocument{
                            { "$first", "$AreaUnit"} } 
                        },
                        { "PriceUnit", new BsonDocument{
                            { "$first", "$PriceUnit"} } 
                        },
                        { "Count", new BsonDocument{
                            { "$sum", 1}} 
                        }}
                }}; 

            var projectStage = new BsonDocument{ 
                { "$project", new BsonDocument{ 
                    {"AveragePrice", new BsonDocument{
                            { "$round", new BsonArray{"$AveragePrice", 2}} } 
                        },
                    {"AverageArea", new BsonDocument{
                            { "$round", new BsonArray{"$AverageArea", 2}} } 
                        },
                    {"AveragePricePerUnit", new BsonDocument{
                            { "$round", new BsonArray{new BsonDocument{
                            { "$divide", new BsonArray{"$AveragePrice", "$AverageArea"}} } , 2}} } 
                        },
                    {"Count", 1 },
                    {"AreaUnit", 1 },
                    {"PriceUnit", 1 },
                    {"CreatedOn", "$_id.DateAdded" },
                    {"City", "$_id.City"},
                    {"Type", "$_id.Type" },
                    {"_id", 0}
                }}
            };

            return new BsonDocument[]{
                groupStage,
                projectStage
            };
        }
    }
}