namespace ScooterRental.Exceptions
{
    public class ScooterNotRentedException: Exception
    {
        public ScooterNotRentedException() : base("This scooter has not been rented out") { }
    }
}
