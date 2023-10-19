using FluentAssertions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCalculationsTests
    {
        private IRentalCalculations _rentalCalculations;

        [TestInitialize]
        public void Setup()
        {
            _rentalCalculations = new RentalCalculations();
        }

        [TestMethod]
        public void CalculateBill_WithSameDay_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("1", 0.1m), new DateTime(2023, 5, 19, 9, 15, 00))
                { RentEnd = new DateTime(2023, 5, 19, 11, 30, 0) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(13.5m);
        }

        [TestMethod]
        public void CalculateBill_WithMultipleDays_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("2", 0.2m), new DateTime(2022, 8, 19, 23, 45, 00))
                { RentEnd = new DateTime(2022, 9, 01, 00, 45, 00) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(252);
        }

        [TestMethod]
        public void CalculateBill_WithMultipleMonths_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("4", 0.2m), new DateTime(2020, 10, 5, 23, 42, 00))
                { RentEnd = new DateTime(2021, 1, 1, 00, 03, 00) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(1744.20m);
        }

        [TestMethod]
        public void CalculateBill_WithEdgeCase_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("5", 0.1m), new DateTime(2023, 9, 5, 21, 30, 00))
                { RentEnd = new DateTime(2023, 9, 6, 02, 30, 00) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(30);
        }

        [TestMethod]
        public void CalculateBill_WithNextDayReturn_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("3", 0.3m), new DateTime(2023, 9, 5, 10, 30, 00))
                { RentEnd = new DateTime(2023, 9, 6, 10, 35, 00) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(40);
        }

        [TestMethod]
        public void CalculateBill_WithSameDayMinutesOverMax_CorrectBillReturned()
        {
            var rentedScooter = new RentedScooter(new Scooter("6", 0.1m), new DateTime(2023, 9, 5, 10, 30, 00))
                { RentEnd = new DateTime(2023, 9, 5, 17, 35, 00) };

            var result = _rentalCalculations.CalculateBill(rentedScooter);
            result.Should().Be(20);
        }

        [TestMethod]
        public void CalculateIncome_WithRentedScooterList_CorrectIncomeReturned()
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
            var result = _rentalCalculations.CalculateIncome(scooterList);
            result.Should().Be(2079.7m);
        }
    }
}
