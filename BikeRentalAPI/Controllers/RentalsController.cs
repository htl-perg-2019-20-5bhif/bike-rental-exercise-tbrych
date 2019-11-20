using BikeRentalAPI.Data;
using BikeRentalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;

        public RentalsController(BikeRentalDbContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public async Task<ActionResult<Rental>> Start([FromQuery] int customerId, [FromQuery] int bikeId)
        {
            if (!(await _context.Customer.AnyAsync(c => c.ID == customerId)) || !(await _context.Bike.AnyAsync(b => b.ID == bikeId)))
            {
                return NotFound("The customer/bike does not exist");
            }

            Customer customer = await _context.Customer.FirstAsync(c => c.ID == customerId);
            Bike bike = await _context.Bike.FirstAsync(b => b.ID == bikeId);

            //Check if customer and bike are available
            if ((customer.Rentals == null || customer.Rentals.Count == 0 || customer.Rentals.Last().End != DateTime.MaxValue) &&
                (bike.Rentals == null || bike.Rentals.Count == 0 || bike.Rentals.Last().End != DateTime.MaxValue))
            {
                Rental rental = new Rental();
                rental.Bike = bike;
                rental.Customer = customer;
                rental.Begin = DateTime.Now;
                rental.End = DateTime.MaxValue;
                rental.TotalCosts = 0;
                rental.Paid = false;

                _context.Rental.Add(rental);
                await _context.SaveChangesAsync();

                return Created("The Rental is created", rental);
            }
            else
            {
                return BadRequest("It is not possible to rent this bike with this customer");
            }
        }

        [HttpPut("end/{id}")]
        public async Task<IActionResult> End(int id)
        {
            if (!(await _context.Rental.AnyAsync(r => r.ID == id)))
            {
                return NotFound("Rental not found");
            }

            Rental rental = await _context.Rental.FirstAsync(r => r.ID == id);

            if (rental.End != DateTime.MaxValue)
            {
                return BadRequest("This rental is not active");
            }

            rental.End = DateTime.Now;
            rental.TotalCosts = CostCalc.calculateCost(rental);

            if (rental.TotalCosts == 0)
            {
                rental.Paid = true;
            }

            _context.Rental.Update(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        [HttpPut("pay/{id}")]
        public async Task<IActionResult> PayRental(int id)
        {
            if (!(await _context.Rental.AnyAsync(r => r.ID == id)))
            {
                return NotFound("Rental not found");
            }

            Rental rental = await _context.Rental.FirstAsync(r => r.ID == id);

            if (rental.End == DateTime.MaxValue)
            {
                return BadRequest("This rental is active");
            }

            if (rental.TotalCosts == 0)
            {
                return BadRequest("This rental must not be paid");
            }

            rental.Paid = true;

            _context.Rental.Update(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        [HttpGet("unpaid")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUnpaidRentals()
        {
            List<Rental> rentals = await _context.Rental.Where(r => r.Paid == false && r.End != DateTime.MaxValue).ToListAsync();

            List<RentalJSON> rentalsJSON = new List<RentalJSON>();

            foreach (var rental in rentals)
            {
                RentalJSON curRental = new RentalJSON();
                curRental.CustomerId = rental.CustomerID;
                curRental.RentalId = rental.ID;
                curRental.FirstName = rental.Customer.FirstName;
                curRental.LastName = rental.Customer.LastName;
                curRental.StartDate = rental.Begin;
                curRental.EndDate = rental.End;
                curRental.TotalPrice = rental.TotalCosts;

                rentalsJSON.Add(curRental);
            }

            return Ok(rentalsJSON);
        }

        private bool RentalExists(int id)
        {
            return _context.Rental.Any(e => e.ID == id);
        }
    }

    public class RentalJSON
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
