using System;
using HtmlAgilityPack;
using Scrapper.External;

namespace Scrapper.DocumentProviders
{
    public class WebDocumentProvider: IDocumentProvider
    {
       HtmlWeb _web { get; set; }

       public WebDocumentProvider (HtmlWebProvider web)
       {
           _web = web;
       }
       public HtmlDocument Get(string url)
       {
            var htmlDoc = _web.Load(url);
            return htmlDoc;
       }
    }
}