using BusinessLogic.Models;

namespace Scheduler
{
    public class UrlBuilder
    {
        private string _city { get; set; }
        private string _type { get; set; }
        private string _regionId { get; set; }
        private string _subRegionId { get; set; }
        private string _cityId { get; set; }


        public UrlBuilder ForCity(City city)
        {
            switch(city)
            {
                case City.Warsaw:
                    ForWarsaw();
                    break;
                case City.Wroclaw:
                    ForWroclaw();
                    break;
            }

            return this;
        }
        private void ForWroclaw()
        {
            _city = "wroclaw";
            _regionId = "1";
            _subRegionId = "381";
            _cityId = "39";
        }

        private void ForWarsaw()
        {
            _city = "warszawa";
            _regionId = "7";
            _subRegionId = "197";
            _cityId = "26";
        }

        public UrlBuilder ForType(OfferType offerType)
        {
            switch(offerType)
            {
                case OfferType.Flat:
                    ForFlat();
                    break;
                case OfferType.House:
                    ForHouse();
                    break;
            }

            return this;
        }


        private void ForFlat()
        {
            _city = "mieszkanie";
        }

        private void ForHouse()
        {
            _city = "dom";
        }

        public string Build()
        {
            return $"https://www.otodom.pl/sprzedaz/{_type}/{_city}/?search%5Bcreated_since%5D=1&search%5Bregion_id%5D={_regionId}&search%5Bsubregion_id%5D={_subRegionId}&search%5Bcity_id%5D={_cityId}&nrAdsPerPage=72";
        }
    }
}