using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;

namespace Scrapper.Parsers
{
    public static class HtmlNodeExtensions
    {
        public static string GetString(this HtmlNode node, string selector) => node.SelectSingleNode(selector).InnerText.Replace(@"\t|\n|\r", String.Empty).Trim();
        
        public static string GetHref(this HtmlNode node, string selector) => node.SelectSingleNode(selector).GetAttributeValue("href", String.Empty);
        
        public static double GetDouble(this HtmlNode node, string selector) 
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            ci.NumberFormat.NumberDecimalSeparator = ",";

            var parsedString = node.SelectSingleNode(selector).InnerText.RemoveNonNumeric();
            return Double.Parse(parsedString, ci);
        }

        public static int GetInt(this HtmlNode node, string selector) 
        {
            return Int32.Parse(node.SelectSingleNode(selector).InnerText.RemoveNonNumeric());
        }

        public static string RemoveNonNumeric(this string s)
        {
            return string.Concat(s.Where(c => (char.IsNumber(c) && c != Char.Parse("²")) || c == ',') ?? "");
        }
    }
}