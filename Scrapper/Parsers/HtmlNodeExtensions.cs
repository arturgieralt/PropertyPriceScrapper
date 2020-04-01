using System;
using System.Linq;
using HtmlAgilityPack;

namespace Scrapper.Parsers
{
    public static class HtmlNodeExtensions
    {
        public static string GetString(this HtmlNode node, string selector) => node.SelectSingleNode(selector).InnerText;
        public static decimal GetDecimal(this HtmlNode node, string selector) 
        {
            decimal value;
            Decimal.TryParse(node.SelectSingleNode(selector).InnerText.RemoveNonNumeric(), out value);
            return value;
        }

        public static int GetInt(this HtmlNode node, string selector) 
        {
            int value;
            Int32.TryParse(node.SelectSingleNode(selector).InnerText.RemoveNonNumeric(), out value);
            return value;
        }

        public static string RemoveNonNumeric(this string s)
        {
            return string.Concat(s.Where(c => char.IsNumber(c) || c == ',') ?? "");
        }
    }
}