namespace ScooterRental;

public interface IRentalRecordsService
{
    void StartRent(Scooter scooter, DateTime starTime);
    RentedScooter EndRent(string id);
    List<RentedScooter> ReturnRentedRecordsList(int? year, bool includeNotCompletedRentals);
}