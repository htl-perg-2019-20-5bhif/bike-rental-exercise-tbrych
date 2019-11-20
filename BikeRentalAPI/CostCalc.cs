using BikeRentalAPI.Model;
using System;

namespace BikeRentalAPI
{
    public class CostCalc
    {
        public static decimal calculateCost(Rental rental)
        {
            TimeSpan diff = rental.End - rental.Begin;

            var minutes = diff.TotalMinutes;
            var hours = (int)Math.Ceiling(minutes / 60);

            if (minutes <= 15)
            {
                return 0;
            }
            else
            {
                decimal costs = 0;
                costs += rental.Bike.PriceFirstHour;
                costs += rental.Bike.PriceAdditionalHour * (hours - 1);

                return costs;
            }
        }
    }
}
