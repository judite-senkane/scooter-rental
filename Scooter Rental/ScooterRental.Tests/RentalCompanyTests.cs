using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private AutoMocker _mocker;
        private IRentalCompany _rentalCompany;
        private const string DEFAULT_COMPANY_NAME = "Best Scooters";

        [TestInitialize]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            var scooterServiceMock = _mocker.GetMock<IScooterService>();
            var recordsServiceMock = _mocker.GetMock<IRentalRecordsService>();
            var rentalCalculationsMock = _mocker.GetMock<IRentalCalculations>();

            _rentalCompany = new RentalCompany(DEFAULT_COMPANY_NAME, scooterServiceMock.Object,
                recordsServiceMock.Object, rentalCalculationsMock.Object);
        }

        [TestMethod]
        public void GetName_CorrectCompanyNameReturned()
        {
            _rentalCompany.Name.Should().Be(DEFAULT_COMPANY_NAME);
        }

        [TestMethod]
        public void StartRent_ScooterRentStarted()
        {
            var scooter = new Scooter("1", 1m);
            _mocker.GetMock<IScooterService>()
                .Setup(s => s.GetScooterById("1"))
                .Returns(scooter);

            _rentalCompany.StartRent("1");

            scooter.IsRented.Should().BeTrue();

            _mocker.GetMock<IRentalRecordsService>()
                .Verify(r => r.StartRent(scooter, It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void EndRent_ScooterRentCompletedAndBillReturned()
        {
            var scooter = new Scooter("1", 1m);
            var rentedScooter = new RentedScooter(scooter, DateTime.Now.AddMinutes(-10)) {RentEnd = DateTime.Now};

            _mocker.GetMock<IScooterService>()
                .Setup(s => s.GetScooterById("1"))
                .Returns(scooter);

            _mocker.GetMock<IRentalRecordsService>()
                .Setup(r => 
                    r.EndRent("1"))
                .Returns(rentedScooter);

            _mocker.GetMock<IRentalCalculations>().Setup(r => r.CalculateBill(rentedScooter)).Returns(10);

            var result = _rentalCompany.EndRent("1");

            _mocker.GetMock<IRentalRecordsService>()
                .Verify(r => r.EndRent(scooter.Id), Times.Once());

            scooter.IsRented.Should().BeFalse();
            result.Should().Be(10);
        }

        [TestMethod]
        public void CalculateIncome_IncomeResultReturned()
        {
            var scooterList = new List<RentedScooter>()
            {
                new RentedScooter(new Scooter("1", 0.1m), new DateTime(2019, 5, 19, 9, 15, 00))
                    { RentEnd = new DateTime(2019, 5, 19, 11, 30, 0) },
                new RentedScooter(new Scooter("2", 0.2m), new DateTime(2020, 8, 19, 23, 45, 00))
                    { RentEnd = new DateTime(2020, 9, 01, 00, 45, 00) },
                new RentedScooter(new Scooter("3", 0.3m), new DateTime(2023, 9, 5, 10, 30, 00))
                    {RentEnd = new DateTime(2023,9,6,10,35,00)},
                new RentedScooter(new Scooter("4", 0.2m), new DateTime(2020, 10, 5, 23, 42, 00))
                    {RentEnd = new DateTime(2021,1,1,00,03,00)},
                new RentedScooter(new Scooter("5", 0.1m), new DateTime(2023, 9, 5, 21, 30, 00))
                    {RentEnd = new DateTime(2023,9,6,02,30,00)}
            };

            _mocker.GetMock<IRentalRecordsService>()
                .Setup(s => s.ReturnRentedRecordsList(null, true))
                .Returns(scooterList);

            _mocker.GetMock<IRentalCalculations>()
                .Setup(l => l.CalculateIncome(scooterList))
                .Returns(2079.7m);


            var result = _rentalCompany.CalculateIncome(null, true);

            _mocker.GetMock<IRentalRecordsService>()
                .Verify(r => r.ReturnRentedRecordsList(null, true), Times.Once());

            _mocker.GetMock<IRentalCalculations>()
                .Verify(r => r.CalculateIncome(scooterList), Times.Once());

            result.Should().Be(2079.7m);
        }
    }
}
