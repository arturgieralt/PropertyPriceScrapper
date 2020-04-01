using System.Collections.Generic;
using Scrapper.Models;
using HtmlAgilityPack;
using System;

namespace Scrapper.Parsers
{
    public class OfferParser
    {
        public IEnumerable<Offer> GetOffers(HtmlDocument document)
        {
            var offers = new List<Offer>();
            var offerNodes = document.DocumentNode.SelectNodes("//article[contains(@data-featured-name, 'listing_no_promo')]");
            foreach(var node in offerNodes)
            {
                // chain of responsibility pattern
                var price = GetPrice(node);
                var area = GetArea(node);
                var pricePerUnit = GerPricePerUnit(node);
                var title = GetTitle(node);
                var url = GetUrl(node);
                var offeredBy = GetOfferedBy(node);
                var location = GetLocation(node);

                var offer = new Offer()
                {
                    Title = title,
                    Area = area,
                    PricePerUnit = pricePerUnit,
                    Price = price,
                    Url = url,
                    OfferedBy = offeredBy,
                    Location = location
                };
                
                offers.Add(offer);
            }

            return offers;
        }

        private decimal GetPrice(HtmlNode node)
        {
            decimal price;
            Decimal.TryParse(node.SelectSingleNode(".//li[contains(@class, 'offer-item-price')]").InnerText.Replace("zł", ""), out price);
            return price;
        } 
        private decimal GetArea(HtmlNode node)
        {
            decimal area;
            Decimal.TryParse(node.SelectSingleNode(".//li[contains(@class, 'offer-item-area')]").InnerText.Replace("m²", ""), out area);
            return area;
        }
        private decimal GerPricePerUnit(HtmlNode node)
        {
            decimal pricePerUnit;
            Decimal.TryParse(node.SelectSingleNode(".//li[contains(@class, 'offer-item-price-per-m')]").InnerText.Replace("zł/m²", ""), out pricePerUnit);
            return pricePerUnit;
        }
        private string GetTitle(HtmlNode node) => node.SelectSingleNode(".//span[contains(@class, 'offer-item-title')]").InnerText;
        private string GetUrl(HtmlNode node) => node.SelectSingleNode(".//a[contains(@data-featured-name, 'listing_no_promo')]").InnerText;
        private string GetOfferedBy(HtmlNode node) => node.SelectSingleNode(".//div[contains(@class, 'offer-item-details-bottom')]").SelectSingleNode(".//li").InnerText;
        private string GetLocation(HtmlNode node) => node.SelectSingleNode(".//header[contains(@class, 'offer-item-header')]").SelectSingleNode(".//p").InnerText;
        
    }
}