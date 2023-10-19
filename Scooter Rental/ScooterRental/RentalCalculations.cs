namespace ScooterRental
{
    public class RentalCalculations : IRentalCalculations
    {
        private const decimal MAX_PRICE_PER_DAY = 20.0m;
        private const int MINUTES_PER_DAY = 1440;

        public decimal CalculateBill(RentedScooter rentedScooter)
        {
            var startTime = rentedScooter.RentStart;
            var endTime = rentedScooter.RentEnd;
            var pricePerMinute = rentedScooter.PricePerMinute;
            
            var timeDifference = endTime - startTime;

            decimal result;

            if (startTime.Day == endTime.Value.Day)
            {
                result = SameDayCalculation(timeDifference, pricePerMinute);
            }
            else
            {
                var minutesFirstDay = MINUTES_PER_DAY - (int)startTime.TimeOfDay.TotalMinutes;
                var minutesLastDay = (int)endTime.Value.TimeOfDay.TotalMinutes;
                var days = ((int)timeDifference.Value.TotalMinutes - minutesFirstDay - minutesLastDay) / MINUTES_PER_DAY;

                result = MultipleDayCalculation(minutesFirstDay, minutesLastDay, days, pricePerMinute);
            }

            return result;
        }

        public decimal CalculateIncome(List<RentedScooter> rentedScooterList)
        {
            return rentedScooterList.Select(CalculateBill).Sum();
        }

        private decimal SameDayCalculation(TimeSpan? timeDifference, decimal pricePerMinute)
        {
            var result = 0m;

            if ((int)timeDifference.Value.TotalMinutes * pricePerMinute > MAX_PRICE_PER_DAY)
            {
                result += MAX_PRICE_PER_DAY;
            }
            else
            {
                result += (int)timeDifference.Value.TotalMinutes * pricePerMinute;
            }

            return result;
        }

        private decimal MultipleDayCalculation(int minutesFirstDay, int minutesLastDay, int days,
            decimal pricePerMinute)
        {
            var result = 0m;

            if (minutesFirstDay * pricePerMinute > MAX_PRICE_PER_DAY)
            {
                result += MAX_PRICE_PER_DAY;
            }
            else
            {
                result += minutesFirstDay * pricePerMinute;
            }

            if (minutesLastDay * pricePerMinute > MAX_PRICE_PER_DAY)
            {
                result += MAX_PRICE_PER_DAY;
            }
            else
            {
                result += minutesLastDay * pricePerMinute;
            }

            if (days > 0)
            {
                result += days * MAX_PRICE_PER_DAY;
            }

            return result;
        }
    }
}
