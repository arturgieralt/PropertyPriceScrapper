using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Models;
using Scrapper.DocumentProviders;
using Scrapper.Parsers;

namespace Scrapper
{
    public class ScrappingManager
    {
        private OfferParser _offerParser {get;}
        private PageCountParser _pageCountParser {get;}
        private WebDocumentProvider _webDocumentProvider {get;}
        public ScrappingManager(
            OfferParser offerParser,
            PageCountParser pageCountParser,
            WebDocumentProvider webDocumentProvider
        )
        {
            _offerParser = offerParser;
            _pageCountParser = pageCountParser;
            _webDocumentProvider = webDocumentProvider;
        }

        public IEnumerable<Offer> GetOffers(string initUrl)
        {
            var document = _webDocumentProvider.Get(initUrl);
            int totalCount;

            try 
            {
                totalCount = _pageCountParser.GetPageCount(document);
            } catch (Exception e){
                totalCount = 0;
            }
            
            var firstPageOffers = _offerParser.GetOffers(document);
            
            if(totalCount > 1)
            {
                return firstPageOffers.Concat(GetTheRestOfTheOffers(totalCount, initUrl));
            }
            
            return firstPageOffers;
        }

        private IEnumerable<Offer> GetTheRestOfTheOffers (int totalCount, string initUrl)
        {
            return Enumerable.Range(2, totalCount -1 ).Select(page => {
                    var url = $"{initUrl}&page={page}";
                    var newDock = _webDocumentProvider.Get(url);
                    
                    return _offerParser.GetOffers(newDock);
                }).SelectMany(aO => aO);
        }
    }
}