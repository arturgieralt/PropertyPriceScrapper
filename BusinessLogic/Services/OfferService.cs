using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseOfferModels = DataAccess.Models;
using DataAccess.Repositories;
using System.Linq;
using System;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public class OfferService
    {
        private OfferRepository _repository { get; }
        public OfferService(OfferRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Offer>> GetAllAsync()
        {
            var offers = await _repository.GetAllAsync().ConfigureAwait(false);

            return offers.Select(o => new Offer()
            {
                Title = o.Title,
                Area = o.Area,
                AreaUnit = (AreaUnit)Enum.Parse(typeof(AreaUnit), o.AreaUnit),
                Price = o.Price,
                PricePerUnit = o.PricePerUnit,
                OfferedBy = o.OfferedBy,
                Location = o.Location,
                Url = o.Url
            });
        }

        public async Task InsertManyAsync(IEnumerable<Offer> offers)
        {
            var offersToInsert = offers.Select(o => new DatabaseOfferModels.Offer()
            {
                Title = o.Title,
                Area = o.Area,
                AreaUnit = Enum.GetName(typeof(AreaUnit), o.AreaUnit),
                Price = o.Price,
                PricePerUnit = o.PricePerUnit,
                OfferedBy = o.OfferedBy,
                Location = o.Location,
                Url = o.Url
            });

            await _repository.InsertManyAsync(offersToInsert);
        }
    }
}