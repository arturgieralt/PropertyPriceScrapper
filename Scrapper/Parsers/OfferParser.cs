using System.Collections.Generic;
using Scrapper.Models;
using HtmlAgilityPack;
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
                    Title = Get(node, TITLE_SELECTOR),
                    Area = Get(node, AREA_SELECTOR, "m²"),
                    PricePerUnit = Get(node, PRICE_PER_M_SELECTOR, "zł/m²"),
                    Price = Get(node, PRICE_SELECTOR, "zł"),
                    Url = Get(node, URL_SELECTOR),
                    OfferedBy = Get(node, OFFERED_BY_SELECTOR),
                    Location = Get(node, LOCATION_SELECTOR)
                };
                
                offers.Add(offer);
            }

            return offers;
        }

        private string Get(HtmlNode node, string selector) => node.SelectSingleNode(selector).InnerText;
        private decimal Get(HtmlNode node, string selector, string replaceText) 
        {
            decimal value;
            Decimal.TryParse(node.SelectSingleNode(selector).InnerText.Replace(replaceText, ""), out value);
            return value;
        }
        
    }
}