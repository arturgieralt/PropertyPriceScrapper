using System;

namespace BusinessLogic.Models
{
    public enum AreaUnit {
        SquareMeter
    }

    public enum PriceUnit {
        PolishZloty
    }

    public enum OfferType {
        House,
        Flat
    }

    public enum City {
        Wroclaw,
        Warsaw
    }
    public class Offer
    {
        public string Title { get; set; }
        public double Area { get; set; }
        public AreaUnit AreaUnit { get; set; }
        public PriceUnit PriceUnit {get; set; }
        public OfferType Type { get; set; }
        public City City { get; set ;}
        public double PricePerUnit { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
        public string OfferedBy {get; set; }
        public DateTime CreatedOn {get; set; }
    }
}