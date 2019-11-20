using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Model
{
    public class Bike
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime PurchaseDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastService { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceFirstHour { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAdditionalHour { get; set; }

        private string bikeCategory;

        //Only Standard/Mountain/Trecking/Racing
        public string BikeCategory
        {
            get
            {
                return bikeCategory;
            }
            set
            {
                if (!value.Equals("Standard") && !value.Equals("Mountain") && !value.Equals("Trecking") && !value.Equals("Racing"))
                {
                    throw new ArgumentException("The Bikecategory is not valid!");
                }
                bikeCategory = value;
            }
        }

        public List<Rental> Rentals { get; set; }
    }
}
