using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
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

        public async Task InsertManyAsync(IEnumerable<Offer> offers)
        {
            await _context
            .Offers
            .InsertManyAsync(offers)
            .ConfigureAwait(false);
        }
    }
}