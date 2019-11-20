using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Model
{
    public class Rental
    {
        public int ID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int BikeID { get; set; }

        [Required]
        public DateTime Begin { get; set; }

        private DateTime end = DateTime.MaxValue;

        public DateTime End
        {
            get { return end; }
            set
            {
                if (Begin >= value)
                {
                    throw new ArgumentException("The end of the rental must be after the beginning");
                }
                end = value;
            }
        }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalCosts { get; set; }

        private bool paid;

        public bool Paid
        {
            get
            {
                return paid;
            }
            set
            {
                if (value == true && end == DateTime.MaxValue)
                {
                    throw new ArgumentException("The ride can only be paid after the ride has endet");
                }
                paid = value;
            }
        }

        public Customer Customer { get; set; }

        public Bike Bike { get; set; }
    }
}
