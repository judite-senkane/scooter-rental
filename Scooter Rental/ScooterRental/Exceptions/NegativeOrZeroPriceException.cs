namespace ScooterRental.Exceptions
{
    public class NegativeOrZeroPriceException : Exception
    {
        public NegativeOrZeroPriceException() : base("Price cannot be negative") { }
    }
}
