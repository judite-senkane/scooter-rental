namespace ScooterRental;

public interface IRentalCalculations
{
    decimal CalculateBill(RentedScooter rentedScooter);
    decimal CalculateIncome(List<RentedScooter> rentedScooterList);
}