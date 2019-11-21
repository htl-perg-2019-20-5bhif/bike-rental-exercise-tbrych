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
    public class BikesController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;

        public BikesController(BikeRentalDbContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        [HttpGet("/active")]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikeActive([FromQuery] string sortString)
        {
            List<Bike> bikes = await _context.Bike
                .Where(b => b.Rentals == null || b.Rentals.Count == 0 || b.Rentals.Last().End == DateTime.MaxValue)
                .ToListAsync();

            if (String.IsNullOrEmpty(sortString))
            {
                return bikes;
            }
            else if (sortString.Equals("priceOfFirstHour"))
            {
                return bikes.OrderBy(b => b.PriceFirstHour).ToList();
            }
            else if (sortString.Equals("priceOfAdditionalHour"))
            {
                return bikes.OrderBy(b => b.PriceAdditionalHour).ToList();
            }
            else if (sortString.Equals("purchaseDate"))
            {
                return bikes.OrderByDescending(b => b.PurchaseDate).ToList();
            }
            else
            {
                return BadRequest("The sortString is not valid!");
            }
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBike()
        {
            return await _context.Bike.ToListAsync();
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.ID)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bike.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.ID }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var bike = await _context.Bike.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            if (bike.Rentals != null && bike.Rentals.Count > 0)
            {
                return BadRequest("Bike cannot be deleted!");
            }

            _context.Bike.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        private bool BikeExists(int id)
        {
            return _context.Bike.Any(e => e.ID == id);
        }
    }
}
