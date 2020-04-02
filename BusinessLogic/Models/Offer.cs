namespace BusinessLogic.Models
{
    public enum AreaUnit {
        SquareMeter
    }
    public class Offer
    {
        public string Title { get; set; }
        public double Area { get; set; }
        public AreaUnit AreaUnit { get; set; }
        public double PricePerUnit { get; set; }
        public double Price { get; set; }

        public string Url { get; set; }
        public string Location { get; set; }
        public string OfferedBy {get; set; }
    }
}