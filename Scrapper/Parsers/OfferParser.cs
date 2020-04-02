using System.Collections.Generic;
using HtmlAgilityPack;
using BusinessLogic.Models;
using System;

namespace Scrapper.Parsers
{
    public class OfferParser
    {
        const string OFFER_SELECTOR = "//article[contains(@data-featured-name, 'listing_no_promo')]";
        const string PRICE_SELECTOR = ".//li[contains(@class, 'offer-item-price')]";
        const string AREA_SELECTOR = ".//li[contains(@class, 'offer-item-area')]";
        const string PRICE_PER_M_SELECTOR = ".//li[contains(@class, 'offer-item-price-per-m')]";
        const string TITLE_SELECTOR = ".//span[contains(@class, 'offer-item-title')]";
        const string URL_SELECTOR = ".//a[contains(@data-featured-name, 'listing_no_promo')]";
        const string OFFERED_BY_SELECTOR = ".//div[contains(@class, 'offer-item-details-bottom')]//li";
        const string LOCATION_SELECTOR = ".//header[contains(@class, 'offer-item-header')]//p";

        public IEnumerable<Offer> GetOffers(HtmlDocument document)
        {
            var offers = new List<Offer>();
            var offerNodes = document.DocumentNode.SelectNodes(OFFER_SELECTOR);
            foreach(var node in offerNodes)
            {
                var offer = new Offer()
                {
                    Title = node.GetString(TITLE_SELECTOR),
                    Area = node.GetDouble(AREA_SELECTOR),
                    AreaUnit = AreaUnit.SquareMeter,
                    PricePerUnit = node.GetDouble(PRICE_PER_M_SELECTOR),
                    Price = node.GetDouble(PRICE_SELECTOR),
                    Url = node.GetString(URL_SELECTOR),
                    OfferedBy = node.GetString(OFFERED_BY_SELECTOR),
                    Location = node.GetString(LOCATION_SELECTOR)
                };
                Console.WriteLine($"priceperunit: {offer.PricePerUnit}");
                offers.Add(offer);
            }

            return offers;
        }


        
    }
}