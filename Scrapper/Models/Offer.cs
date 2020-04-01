namespace Scrapper.Models
{
    public enum AreaUnit {
        SquareMeter
    }
    public class Offer
    {
        public string Title { get; set; }
        public decimal Area { get; set; }
        public AreaUnit AreaUnit { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Price { get; set; }

        public string Url { get; set; }
        public string Location { get; set; }
        public string OfferedBy {get; set; }
    }
}