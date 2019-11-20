using BikeRentalAPI;
using BikeRentalAPI.Model;
using System;
using Xunit;

namespace BikeRentalTests
{
    public class CostCalcTest
    {
        [Fact]
        public void CalculateLessThan15()
        {
            Bike bike = new Bike() { ID = 1, Brand = "Test", BikeCategory = "Racing", PurchaseDate = DateTime.Now, PriceFirstHour = 3, PriceAdditionalHour = 5 };
            Rental rental = new Rental() { Bike = bike, Begin = DateTime.Now, End = DateTime.Now.AddMinutes(7) };

            var result = CostCalc.calculateCost(rental);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Calculate15Minutes()
        {
            Bike bike = new Bike() { ID = 1, Brand = "Test", BikeCategory = "Racing", PurchaseDate = DateTime.Now, PriceFirstHour = 3, PriceAdditionalHour = 5 };
            Rental rental = new Rental() { Bike = bike, Begin = DateTime.Now, End = DateTime.Now.AddMinutes(15) };

            var result = CostCalc.calculateCost(rental);
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateMoreThanOneHour()
        {
            Bike bike = new Bike() { ID = 1, Brand = "Test", BikeCategory = "Racing", PurchaseDate = DateTime.Now, PriceFirstHour = 3, PriceAdditionalHour = 5 };
            Rental rental = new Rental() { Bike = bike, Begin = DateTime.Now, End = DateTime.Now.AddMinutes(100) };

            var result = CostCalc.calculateCost(rental);
            Assert.Equal(8, result);
        }

        [Fact]
        public void CalculateMoreThanTwoHours()
        {
            Bike bike = new Bike() { ID = 1, Brand = "Test", BikeCategory = "Racing", PurchaseDate = DateTime.Now, PriceFirstHour = 3, PriceAdditionalHour = 5 };
            Rental rental = new Rental() { Bike = bike, Begin = DateTime.Now, End = DateTime.Now.AddMinutes(150) };

            var result = CostCalc.calculateCost(rental);
            Assert.Equal(13, result);
        }
    }
}
