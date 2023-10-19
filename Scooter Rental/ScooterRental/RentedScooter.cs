namespace ScooterRental
{
    public class RentedScooter
    {
        public RentedScooter(Scooter scooter, DateTime startTime)
        {
            Id = scooter.Id;
            PricePerMinute = scooter.PricePerMinute;
            RentStart = startTime;
        }

        public string Id { get; }
        public decimal PricePerMinute { get; }
        public DateTime RentStart { get; }
        public DateTime? RentEnd { get; set; }
    }
}
