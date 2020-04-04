namespace BusinessLogic.Models
{
    public class AggregatedOffer
    {
        public double AverageArea { get; set; }
        public string AreaUnit { get; set; }
        public string PriceUnit {get; set; }
        public double AveragePricePerUnit {get; set; }
        public string Type { get; set; }
        public string City { get; set ;}
        public double AveragePrice { get; set; }
        public string Location { get; set; }
        public string CreatedOn {get; set; }

        public int Count{ get; set; }
    }
}