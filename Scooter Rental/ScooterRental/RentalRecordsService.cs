using ScooterRental.Exceptions;

namespace ScooterRental
{ 
    public class RentalRecordsService : IRentalRecordsService
    {
        private readonly List<RentedScooter> _rentedScooterList;

        public RentalRecordsService(List<RentedScooter> rentedScooterList)
        {
            _rentedScooterList = rentedScooterList;
        }

        public void StartRent(Scooter scooter, DateTime rentStart)
        {
            _rentedScooterList.Add(new RentedScooter(scooter, rentStart));
        }

        public RentedScooter EndRent(string id)
        {

            var rentalRecord = _rentedScooterList
                    .FirstOrDefault(s => s.Id == id && !s.RentEnd.HasValue);

            if (rentalRecord == null) throw new ScooterNotRentedException();

            rentalRecord.RentEnd = DateTime.Now;
            return rentalRecord;
        }

        public List<RentedScooter> ReturnRentedRecordsList(int? year, bool includeNotCompletedRentals)
        {
            List<RentedScooter> result;

            if (!year.HasValue && includeNotCompletedRentals == false)
            {
                result = _rentedScooterList.Where(r => r.RentEnd != null).ToList();
            } 
            else if (!year.HasValue && includeNotCompletedRentals == true)
            {
                result = _rentedScooterList.Select(r => r.RentEnd.HasValue ? r : EndRent(r.Id)).ToList();
            } 
            else if (year.HasValue && includeNotCompletedRentals == false)
            {
                result = _rentedScooterList.Where(r => r.RentEnd.HasValue).Where(s => s.RentEnd.Value.Year == year.Value).ToList();
            } 
            else
            {
                result = _rentedScooterList.Where(r => r.RentStart.Year == year.Value).Select(s => s.RentEnd.HasValue ? s : EndRent(s.Id)).ToList();
            }

            return result;
        }
    }
}
