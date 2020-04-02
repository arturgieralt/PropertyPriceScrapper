using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseOfferModels = DataAccess.Models;
using ScrapperOfferModels = Scrapper.Models;
using DataAccess.Repositories;
using System.Linq;
using System;

namespace BusinessLogic.Services
{
    public class OfferService
    {
        private OfferRepository _repository { get; }
        public OfferService(OfferRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DatabaseOfferModels.Offer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task InsertManyAsync(IEnumerable<ScrapperOfferModels.Offer> offers)
        {
            var offersToInsert = offers.Select(o => new DatabaseOfferModels.Offer()
            {
                Title = o.Title,
                Area = o.Area,
                AreaUnit = Enum.GetName(typeof(ScrapperOfferModels.AreaUnit), o.AreaUnit),
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