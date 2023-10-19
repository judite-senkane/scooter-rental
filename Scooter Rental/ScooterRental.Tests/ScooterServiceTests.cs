using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _scooterStorage;
        private const string DEFAULT_SCOOTER_ID = "1";
        private const decimal DEFAULT_PRICE_PER_MINUTE = 0.2m;

        [TestInitialize]
        public void SetUp()
        {
            _scooterStorage = new List<Scooter>();
            _scooterService = new ScooterService(_scooterStorage);
        }

        [TestMethod]
        public void AddScooter_WithIdAndPricePerMinute_ScooterAdded()
        {
            _scooterService.AddScooter (DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);

            _scooterStorage.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddScooter_WithId1AndPricePerMinute1_ScooterAddedWithId1AndPrice1()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, 1m);

            var scooter = _scooterStorage.First();

            scooter.Id.Should().Be(DEFAULT_SCOOTER_ID);
            scooter.PricePerMinute.Should().Be(1m);
        }

        [TestMethod]
        public void AddScooter_AddDuplicateScooter_ThrowsDuplicateScooterException()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);

            action.Should().Throw<DuplicateScooterException>();
        }

        [TestMethod]
        public void AddScooter_AddScooterWithNegativePrice_ThrowsNegativeOrZeroPriceException()
        {
            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, -1m);

            action.Should().Throw<NegativeOrZeroPriceException>();
        }

        [TestMethod]
        public void AddScooter_AddScooterWithPriceZero_ThrowsNegativeOrZeroPriceException()
        {
            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, 0m);

            action.Should().Throw<NegativeOrZeroPriceException>();
        }

        [TestMethod]
        public void AddScooter_AddScooterWithEmptyId_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.AddScooter(string.Empty, 0.2m);

            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void RemoveScooter_ScooterRemovedFromList()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            _scooterService.RemoveScooter(DEFAULT_SCOOTER_ID);

            _scooterStorage.Count.Should().Be(0);
            _scooterStorage.Where(s => s.Id == DEFAULT_SCOOTER_ID).Should().BeNullOrEmpty();
        }

        [TestMethod]
        public void RemoveScooter_RemoveScooterWithEmptyId_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.RemoveScooter(string.Empty);

            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void RemoveScooter_RemoveScooterWithInvalidId_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.RemoveScooter("101");

            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void GetScooters_WithScooterAdded_AListOfScootersReturned()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID,  DEFAULT_PRICE_PER_MINUTE));
            var result = _scooterService.GetScooters();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<Scooter>>();
            result.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetScooterById_WithScooterAdded_ScooterReturned()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            var scooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            scooter.Id.Should().Be(DEFAULT_SCOOTER_ID);
            scooter.Should().BeOfType<Scooter>();
        }

        [TestMethod]
        public void GetScooterById_WithEmptyId_ThrowsInvalidIdException()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            Action action = () => _scooterService.GetScooterById(string.Empty);

            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void GetScooterById_WithInvalidId_ThrowsInvalidIdException()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            Action action = () => _scooterService.GetScooterById("2");

            action.Should().Throw<InvalidIdException>();
        }
    }
}