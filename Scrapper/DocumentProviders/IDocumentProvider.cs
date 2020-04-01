using HtmlAgilityPack;

namespace Scrapper.DocumentProviders
{
    public interface IDocumentProvider
    {
         HtmlDocument Get(string url);
    }
}