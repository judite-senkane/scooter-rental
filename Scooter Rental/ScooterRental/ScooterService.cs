using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService: IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooterStorage)
        {
            _scooters = scooterStorage;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooters.Any(s => s.Id == id))
            {
                throw new DuplicateScooterException();
            }

            if (pricePerMinute <= 0) throw new NegativeOrZeroPriceException();

            if (string.IsNullOrEmpty(id)) throw new InvalidIdException();

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new InvalidIdException();
            
            var scooter = _scooters.FirstOrDefault(s => s.Id == id);
            
            if (scooter == null) throw new InvalidIdException();

            _scooters.Remove(scooter);
            
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters;
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (string.IsNullOrEmpty(scooterId)) throw new InvalidIdException();

            var scooter = _scooters.FirstOrDefault(s => s.Id == scooterId);

            if (scooter == null) throw new InvalidIdException();

            return scooter;
        }
    }
}
