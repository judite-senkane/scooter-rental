using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalRecordsServiceTests
    {
        private RentalRecordsService _rentalRecordsService;
        private List<RentedScooter> _rentedScooterList;

        [TestInitialize]
        public void Setup()
        {
            _rentedScooterList = new List<RentedScooter>();
            _rentalRecordsService = new RentalRecordsService(_rentedScooterList);
        }

        [TestMethod]
        public void StartRent_AddValidScooter_ScooterAddedToRecords()
        {
            var scooter = new Scooter("1", 0.2m);
            var startTime = DateTime.Now;

            _rentalRecordsService.StartRent(scooter, startTime);

            _rentedScooterList.Should().HaveCount(1);
            _rentedScooterList.First().Id.Should().Be(scooter.Id);
            _rentedScooterList.First().PricePerMinute.Should().Be(scooter.PricePerMinute);
        }

        [TestMethod]
        public void EndRent_WithExistingScooter_RentEnded()

        {
            var scooter = new Scooter("1", 0.2m);
            var startTime = DateTime.Now;
            _rentedScooterList.Add(new RentedScooter(scooter, startTime));

            var rentalRecord = _rentalRecordsService.EndRent(scooter.Id);
            rentalRecord.Should().NotBeNull();
            rentalRecord.Id.Should().Be(scooter.Id);
            rentalRecord.RentEnd.Should().NotBeNull();
        }

        [TestMethod]
        public void EndRent_WithScooterNotBeingRented_ThrowsScooterNotRentedException()

        {
            var scooter = new Scooter("1", 0.2m) {};
            var startTime = DateTime.Now;
            _rentedScooterList.Add(new RentedScooter(scooter, startTime) {RentEnd = DateTime.Now.AddMinutes(10)});

            Action action = () => _rentalRecordsService.EndRent(scooter.Id);

            action.Should().Throw<ScooterNotRentedException>();
        }

        [TestMethod]
        public void EndRent_WithInvalidScooterId_ThrowsScooterNotRentedException()

        {
            Action action = () => _rentalRecordsService.EndRent("1");

            action.Should().Throw<ScooterNotRentedException>();
        }

        [TestMethod]
        public void ReturnRentedRecordsList_WithYearNullAndUnfinishedRentalsNotIncluded_ReturnsAllRentCompletedScooters()

        {
            _rentedScooterList.AddRange(GetScooterList());

            var result = _rentalRecordsService.ReturnRentedRecordsList(null, false);
            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void ReturnRentedRecordsList_WithYearNullAndUnfinishedRentalsIncluded_ReturnsAllScooters()

        {
            _rentedScooterList.AddRange(GetScooterList());

            var result = _rentalRecordsService.ReturnRentedRecordsList(null, true);
            result.Should().HaveCount(4);
            result.All(r => r.RentEnd.HasValue).Should().BeTrue();
        }

        [TestMethod]
        public void ReturnRentedRecordsList_WithYear2020AndUnfinishedRentalsIncluded_ReturnsAllScootersRentedIn2020()

        {
            _rentedScooterList.AddRange(GetScooterList());

            var result = _rentalRecordsService.ReturnRentedRecordsList(2020, true);
            result.Should().HaveCount(2);
            result.All(r => r.RentEnd.HasValue).Should().BeTrue();
        }

        [TestMethod]
        public void ReturnRentedRecordsList_WithYear2020AndUnfinishedRentalsNotIncluded_ReturnsAllScootersRentedIn2020ExceptStillRented()

        {
            _rentedScooterList.AddRange(GetScooterList());

            var result = _rentalRecordsService.ReturnRentedRecordsList(2020, false);
            result.Should().HaveCount(1);
        }

        private List<RentedScooter> GetScooterList()
        {
            return new List <RentedScooter> {
                new RentedScooter(new Scooter("1", 0.1m), new DateTime(2019, 5, 19, 9, 15, 00))
                    { RentEnd = new DateTime(2019, 5, 19, 11, 30, 0) },
                new RentedScooter(new Scooter("2", 0.2m), new DateTime(2020, 8, 19, 10, 45, 00))
                    { RentEnd = new DateTime(2020, 9, 01, 10, 00, 00) },
                new RentedScooter(new Scooter("3", 0.3m), new DateTime(2023, 9, 5, 10, 30, 00)),
                new RentedScooter(new Scooter("4", 0.2m), new DateTime(2020, 10, 5, 17, 31, 00))
            };
        }
    }
}
