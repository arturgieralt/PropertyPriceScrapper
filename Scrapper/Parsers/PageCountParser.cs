using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Scrapper.Parsers
{
    public class PageCountParser
    {
        const string PAGE_LIST_SELECTOR = "//ul[contains(@class, 'pager')]//li[not(contains(@class, 'pager-next')) and not(contains(@class, 'pager-prev'))][last()]";
        
        public int GetPageCount(HtmlDocument document) => document.DocumentNode.GetInt(PAGE_LIST_SELECTOR);

    }
}