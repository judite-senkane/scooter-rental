namespace ScooterRental
{
    public class RentalCompany: IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly IRentalRecordsService _rentalRecordsService;
        private readonly IRentalCalculations _rentalCalculations;

        public RentalCompany(string name, IScooterService scooterService, IRentalRecordsService rentalRecordsService, IRentalCalculations rentalCalculations)
        {
            Name = name;
            _scooterService = scooterService;
            _rentalRecordsService = rentalRecordsService;
            _rentalCalculations = rentalCalculations;
        }

        public string Name { get; }

        public void StartRent(string id)
        {
           var scooter = _scooterService.GetScooterById(id);
           scooter.IsRented = true;
           _rentalRecordsService.StartRent(scooter, DateTime.Now);
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            scooter.IsRented = false;
            var rentalRecord = _rentalRecordsService.EndRent(scooter.Id);

            var result = _rentalCalculations.CalculateBill(rentalRecord);

            return result;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var rentalRecordsList =
                _rentalRecordsService.ReturnRentedRecordsList(year, includeNotCompletedRentals);
            var result = _rentalCalculations.CalculateIncome(rentalRecordsList);

            return result;
        }
    }
}
